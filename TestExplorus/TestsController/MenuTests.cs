using Explorus_K.Controllers;
using Explorus_K.Game;
using Explorus_K.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestExplorus.TestsController
{
    [TestClass]
    public class MenuTests
    {
        private List<MenuOption> menuOptions;

        [TestInitialize]
        public void initializeMenu()
        {
            menuOptions = new List<MenuOption>();
            
            menuOptions.Add(new MenuOption(MenuCursor.START_GAME, new Bitmap[] { Resources.startgame_noir, Resources.startgame_bleu }));
            menuOptions.Add(new MenuOption(MenuCursor.AUDIO, new Bitmap[] { Resources.audio_noir, Resources.audio_bleu }));
            menuOptions.Add(new MenuOption(MenuCursor.DIFFICULTY, new Bitmap[] { Resources.difficulty_noir, Resources.difficulty_bleu }));
            menuOptions.Add(new MenuOption(MenuCursor.EXIT_GAME, new Bitmap[] { Resources.exitgame_noir, Resources.exitgame_bleu }));
            //menuOptions.Add(new MenuOption(MenuCursor.RESUME, new Bitmap[] { Resources.resume_noir, Resources.resume_bleu }));            
        }

        [TestMethod]
        public void startGameTest()
        {
            Binding binding = new Binding(Keys.A, Actions.none);

            Assert.AreEqual(binding.Action, Actions.none);
            Assert.AreEqual(binding.Key, Keys.A);

            binding.Action = Actions.pause;
            binding.Key = Keys.B;

            Assert.AreEqual(binding.Action, Actions.pause);
            Assert.AreEqual(binding.Key, Keys.B);
        }
    }
}
