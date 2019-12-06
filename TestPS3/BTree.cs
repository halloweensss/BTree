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

            if (node.IsLeaf)
            {
                node.Entries.Insert(positionToInsert, new Entry<T>() {Value = value});
                return;
            }

            Node<T> child = node.Children[positionToInsert];
            if (child.HasReachedMaxEntries)
            {
                this.SplitChild(node, positionToInsert, child);
                if (value.CompareTo(node.Entries[positionToInsert].Value) > 0)
                {
                    positionToInsert++;
                }
            }
            
            this.InsertNonFull(node.Children[positionToInsert], value);

        }

        private void SplitChild(Node<T> parentNode, int nodeToBeSplitIndex, Node<T> nodeToBeSplit)
        {
            Node<T> newNode = new Node<T>(this.Degree);
            
            parentNode.Entries.Insert(nodeToBeSplitIndex, nodeToBeSplit.Entries[this.Degree - 1]);
            parentNode.Children.Insert(nodeToBeSplitIndex + 1, newNode);
            
            newNode.Entries.AddRange(nodeToBeSplit.Entries.GetRange(this.Degree, this.Degree - 1));
            
            nodeToBeSplit.Entries.RemoveRange(this.Degree - 1, this.Degree);

            if (!nodeToBeSplit.IsLeaf)
            {
                newNode.Children.AddRange(nodeToBeSplit.Children.GetRange(this.Degree, this.Degree));
                nodeToBeSplit.Children.RemoveRange(this.Degree, this.Degree);
            }
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

            return node.IsLeaf ? null : this.SearchInternal(node.Children[i], value);
        }

        public void Delete(T value)
        {
            DeleteInternal(this.Root, value);

            if (this.Root.Entries.Count == 0 && !this.Root.IsLeaf)
            {
                this.Root = this.Root.Children.Single();
                this.Height--;
            }
        }

        private void DeleteInternal(Node<T> node, T value)
        {
            int i = node.Entries.TakeWhile(entry => value.CompareTo(entry.Value) > 0).Count();

            if (i < node.Entries.Count && node.Entries[i].Value.CompareTo(value) == 0)
            {
                node.Entries.RemoveAt(i);
                return;
            }

            if (!node.IsLeaf)
            {
                this.DeleteInternal(node.Children[i], value);
            }
        }
    }
}