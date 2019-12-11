using Eyttymkkn.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Eyttymkkn
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void Btn_InitData_Click(object sender, RoutedEventArgs e)
        {
            DataManager.Init();
            ListBox_Skill.ItemsSource = DataManager.SKILLS;
            ListBox_Unit.ItemsSource = DataManager.PERSONS;
            ComboBox_PersonSortCriteria.ItemsSource = Enum.GetNames(typeof(SORT_CRITERIA));
        }

        private void RefreshFilter(object sender, RoutedEventArgs e)
        {
            ListBox_Skill.ItemsSource = DataManager.SKILLS;
            ListBox_Unit.ItemsSource = DataManager.PERSONS;
            Filter(sender, e);
            PersonFilter(sender, e);
        }


        private void Filter(object sender, RoutedEventArgs e)
        {
            ListBox_Skill.ItemsSource = DataManager.SKILLS.Where(sk =>
                sk.sp_cost >= (SpCheck.IsChecked == true ? 200 : 0) &&
                (Check_Weapon.IsChecked==true? sk.category == Objects.SkillCategory.武器 : true)&&
                (Check_Spe.IsChecked == true ? sk.category == Objects.SkillCategory.奥义 : true)&&
                (Check_A.IsChecked == true ? sk.category == Objects.SkillCategory.A : true)&&
                (Check_B.IsChecked == true ? sk.category == Objects.SkillCategory.B : true)&&
                (Check_C.IsChecked == true ? sk.category == Objects.SkillCategory.C : true)&&
                (Check_S.IsChecked == true ? sk.category == Objects.SkillCategory.S : true)&&
                (TextBox_SkillName.Text != "" ? sk.Name.Contains(TextBox_SkillName.Text) : true));
        }

        private void PersonFilter(object sender, RoutedEventArgs e)
        {
            ListBox_Unit.ItemsSource = DataManager.PERSONS.Where(p =>
                (TextBox_PersonName.Text != "" ? p.Name.Contains(TextBox_PersonName.Text) : true));
        }

        private void ListBox_Skill_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Skill selected = ListBox_Skill.SelectedItems[0] as Skill;
            ListBox_Unit.ItemsSource = selected.Owners;
            MainTab.SelectedIndex = 0;
        }

        private void ListBox_Unit_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Person selected = ListBox_Unit.SelectedItems[0] as Person;
            Clipboard.SetText(selected.MorphId);
        }

        private void PersonSort(object sender, SelectionChangedEventArgs e)
        {
            int index = ComboBox_PersonSortCriteria.SelectedIndex;
            switch((SORT_CRITERIA)index)
            {
                case SORT_CRITERIA.ATK:
                    ListBox_Unit.ItemsSource = (ListBox_Unit.ItemsSource as IEnumerable<Person>).OrderBy(p => p.Atk).Reverse();
                    break;
                case SORT_CRITERIA.DEF:
                    ListBox_Unit.ItemsSource = (ListBox_Unit.ItemsSource as IEnumerable<Person>).OrderBy(p => p.Def).Reverse();
                    break;
                case SORT_CRITERIA.DEF_RES:
                    ListBox_Unit.ItemsSource = (ListBox_Unit.ItemsSource as IEnumerable<Person>).OrderBy(p => p.Def + p.Res).Reverse();
                    break;
                case SORT_CRITERIA.HP:
                    ListBox_Unit.ItemsSource = (ListBox_Unit.ItemsSource as IEnumerable<Person>).OrderBy(p => p.Hp).Reverse();
                    break;
                case SORT_CRITERIA.HP_DEF:
                    ListBox_Unit.ItemsSource = (ListBox_Unit.ItemsSource as IEnumerable<Person>).OrderBy(p => p.Hp + p.Def).Reverse();
                    break;
                case SORT_CRITERIA.HP_RES:
                    ListBox_Unit.ItemsSource = (ListBox_Unit.ItemsSource as IEnumerable<Person>).OrderBy(p => p.Hp + p.Res).Reverse();
                    break;
                case SORT_CRITERIA.RES:
                    ListBox_Unit.ItemsSource = (ListBox_Unit.ItemsSource as IEnumerable<Person>).OrderBy(p => p.Res).Reverse();
                    break;
                case SORT_CRITERIA.SPD:
                    ListBox_Unit.ItemsSource = (ListBox_Unit.ItemsSource as IEnumerable<Person>).OrderBy(p => p.Spd).Reverse();
                    break;
                case SORT_CRITERIA.TOTAL:
                    ListBox_Unit.ItemsSource = (ListBox_Unit.ItemsSource as IEnumerable<Person>).OrderBy(p => p.Total).Reverse();
                    break;
                default:
                    break;
            }
        }
    }
}
