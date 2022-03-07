using LHZ.IntArrayCompress.Interface;
using System;
using System.Collections.Generic;
using System.IO;

namespace LHZ.IntArrayCompress
{
    public class Int32Compress : IArrayCompress<Int32>, IDisposable
    {
        private MemoryStream _stream;
        public void Compress(ICollection<Int32> source)
        {
            if (_stream == null)
            {
                _stream = new MemoryStream();
            }
            else
            {
                _stream.Close();
                _stream.Dispose();

                _stream = new MemoryStream();
            }

            byte[] cache = new byte[512];
            int bitindex = 0;

            foreach (var item in source)
            {
                byte btemp = 0;
                ulong stemp = 0;
                byte bitlength = 4;

                stemp |= (uint)(item & 0xff000000) >> 24;
                if ((stemp & 0xff) != 0)
                {
                    btemp |= 0x8;
                    stemp <<= 8;
                    bitlength += 8;
                }
                stemp |= (uint)(item & 0x00ff0000) >> 16;
                if ((stemp & 0xff) != 0)
                {
                    btemp |= 0x4;
                    stemp <<= 8;
                    bitlength += 8;
                }
                stemp |= (uint)(item & 0x0000ff00) >> 8;
                if ((stemp & 0xff) != 0)
                {
                    btemp |= 0x2;
                    stemp <<= 8;
                    bitlength += 8;
                }
                stemp |= (uint)item & 0x000000ff;
                if ((stemp & 0xff) != 0)
                {
                    btemp |= 0x1;
                    bitlength += 8;
                }
                else
                {
                    stemp >>= 8;
                }
                stemp <<= 64 - bitlength;
                stemp |= (ulong)btemp << 60;

                byte bitlengthtemp = bitlength;
                while (bitlengthtemp > 0)
                {
                    byte curfreelength = (byte)(8 - (bitindex % 8));

                    cache[bitindex / 8] =  (byte)(((uint)(cache[bitindex / 8]>>curfreelength<<curfreelength)) | stemp >> (64 - curfreelength));
                    if (bitlengthtemp > curfreelength)
                    {
                        stemp <<= curfreelength;
                        bitlengthtemp -= curfreelength;
                        bitindex += curfreelength;
                    }
                    else
                    {
                        bitindex += bitlengthtemp;
                        bitlengthtemp = 0;
                    }
                }
                if (bitindex / 8 >= 500)
                {
                    _stream.Write(cache, 0, bitindex / 8);

                    cache[0] = cache[bitindex / 8];
                    cache[1] = cache[bitindex / 8 + 1];
                    cache[2] = cache[bitindex / 8 + 2];
                    cache[3] = cache[bitindex / 8 + 3];
                    cache[4] = cache[bitindex / 8 + 4];

                    bitindex -= 4000;
                }
            }
            _stream.Write(cache, 0, bitindex / 8 + 1);
        }

        public Int32[] DeCompress()
        {
            throw new Exception();
        }

        public void Dispose()
        {
            if (_stream != null)
            {
                _stream.Close();
                _stream.Dispose();
            }
        }
    }
}
