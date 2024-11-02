using System;

namespace SpaceInvaders
{
    public partial class FrmMain : Form
    {
        private readonly List<Point> playerShip = [];
        private readonly List<Alien> enemyList = [];
        private readonly List<Rectangle> playerBullet = [];
        private readonly List<Rectangle> enemyBullet = [];
        readonly int sqSize = 10;
        int score = 0;
        int remainingLives = 2;
        bool enemyDirection = true;


        public FrmMain()
        {
            InitializeComponent();
            CreatePlayerShip();
            CreateEnemies();

            this.KeyPreview = true;

            this.KeyDown += new KeyEventHandler(OnKeyDown);
            this.Paint += new PaintEventHandler(OnPaint);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            tmrUpdate.Start();
            tmrUpdateEnemies.Start();
            tmrEnemyBullet.Start();
        }

        private void SpawnPlayer()
        {
            int formWidth = this.ClientSize.Width;
            int formHeight = this.ClientSize.Height;

            playerShip.Add(new Point(formWidth / 2 - sqSize, formHeight - 100));
            playerShip.Add(new Point(formWidth / 2, formHeight - 100 - sqSize));
            playerShip.Add(new Point(formWidth / 2 + sqSize, formHeight - 100));
        }

        private void CreatePlayerShip()
        {
            SpawnPlayer();
        }

        private void CreateEnemies()
        {
            for (int i = 0; i < 10; i++)
            {
                int x = 40 + sqSize * 6 * i;
                int y = 40;

                enemyList.Add(new Alien(sqSize, x, y, 10));
                enemyList.Add(new Alien(sqSize, x, y + 40, 5));
                enemyList.Add(new Alien(sqSize, x, y + 80, 5));
                enemyList.Add(new Alien(sqSize, x, y + 120));
                enemyList.Add(new Alien(sqSize, x, y + 160));
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Space:
                    if (playerBullet.Count < 3)
                    {
                        CreatePlayerBullet();
                    }
                    break;
                case Keys.Left:
                    MovePlayer("left");
                    break;
                case Keys.Right:
                    MovePlayer("right");
                    break;
            }
        }

        private void MovePlayer(string direction)
        {
            if (direction == "left" && playerShip[0].X > sqSize)
            {
                for (int i = 0; i < playerShip.Count; i++)
                {
                    playerShip[i] = new Point(playerShip[i].X - sqSize, playerShip[i].Y);
                }
            }
            else if (direction == "right" && playerShip[2].X < this.ClientSize.Width - sqSize * 2)
            {
                for (int i = 0; i < playerShip.Count; i++)
                {
                    playerShip[i] = new Point(playerShip[i].X + sqSize, playerShip[i].Y);
                }
            }
        }

        private void MoveEnemies()
        {
            int maxHeight = 0;

            // Move right if true, move left if false
            if (enemyDirection)
            {
                for (int i = 0; i < enemyList.Count; i++)
                {
                    int x = enemyList[i].XYCoordinates.X + sqSize;
                    int y = enemyList[i].XYCoordinates.Y;
                    maxHeight = Math.Max(maxHeight, y);

                    enemyList[i] = new Alien(sqSize, x, y, enemyList[i].PointValue);
                }
            }
            else if (!enemyDirection)
            {
                for (int i = 0; i < enemyList.Count; i++)
                {
                    int x = enemyList[i].XYCoordinates.X - sqSize;
                    int y = enemyList[i].XYCoordinates.Y;
                    maxHeight = Math.Max(maxHeight, y);

                    enemyList[i] = new Alien(sqSize, x, y, enemyList[i].PointValue);
                }
            }

            // Enemies move down when they reach the left or right border but will stop before reaching the player
            int rightMostEnemy = enemyList[^1].XYCoordinates.X;
            int leftMostEnemy = enemyList[0].XYCoordinates.X;
            int rightBorder = ClientSize.Width - sqSize * 3;
            int leftBorder = sqSize;

            if (rightMostEnemy == rightBorder || leftMostEnemy == leftBorder)
            {
                enemyDirection = !enemyDirection;
                for (int i = 0; i < enemyList.Count; i++)
                {
                    int x = enemyList[i].XYCoordinates.X;
                    int y = enemyList[i].XYCoordinates.Y;

                    if (maxHeight < ClientSize.Height - 150)
                    {
                        y += sqSize * 2;
                    }

                    enemyList[i] = new Alien(sqSize, x, y, enemyList[i].PointValue);
                }
            }

        }

        private void CreatePlayerBullet()
        {
            Point center = playerShip[1];

            playerBullet.Add(new Rectangle(center.X, center.Y - sqSize, sqSize, sqSize));
        }

        private void CreateEnemyBullet()
        {
            // random enemy shoots at the player
            Random random = new Random();
            int num = random.Next(0, enemyList.Count -1);

            Rectangle center = enemyList[num].XYCoordinates;
            enemyBullet.Add(new Rectangle(center.X + sqSize, center.Y + sqSize, sqSize, sqSize));
        }

