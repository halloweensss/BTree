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
            if (!Root.HasReachedMaxEntries)
            {
                this.InsertNonFull(this.Root, value);
                return;
            }

            Node<T> oldRoot = this.Root;
            this.Root = new Node<T>(this.Degree);
            this.Root.Children.Add(oldRoot);
            this.InsertNonFull(this.Root, value);
            this.Height++;
        }

        private void InsertNonFull(Node<T> node, T value)
        {
            int positionToInsert = node.Entries.TakeWhile(entry => value.CompareTo(entry.Value) >= 0).Count();

            node.Entries.Insert(positionToInsert, new Entry<T>() {Value = value});

        }

        public Entry<T> Search(T value)
        {
            return SearchInternal(this.Root, value);
        }

        private Entry<T> SearchInternal(Node<T> node, T value)
        {
            int i = node.Entries.TakeWhile(entry => value.CompareTo(entry.Value) > 0).Count();
            
            if (i < node.Entries.Count && node.Entries[i].Value.CompareTo(value) == 0)
            {
                return node.Entries[i];
            }

            return node.IsLeaf ? null : this.SearchInternal(node.Children[0], value);
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