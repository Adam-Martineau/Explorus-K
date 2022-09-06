﻿using Explorus_K.Controllers;
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
            this.DoubleBuffered = true;
            this.gameEngine = gameEngine;
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            gameEngine.KeyEventHandler(e);
        }

        private void GameForm_SizeChanged(object sender, EventArgs e)
        {
            if (gameEngine != null)
                gameEngine.resize();
        }
    }
}
