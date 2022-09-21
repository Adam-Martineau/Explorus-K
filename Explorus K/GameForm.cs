using Explorus_K.Game;
using Explorus_K.Views;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Explorus_K
{
    public partial class GameForm : Form
    {
        private GameView gameView;
        public GameForm(GameView gameView)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.gameView = gameView;
            musicTrackBar.Value = Constant.MUSIC_VOLUME;
            musicTrackBar.Maximum = 100;
            musicTrackBar.Minimum = 0;
            musicTrackBar.TickFrequency = 5;
            soundTrackBar.Maximum = 100;
            soundTrackBar.Minimum = 0;
            soundTrackBar.TickFrequency = 5;
            soundTrackBar.Value = Constant.SOUND_VOLUME;
            musicValueLabel.Text = musicTrackBar.Value.ToString();
            soundValueLabel.Text = soundTrackBar.Value.ToString();
            soundOptionsPanel.Width = 400;
            soundOptionsPanel.Height = 100;
            soundOptionsPanel.Location = new Point(this.Width / 2 - soundOptionsPanel.Size.Width / 2,this.Height / 2);
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            this.ActiveControl = null;
            gameView.ReceiveKeyEvent(e);
        }

        private void GameForm_SizeChanged_1(object sender, EventArgs e)
        {
            if (gameView != null) {
                gameView.resize();
            }
        }

        private void GameForm_Enter(object sender, EventArgs e)
        {
            gameView.GainFocus();
        }

        private void GameForm_Leave(object sender, EventArgs e)
        {
            gameView.LostFocus();
        }

        public void UpdateStatus(string msg, Color color)
        {
            statusLabel.Text = msg;
            statusLabel.ForeColor = color;
        }

        public void UpdateLevel(string msg, Color color)
        {
            levelLabel.Text = msg;
            levelLabel.ForeColor = color;
        }

        public void setVisibleSoundOptions(bool visible)
        {
            soundOptionsPanel.Visible = visible;
        }

        private void musicTrackBar_Scroll(object sender, EventArgs e)
        {
            gameView.setMusicVolume(musicTrackBar.Value);
            musicValueLabel.Text = musicTrackBar.Value.ToString();
        }

        private void soundTrackBar_Scroll(object sender, EventArgs e)
        {
            gameView.setSoundVolume(soundTrackBar.Value);
            soundValueLabel.Text = soundTrackBar.Value.ToString();
        }

        private void muteMusic_Click(object sender, EventArgs e)
        {
            musicTrackBar.Value = 0;
            gameView.setMusicVolume(musicTrackBar.Value);
            musicValueLabel.Text = musicTrackBar.Value.ToString();
        }

        private void muteSound_Click(object sender, EventArgs e)
        {
            soundTrackBar.Value = 0;
            gameView.setSoundVolume(soundTrackBar.Value);
            soundValueLabel.Text = soundTrackBar.Value.ToString();
        }
    }
}
