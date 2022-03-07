using System;
using System.Collections.Generic;
using System.Text;

namespace LHZ.IntArrayCompress.Interface
{
    public interface IArrayCompress<T> : IDisposable
    {
        void Compress(ICollection<T> source);
        T[] DeCompress();
    }
}
