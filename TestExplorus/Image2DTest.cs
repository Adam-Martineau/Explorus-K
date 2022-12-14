using Explorus_K.Models;
using Explorus_K.Game;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;

namespace TestExplorus
{
    [TestClass]
    public class Image2DTest
    {
        private int someX = 1;
        private int someY = 2;


        [TestMethod]
        public void givenAllElements_whenCallingConstructor_thenImage2DObjectShouldContainsThemAll()
        {
            int expectedX = 4;
            int expectedY = 5;
            SpriteType expectedSpriteType = SpriteType.GEM;
            ImageType expectedImageType = ImageType.WALL;

            Image2D image2D = new Image2D(expectedSpriteType, expectedImageType, expectedX, expectedY);

            Assert.AreEqual(expectedX, image2D.X);
            Assert.AreEqual(expectedY, image2D.Y);
            Assert.AreEqual(expectedSpriteType, image2D.getId());
            Assert.AreEqual(expectedImageType, image2D.getType());

        }

        [TestMethod]
        public void givenValidIamge2DObject_whenGettingImageFromContainer_thenShouldReturnImage()
        {
            Image2D image2D = givenValidImage2D();
            Bitmap bitmapResult = image2D.getBitmapFromContainer();

            Assert.AreEqual(SpriteContainer.getInstance().getBitmapByImageType(image2D.getType()), bitmapResult);
        }

        private Image2D givenValidImage2D()
        {
            return new Image2D().withId(SpriteType.GEM).withId(SpriteType.GEM).withX(someX).withY(someY);
        }
    }
}
