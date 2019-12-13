using System;

namespace TestPS3
{
    class Program
    {
        static void Main(string[] args)
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
            tree.Delete(40);
            tree.Delete(20);
            tree.Delete(60);
            tree.Delete(30);
            tree.Delete(70);
            tree.Delete(50);
            tree.Delete(80);
            Console.WriteLine(tree.ToString());

        }
    }
}