using Explorus_K.Game.Audio;
using Explorus_K.Threads;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestExplorus
{
    [TestClass]
    public class ThreadsTest
    {
        AudioBabillard audioBabillard;
        AudioThread audioThreadRef;
        ThreadStart audioThreadStart;
        Thread audioThread;


        /// =====================================
        ///          AUDIO THREAD TESTS
        /// =====================================

        [TestInitialize]
        public void initializeThread()
        {
             audioBabillard = new AudioBabillard();
             audioThreadRef = new AudioThread(audioBabillard);
             audioThreadStart = new ThreadStart(audioThreadRef.Run);
             audioThread = new Thread(audioThreadStart);
        }

        [TestCleanup]
        public void killAllThread()
        {
            audioThreadRef.Stop();
            audioThread.Join();
        }

        [TestMethod]
        public void givenThreadIsRunning_whenDoingNothing_thenShouldBeRunning()
        {
            audioThread.Start();

            Assert.AreEqual(audioThread.ThreadState, ThreadState.Running);
            Assert.IsTrue(audioThread.IsAlive);
        }

        [TestMethod]
        public void givenThreadIsRunning_whenStoppingThread_thenThreadShouldStop()
        {
            audioThread.Start();

            Thread.Sleep(100);

            audioThreadRef.Stop();
            audioThread.Join();

            Assert.AreEqual(audioThread.ThreadState, ThreadState.Stopped);
            Assert.IsFalse(audioThread.IsAlive);
        }

        [TestMethod]
        public void givenThreadIsRunning_whenConsumerNotify_thenThreadShouldProcessElement()
        {
            audioThread.Start();

            audioBabillard.AddMessage(AudioName.OPEN_DOOR);
            Assert.IsTrue(audioBabillard.HasMessages());

            Thread.Sleep(100);

            Assert.IsFalse(audioBabillard.HasMessages());
        }

        [TestMethod]
        public void givenThreadIsRunning_whenChangingSoundVolume_thenThreadShouldProcessElement()
        {
            audioThread.Start();

            audioBabillard.AddMessage(AudioName.SET_SOUND, 10);
            Assert.IsTrue(audioBabillard.HasMessages());

            Thread.Sleep(100);

            Assert.IsFalse(audioBabillard.HasMessages());
        }

        [TestMethod]
        public void givenThreadIsRunning_whenChangingMusicVolume_thenThreadShouldProcessElement()
        {
            audioThread.Start();

            audioBabillard.AddMessage(AudioName.SET_MUSIC, 20);
            Assert.IsTrue(audioBabillard.HasMessages());

            Thread.Sleep(100);

            Assert.IsFalse(audioBabillard.HasMessages());
        }


    }
}
