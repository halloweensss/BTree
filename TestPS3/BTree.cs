using System;
using System.Linq;

namespace TestPS3
{
    public class BTree<T> where T : IComparable<T>
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

        public Entry<T> Search(T value)
        {
            int i = Root.Entries.TakeWhile(entry => value.CompareTo(entry.Value) > 0).Count();

            if (i < Root.Entries.Count && Root.Entries[i].Value.CompareTo(value) == 0)
            {
                return Root.Entries[i];
            }

            return null;
        }

        public void Delete(T value)
        {
            int i = Root.Entries.TakeWhile(entry => value.CompareTo(entry.Value) > 0).Count();

            if (i < Root.Entries.Count && Root.Entries[i].Value.CompareTo(value) == 0)
            {
                Root.Entries.RemoveAt(i);
            }
        }
    }
}