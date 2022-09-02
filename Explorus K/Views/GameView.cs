using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Explorus_K.Views
{
    class GameView
    {
        public GameForm oGameForm;
        int i = 0;
        string gameTitle;

        public GameView()
        {
            oGameForm = new GameForm();
            oGameForm.Paint += new PaintEventHandler(this.GameRenderer);
        }

        public void Show() 
        {
            Application.Run(oGameForm); 
        }

        public void Render()
        {
            if (oGameForm.Visible)
                oGameForm.BeginInvoke((MethodInvoker)delegate {
                    oGameForm.Refresh();
                });
        }

        public void Close()
        {
            if (oGameForm.Visible)
                oGameForm.BeginInvoke((MethodInvoker)delegate {
                    oGameForm.Close();
                });
        }

        public void Update(double elapsed)
        {
            i += (int) elapsed;
            double FPS = Math.Round(1000 / elapsed, 1);
            setWindowTitle("Explorus-K - FPS " + FPS.ToString());
        }

        private void GameRenderer(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.Black);
            g.FillRectangle(new SolidBrush(Color.Yellow), i, 20, 20, 20);
            oGameForm.Text = gameTitle;
        }

        public void setWindowTitle(string title)
        {
            gameTitle = title;
        }
    }
}
