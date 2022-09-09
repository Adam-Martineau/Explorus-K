using Explorus_K.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using Binding = Explorus_K.Controllers.Binding;

namespace TestExplorus
{
    [TestClass]
    public class BindingTest
    {
        [TestMethod]
        public void createBindingTest()
        {
            Binding binding = new Binding(Keys.A, Actions.none);

            Assert.AreEqual(binding.Action, Actions.none);
            Assert.AreEqual(binding.Key, Keys.A);

            binding.Action = Actions.pause;
            binding.Key = Keys.B;

            Assert.AreEqual(binding.Action, Actions.pause);
            Assert.AreEqual(binding.Key, Keys.B);
        }
    }
}
