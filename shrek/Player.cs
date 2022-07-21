using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace shrek
{
    class Player
    {
        public PointF Location;
        public float stepSize = 55;
        public int score = 0;
        public Player(Point startLoc)
        {
            Location = startLoc;
        }
        public void Move(int direction)
        {
            switch (direction)
            {
                case 1:
                    { if (Location.Y > 95)
                        {
                            Location.Y -= stepSize; 
                        }
                        break;
                    }
                case 2:
                    {
                        if(Location.Y < 315)
                        {
                            Location.Y += stepSize;
                        }
                        break; 
                    }
            }
        }

    }
}
