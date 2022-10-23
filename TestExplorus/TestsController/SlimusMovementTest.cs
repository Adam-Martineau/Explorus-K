using Explorus_K;
using Explorus_K.Controllers;
using Explorus_K.Game;
using Explorus_K.Models;
using Explorus_K.Threads;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestExplorus.TestsController
{
    [TestClass]
    public class SlimusMovementTest
    {
        private GameEngine oController;
        private Thread oGameLoopThread;

        private object[,] testMap =new string[,]{
            { ".", ".", "."},
            { ".", "s" ,"."},
            { ".", ".", "."}
        };

        [TestInitialize]
        public void testInit()
        {
            oController = new GameEngine(testMap);
            Assert.IsNotNull(oController);
            oController.IsWindowLess = true;

            oGameLoopThread = new Thread(oController.GameLoop);
            oGameLoopThread.Start();

            Assert.IsTrue(oGameLoopThread.IsAlive);
        }

        [TestMethod]
        public void givenLeftArrowEvent_whenRunningGameLoop_thenSlimusShoudMoveLeft()
        {
            startGame();
            Slimus slimus = (Slimus)oController.gameView.getSlimusObject();
            int initialPosX = slimus.getPosX();
            int initialPosY = slimus.getPosY();

            oController.gameView.ReceiveKeyEvent(new KeyEventArgs(Keys.Left));
            Thread.Sleep(1000); //Waiting for the move to finish

            Assert.AreEqual(initialPosY, slimus.getPosY());
            Assert.IsTrue(initialPosX > slimus.getPosX());
        }

        [TestMethod]
        public void givenRightArrowEvent_whenRunningGameLoop_thenSlimusShoudMoveRight()
        {
            startGame();
            Slimus slimus = (Slimus)oController.gameView.getSlimusObject();
            int initialPosX = slimus.getPosX();
            int initialPosY = slimus.getPosY();

            oController.gameView.ReceiveKeyEvent(new KeyEventArgs(Keys.Right));
            Thread.Sleep(1000); //Waiting for the move to finish

            Assert.AreEqual(initialPosY, slimus.getPosY());
            Assert.IsTrue(initialPosX < slimus.getPosX());
        }

        [TestMethod]
        public void givenDownArrowEvent_whenRunningGameLoop_thenSlimusShoudMoveDown()
        {
            startGame();
            Slimus slimus = (Slimus)oController.gameView.getSlimusObject();
            int initialPosX = slimus.getPosX();
            int initialPosY = slimus.getPosY();

            oController.gameView.ReceiveKeyEvent(new KeyEventArgs(Keys.Down));
            Thread.Sleep(1000); //Waiting for the move to finish

            Assert.AreEqual(initialPosX, slimus.getPosX());
            Assert.IsTrue(initialPosY < slimus.getPosY());
        }

        [TestMethod]
        public void givenUpArrowEvent_whenRunningGameLoop_thenSlimusShoudMoveUp()
        {
            startGame();
            Slimus slimus = (Slimus)oController.gameView.getSlimusObject();
            int initialPosX = slimus.getPosX();
            int initialPosY = slimus.getPosY();

            oController.gameView.ReceiveKeyEvent(new KeyEventArgs(Keys.Up));
            Thread.Sleep(1000); //Waiting for the move to finish

            Assert.AreEqual(initialPosX, slimus.getPosX());
            Assert.IsTrue(initialPosY > slimus.getPosY());
        }

        [TestCleanup]
        public void killAllThread()
        {
            if (oGameLoopThread.IsAlive)
            {
                oGameLoopThread.Abort();
                oGameLoopThread.Join();
            }
        }

        private void startGame()
        {
            MenuOptions menuOptions = oController.gameView.getMenuOptions();
            Assert.AreEqual(0, menuOptions.getCurrentIndex());
            oController.gameView.selectMenu();
            Assert.AreEqual(GameState.RESUME, oController.State);

            Thread.Sleep(4000);

            Assert.AreEqual(GameState.PLAY, oController.State);
        }

    }
}
