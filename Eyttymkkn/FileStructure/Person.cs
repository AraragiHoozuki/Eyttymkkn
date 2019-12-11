using Eyttymkkn.Objects;
using System.Collections.Generic;
using System.Windows;

namespace Eyttymkkn.FileStructure
{
    class PersonFile : FEHFile
    {
        private static byte[] msg_xor_key = {
            0x81, 0x00, 0x80, 0xA4, 0x5A, 0x16, 0x6F, 0x78,
            0x57, 0x81, 0x2D, 0xF7, 0xFC, 0x66, 0x0F, 0x27,
            0x75, 0x35, 0xB4, 0x34, 0x10, 0xEE, 0xA2, 0xDB,
            0xCC, 0xE3, 0x35, 0x99, 0x43, 0x48, 0xD2, 0xBB,
            0x93, 0xC1
        };
        public override byte[] MSG_XOR_KEY()
        {
            return msg_xor_key;
        }
        public override uint OFFSET()
        {
            return 0x20;
        }
        public List<Person> persons;

        public PersonFile(string path) : base(path) {
            persons = new List<Person>();
            ReadData();
        }
        

        

        public string[] ReadSkills()
        {
            List<string> skills = new List<string>();
            string sid;
            for (int i = 0; i < 70; i++)
            {
                
                sid = ReadCryptedString();
                if (sid != "Null")
                {
                    skills.Add(sid);
                }
            }
            return skills.ToArray();
        }

        public Person ReadPerson()
        {
            Person p = new Person();
            p.id = ReadCryptedString(out p.idpos);
            p.roman = ReadCryptedString();
            p.face = ReadCryptedString();
            p.face2 = ReadCryptedString();
            p.legendary_bonus = stream.ReadInt64();

            long pos = stream.BaseStream.Position;
            p.morphid = stream.ReadBytes(16);
            stream.BaseStream.Seek(pos, System.IO.SeekOrigin.Begin);
            
            p.timestamp = ReadXored64(0xBDC1E742E9B6489B);
            p.internal_id = ReadXored32(0x5F6E4E18);
            p.sort = ReadXored32(0x2A80349B);
            p.weapon_type = (WeaponType)ReadXored8(0x06);
            p.tome_class = (Element)ReadXored8(0x35);
            p.move = (Move)ReadXored8(0x2A);
            p.origin = ReadXored8(0x43);
            p.summonable = ReadXored8(0xA1);
            p.permanent = ReadXored8(0xC7);
            p.base_vector = ReadXored8(0x3D);
            p.is_refresher = ReadXored8(0x3D);
            p.unknown = ReadXored8(0x00);
            stream.BaseStream.Seek(7, System.IO.SeekOrigin.Current);
            p.stats = ReadStats();
            p.grow = ReadStats();
            p.skills = ReadSkills();
            return p;
        }

        private void ReadData()
        {
            stream.BaseStream.Seek(0x28, System.IO.SeekOrigin.Begin);
            ulong num = ReadXored64(0xDE51AB793C3AB9E1);
            while (num > 0)
            {
                persons.Add(ReadPerson());
                num--;
            }
        }
    }

    
}
