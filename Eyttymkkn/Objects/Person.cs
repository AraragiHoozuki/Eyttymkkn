using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Text;
using System.Threading.Tasks;

namespace Eyttymkkn.Objects
{
    public class Person
    {
        public class Stats
        {
            public ushort hp;
            public ushort atk;
            public ushort spd;
            public ushort def;
            public ushort res;

            public int this[int index]
            {
                get
                {
                    int value;
                    switch (index)
                    {
                        case 0:
                            value = hp;
                            break;
                        case 1:
                            value = atk;
                            break;
                        case 2:
                            value = spd;
                            break;
                        case 3:
                            value = def;
                            break;
                        case 4:
                            value = res;
                            break;
                        default:
                            value = 0;
                            break;
                    }
                    return value;
                }
            }
        }

        public string id;
        public string roman;
        public string face;
        public string face2;
        public Int64 legendary_bonus;
        public ulong timestamp;
        public uint internal_id;
        public uint sort;
        public WeaponType weapon_type;
        public Element tome_class;
        public Move move;
        public byte origin;
        public byte summonable;
        public byte permanent;
        public byte base_vector;
        public byte is_refresher;
        public byte unknown;
        //7bytes offset
        public Stats stats;
        public Stats grow;
        public string[] skills;
        
        public ulong idpos;
        public byte[] morphid;
        public List<Skill> skill_objs;

        public string MorphId
        {
            get
            {
                return BitConverter.ToString(morphid).Replace("-", string.Empty);
            }
        }

        public string Name
        {
            get => DataManager.GetMessage("M" + id);

        }

        public int Stat(int index, int hone = 0)
        {
            int value = grow[index] + 5 * hone;
            value = value * 114 / 100;
            value = value * 39 / 100;
            value = value + stats[index] + 1 + hone;
            return value;
        }

        public int Hp => Stat(0);
        public int HpAsset => Stat(0, 1);
        public int HpFlaw => Stat(0, -1);
        public int Atk => Stat(1);
        public int AtkAsset => Stat(1, 1);
        public int AtkFlaw => Stat(1, -1);
        public int Spd => Stat(2);
        public int SpdAsset => Stat(2, 1);
        public int SpdFlaw => Stat(2, -1);
        public int Def => Stat(3);
        public int DefAsset => Stat(3, 1);
        public int DefFlaw => Stat(3, -1);
        public int Res => Stat(4);
        public int ResAsset => Stat(4, 1);
        public int ResFlaw => Stat(4, -1);
        public int Total => Hp + Atk + Spd + Def + Res;
        public Skill[] Skills
        {
            get
            {
                if (skill_objs != null) return skill_objs.ToArray();
                skill_objs = new List<Skill>();
                foreach (string s in skills)
                {
                    Skill skl = DataManager.FindSkill(s);
                    skill_objs.Add(skl);
                }

                skill_objs.RemoveAll(s => skills.Contains(s.next_skill) || skills.Contains(s.passive_next) || s.category == SkillCategory.奥义 || s.category == SkillCategory.武器 || s.category == SkillCategory.支援);

                return skill_objs.ToArray();
            }
        }

        public BitmapSource[] SkillIcons
        {
            get
            {
                return Skills.Select(s => DataManager.GetIcon((int)s.icon)).ToArray();
            }
        }

        public BitmapSource WeaponIcon => DataManager.GetWeaponIcon((int)weapon_type);
        public BitmapSource MoveIcon => DataManager.GetMoveIcon((int)move);
    }
}
