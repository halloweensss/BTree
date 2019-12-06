
using System.Collections.Generic;

namespace TestPS3
{
    public class Node<TK, TP>
    {
        private int _degree;
        public List<Node<TK, TP>> Children { get; set; }
        public List<Entry<TK, TP>> Entries { get; set; }

        public Node(int degree)
        {
            this._degree = _degree;
            Children = new List<Node<TK, TP>>();
            Entries = new List<Entry<TK, TP>>();
        }
    }
}