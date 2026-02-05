namespace Global
{
    using Global;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Forms;
    using WMPLib;
    using static Global.EasyObject;
    public class EasyMediaControl: Panel
    {
        protected Form parent;
        protected EasyMediaPlayer MediaPlayer;

        public EasyMediaControl(Form parent, int volume = 100)
        {
            this.parent = parent;
            this.MediaPlayer = new EasyMediaPlayer();
            this.Controls.Add(this.MediaPlayer);
            this.MediaPlayer.Dock = DockStyle.Fill;
            this.parent.Load += (s, e) =>
            {
                var timer = new System.Threading.Timer((state) =>
                {
                    this.Invoke((MethodInvoker)(() => {
                        MediaPlayer.SetVolume(volume);
                    }));
                    ((System.Threading.Timer)state!).Dispose();
                });
                timer.Change(TimeSpan.FromMilliseconds(50), TimeSpan.Zero);
            };
        }
        public bool HandleDialogKey(System.Windows.Forms.Keys keyData)
        {
            return this.MediaPlayer.HandleDialogKey(keyData);
        }
        public void SetVolume(int volume)
        {
            this.MediaPlayer.SetVolume(volume);
        }
        public void Play()
        {
            this.MediaPlayer.Ctlcontrols.play();
        }
        public EasyObject Info
        {
            get
            {
                return FromObject(new { a = "" });
            }
        }
        public EasyObject currentMedia
        {
            get
            {
                string? sourceURL = null;
                try
                {
                    sourceURL = this.MediaPlayer.currentMedia.sourceURL;
                }
                catch (Exception)
                {
                    ;
                }
                int duration = 0;
                try
                {
                    duration = (int)this.MediaPlayer.currentMedia.duration;
                }
                catch (Exception)
                {
                    ;
                }
                return FromObject(
                    new
                    {
                        sourceURL,
                        duration
                    });
            }
        }
        public int currentPosition
        {
            get
            {
                return (int)this.MediaPlayer.Ctlcontrols.currentPosition;
            }
        }
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string URL
        {
            get
            {
                return this.MediaPlayer.URL;
            }
            set
            {
                this.MediaPlayer.URL = value;
            }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string[] PlayList1
        {
            get
            {
                return [];
            }
            set
            {
                IWMPPlaylist playList = MediaPlayer.playlistCollection.newPlaylist("Anonymous PlayList");
                foreach (string file in value)
                {
                    IWMPMedia mediaItem = MediaPlayer.newMedia(file);
                    playList.appendItem(mediaItem);
                }
                MediaPlayer.currentPlaylist = playList;
            }
        }
    }


    public class EasyMediaPlayer : AxWMPLib.AxWindowsMediaPlayer
    {
        public EasyMediaPlayer()
        {
        }
        public bool HandleDialogKey(System.Windows.Forms.Keys keyData)
        {
            if ((keyData & Keys.KeyCode) == Keys.Space)
            {
                if (this.playState == WMPLib.WMPPlayState.wmppsPaused || this.playState == WMPLib.WMPPlayState.wmppsStopped)
                    this.Ctlcontrols.play();
                else
                    this.Ctlcontrols.pause();
                return true;
            }
            if ((keyData & Keys.KeyCode) == Keys.C)
                CopyMediaPlayerInfo();
            if ((keyData & Keys.KeyCode) == Keys.Left)
                OnKeyDownLeft();
            if ((keyData & Keys.KeyCode) == Keys.Right)
                OnKeyDownRight();

            return false;
        }
        void CopyMediaPlayerInfo()
        {
            var curMedia = this.currentMedia;
            if (curMedia != null)
            {
                string sourceURL = curMedia.sourceURL;
                int duration = (int)this.currentMedia.duration;
                int curPosition = (int)this.Ctlcontrols.currentPosition;
                TimeSpan span1 = new TimeSpan(0, 0, duration);
                TimeSpan span2 = new TimeSpan(0, 0, curPosition);
                string str = String.Format(
                    "ファイルパス：{0}\n長さ：{1}\n現在位置：{2}",
                    sourceURL, span1.ToString(), span2.ToString());
                Clipboard.SetText(str);
            }
        }
        void OnKeyDownLeft()
        {
            double value = this.Ctlcontrols.currentPosition;
            if (Control.ModifierKeys != Keys.Control)
                value--;
            else
                value -= 10;

            if (value > 0)
            {
                this.Ctlcontrols.currentPosition = value;
            }
        }
        void OnKeyDownRight()
        {
            double value = this.Ctlcontrols.currentPosition;
            if (Control.ModifierKeys != Keys.Control)
                value++;
            else
                value += 10;
            if (value < this.currentMedia.duration)
            {
                this.Ctlcontrols.currentPosition = value;
            }
        }
        public void SetVolume(int volume)
        {
            // Ensure volume is within the valid range (0 to 100)
            if (volume >= 0 && volume <= 100)
            {
                this.settings.volume = volume;
            }
            else
            {
                // Handle invalid input if necessary
                //Console.WriteLine("Volume must be between 0 and 100.");
                throw new ArgumentException("Volume must be between 0 and 100.");
            }
        }
    }
}
