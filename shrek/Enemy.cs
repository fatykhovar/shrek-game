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
    class Enemy
    {
        public PointF Location;
        public float stepSize = 3;
        TimerCallback tclX = new TimerCallback(moveX);
        TimerCallback tclY = new TimerCallback(moveY);
        public Timer movement;
        static Random r = new Random();
        internal bool isDead = false;
        static AnimationEnemy enemy = new AnimationEnemy(AnimationEnemy.states.enemy_run, 200);
        public static float lastLocationX = 500;
        const float TIMER = 0.03f;
        internal static float time = 0;
        public float startLocationY;
        public Enemy(bool isFalling)
        {
            Location.X = lastLocationX;
            int position= r.Next(1, 5);
            switch (position)
            {
                case 1: { Location.Y = 95; break; }
                case 2: { Location.Y = 150; break; }
                case 3: { Location.Y = 205; break; }
                case 4: { Location.Y = 260; break; }
                case 5: { Location.Y = 315; break; }
            }
            startLocationY = Location.Y;
            if (isFalling)
            {
                movement = new Timer(tclY, this, 30, 50);
            }
            else
            {
                movement = new Timer(tclX, this, 30, 50);
            }
        }

        private static void moveX(object state)
        {
            Enemy current = (Enemy)state;
            if (current.isDead) return;
            current.MoveX(-current.stepSize);
            enemy.ChangeState(AnimationEnemy.states.enemy_run);
            
        }

        private void MoveX(float divx)
        {
            Location.X = Location.X + divx;
        }

        private static void moveY(object state)
        {
            Enemy current = (Enemy)state;
            if (current.isDead) return;
            time += TIMER;
            current.MoveY(time);
            enemy.ChangeState(AnimationEnemy.states.enemy_run);

        }

        private void MoveY(float divy)
        {
            Location.Y = Location.Y + divy;
        }
        public void ChangeMovement()
        {
            movement.Dispose();
            movement = new Timer(tclX, this, 0, 50);
        }
    }
}
