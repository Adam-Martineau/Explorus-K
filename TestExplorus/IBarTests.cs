using Explorus_K.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestExplorus
{
    [TestClass]
    public class IBarTests
    {
        [TestMethod]
        public void healthBarTest()
        {
            HealthBarCreator healthBarCreator = new HealthBarCreator();
            IBar iBar = healthBarCreator.InitializeBar(3, 6);

            Assert.AreEqual(iBar.getLength(), 3);
            Assert.AreEqual(iBar.getCurrent(), 3);

            iBar.Decrease();

            Assert.AreEqual(iBar.getCurrent(), 2);

            iBar.Increase();

            Assert.AreEqual(iBar.getCurrent(), 3);
        }

        [TestMethod]
        public void bubbleBarTest()
        {
            BubbleBarCreator bubbleBarCreator = new BubbleBarCreator();
            IBar iBar = bubbleBarCreator.InitializeBar(3, 6);

            Assert.AreEqual(iBar.getLength(), 3);
            Assert.AreEqual(iBar.getCurrent(), 3);

            iBar.Decrease();

            Assert.AreEqual(iBar.getCurrent(), 2);

            iBar.Increase();

            Assert.AreEqual(iBar.getCurrent(), 3);
        }

        [TestMethod]
        public void gemBarTest()
        {
            GemBarCreator gemBarCreator = new GemBarCreator();
            IBar iBar = gemBarCreator.InitializeBar(3, 6);

            Assert.AreEqual(iBar.getLength(), 3);
            Assert.AreEqual(iBar.getCurrent(), 0);

            iBar.Increase();

            Assert.AreEqual(iBar.getCurrent(), 1);

            iBar.Decrease();

            Assert.AreEqual(iBar.getCurrent(), 0);
        }
    }
}
