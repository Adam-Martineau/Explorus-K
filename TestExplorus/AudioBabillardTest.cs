using Explorus_K.Game.Audio;
using Explorus_K.Threads;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;

namespace TestExplorus
{
    [TestClass]
    public class AudioBabillardTest
    {
        private AudioBabillard audioBabillard;
        private AudioFileNameContainer audioFileNameContainer = AudioFileNameContainer.getInstance();

        [TestInitialize]
        public void initializeThread()
        {
            audioBabillard = new AudioBabillard();
        }

        [TestMethod]
        public void givenAudioBabillard_whenAddingMessages_thenShouldContainThem()
        {
            int expectedCount = 2;
            Tuple<string, int> firstMessage = new Tuple<string, int>(audioFileNameContainer.getFileName(AudioName.BOOM), -1);
            Tuple<string, int> secondMessage = new Tuple<string, int>(audioFileNameContainer.getFileName(AudioName.OPEN_DOOR), -1);

            audioBabillard.AddMessage(AudioName.BOOM);
            audioBabillard.AddMessage(AudioName.OPEN_DOOR);
            List<(string, int)> result = audioBabillard.GetMessages();

            Assert.AreEqual(result.Count, expectedCount);
            Assert.AreEqual(result[0].Item1, firstMessage.Item1);
            Assert.AreEqual(result[0].Item2, firstMessage.Item2);
            Assert.AreEqual(result[1].Item1, secondMessage.Item1);
            Assert.AreEqual(result[1].Item2, secondMessage.Item2);
        }

        [TestMethod]
        public void givenAudioBabillard_whenAddingMessagesAndVolume_thenShouldContainThem()
        {
            int expectedCount = 1;
            int expectedVolume = 30;
            Tuple<string, int> firstMessage = new Tuple<string, int>(audioFileNameContainer.getFileName(AudioName.BOOM), expectedVolume);

            audioBabillard.AddMessage(AudioName.BOOM, expectedVolume);
            List<(string, int)> result = audioBabillard.GetMessages();

            Assert.AreEqual(result.Count, expectedCount);
            Assert.AreEqual(result[0].Item1, firstMessage.Item1);
            Assert.AreEqual(result[0].Item2, firstMessage.Item2);
        }
    }
}
