using System;

namespace TestPS3
{
    public class BTree<T>
    {
        public int Degree { get; private set; }
        public int Height { get; private set; }

        public Node<T> Root { get; private set; }
        
        public BTree(int degree)
        {
            this.Degree = degree;
            this.Height = 1;
            this.Root = new Node<T>(degree);
        }

        public void Insert(T value)
        {
            Root.Entries.Add(new Entry<T>() {Value = value});
        }
    }
}