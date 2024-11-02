using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    internal class Alien
    {
        public Rectangle XYCoordinates { get; set; }
        public int PointValue { get; set; }
        //public string color;

        public Alien(int sqSize, int x, int y, int pointValue = 1)
        {
            XYCoordinates = new Rectangle(x, y, sqSize * 3, sqSize * 2);
            this.PointValue = pointValue;
            //this.color = color;
        }
    }
}
