
using System.Collections.Generic;

namespace TestPS3
{
    public class Node<T>
    {
        private int _degree;
        public List<Node<T>> Children { get; set; }
        public List<Entry<T>> Entries { get; set; }
        
        public bool HasReachedMaxEntries => this.Entries.Count == (2 * this._degree) - 1;

        public Node(int degree)
        {
            this._degree = degree;
            Children = new List<Node<T>>(degree);
            Entries = new List<Entry<T>>(degree);
        }
    }
}