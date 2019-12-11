using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Eyttymkkn.Objects
{
    public class Skill
    {
        public string id;
        public string refine_base;
        public string name;
        public string description;
        public string refine_id;
        public string beast_effect_id;
        public string requirements1;
        public string requirements2;
        public string next_skill;
        public string[] sprites;
        public Person.Stats stats;
        public Person.Stats dagger_debuff;
        public Person.Stats skill_params;
        public Person.Stats refine_stats;
        public uint internal_id;
        public uint sort;
        public uint icon;
        public uint wep_equip;
        public uint mov_equip;
        public uint sp_cost;
        public SkillCategory category;
        public Element tome_class;
        public byte is_exclusive;
        public byte enemy_only;
        public byte range;
        public byte might;
        public byte cooldown;
        public byte assist_cd;
        public byte healing;
        public byte skill_range;
        public UInt16 score;
        public byte promotion_tier;
        public byte promotion_rarity;
        public byte is_refined;
        public byte refine_sort_id;
        public uint tokkou_wep;
        public uint tokkou_mov;
        public uint shield_wep;
        public uint shield_mov;
        public uint weak_wep;
        public uint weak_mov;
        public uint adaptive_wep;
        public uint adaptive_mov;
        public uint timing;
        public uint ability;
        public uint limit1;
        public ushort[] limit1_params;
        public uint limit2;
        public ushort[] limit2_params;
        public uint target_wep;
        public uint target_mov;
        public string passive_next;
        public ulong timestamp;
        public byte random_allowed;
        public byte min_lv;
        public byte max_lv;
        public byte tt_inherit_base;
        public byte random_mode;

        public ulong idpos;

        public string Name {
            get => DataManager.GetMessage(name);

        }

        public string Description
        {
            get  => DataManager.GetMessage(description);
        }

        public string DetailInfo
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("| 位置: ");
                sb.Append(category.ToString());
                sb.Append(" | ");
                if (category == SkillCategory.武器) sb.Append("攻击: " + might + " | ");
                if (category == SkillCategory.奥义) sb.Append("CD: " + cooldown + " | ");
                return sb.ToString();
            }
        }

        public BitmapSource Icon
        {
            get => DataManager.GetIcon((int)icon); 

        }

        public List<Person> Owners
        {
            get
            {
                return DataManager.PERSONS.FindAll(p => p.skills.Contains(id));
            }
        }
    }
}
