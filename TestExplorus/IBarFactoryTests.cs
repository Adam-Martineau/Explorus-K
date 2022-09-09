using Explorus_K.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestExplorus
{
    [TestClass]
    public class IBarFactoryTests
    {
        [TestMethod]
        public void healthBarFactoryTest()
        {
            HealthBarCreator healthBarCreator = new HealthBarCreator();
            IBar iBar = healthBarCreator.InitializeBar(1);
            Assert.IsNotNull(iBar);
            Assert.IsInstanceOfType(iBar, typeof(HealthBar));
        }

        [TestMethod]
        public void bubbleBarFactoryTest()
        {
            BubbleBarCreator bubbleBarCreator = new BubbleBarCreator();
            IBar iBar = bubbleBarCreator.InitializeBar(1);
            Assert.IsNotNull(iBar);
            Assert.IsInstanceOfType(iBar, typeof(BubbleBar));
        }

        [TestMethod]
        public void gemBarFactoryTest()
        {
            GemBarCreator gemBarCreator = new GemBarCreator();
            IBar iBar = gemBarCreator.InitializeBar(1);
            Assert.IsNotNull(iBar);
            Assert.IsInstanceOfType(iBar, typeof(GemBar));
        }
    }
}
