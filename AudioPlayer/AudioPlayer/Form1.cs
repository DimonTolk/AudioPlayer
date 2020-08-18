using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Windows.Media;
using System.Drawing.Drawing2D;

namespace AudioPlayer
{
    public partial class Form1 : Form
    {
        #region VARIABLES
        int angle = 5;
        bool isPaused = false;
        Image img;
        MediaPlayer player;
        int curIndex = 0;
        List<string> uploadedSongsUri = new List<string>();
        bool isLooped = false;
        #endregion
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
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            player.Close();
        }
        #region BUTTONS_EVENTS
        private void playBtn_Click(object sender, EventArgs e)
        {
            Play();
            listBox1.Enabled = false;
            shuffleBtn.Enabled = false;
            clearBtn.Enabled = false;
        }

        private void pauseBtn_Click(object sender, EventArgs e)
        {
            if (isPaused)
            {
                isPaused = false;
                player.Play();
                timer1.Start();
                listBox1.Enabled = false;
                shuffleBtn.Enabled = false;
                clearBtn.Enabled = false;
            }
            else
            {
                isPaused = true;
                player.Pause();
                timer1.Stop();
                listBox1.Enabled = true;
                shuffleBtn.Enabled = true;
                clearBtn.Enabled = true;
            }
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            player.Stop();
            timer1.Stop();
            listBox1.Enabled = true;
            shuffleBtn.Enabled = true;
            clearBtn.Enabled = true;
        }
        private void prevBtn_Click(object sender, EventArgs e)
        {
            curIndex--;
            if (curIndex < 0 && isLooped)
            {
                curIndex = listBox1.Items.Count - 1; ;
            }
            Play();
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            uploadedSongsUri.Clear();
        }

        private void shuffleBtn_Click(object sender, EventArgs e)
        {
            ShuffleTracks();
        }

        private void nextBtn_Click(object sender, EventArgs e)
        {
            curIndex++;
            if(curIndex>=listBox1.Items.Count && isLooped)
            {
                curIndex = 0;
            }
            Play();
        }
        #endregion
        private void Play()
        {
            try
            {
                listBox1.SelectedIndex = curIndex;
                textBox1.Text = listBox1.SelectedItem.ToString();
                player.Open(new Uri(uploadedSongsUri[curIndex]));
                player.Volume = trackBar1.Value / 100.0;
                player.Play();
                timer1.Start();
            }
            catch
            {
                MessageBox.Show("Nothing to play", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChooseMedia();
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

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            player.Volume = trackBar1.Value/100.0;
        }

        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            textBox1.Text = Path.GetFileName(listBox1.SelectedItem.ToString());
            curIndex = listBox1.SelectedIndex;
        }

        private void ShuffleTracks()
        {
            var rnd = new Random();
            uploadedSongsUri = uploadedSongsUri.OrderBy(o => rnd.Next()).ToList();

            listBox1.Items.Clear();
            foreach (var item in uploadedSongsUri)
            {
                listBox1.Items.Add(Path.GetFileName(item));
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
        public void ChooseMedia()
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    foreach (var item in openFileDialog1.FileNames)
                    {
                        uploadedSongsUri.Add(item);
                        listBox1.Items.Add(Path.GetFileName(item));
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            isLooped = !isLooped;
        }
    }
}
