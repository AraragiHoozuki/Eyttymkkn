using Eyttymkkn.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Eyttymkkn.FileStructure
{
    /// <summary>
    /// MorphidWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MorphidWindow : Window
    {
        public MorphidWindow(Skill sk)
        {
            InitializeComponent();
            string s = "";
            foreach (Person p in sk.Owners)
            {
                s += p.Name + ": " + p.MorphId + "\n";
            }
            TextBox_IdList.Text = s;
            Title = sk.Name + "'s owner units";
        }
    }
}
