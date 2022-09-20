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

        private Queue<string> fileNameQueue;

        private AudioFileNameContainer audioFileNameContainer;

        public AudioBabillard()
        {
            listeners_ = new List<IListener>();
            fileNameQueue = new Queue<string>();
            audioFileNameContainer = AudioFileNameContainer.getInstance();
        }

        public void AddMessage(AudioName audioName)
        {
            lock (this)
            {
                string fileName = audioFileNameContainer.getFileName(audioName);
                this.fileNameQueue.Enqueue(fileName);

                if (this.fileNameQueue.Count > 0)
                    foreach (IListener l in listeners_)
                        l.Notify();
            }
        }

        public void RegisterListener(IListener lst)
        {
            listeners_.Add(lst);
        }

        public bool HasMessages()
        {
            return fileNameQueue.Count > 0;
        }

        public List<string> GetMessages()
        {
            List<string> messages = new List<string>();
            lock (this)
            {      
                foreach (string fileName in fileNameQueue)
                {
                    messages.Add(fileName);
                }

                fileNameQueue.Clear();
            }

            return messages;
        }
    }
}
