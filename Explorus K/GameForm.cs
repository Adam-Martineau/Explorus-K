using Explorus_K.Views;
using System;
using System.Drawing;
using System.Windows.Forms;

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
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
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
            gameView.LostFocus();
        }

        private void GameForm_Leave(object sender, EventArgs e)
        {
            gameView.GainFocus();
        }

        public void UpdateStatusBar(String msg, Color color)
        {
            statusStrip1.Text = msg;
            statusStrip1.ForeColor = color;
        }
    }
}
