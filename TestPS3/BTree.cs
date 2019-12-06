using System;

namespace TestPS3
{
    public class BTree<TK, TP> where TK: IComparable<TK>
    {
        public int Degree { get; private set; }
        public int Heigth { get; private set; }
        
        public BTree(int degree)
        {
            this.Degree = degree;
            this.Heigth = 1;
        } 
    }
}