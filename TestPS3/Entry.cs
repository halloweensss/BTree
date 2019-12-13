using System;
using System.Threading;

namespace TestPS3
{
    public class Entry<T> : IEquatable<Entry<T>>
    {
        public T Value { get; set; }

        public bool Equals(Entry<T> other)
        {
            return this.Value.Equals(other.Value);
        }
        
    }
}