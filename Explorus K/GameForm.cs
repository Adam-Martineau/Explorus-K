using Explorus_K.Controllers;
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
        private GameEngine gameEngine;
        public GameForm(GameEngine gameEngine)
        {
            InitializeComponent();
            this.gameEngine = gameEngine;
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            gameEngine.KeyEventHandler(e);
        }

        private void GameForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //ToDo: find a way to stop all processes
        }
    }
}
