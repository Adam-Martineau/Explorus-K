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
            Assert.AreEqual(0, menuOptions.getCurrentIndex());
            oController.gameView.selectMenu();
            Assert.AreEqual(GameState.RESUME, oController.State);

            Thread.Sleep(4000);

            Assert.AreEqual(GameState.PLAY, oController.State);
        }

        [TestMethod]
        public void stopGameTest()
        {
            MenuOptions menuOptions = oController.gameView.getMenuOptions();
            Assert.AreEqual(0, menuOptions.getCurrentIndex());
            oController.gameView.cursorDown();
            Assert.AreEqual(1, menuOptions.getCurrentIndex());
            oController.gameView.cursorDown();
            Assert.AreEqual(2, menuOptions.getCurrentIndex());
            oController.gameView.cursorDown();
            Assert.AreEqual(3, menuOptions.getCurrentIndex());
            Assert.AreEqual(MenuCursor.EXIT_GAME, menuOptions.getCurrentType());
            oController.gameView.selectMenu();
            Assert.AreEqual(GameState.STOP, oController.State);
        }

        [TestMethod]
        public void audioSelectTest()
        {
            MenuOptions menuOptions = oController.gameView.getMenuOptions();
            Assert.AreEqual(0, menuOptions.getCurrentIndex());
            oController.gameView.cursorDown();
            Assert.AreEqual(1, menuOptions.getCurrentIndex());
            oController.gameView.selectMenu();
            menuOptions = oController.gameView.getMenuOptions();
            Assert.AreEqual(MenuCursor.MUSIC_VOLUME, menuOptions.getCurrentType());
        }

        [TestMethod]
        public void returnAudioTest()
        {
            oController.gameView.cursorDown();
            oController.gameView.selectMenu();
            MenuOptions menuOptions = oController.gameView.getMenuOptions();

            Assert.AreEqual(MenuCursor.MUSIC_VOLUME, menuOptions.getCurrentType());
            oController.gameView.cursorDown();
            Assert.AreEqual(1, menuOptions.getCurrentIndex());
            oController.gameView.cursorDown();
            Assert.AreEqual(2, menuOptions.getCurrentIndex());
            Assert.AreEqual(MenuCursor.RETURN, menuOptions.getCurrentType());
            oController.gameView.selectMenu();
            menuOptions = oController.gameView.getMenuOptions();
            Assert.AreEqual(MenuCursor.START_GAME, menuOptions.getCurrentType());
        }

        [TestMethod]
        public void musicVolumeTest()
        {
            oController.gameView.cursorDown();
            oController.gameView.selectMenu();
            MenuOptions menuOptions = oController.gameView.getMenuOptions();

            Assert.AreEqual(MenuCursor.MUSIC_VOLUME, menuOptions.getCurrentType());
            Assert.AreEqual(5, oController.MusicVolume);
            oController.gameView.volumeUp();
            Assert.AreEqual(6, oController.MusicVolume);
            oController.gameView.volumeDown();
            Assert.AreEqual(5, oController.MusicVolume);
        }

        [TestMethod]
        public void musicMuteTest()
        {
            oController.gameView.cursorDown();
            oController.gameView.selectMenu();
            MenuOptions menuOptions = oController.gameView.getMenuOptions();

            Assert.AreEqual(MenuCursor.MUSIC_VOLUME, menuOptions.getCurrentType());
            Assert.AreEqual(5, oController.MusicVolume);
            oController.gameView.volumeUp();
            oController.gameView.volumeUp();
            Assert.AreEqual(7, oController.MusicVolume);
            oController.gameView.mutevolume();
            Assert.IsTrue(oController.MuteMusic);
            oController.gameView.mutevolume();
            Assert.IsFalse(oController.MuteMusic);
            Assert.AreEqual(7, oController.MusicVolume);
        }

        [TestMethod]
        public void soundVolumeTest()
        {
            oController.gameView.cursorDown();
            oController.gameView.selectMenu();
            MenuOptions menuOptions = oController.gameView.getMenuOptions();

            Assert.AreEqual(MenuCursor.MUSIC_VOLUME, menuOptions.getCurrentType());
            oController.gameView.cursorDown();
            Assert.AreEqual(MenuCursor.SOUND_VOLUME, menuOptions.getCurrentType());
            Assert.AreEqual(50, oController.SoundVolume);
            oController.gameView.volumeUp();
            Assert.AreEqual(51, oController.SoundVolume);
            oController.gameView.volumeDown();
            Assert.AreEqual(50, oController.SoundVolume);
        }

        [TestMethod]
        public void soundMuteTest()
        {
            oController.gameView.cursorDown();
            oController.gameView.selectMenu();
            MenuOptions menuOptions = oController.gameView.getMenuOptions();

            Assert.AreEqual(MenuCursor.MUSIC_VOLUME, menuOptions.getCurrentType());
            oController.gameView.cursorDown();
            Assert.AreEqual(MenuCursor.SOUND_VOLUME, menuOptions.getCurrentType());
            Assert.AreEqual(50, oController.SoundVolume);
            oController.gameView.volumeUp();
            oController.gameView.volumeUp();
            Assert.AreEqual(52, oController.SoundVolume);
            oController.gameView.mutevolume();
            Assert.IsTrue(oController.MuteSound);
            oController.gameView.mutevolume();
            Assert.IsFalse(oController.MuteSound);
            Assert.AreEqual(52, oController.SoundVolume);
        }

        [TestMethod]
        public void difficultySelectTest()
        {
            MenuOptions menuOptions = oController.gameView.getMenuOptions();
            Assert.AreEqual(0, menuOptions.getCurrentIndex());
            oController.gameView.cursorDown();
            Assert.AreEqual(1, menuOptions.getCurrentIndex());
            oController.gameView.cursorDown();
            Assert.AreEqual(2, menuOptions.getCurrentIndex());
            Assert.AreEqual(MenuCursor.DIFFICULTY, menuOptions.getCurrentType());
            Assert.AreEqual(Difficulties.NORMAL, oController.GameDifficulty.Difficulty);
            oController.gameView.selectMenu();
            Assert.AreEqual(Difficulties.EXPERT, oController.GameDifficulty.Difficulty);
            oController.gameView.selectMenu();
            Assert.AreEqual(Difficulties.IMPOSSIBLE, oController.GameDifficulty.Difficulty);
            oController.gameView.selectMenu();
            Assert.AreEqual(Difficulties.EASY, oController.GameDifficulty.Difficulty);
            oController.gameView.selectMenu();
            Assert.AreEqual(Difficulties.NORMAL, oController.GameDifficulty.Difficulty);
        }
    }
}
