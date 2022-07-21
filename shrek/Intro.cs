using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace shrek
{
    public partial class Intro : Form
    {
        BufferedGraphicsContext currentContext;
        BufferedGraphics myBuffer;
        Image intro = Image.FromFile(@"images\intro.jpg");
        SoundPlayer soundPlayer = new SoundPlayer("All Star.wav");
        public Intro()
        {
            currentContext = BufferedGraphicsManager.Current;
            myBuffer = currentContext.Allocate(this.CreateGraphics(), new Rectangle(0, 0, 1920, 1080));
            InitializeComponent();
            soundPlayer.Play();
        }

        private void Intro_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            myBuffer.Graphics.Clear(this.BackColor);
            myBuffer.Graphics.DrawImage(intro, new RectangleF(0, 0, DisplayRectangle.Width, DisplayRectangle.Height));
            myBuffer.Render();
            myBuffer.Render(this.CreateGraphics());
        }

        private void button_Play(object sender, EventArgs e)
        {
            Game f = new Game();
            f.Show();
            this.Hide();
        }

    }
}
