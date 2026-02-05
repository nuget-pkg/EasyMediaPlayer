using Global;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Global.EasyObject;

namespace MyMediaPlayer
{
    public partial class Form1 : Form
    {
        //EasyMediaPlayer MediaPlayer = new EasyMediaPlayer();
        EasyMediaControl MediaPlayer;
        System.Windows.Forms.Timer Timer = new System.Windows.Forms.Timer();
        public Form1()
        {
            InitializeComponent();
            this.MediaPlayer = new EasyMediaControl(
                this,
                autoStart: true,
                volume: 50
                );
            this.Icon = Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location);
            this.StartPosition = FormStartPosition.Manual;
            Rectangle screen = Screen.FromPoint(Cursor.Position).WorkingArea;
            this.ClientSize = new Size((int)(screen.Width * 0.75), (int)(screen.Height * 0.75)); /**/
            int w = Width >= screen.Width ? screen.Width : (screen.Width + Width) / 2;
            int h = Height >= screen.Height ? screen.Height : (screen.Height + Height) / 2;
            this.Location = new Point(screen.Left + (screen.Width - w) / 2, screen.Top + (screen.Height - h) / 2);
            this.Size = new Size(w, h);
            Timer.Interval = 1000;
            Timer.Tick += Timer_Tick;
            Timer.Start();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            panel1.Dock = DockStyle.Fill;
            panel1.Controls.Add(this.MediaPlayer);
            MediaPlayer.Dock = DockStyle.Fill;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            var curMedia = MediaPlayer.CurrentMedia;
            if (curMedia != null)
            {
                string sourceURL = curMedia.Dynamic.sourceURL;
                //Log(sourceURL, "sourceURL");
                string fileName = Path.GetFileName(sourceURL);
                //Log(fileName, "fileName");
                if (this.Text != fileName)
                {
                    this.Text = fileName;
                }
                int duration = (int)MediaPlayer.CurrentMedia.Dynamic.duration;
                int curPosition = (int)MediaPlayer.CurrentPosition;
                TimeSpan span1 = new TimeSpan(0, 0, duration);
                TimeSpan span2 = new TimeSpan(0, 0, curPosition);
                toolStripLabel1.Text = String.Format("{0} / {1}  ", span2.ToString(), span1.ToString());
                toolStripLabel2.Text = sourceURL;
            }
            else
            {
                toolStripLabel1.Text = "";
                toolStripLabel2.Text = "";
            }
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            return this.MediaPlayer.HandleDialogKey(keyData);
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            MediaPlayer.URL = @"C:\Users\user\Music\@1080p\[1080p]  Balo TikTok 【抖音背包】 『Everytime We Touch (Original Mix) - xxxCr3 ｜ 2022抖音最火的歌曲 ｜ Trending TikTok』 【ID：TQ_oIxIDKTA】.mp4";
            //axWindowsMediaPlayer1.Ctlcontrols.play();
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Media Files|*.mp3;*.m4a;*.wma;*.wav;*.mp4;*.wmv|All Files|*.*";
            openFileDialog.Multiselect = true; // Allow multiple selections
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                MediaPlayer.PlayList1 = openFileDialog.FileNames;
                MediaPlayer.Play();
            }
        }
    }
}
