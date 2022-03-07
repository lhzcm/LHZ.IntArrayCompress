using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LHZ.IntArrayCompress.Test
{
    public class ArrayCompressTest
    {
        [Fact]
        public void CompressTest()
        {
            Int32Compress c = new Int32Compress();
            c.Compress(new int[] { 1, 2, 30444, 32423423, -1 });
        }
    }
}
//12 * 12 * 20 * 36