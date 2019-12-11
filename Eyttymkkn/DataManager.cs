using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;
using Eyttymkkn.FileStructure;
using Eyttymkkn.Objects;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using System.Windows.Media;
using DSDecmp.Formats.Nitro;

namespace Eyttymkkn
{
    static class DataManager
    {
        public static Dictionary<string, string> MESSAGES;
        public static List<Skill> SKILLS;
        public static List<Person> PERSONS;
        private static BitmapSource[] ICON_ATLAS;
        private static BitmapSource STATUS;
        private static string DATAEXT = "*.lz";
        public static readonly string MSG_PATH = System.IO.Directory.GetCurrentDirectory() + @"\TWZH\Message\Data\";
        public static readonly string SKL_PATH = System.IO.Directory.GetCurrentDirectory() + @"\Common\SRPG\Skill\";
        public static readonly string PERSON_PATH = System.IO.Directory.GetCurrentDirectory() + @"\Common\SRPG\Person\";
        public static readonly string UI_PATH = System.IO.Directory.GetCurrentDirectory() + @"\Common\UI\";

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        private static BitmapSource Bitmap2BitmapImage(Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            BitmapSource retval;
            try
            {
                retval = Imaging.CreateBitmapSourceFromHBitmap(
                             hBitmap,
                             IntPtr.Zero,
                             System.Windows.Int32Rect.Empty,
                             BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(hBitmap);
            }

            return retval;
        }
        public static BitmapSource LoadWebpImage(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);
            Bitmap bitmap = Dynamicweb.WebP.Decoder.Decode(bytes);
            return Bitmap2BitmapImage(bitmap);
        }

        private static byte[] LZ11Decompress(byte[] compressed)
        {
            using (MemoryStream cstream = new MemoryStream(compressed))
            {
                using (MemoryStream dstream = new MemoryStream())
                {
                    (new LZ11()).Decompress(cstream, compressed.Length, dstream);
                    return dstream.ToArray();
                }
            }
        }

        public static byte[] ReadLZ(string path)
        {
            byte[] filedata = File.ReadAllBytes(path);
            var xorkey = BitConverter.ToUInt32(filedata, 0) >> 8;
            xorkey *= 0x8083;
            for (int i = 8; i < filedata.Length; i += 0x4)
            {
                BitConverter.GetBytes(BitConverter.ToUInt32(filedata, i) ^ xorkey).CopyTo(filedata, i);
                xorkey ^= BitConverter.ToUInt32(filedata, i);
            }
            filedata = filedata.Skip(4).ToArray();
            return LZ11Decompress(filedata);
        }

        public static void Init()
        {
            InitImage();
            InitMessage();
            InitSkill();
            InitPerson();
        }
        public static void InitMessage()
        {
            string[] messages = Directory.GetFiles(MSG_PATH, DATAEXT);
            MESSAGES = new Dictionary<string, string>();
            foreach(string path in messages)
            {
                MessageFile mf = new MessageFile(path);
                MESSAGES = MESSAGES.Concat(mf.msgs).ToDictionary(x => x.Key, x => x.Value);
            }
        }

        public static string GetMessage(string key)
        {
            if (DataManager.MESSAGES.ContainsKey(key))
            {
                return DataManager.MESSAGES[key];
            }
            else
            {
                return key;
            }
        }

        public static void InitSkill()
        {
            string[] skills = Directory.GetFiles(SKL_PATH, DATAEXT);
            SKILLS = new List<Skill>();
            foreach(string path in skills)
            {
                SkillFile sf = new SkillFile(path);
                SKILLS.AddRange(sf.skills);
            }
        }

        public static Skill FindSkill(string id)
        {
            return SKILLS.Find(skl => skl.id == id);
        }

        public static void InitPerson()
        {
            string[] skills = Directory.GetFiles(PERSON_PATH, DATAEXT);
            PERSONS = new List<Person>();
            foreach (string path in skills)
            {
                PersonFile pf = new PersonFile(path);
                PERSONS.AddRange(pf.persons);
            }
        }

        public static void InitImage()
        {
            ICON_ATLAS = new BitmapSource[6];
            for (int i = 0; i<6; i++)
            {
                ICON_ATLAS[i] = LoadWebpImage(UI_PATH + "Skill_Passive" + (i + 1) + ".png");
            }

            STATUS = LoadWebpImage(UI_PATH + "Status.png");
        }

        public static BitmapSource GetIcon(int id)
        {
            int pic = id / 169;
            if (pic > ICON_ATLAS.Length) { pic = 0; id = 1; }
            int pos = id % 169;
            int row = pos / 13;
            int col = pos - row * 13;
            return new CroppedBitmap(ICON_ATLAS[pic], new System.Windows.Int32Rect(col * 76, row * 76, 76, 76));
        }

        public static BitmapSource GetWeaponIcon(int id)
        {
            return new CroppedBitmap(STATUS, new System.Windows.Int32Rect(56 * id, 0, 56, 56));
        }

        public static BitmapSource GetMoveIcon(int id)
        {
            CroppedBitmap cm =  new CroppedBitmap(STATUS, new System.Windows.Int32Rect(1963, 59 + 54 * id, 54, 54));
            TransformedBitmap tbm = new TransformedBitmap(cm, new RotateTransform(-90));
            return tbm;
       
        }
    }
}
