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
    public class KeyStateTests
    {
        [TestMethod]
        public void stateChangeTest()
        {
            Context context = new Context(new NoKeyState());

            Assert.IsNotNull(context);
            Assert.AreEqual(context.CurrentState(), "NoKeyState");

            context.RequestChangingState();

            Assert.AreEqual(context.CurrentState(), "WithKeyState");

            context.TransitionTo(new NoKeyState());

            Assert.AreEqual(context.CurrentState(), "NoKeyState");
        }
    }
}