        private void MoveBullets(List<Rectangle> bullets, bool isPlayerBullet)
        {
            int direction = isPlayerBullet ? -1 : 1;

            // clear bullet if it goes off screen
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                if (isPlayerBullet ? bullets[i].Y < sqSize : bullets[i].Y >= ClientSize.Height)
                {
                    bullets.RemoveAt(i);
                } else
                {
                    bullets[i] = new Rectangle(bullets[i].X, bullets[i].Y + (sqSize * direction), bullets[i].Width, bullets[i].Height);
                }

            }

            this.Invalidate();
        }

        private void CheckBulletCollision()
        {
            // when shooting an enemy bullet, sometimes they pass through each other instead of destroying each other
            for (int i = enemyBullet.Count - 1; i >= 0; i--)
            {
                for (int k = playerBullet.Count - 1; k >= 0; k--)
                {
                    if (enemyBullet[i].IntersectsWith(playerBullet[k]))
                    {
                        enemyBullet.RemoveAt(i);
                        playerBullet.RemoveAt(k);
                        i--;
                        break;
                    }
                }
            }

            for (int i = playerBullet.Count - 1; i >= 0; i--)
            {
                // check if each bullet intersects with each enemy
                for (int j = enemyList.Count - 1; j >= 0; j--)
                {
                    if (playerBullet[i].IntersectsWith(enemyList[j].XYCoordinates))
                    {
                        score += enemyList[j].PointValue;
                        lblScore.Text = $"Score: {score}";
                        enemyList.RemoveAt(j);
                        playerBullet.RemoveAt(i);
                        break;
                    }
                }
            }

            // removes player if they get hit by enemy
            for (int i = enemyBullet.Count - 1; i >= 0; i--)
            {
                foreach(var ship in playerShip)
                {
                    if (enemyBullet[i].Contains(ship))
                    {
                        playerShip.Clear();
                        enemyBullet.RemoveAt(i);
                        remainingLives--;
                        lblRemainingLives.Text = $"Remaining lives: {remainingLives}";

                        if (remainingLives < 0)
                        {
                            GameOver();
                        }

                        SpawnPlayer();
                        break;
                    }
                }
            }

        }

        private void GameOver()
        {
            tmrUpdate.Stop();
            tmrUpdateEnemies.Stop();
            tmrEnemyBullet.Stop();
            MessageBox.Show($"Game Over. Score: {score}");
            Application.Exit();
            return;
        }

        private void TmrUpdateEnemies_Tick(object sender, EventArgs e)
        {
            MoveEnemies();

            int count = enemyList.Count;

            if (count < 20)
            {
                tmrUpdateEnemies.Interval = 300;
                tmrEnemyBullet.Interval = 300;
            }
            else if (count < 30)
            {
                tmrUpdateEnemies.Interval = 700;
                tmrEnemyBullet.Interval = 700;
            }
            else if (count < 40)
            {
                tmrUpdateEnemies.Interval = 1200;
                tmrEnemyBullet.Interval = 1200;
            }

        }

        // Creates an enemy bullet every X seconds
        private void TmrEnemyBullet_Tick(object sender, EventArgs e)
        {
            if (enemyBullet.Count < 5)
            {
                CreateEnemyBullet();
            }
        }


        private void TmrUpdate_Tick(object sender, EventArgs e)
        {
            MoveBullets(playerBullet, true);
            MoveBullets(enemyBullet, false);
            CheckBulletCollision();

            if (enemyList.Count == 0)
            {
                GameOver();
            }
        }


        // draws all the graphics
        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            foreach (Point player in playerShip)
            {
                g.FillRectangle(Brushes.Blue, player.X, player.Y, sqSize, sqSize);
            }


            for (int i = 0; i < enemyList.Count; i++)
            {
                Alien alien = enemyList[i];
                Brush brush = Brushes.Red;

                switch(alien.PointValue)
                {
                    case 1:
                        brush = Brushes.Purple; break;
                    case 5:
                        brush = Brushes.Orange; break;
                    case 10:
                        brush = Brushes.Red; break;
                }

                g.FillRectangle(brush, alien.XYCoordinates.X, alien.XYCoordinates.Y, alien.XYCoordinates.Width, alien.XYCoordinates.Height);
            }

            if (playerBullet != null)
            {
                foreach (Rectangle bullet in playerBullet)
                {
                    g.FillRectangle(Brushes.Green, bullet.X, bullet.Y, bullet.Width, bullet.Height);
                }
            }

            if (enemyBullet != null)
            {
                foreach (Rectangle bullet in enemyBullet)
                {
                    g.FillRectangle(Brushes.YellowGreen, bullet.X, bullet.Y, bullet.Width, bullet.Height);
                }
            }

        }

    }
}
