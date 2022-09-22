using Explorus_K.Controllers;
using Explorus_K.Game;
using Explorus_K.Game.Audio;
using Explorus_K.Models;
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

        PhysicsThread physicsThreadRef;
        Thread physicsThread;
        static EventWaitHandle physicsWaitHandle;
        LabyrinthImage labyrinthImage;
        Labyrinth labyrinth;
        BubbleManager bubbleManager;


        [TestInitialize]
        public void initializeThread()
        {
            audioBabillard = new AudioBabillard();
            audioThreadRef = new AudioThread(audioBabillard);
            audioThreadStart = new ThreadStart(audioThreadRef.Run);
            audioThread = new Thread(audioThreadStart);

            labyrinth = new Labyrinth();
            bubbleManager = new BubbleManager();
            labyrinthImage = new LabyrinthImage(labyrinth, bubbleManager);
            physicsWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
            physicsThreadRef = new PhysicsThread(labyrinthImage, audioBabillard);
            physicsThread = new Thread(new ThreadStart(physicsThreadRef.startThread));
        }

        [TestCleanup]
        public void killAllThread()
        {
            if (audioThread.IsAlive)
            {
                audioThreadRef.Stop();
                audioThread.Join();
            }
            
            if (physicsThread.IsAlive)
            {
                physicsThreadRef.Stop();
                physicsThread.Join();
            }
            
        }

        /// =====================================
        ///          AUDIO THREAD TESTS
        /// =====================================

        [TestMethod]
        public void givenAudioThreadIsRunning_whenDoingNothing_thenShouldBeRunning()
        {
            audioThread.Start();

            Assert.AreEqual(audioThread.ThreadState, ThreadState.Running);
            Assert.IsTrue(audioThread.IsAlive);
        }

        [TestMethod]
        public void givenAudioThreadIsRunning_whenStoppingThread_thenThreadShouldStop()
        {
            audioThread.Start();

            Thread.Sleep(100);

            audioThreadRef.Stop();
            audioThread.Join();

            Assert.AreEqual(audioThread.ThreadState, ThreadState.Stopped);
            Assert.IsFalse(audioThread.IsAlive);
        }

        [TestMethod]
        public void givenAudioThreadIsRunning_whenConsumerNotify_thenThreadShouldProcessElement()
        {
            audioThread.Start();

            audioBabillard.AddMessage(AudioName.OPEN_DOOR);
            Assert.IsTrue(audioBabillard.HasMessages());

            Thread.Sleep(100);

            Assert.IsFalse(audioBabillard.HasMessages());
        }

        [TestMethod]
        public void givenAudioThreadIsRunning_whenChangingSoundVolume_thenThreadShouldProcessElement()
        {
            audioThread.Start();

            audioBabillard.AddMessage(AudioName.SET_SOUND, 10);
            Assert.IsTrue(audioBabillard.HasMessages());

            Thread.Sleep(100);

            Assert.IsFalse(audioBabillard.HasMessages());
        }

        [TestMethod]
        public void givenAudioThreadIsRunning_whenChangingMusicVolume_thenThreadShouldProcessElement()
        {
            audioThread.Start();

            audioBabillard.AddMessage(AudioName.SET_MUSIC, 20);
            Assert.IsTrue(audioBabillard.HasMessages());

            Thread.Sleep(100);

            Assert.IsFalse(audioBabillard.HasMessages());
        }

        /// =====================================
        ///          PHYSICS THREAD TESTS
        /// =====================================
        /// 

        [TestMethod]
        public void givenPhysicsThreadIsRunning_whenDoingNothing_thenShouldBeRunning()
        {
            physicsThread.Start();
            Assert.AreEqual(physicsThread.ThreadState, ThreadState.Running);
            Assert.IsTrue(physicsThread.IsAlive);
        }

        [TestMethod]
        public void givenPhysicsThreadIsRunning_whenStoppingThread_thenThreadShouldStop()
        {
            physicsThread.Start();

            Thread.Sleep(100);

            physicsThreadRef.Stop();
            physicsThread.Join();

            Assert.AreEqual(physicsThread.ThreadState, ThreadState.Stopped);
            Assert.IsFalse(physicsThread.IsAlive);
        }

        [TestMethod]
        public void givenThreadIsRunning_whenCollidingWithGem_thenThreadShouldIncreaseGemBar()
        {
            labyrinthImage.labyrinthImages.Add(new Image2D(SpriteType.GEM, ImageType.GEM, labyrinthImage.slimus.getPosX(), labyrinthImage.slimus.getPosY()));
            physicsThreadRef.setLabyrinthImage(labyrinthImage);

            physicsThread.Start();

            //physicsThreadRef.Set();

            Thread.Sleep(100);

            Assert.AreEqual(GameState.PLAY, physicsThreadRef.getGameState());
        }
    }
}
