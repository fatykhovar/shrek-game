using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApp1;
using System.IO;

namespace shrek
{
    public partial class Game : Form
    {
        BufferedGraphicsContext currentContext;
        BufferedGraphics myBuffer;
        Image background = Image.FromFile(@"images\background.png");
        Image game_over = Image.FromFile(@"images\gameover.png");
        Image you_won = Image.FromFile(@"images\youwon.png");
        Animation shrek = new Animation(Animation.states.idle, 200);
        Player pl = new Player(new Point(100, 205));
        AnimationEnemy enemy = new AnimationEnemy(AnimationEnemy.states.enemy_run, 200);
        const int ENEMIES_INITIAL_COUNT = 40;
        const int AMMOS_INITIAL_COUNT = 10;
        List<Enemy> enemies = new List<Enemy>(ENEMIES_INITIAL_COUNT);
        List<Ammo> ammos = new List<Ammo>(AMMOS_INITIAL_COUNT);
        int enemies_dead_count = 0;
        
        public Game()
        {
            InitializeComponent();
            shrek.LoadAnimationForState(Animation.states.run);
            shrek.LoadAnimationForState(Animation.states.attack);
            enemy.LoadAnimationForState(AnimationEnemy.states.enemy_dead);
            currentContext = BufferedGraphicsManager.Current;   
            myBuffer = currentContext.Allocate
                (this.CreateGraphics(), this.DisplayRectangle);

            for (int i = 0; i < 3; i++)
            {
                enemies.Add(new Enemy(true));
                Enemy.lastLocationX += 50 / (float)Math.Log(i + 10);
                enemies[i].stepSize += 0.5f;
                enemies[i].Location.Y -= 10000;
            }
            Enemy.lastLocationX = 500;
            for (int i = 3; i < 6; i++)
            {
                enemies.Add(new Enemy(true));
                Enemy.lastLocationX += 50 / (float)Math.Log(i + 10);
                enemies[i].stepSize += 0.3f;
                enemies[i].Location.Y -= 25000;
            }
            Enemy.lastLocationX = 500;
            for (int i = 6; i < 9; i++)
            {
                enemies.Add(new Enemy(true));
                Enemy.lastLocationX += 50 / (float)Math.Log(i + 10);
                enemies[i].stepSize += 0.3f;
                enemies[i].Location.Y -= 40000;
            }
            for (int i = 9; i < ENEMIES_INITIAL_COUNT; i++)
            {
                enemies.Add(new Enemy(false));
                Enemy.lastLocationX += 250/(float)Math.Log(i+10);
                enemies[i].stepSize += 0.5f;
            }
            for (int i = 0; i < AMMOS_INITIAL_COUNT; i++)
            {
                ammos.Add(new Ammo(new PointF(-50, -50)));
            }
        }

        private void Game_Load(object sender, EventArgs e)
        {

        }



