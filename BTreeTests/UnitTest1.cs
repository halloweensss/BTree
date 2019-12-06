using NUnit.Framework;
using TestPS3;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void CanCreateBTree()
        {
          BTree<int, int> tree = new BTree<int, int>(3);
          Assert.True(tree != null);
          Assert.AreEqual(3, tree.Degree);
          Assert.AreEqual(1, tree.Heigth);
        }
        
        
    }
}