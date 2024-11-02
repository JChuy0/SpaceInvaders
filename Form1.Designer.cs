namespace SpaceInvaders
{
    partial class FrmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            grpStats = new GroupBox();
            lblRemainingLives = new Label();
            lblScore = new Label();
            tmrUpdate = new System.Windows.Forms.Timer(components);
            tmrUpdateEnemies = new System.Windows.Forms.Timer(components);
            tmrEnemyBullet = new System.Windows.Forms.Timer(components);
            grpStats.SuspendLayout();
            SuspendLayout();
            // 
            // grpStats
            // 
            grpStats.Controls.Add(lblRemainingLives);
            grpStats.Controls.Add(lblScore);
            grpStats.Location = new Point(0, 419);
            grpStats.Name = "grpStats";
            grpStats.Size = new Size(678, 80);
            grpStats.TabIndex = 0;
            grpStats.TabStop = false;
            grpStats.Text = "Stats:";
            // 
            // lblRemainingLives
            // 
            lblRemainingLives.AutoSize = true;
            lblRemainingLives.Location = new Point(153, 38);
            lblRemainingLives.Name = "lblRemainingLives";
            lblRemainingLives.Size = new Size(131, 20);
            lblRemainingLives.TabIndex = 2;
            lblRemainingLives.Text = "Remaining Lives: 3";
            // 
            // lblScore
            // 
            lblScore.AutoSize = true;
            lblScore.Location = new Point(12, 38);
            lblScore.Name = "lblScore";
            lblScore.Size = new Size(61, 20);
            lblScore.TabIndex = 1;
            lblScore.Text = "Score: 0";
            // 
            // tmrUpdate
            // 
            tmrUpdate.Tick += TmrUpdate_Tick;
            // 
            // tmrUpdateEnemies
            // 
            tmrUpdateEnemies.Interval = 1500;
            tmrUpdateEnemies.Tick += TmrUpdateEnemies_Tick;
            // 
            // tmrEnemyBullet
            // 
            tmrEnemyBullet.Interval = 1500;
            tmrEnemyBullet.Tick += TmrEnemyBullet_Tick;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(680, 500);
            Controls.Add(grpStats);
            Name = "FrmMain";
            Text = "Space Invaders";
            grpStats.ResumeLayout(false);
            grpStats.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox grpStats;
        private Label lblScore;
        private System.Windows.Forms.Timer tmrUpdate;
        private Label lblRemainingLives;
        private System.Windows.Forms.Timer tmrUpdateEnemies;
        private System.Windows.Forms.Timer tmrEnemyBullet;
    }
}
