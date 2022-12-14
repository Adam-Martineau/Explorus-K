using Microsoft.VisualBasic;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorus_K.Game.Audio
{
    public class AudioBabillard
    {
        private List<IListener> listeners_;

        private ConcurrentQueue<(string,int)> messageQueue;

        private AudioFileNameContainer audioFileNameContainer;

        public AudioBabillard()
        {
            listeners_ = new List<IListener>();
            messageQueue = new ConcurrentQueue<(string, int)>();
            audioFileNameContainer = AudioFileNameContainer.getInstance();
        }

        public void AddMessage(AudioName audioName)
        {

                string fileName = audioFileNameContainer.getFileName(audioName);
                this.messageQueue.Enqueue((fileName, -1));

                if (this.messageQueue.Count > 0)
                    foreach (IListener l in listeners_)
                        l.Notify();

        }

        public void AddMessage(AudioName audioName, int value)
        {

                string fileName = audioFileNameContainer.getFileName(audioName);
                this.messageQueue.Enqueue((fileName, value));

                if (this.messageQueue.Count > 0)
                    foreach (IListener l in listeners_)
                        l.Notify();

        }

        public void RegisterListener(IListener lst)
        {
            listeners_.Add(lst);
        }

        public bool HasMessages()
        {
            return messageQueue.Count > 0;
        }

        public List<(string, int)> GetMessages()
        {
            List<(string, int)> messages = new List<(string, int)>();

            foreach ((string, int) item in messageQueue)
            {
                messages.Add(item);
            }

            messageQueue = new ConcurrentQueue<(string, int)>();

            return messages;
        }
    }
}
