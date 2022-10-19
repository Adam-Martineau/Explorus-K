using Explorus_K.Controllers;
using Explorus_K.Game;
using Explorus_K.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestExplorus.TestsController
{
    [TestClass]
    public class MenuTests
    {
        private GameEngine oController;
        private Thread oGameLoopThread;

        [TestInitialize]
        public void MenuTestInit()
        {
            oController = new GameEngine();
            Assert.IsNotNull(oController);
            oController.IsWindowLess = true;

            oGameLoopThread = new Thread(oController.GameLoop);
            oGameLoopThread.Start();

            Assert.IsTrue(oGameLoopThread.IsAlive);
        }

        [TestMethod]
        public void startGameTest()
        {
            MenuOptions menuOptions = oController.gameView.getMenuOptions();
            Assert.AreEqual(menuOptions.getCurrentIndex(), 0);
            oController.gameView.selectMenu();
            Assert.AreEqual(oController.State, GameState.RESUME);

            Thread.Sleep(4000);

            Assert.AreEqual(oController.State, GameState.PLAY);
        }

        [TestMethod]
        public void stopGameTest()
        {
            MenuOptions menuOptions = oController.gameView.getMenuOptions();
            Assert.AreEqual(menuOptions.getCurrentIndex(), 0);
            oController.gameView.cursorDown();
            Assert.AreEqual(menuOptions.getCurrentIndex(), 1);
            oController.gameView.cursorDown();
            Assert.AreEqual(menuOptions.getCurrentIndex(), 2);
            oController.gameView.cursorDown();
            Assert.AreEqual(menuOptions.getCurrentIndex(), 3);
            Assert.AreEqual(menuOptions.getCurrentType(), MenuCursor.EXIT_GAME);
            oController.gameView.selectMenu();
            Assert.AreEqual(oController.State, GameState.STOP);
        }
    }
}
