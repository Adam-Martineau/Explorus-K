using Explorus_K.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestExplorus
{
    [TestClass]
    public class SpriteContainerTests
    {
        [TestMethod]
        public void spriteContainerTest()
        {
            SpriteContainer spriteContainer = SpriteContainer.getInstance();
            Assert.IsNotNull(spriteContainer);

            Bitmap image = spriteContainer.getBitmapByImageType(ImageType.KEY);
            Assert.IsNotNull(image);
        }

    }
}
