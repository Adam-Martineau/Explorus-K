using Explorus_K.Game;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestExplorus
{
    [TestClass]
    public class LabyrinthTests
    {
        [TestMethod]
        public void labyrinthTest()
        {
            Labyrinth labyrinth = new Labyrinth();

            Assert.IsNotNull(labyrinth);
            Assert.IsNotNull(labyrinth.Map);
            Assert.IsNotNull(labyrinth.MapIterator);
            Assert.AreEqual("w", labyrinth.getMapEntryAt(0, 0));
        }
    }
}
