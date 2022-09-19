using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorus_K.Game.Audio
{
    public class Message
    {
        public Message()
        {
            this.Author = "";
            this.Content = "";
            this.Date = DateTime.Now;
        }
        public Message(Message msg)
        {
            this.Author = msg.Author;
            this.Content = msg.Content;
            this.Date = msg.Date;

        }

        public String   Author  { get; set; }
        public String   Content { get; set; }
        public DateTime Date    { get; set; }

    }
}
