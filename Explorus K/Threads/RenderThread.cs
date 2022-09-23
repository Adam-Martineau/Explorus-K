using Explorus_K.Controllers;
using Explorus_K.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Explorus_K.Threads
{
    internal class RenderThread
    {
        public void startThread()
        {
            while (true)
            {
                GameView.renderWaitHandle.WaitOne();

                if (GameView.gameForm != null)
                    if (GameView.gameForm.Visible)
                        GameView.gameForm.BeginInvoke((MethodInvoker)delegate
                        {
                            GameView.gameForm.Refresh();
                        });
            }
        }
    }
}
