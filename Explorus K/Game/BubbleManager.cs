using Explorus_K.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorus_K.Game
{
    public class BubbleManager
    {
        private List<Bubble> bubbles;

        public BubbleManager()
        {
            bubbles = new List<Bubble>();
        }

        public void addBubble(Bubble bubble)
        {
            bubbles.Add(bubble);
        }

        public void removeBubble(Bubble bubble)
        {
            bubbles.Remove(bubble); 
        }

        public List<Bubble> getBubbleList()
        {
            return bubbles;
        }
    }
}
