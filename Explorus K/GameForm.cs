using Explorus_K.Controllers;
using Explorus_K.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
