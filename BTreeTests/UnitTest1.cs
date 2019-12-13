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
          BTree<int> tree = new BTree<int>(3);
          Assert.True(tree != null);
          Assert.AreEqual(3, tree.Degree);
          Assert.AreEqual(1, tree.Height);
          Assert.AreEqual(0, tree.Root.Entries.Count);
          Assert.AreEqual(0, tree.Root.Children.Count);
          Assert.NotNull(tree.Root.Entries);
          Assert.NotNull(tree.Root.Children);
          Assert.True(tree.Root != null);
        }

        [Test]
        public void CanInsertValuesIntoBTree()
        {
            BTree<int> tree = new BTree<int>(2);
            tree.Insert(1);
            tree.Insert(2);
            tree.Insert(3);
            tree.Insert(4);
            tree.Insert(5);
            tree.Insert(6);
            tree.Insert(7);
            tree.Insert(8);
            tree.Insert(9);
            tree.Insert(10);
            tree.Insert(11);
            tree.Insert(12);
            tree.Insert(13);
            tree.Insert(14);
            tree.Insert(15);
            tree.Insert(16);
            Assert.AreEqual(3, tree.Height);
        }

        [Test]
        public void CanFindTheValueInTheBTree()
        {
            BTree<int> tree = new BTree<int>(2);
            tree.Insert(1);
            tree.Insert(2);
            tree.Insert(3);
            tree.Insert(4);
            Assert.AreEqual(4, tree.Search(4).Value);
            Assert.AreEqual(1, tree.Search(1).Value);
            Assert.AreEqual(null, tree.Search(5));
        }

        [Test]
        public void CanRemoveValueFromTreePredecessor()
        {
            BTree<int> tree = new BTree<int>(2);
            tree.Insert(10);
            tree.Insert(20);
            tree.Insert(30);
            tree.Insert(40);
            tree.Insert(50);
            tree.Insert(60);
            tree.Insert(70);
            tree.Insert(80);
            tree.Insert(90);
            tree.Insert(100);
            tree.Insert(110);
            tree.Insert(120);
            tree.Insert(130);
            tree.Insert(140);
            tree.Insert(1);
            tree.Insert(2);
            tree.Insert(3);
            tree.Insert(0);
            Assert.AreEqual(40, tree.Search(40).Value);
            tree.Delete(40);
            Assert.AreEqual(null, tree.Search(40));
        }

        [Test]
        public void CanRemoveValueFromTreeSuccessor()
        {
            BTree<int> tree = new BTree<int>(2);
            tree.Insert(10);
            tree.Insert(20);
            tree.Insert(30);
            tree.Insert(40);
            tree.Insert(50);
            tree.Insert(60);
            tree.Insert(70);
            tree.Insert(80);
            tree.Insert(90);
            tree.Insert(100);
            tree.Insert(110);
            tree.Insert(120);
            tree.Insert(130);
            tree.Insert(140);
            tree.Insert(49);
            tree.Insert(48);
            tree.Insert(47);
            Assert.AreEqual(40, tree.Search(40).Value);
            Assert.AreEqual(47, tree.Search(47).Value);
            tree.Delete(40);
            Assert.AreEqual(null, tree.Search(40));
            tree.Delete(47);
            Assert.AreEqual(null, tree.Search(47));
        }
        

        [Test]
        public void CanRemoveValueFromSubtree()
        {
            BTree<int> tree = new BTree<int>(2);
            tree.Insert(10);
            tree.Insert(20);
            tree.Insert(30);
            tree.Insert(40);
            tree.Insert(50);
            tree.Insert(60);
            tree.Insert(70);
            tree.Insert(80);
            tree.Insert(90);
            tree.Insert(100);
            tree.Insert(110);
            tree.Insert(120);
            tree.Insert(130);
            tree.Insert(140);
            tree.Insert(91);
            tree.Insert(92);
            tree.Insert(93);
            tree.Insert(150);
            tree.Insert(160);
            tree.Insert(170);
            tree.Insert(11);
            Assert.AreEqual(140, tree.Search(140).Value);
            Assert.AreEqual(20, tree.Search(20).Value);
            Assert.AreEqual(91, tree.Search(91).Value);
            tree.Delete(140);
            Assert.AreEqual(null, tree.Search(140));
            Assert.AreEqual(40, tree.Search(40).Value);
            Assert.AreEqual(11, tree.Search(11).Value);
            tree.Delete(20);
            Assert.AreEqual(null, tree.Search(20));
            tree.Delete(91);
            Assert.AreEqual(null, tree.Search(91));
            tree.Delete(40);
            Assert.AreEqual(null, tree.Search(40));
            tree.Delete(11);
            Assert.AreEqual(null, tree.Search(11));
        }

        [Test]
        public void ToStringTest()
        {
            BTree<int> tree = new BTree<int>(2);
            tree.Insert(10);
            tree.Insert(20);
            tree.Insert(30);
            tree.Insert(40);
            tree.Insert(50);
            tree.Insert(60);
            tree.Insert(70);
            tree.Insert(80);
            tree.Insert(90);
            tree.Insert(100);
            tree.Insert(110);
            tree.Insert(120);
            tree.Insert(130);
            tree.Insert(140);
            tree.Insert(91);
            tree.Insert(92);
            tree.Insert(93);
            tree.Insert(150);
            tree.Insert(160);
            tree.Insert(170);
            tree.Insert(11);
            string s = tree.ToString();
        }

        [Test]
        public void CanATreeResize()
        {
            BTree<int> tree = new BTree<int>(2);
            Assert.AreEqual(1, tree.Height);
            tree.Insert(10);
            tree.Insert(20);
            tree.Insert(30);
            tree.Insert(40);
            Assert.AreEqual(2, tree.Height);
            tree.Insert(50);
            tree.Insert(60);
            tree.Insert(70);
            tree.Insert(80);
            tree.Insert(90);
            Assert.AreEqual(3, tree.Height);
            tree.Delete(40);
            Assert.AreEqual(2, tree.Height);
            tree.Delete(20);
            tree.Delete(60);
            tree.Delete(30);
            tree.Delete(70);
            tree.Delete(50);
            tree.Delete(80);
            Assert.AreEqual(1, tree.Height);
            Assert.AreEqual(true, tree.Root.IsLeaf);
        }

    }
}