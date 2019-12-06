
using System.Collections.Generic;

namespace TestPS3
{
    public class Node<T>
    {
        private int _degree;
        public List<Node<T>> Children { get; set; }
        public List<Entry<T>> Entries { get; set; }

        public Node(int degree)
        {
            this._degree = _degree;
            Children = new List<Node<T>>();
            Entries = new List<Entry<T>>();
        }
    }
}