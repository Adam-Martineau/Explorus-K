using Explorus_K.Controllers;
using Explorus_K.Views;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Explorus_K.Threads
{
    public class Render
    {
        static readonly object _lockObject = new object();
        private Queue<RenderTask> _renderQueue = new Queue<RenderTask>();

        public void startThread()
        {
            while (true)
            {
                GameEngine.renderWaitHandle.WaitOne();

                RenderTask currentTask = null;

                if (_renderQueue.Count > 0)
                    currentTask = getTask();

                if (currentTask != null)
                    currentTask.execute();
            }
        }

        public void addTask(RenderTask renderTask)
        {
            Monitor.Enter(_lockObject);
            try
            {
                _renderQueue.Enqueue(renderTask);
            }
            finally
            {
                Monitor.Exit(_lockObject);
            }
        }

        public RenderTask getTask()
        {
            Monitor.Enter(_lockObject);
            try
            {
                return _renderQueue.Dequeue();
            }
            finally
            {
                Monitor.Exit(_lockObject);
            }
        }
    }

    public interface RenderTask { void execute(); }

    public class drawImageTask : RenderTask
    {

        private Graphics graphics;
        private Image image;
        private float x;
        private float y;
        private float width;
        private float height;

        public drawImageTask(Graphics graphics, Image image, float x, float y, float width, float height)
        {
            this.graphics = graphics;
            this.image = image;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public void execute()
        {
            graphics.DrawImage(image, x, y, width, height);
        }
    }

    public class drawStringTask : RenderTask
    {
        private Graphics graphics;
        private string s;
        private Font font;
        private Brush brush;
        private RectangleF layoutRectangle;
        private StringFormat format;

        public drawStringTask(Graphics graphics, string s, Font font, Brush brush, RectangleF layoutRectangle, StringFormat format)
        {
            this.graphics = graphics;
            this.s = s;
            this.font = font;
            this.brush = brush;
            this.layoutRectangle = layoutRectangle;
            this.format = format;
        }

        public void execute()
        {
            GameView.gameForm.CreateGraphics().DrawString(s, font, brush, layoutRectangle, format);
        }
    }

    public class fillRectengleTask : RenderTask
    {
        private Graphics graphics;
        private Brush brush;
        private Rectangle rect;

        public fillRectengleTask(Graphics graphics, Brush brush, Rectangle rect)
        {
            this.graphics = graphics;
            this.brush = brush;
            this.rect = rect;
        }

        public void execute()
        {
            graphics.FillRectangle(brush, rect);
        }
    }
    public class clearTask : RenderTask
    {
        private Graphics graphics;
        private Color color;

        public clearTask(Graphics graphics, Color color)
        {
            this.graphics = graphics;
            this.color = color;
        }

        public void execute()
        {
            graphics.Clear(color);
        }
    }
}
