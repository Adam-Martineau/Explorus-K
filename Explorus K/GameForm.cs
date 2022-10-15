using Explorus_K.Game;
using Explorus_K.Views;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace Explorus_K
{
    public partial class GameForm : Form
    {
        delegate void SetVisibleCallback(Form f, Control ctrl, bool value);
        delegate void SetStatusCallback(Form f, string msg, Color color);
        private GameView gameView;
        public GameForm(GameView gameView)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.gameView = gameView;
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

        public void UpdateStatus(Form form, string msg, Color color)
        {
            if (statusBar.InvokeRequired)
            {
                SetStatusCallback d = new SetStatusCallback(UpdateStatus);
                form.Invoke(d, new object[] { form, msg, color });
            }
            else
            {
                statusLabel.Text = msg;
                statusLabel.ForeColor = color;
            }
        }

        public void showControl(Form form, Control ctrl, bool value)
        {
            if (ctrl.InvokeRequired)
            {
                SetVisibleCallback d = new SetVisibleCallback(showControl);
                form.Invoke(d, new object[] { form, ctrl, value });
            }
            else
            {
                ctrl.Visible = value;
            }
        }

        public void UpdateLevel(string msg, Color color)
        {
            levelLabel.Text = msg;
            levelLabel.ForeColor = color;
        }
    }
}
