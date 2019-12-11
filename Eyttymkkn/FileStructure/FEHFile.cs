using Eyttymkkn.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyttymkkn
{
    public class FEHFile
    {
        public BinaryReader stream;

        public virtual byte[] MSG_XOR_KEY() { 
            return new byte[]{ 0x00 };
        }
        public virtual uint OFFSET()
        {
            return 0;
        }



        public FEHFile(string path)
        {
            if (Path.GetExtension(path).ToLower() == ".lz")
            {
                stream = new BinaryReader(new MemoryStream(DataManager.ReadLZ(path)));
            } else
            {
                stream = new BinaryReader(new FileStream(path, FileMode.Open));
            }
            
        }

        public byte ReadXored8(byte key) {
            return (byte)(stream.ReadByte() ^ key);
        }

        public ushort ReadXored16(ushort key)
        {
            return (ushort)(stream.ReadUInt16() ^ key);
        }

        public uint ReadXored32(uint key)
        {
            return stream.ReadUInt32() ^ key;
        }

        public ulong ReadXored64(ulong key)
        {
            return stream.ReadUInt64() ^ key;
        }

        private byte[] ReadTilNull()
        {
            List<byte> list = new List<byte>();
            byte reading = stream.ReadByte();
            while (reading != 0)
            {
                list.Add(reading);
                reading = stream.ReadByte();
            }
            return list.ToArray();
        }

        public string ReadCryptedString(out ulong str_addr)
        {
            str_addr = stream.ReadUInt64();
            long pos = stream.BaseStream.Position;
            byte[] enc;
            if (str_addr != 0)
            {
                stream.BaseStream.Seek((long)(OFFSET() + str_addr), SeekOrigin.Begin);
                enc = ReadTilNull();
                stream.BaseStream.Seek(pos, SeekOrigin.Begin);
                for (int i = 0; i < enc.Length; i++)
                {
                    if (enc[i] != MSG_XOR_KEY()[i % MSG_XOR_KEY().Length]) enc[i] ^= MSG_XOR_KEY()[i % MSG_XOR_KEY().Length];
                }
                return Encoding.UTF8.GetString(enc);
            }
            return "Null";
        }

        public string ReadCryptedString()
        {
            ulong str_addr = stream.ReadUInt64();
            long pos = stream.BaseStream.Position;
            byte[] enc;
            if (str_addr != 0)
            {
                stream.BaseStream.Seek((long)(OFFSET() + str_addr), SeekOrigin.Begin);
                enc = ReadTilNull();
                stream.BaseStream.Seek(pos, SeekOrigin.Begin);
                for (int i = 0; i < enc.Length; i++)
                {
                    if (enc[i] != MSG_XOR_KEY()[i % MSG_XOR_KEY().Length]) enc[i] ^= MSG_XOR_KEY()[i % MSG_XOR_KEY().Length];
                }
                return Encoding.UTF8.GetString(enc);
            }
            return "Null";
        }

        public string ReadString()
        {
            ulong str_addr = stream.ReadUInt64();
            long pos = stream.BaseStream.Position;
            byte[] enc;
            if (str_addr !=0)
            {
                stream.BaseStream.Seek((long)(OFFSET() + str_addr), SeekOrigin.Begin);
                enc = ReadTilNull();
                stream.BaseStream.Seek(pos, SeekOrigin.Begin);
                return Encoding.UTF8.GetString(enc);
            }
            return "Null";
        }

        public Person.Stats ReadStats()
        {
            Person.Stats stat = new Person.Stats
            {
                hp = ReadXored16(0xD632),
                atk = ReadXored16(0x14A0),
                spd = ReadXored16(0xA55E),
                def = ReadXored16(0x8566),
                res = ReadXored16(0xAEE5)
            };
            stream.BaseStream.Seek(6, System.IO.SeekOrigin.Current);
            return stat;
        }
    }
}
