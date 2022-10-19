using Explorus_K;
using Explorus_K.Controllers;
using Explorus_K.Game;
using Explorus_K.Game.Audio;
using Explorus_K.Models;
using Explorus_K.Threads;
using Explorus_K.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
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
        LabyrinthImage labyrinthImage;
        Labyrinth labyrinth;
        BubbleManager bubbleManager;

        RenderThread render;
        Thread renderThread;


        [TestInitialize]
        public void initializeThread()
        {
            audioBabillard = new AudioBabillard();
            audioThreadRef = new AudioThread(audioBabillard);
            audioThreadStart = new ThreadStart(audioThreadRef.Run);
            audioThread = new Thread(audioThreadStart);

            labyrinth = new Labyrinth();
            bubbleManager = new BubbleManager(0);
            labyrinthImage = new LabyrinthImage(labyrinth, bubbleManager, new GameDifficulty());
            physicsThreadRef = new PhysicsThread(labyrinthImage, audioBabillard, new Explorus_K.Game.Replay.Invoker());
            physicsThread = new Thread(new ThreadStart(physicsThreadRef.startThread));

            render = new RenderThread();
            renderThread = new Thread(new ThreadStart(render.startThread));
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
        [Timeout(2000)]
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
        public void givenPhysicsThreadIsRunning_whenCollidingWithGem_thenThreadShouldIncreaseGemBar()
        {
            labyrinthImage.GemBar.Initialize(6, 0);
            labyrinthImage.labyrinthImages.Add(new Image2D(SpriteType.GEM, ImageType.GEM, labyrinthImage.slimus.getPosX(), labyrinthImage.slimus.getPosY()));
            physicsThreadRef.setLabyrinthImage(labyrinthImage);

            physicsThread.Start();

            Assert.AreEqual(0, physicsThreadRef.getLabyrinthImage().GemBar.getCurrent());

            physicsThreadRef.Notify();

            Thread.Sleep(2000);

            Assert.AreEqual(1, physicsThreadRef.getLabyrinthImage().GemBar.getCurrent());
        }

        [TestMethod]
        public void givenPhysicsThreadIsRunning_whenCollidingWithToxicSlime_thenThreadShouldDecreaseHealthBar()
        {
            labyrinthImage.HealthBar.Initialize(6, 6);
            labyrinthImage.labyrinthImages.Add(new Image2D(SpriteType.TOXIC_SLIME, ImageType.TOXIC_SLIME_DOWN_ANIMATION_1, labyrinthImage.slimus.getPosX(), labyrinthImage.slimus.getPosY()));
            physicsThreadRef.setLabyrinthImage(labyrinthImage);

            physicsThread.Start();

            Assert.AreEqual(6, physicsThreadRef.getLabyrinthImage().HealthBar.getCurrent());

            physicsThreadRef.Notify();

            Thread.Sleep(2000);

            Assert.AreEqual(5, physicsThreadRef.getLabyrinthImage().HealthBar.getCurrent());
        }

        [TestMethod]
        public void givenPhysicsThreadIsRunning_whenBubbleCollidingWithToxicSlime_thenThreadShouldDecreaseToxicLife()
        {
            Player toxic = labyrinthImage.getPlayerList()[labyrinthImage.getPlayerList().Count - 1];
            Bubble bubble = new Bubble(toxic.getPosX(), toxic.getPosY(), ImageType.BUBBLE_BIG, MovementDirection.up, new Point());
            bubbleManager.addBubble(bubble);
            labyrinthImage.labyrinthImages.Add(bubble.refreshBubble());
            physicsThreadRef.setLabyrinthImage(labyrinthImage);

            physicsThread.Start();

            Assert.AreEqual(2, physicsThreadRef.getLabyrinthImage().getPlayerList()[physicsThreadRef.getLabyrinthImage().getPlayerList().Count-1].getLifes());

            physicsThreadRef.Notify();

            Thread.Sleep(2000);

            Assert.AreEqual(1, physicsThreadRef.getLabyrinthImage().getPlayerList()[physicsThreadRef.getLabyrinthImage().getPlayerList().Count - 1].getLifes());
        }

        /// =====================================
        ///          RENDER THREAD TESTS
        /// =====================================
        /// 

        [TestMethod]
        public void testIsRenderThreadRunning()
        {
            renderThread.Start();
            Assert.AreEqual(renderThread.ThreadState, ThreadState.Running);
            Assert.IsTrue(renderThread.IsAlive);
            renderThread.Abort();
        }
    }
}
