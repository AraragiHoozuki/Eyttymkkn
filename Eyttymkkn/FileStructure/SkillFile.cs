using Eyttymkkn.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Eyttymkkn.FileStructure
{
    class SkillFile : FEHFile
    {
        private static readonly byte[] msg_xor_key = {
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

        public List<Skill> skills;

        public SkillFile(string path) : base(path)
        {
            skills = new List<Skill>();
            ReadData();
        }

        public Skill ReadSkill()
        {
            Skill s = new Skill();
            s.id = ReadCryptedString(out s.idpos);
            s.refine_base = ReadCryptedString();
            s.name = ReadCryptedString();
            s.description = ReadCryptedString();
            s.refine_id = ReadCryptedString();
            s.beast_effect_id = ReadCryptedString();
            s.requirements1 = ReadCryptedString();
            s.requirements2 = ReadCryptedString();
            s.next_skill = ReadCryptedString();
            s.sprites = new string[4];
            for (int i = 0; i < 4; i++)
            {
                s.sprites[i] = ReadString();
            }
            s.stats = ReadStats();
            s.dagger_debuff = ReadStats();
            s.skill_params = ReadStats();
            s.refine_stats = ReadStats();
            s.internal_id = ReadXored32(0xC6A53A23);
            s.sort = ReadXored32(0x8DDBF8AC);
            s.icon = ReadXored32(0xC6DF2173);
            s.wep_equip = ReadXored32(0x35B99828);
            s.mov_equip = ReadXored32(0xAB2818EB);
            s.sp_cost = ReadXored32(0xC031F669);
            s.category = (SkillCategory)ReadXored8(0xBC);
            s.tome_class = (Element)ReadXored8(0xF1);
            s.is_exclusive = ReadXored8(0xCC);
            s.enemy_only = ReadXored8(0x4F);
            s.range = ReadXored8(0x56);
            s.might = ReadXored8(0xD2);
            s.cooldown = ReadXored8(0x56);
            s.assist_cd = ReadXored8(0xF2);
            s.healing = ReadXored8(0x95);
            s.skill_range = ReadXored8(0x09);
            s.score = ReadXored16(0xA232);
            s.promotion_tier = ReadXored8(0xE0);
            s.promotion_rarity = ReadXored8(0x75);
            s.is_refined = ReadXored8(0x02);
            s.refine_sort_id = ReadXored8(0xFC);
            s.tokkou_wep = ReadXored32(0x23BE3D43);
            s.tokkou_mov = ReadXored32(0x823FDAEB);
            s.shield_wep = ReadXored32(0xAABAB743);
            s.shield_mov = ReadXored32(0x0EBEF25B);
            s.weak_wep = ReadXored32(0x005A02AF);
            s.weak_mov = ReadXored32(0xB269B819);
            s.adaptive_wep = ReadXored32(0x494E2629);
            s.adaptive_mov = ReadXored32(0xEE6CEF2E);
            s.timing = ReadXored32(0x9C776648);
            s.ability = ReadXored32(0x72B07325);

            s.limit1 = ReadXored32(0x0EBDB832);
            s.limit1_params = new ushort[2];
            s.limit1_params[0] = ReadXored16(0xA590);
            s.limit1_params[1] = ReadXored16(0xA590);
            s.limit2 = ReadXored32(0x0EBDB832);
            s.limit2_params = new ushort[2];
            s.limit2_params[0] = ReadXored16(0xA590);
            s.limit2_params[1] = ReadXored16(0xA590);
            s.target_wep = ReadXored32(0x409FC9D7);
            s.target_mov = ReadXored32(0x6C64D122);
            s.passive_next = ReadCryptedString();
            s.timestamp = ReadXored64(0xEd3F39F93BFE9F51);
            s.random_allowed = ReadXored8(0x10);
            s.min_lv = ReadXored8(0x90);
            s.max_lv = ReadXored8(0x24);
            s.tt_inherit_base = ReadXored8(0x19);
            s.random_mode = ReadXored8(0xBE);

            stream.BaseStream.Seek(11, System.IO.SeekOrigin.Current);

            return s;
        }

        private void ReadData()
        {
            stream.BaseStream.Seek(0x30, System.IO.SeekOrigin.Begin);
            skills.Add(ReadSkill());
            while ((ulong)stream.BaseStream.Position < skills[0].idpos)
            {
                skills.Add(ReadSkill());
            }
        }
    }
}
