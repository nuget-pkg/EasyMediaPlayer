namespace Global
{
    using System;
    using System.Windows.Forms;
    public class EasyMediaPlayer : AxWMPLib.AxWindowsMediaPlayer
    {
        public EasyMediaPlayer()
        {
            //SetWMPVolume(100);
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
            //if (this.playState == WMPLib.WMPPlayState.wmppsPaused || this.playState == WMPLib.WMPPlayState.wmppsStopped)
            //    this.Ctlcontrols.play();
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
            //if (this.playState == WMPLib.WMPPlayState.wmppsPaused || this.playState == WMPLib.WMPPlayState.wmppsStopped)
            //    this.Ctlcontrols.play();
        }
        // メニュー項目クリック時の処理例
        public void SetWMPVolume(int volume)
        {
            // Ensure volume is within the valid range (0 to 100)
            if (volume >= 0 && volume <= 100)
            {
                this.settings.volume = volume;
            }
            else
            {
                // Handle invalid input if necessary
                Console.WriteLine("Volume must be between 0 and 100.");
            }
        }
    }
}
