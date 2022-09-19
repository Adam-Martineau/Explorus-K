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

        private Queue<Message> messages_;

        public AudioBabillard()
        {
            listeners_ = new List<IListener>();
            messages_ = new Queue<Message>();
        }

        public void AddMessage(Message msg)
        {
            lock (this)
            {
                messages_.Enqueue(msg);

                if (messages_.Count > 5)
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
            return messages_.Count > 0;
        }

        public List<Message> GetMessages()
        {
            List<Message> messages = new List<Message>();
            lock (this)
            {      
                foreach (Message msg in messages_)
                {
                    messages.Add(new Message(msg));
                }

                messages_.Clear();
            }

            return messages;
        }
    }
}