        private void MainTimer(object sender, EventArgs e)
        {
            myBuffer.Graphics.Clear(this.BackColor);
            myBuffer.Graphics.DrawImage(background, new RectangleF(0, 0, DisplayRectangle.Width, DisplayRectangle.Height));
            Image current = shrek.getImage();
            myBuffer.Graphics.DrawImage(current, new RectangleF(pl.Location.X, pl.Location.Y, current.Width/2, current.Height/2));
            myBuffer.Graphics.DrawString(pl.score.ToString(), Font, Brushes.Black, new PointF(450, 20));
            myBuffer.Graphics.DrawString('/'+ENEMIES_INITIAL_COUNT.ToString(), Font, Brushes.Black, new PointF( 462, 20));

            Image current_enemy = enemy.getImage();
            for (int i = 0; i < 9; i++)
            {
                if (enemies[i].Location.Y >=enemies[i].startLocationY)
                {
                    enemies[i].Location.Y = enemies[i].startLocationY;
                    enemies[i].ChangeMovement();
                    enemies[i].stepSize = 1;
                }
                myBuffer.Graphics.DrawImage(current_enemy, new RectangleF(enemies[i].Location.X, enemies[i].Location.Y, current_enemy.Width / 2, current_enemy.Height / 2));
                if (enemies[i].Location.X < pl.Location.X && !enemies[i].isDead)
                {
                    myBuffer.Graphics.DrawImage(game_over, new RectangleF(DisplayRectangle.Width / 3, DisplayRectangle.Height / 3, DisplayRectangle.Width / 3, DisplayRectangle.Height / 3));
                }

            }
            for (int i = 9; i < ENEMIES_INITIAL_COUNT; i++)
            {
                myBuffer.Graphics.DrawImage(current_enemy, new RectangleF(enemies[i].Location.X, enemies[i].Location.Y, current_enemy.Width / 2, current_enemy.Height / 2));
                if (enemies[i].Location.X < pl.Location.X && !enemies[i].isDead)
                {
                    myBuffer.Graphics.DrawImage(game_over, new RectangleF(DisplayRectangle.Width / 3, DisplayRectangle.Height / 3, DisplayRectangle.Width / 3, DisplayRectangle.Height / 3));
                }
            }


            for (int i = 0;ammos.Count!=0 && i < AMMOS_INITIAL_COUNT ; i++)
            {
                if (ammos[i].isFlying)
                {
                    myBuffer.Graphics.FillEllipse(Brushes.SaddleBrown, ammos[i].bounds);
                }
                if (ammos[i].Location.X >= 600)
                {
                    ammos[i].Location = new PointF(-50, -50);
                    ammos[i].isFlying = false;
                }
                for (int j = 0; j < ENEMIES_INITIAL_COUNT; j++)
                {
                    if (ammos[i].Location.X >= enemies[j].Location.X &&
                        ammos[i].Location.Y >= enemies[j].Location.Y &&
                        ammos[i].Location.Y <= (enemies[j].Location.Y + 50) &&
                        !enemies[j].isDead &&
                        pl.Location.X<=enemies[j].Location.X)
                    {
                        ammos[i].Location = new PointF(-50, -50);
                        ammos[i].isFlying = false;
                        enemies[j].isDead = true;
                        enemies[j].Location = new PointF(-100, -100);
                        pl.score++;
                    }
                }
            }
            if (pl.score==ENEMIES_INITIAL_COUNT)
            {
                myBuffer.Graphics.DrawImage(you_won, new RectangleF(DisplayRectangle.Width / 3, DisplayRectangle.Height / 3, DisplayRectangle.Width / 3, DisplayRectangle.Height / 3));
            }
            myBuffer.Render();
            myBuffer.Render(this.CreateGraphics());
        }

        private void Game_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                pl.Move(1);
                shrek.ChangeState(Animation.states.run);
            }
            if (e.KeyCode == Keys.S)
            {
                pl.Move(2);
                shrek.ChangeState(Animation.states.run);
            }
            if (e.KeyCode == Keys.D)
            {
                timer2.Enabled = true;
                shrek.ChangeState(Animation.states.attack);
            }
        }

        private void Game_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                shrek.ChangeState(Animation.states.idle);
            }
            if (e.KeyCode == Keys.S)
            {
                shrek.ChangeState(Animation.states.idle);
            }
            if (e.KeyCode == Keys.D)
            {
                shrek.ChangeState(Animation.states.idle);
                timer2.Enabled = false;
            }
        }
        DateTime lastshot=DateTime.Now;
        int sfs = 400;
        private void timer2_Tick(object sender, EventArgs e)
        {
            if ((DateTime.Now - lastshot).Milliseconds >= sfs)
            {
                for (int i = 0; i < AMMOS_INITIAL_COUNT; i++) 
                {
                    if (!ammos[i].isFlying)
                    {
                        ammos[i].Location = new PointF(pl.Location.X + 50, pl.Location.Y + 10);
                        ammos[i].isFlying = true;
                        lastshot = DateTime.Now;
                        break;
                    }
                }
                sfs = 200;
            }
        }

        private void Game_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
