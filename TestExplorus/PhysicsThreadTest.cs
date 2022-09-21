using Explorus_K.Game.Audio;
using Explorus_K.Game;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;
using Explorus_K.Threads;
using Explorus_K.Controllers;
using Explorus_K.Models;

namespace TestExplorus
{
    [TestClass]
    public class PhysicsThreadTest
    {
        PhysicsThread physics;
        Thread physicsThread;
        public static EventWaitHandle physicsWaitHandle;

        [TestInitialize]
        public void InitPhysicsThread()
        {
            GameEngine ge = new GameEngine();
            AudioBabillard audioBabillard = new AudioBabillard();
            physics = new PhysicsThread(ge, audioBabillard);
            physicsWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
            physicsThread = new Thread(new ThreadStart(physics.startThread));
        }

        [TestMethod]
        public void startThreadTest()
        {
            physicsThread.Start();
            Assert.AreEqual(ThreadState.Running, physicsThread.ThreadState);
            physicsThread.Abort();
        }

        [TestMethod]
        public void searchCollisionTest()
        {
            physics.searchForCollisionWithSprite();
        }
    }
}
