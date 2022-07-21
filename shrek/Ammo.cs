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
    class Ammo
    {
        public RectangleF bounds;
        public PointF Location;
        public float speed;
        public bool isFlying = false;
        public double radius;
        TimerCallback tcl = new TimerCallback(move);
        Timer movement;


        public Ammo(PointF startLocation)
        {
            movement = new Timer(tcl, this, 3, 80);
            radius = 10;
            speed = 12f;
            Location = startLocation;
            bounds = new RectangleF(new Point((int)Location.X, (int)Location.Y), new Size((int)radius, (int)radius));
        }
        private static void move(object state)
        {
            Ammo current = (Ammo)state;
            current.Move(current.speed);
            current.bounds = new RectangleF(new Point((int)current.Location.X, (int)current.Location.Y), new Size((int)current.radius, (int)current.radius));
        }

        private void Move(float divx)
        {
            Location.X = Location.X + divx;
        }
    }
}
