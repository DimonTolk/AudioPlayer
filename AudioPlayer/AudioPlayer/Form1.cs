using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Media;
using System.Drawing.Drawing2D;

namespace AudioPlayer
{
    public partial class Form1 : Form
    {
        int angle = 5;
        Image img;
        string fileName = string.Empty;
        MediaPlayer player;
        public Form1()
        {
            InitializeComponent();
            img = pictureBox1.Image;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            player = new MediaPlayer();
            trackBar1.Value = 50;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                player.Open(new Uri(fileName));
                player.Volume = trackBar1.Value/100.0;
                player.Play();
                timer1.Start();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            player.Pause();
            timer1.Stop();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            player.Stop();
            timer1.Stop();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChooseMedia();
        }

        public void ChooseMedia()
        {
            try
            {
                if(openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    fileName = openFileDialog1.FileName;
                    textBox1.Text = Path.GetFileName(fileName);
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        public static Image RotateImage(Image img, float rotationAngle)
        {
            Bitmap bmp = new Bitmap(img.Width, img.Height);

            Graphics gfx = Graphics.FromImage(bmp);

            gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);

            gfx.RotateTransform(rotationAngle);

            gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;

            gfx.DrawImage(img, new Point(0, 0));

            gfx.Dispose();

            return bmp;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Image = RotateImage(img, angle);
            angle+=5;
            if (angle > 360)
            {
                angle = 5;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            player.Close();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            player.Volume = trackBar1.Value/100.0;
        }
    }
}
