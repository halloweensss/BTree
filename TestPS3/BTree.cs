using System;

namespace TestPS3
{
    public class BTree<TK, TP> where TK: IComparable<TK>
    {
        public int Degree { get; private set; }
        public int Height { get; private set; }

        public Node<TK, TP> Root { get; private set; }
        
        public BTree(int degree)
        {
            this.Degree = degree;
            this.Height = 1;
            this.Root = new Node<TK, TP>(degree);
        } 
    }
}