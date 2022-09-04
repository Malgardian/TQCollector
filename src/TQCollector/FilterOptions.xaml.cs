using System;
using System.Windows;

namespace TQCollector
{
    /// <summary>
    /// Interaction logic for FilterOptions.xaml
    /// </summary>
    public partial class FilterOptions : Window
    {
        public FilterOptions()
        {
            InitializeComponent();
            Do_MI_Load();
            Do_Relics_Load();
            Do_Charms_Load();
            Do_Artifacts_Load();
            Do_Sets_Load();
            Do_Uniques_Load();
            Do_Parchments_Load();
            Do_Formulae_Load();
            Do_Misc_Load();
            button2.Content = Files.Language["button01"];
            this.Title = Files.Language["window01"];
        }

        private void MI_Change_Radios(Boolean b)
        {
            MI_Check_Normal.IsEnabled = b;
            MI_Check_Epic.IsEnabled = b;
            MI_Check_Legendary.IsEnabled = b;
        }

        private void Relics_Change_Radios(Boolean b)
        {
            Relics_Check_Ess.IsEnabled = b;
            Relics_Check_Embod.IsEnabled = b;
            Relics_Check_Inc.IsEnabled = b;
        }

        private void Arti_Change_Radios(Boolean b)
        {
            Arti_Check_Lesser.IsEnabled = b;
            Arti_Check_Greater.IsEnabled = b;
            Arti_Check_Divine.IsEnabled = b;
        }

        private void Charms_Change_Radios(Boolean b)
        {
            Charms_Check_Normal.IsEnabled = b;
            Charms_Check_Epic.IsEnabled = b;
            Charms_Check_Legendary.IsEnabled = b;
        }

        private void Sets_Change_Radios(Boolean b)
        {
            Sets_Check_Epic.IsEnabled = b;
            Sets_Check_Legendary.IsEnabled = b;
        }

        private void Formulae_Change_Radios(bool b)
        {
            Formulae_Check_Lesser.IsEnabled = b;
            Formulae_Check_Greater.IsEnabled = b;
            Formulae_Check_Divine.IsEnabled = b;
        }

        private void MI_Radio_None_Checked(object sender, RoutedEventArgs e)
        {
            MI_Change_Radios(false);
            Files.Configuration.Filters.MonsterInfrequent.Amount = Amount.None;
        }

        private void MI_Radio_Some_Checked(object sender, RoutedEventArgs e)
        {
            MI_Change_Radios(true);
            Files.Configuration.Filters.MonsterInfrequent.Amount = Amount.Some;
        }

        private void MI_Radio_Any_Checked(object sender, RoutedEventArgs e)
        {
            MI_Change_Radios(false);
            Files.Configuration.Filters.MonsterInfrequent.Amount = Amount.Any;
        }

        private void MI_Radio_All_Checked(object sender, RoutedEventArgs e)
        {
            MI_Change_Radios(false);
            Files.Configuration.Filters.MonsterInfrequent.Amount = Amount.All;
        }

        private void Relics_Radio_None_Checked(object sender, RoutedEventArgs e)
        {
            Relics_Change_Radios(false);
            Files.Configuration.Filters.Relics.Amount = Amount.None;
        }

        private void Relics_Radio_Some_Checked(object sender, RoutedEventArgs e)
        {
            Relics_Change_Radios(true);
            Files.Configuration.Filters.Relics.Amount = Amount.Some;
        }

        private void Arti_Radio_None_Checked(object sender, RoutedEventArgs e)
        {
            Arti_Change_Radios(false);
            Files.Configuration.Filters.Artifacts.Amount = Amount.None;
        }

        private void Arti_Radio_All_Checked(object sender, RoutedEventArgs e)
        {
            Arti_Change_Radios(false);
            Files.Configuration.Filters.Artifacts.Amount = Amount.All;
        }

        private void Arti_Radio_Some_Checked(object sender, RoutedEventArgs e)
        {
            Arti_Change_Radios(true);
            Files.Configuration.Filters.Artifacts.Amount = Amount.Some;
        }

        private void Charms_Radio_None_Checked(object sender, RoutedEventArgs e)
        {
            Charms_Change_Radios(false);
            Files.Configuration.Filters.Charms.Amount = Amount.None;
        }

        private void Charms_Radio_Some_Checked(object sender, RoutedEventArgs e)
        {
            Charms_Change_Radios(true);
            Files.Configuration.Filters.Charms.Amount = Amount.Some;
        }

        private void Sets_Radio_None_Checked(object sender, RoutedEventArgs e)
        {
            Sets_Change_Radios(false);
            Files.Configuration.Filters.Sets.Amount = Amount.None;
            Files.Configuration.Filters.Sets.Count = false;
        }

        private void Sets_Radio_Count_Checked(object sender, RoutedEventArgs e)
        {
            Sets_Change_Radios(false);
            Files.Configuration.Filters.Sets.Count = true;
            Files.Configuration.Filters.Sets.Amount = Amount.None;
        }

        private void Sets_Radio_All_Checked(object sender, RoutedEventArgs e)
        {
            Sets_Change_Radios(false);
            Files.Configuration.Filters.Sets.Amount = Amount.All;
            Files.Configuration.Filters.Sets.Count = false;
        }

        private void Relics_Radio_All_Checked(object sender, RoutedEventArgs e)
        {
            Relics_Change_Radios(false);
            Files.Configuration.Filters.Relics.Amount = Amount.All;
        }

        private void Sets_Radio_Some_Checked(object sender, RoutedEventArgs e)
        {
            Sets_Change_Radios(true);
            Files.Configuration.Filters.Sets.Amount = Amount.Some;
            Files.Configuration.Filters.Sets.Count = false;
        }

        private void Charms_Radio_All_Checked(object sender, RoutedEventArgs e)
        {
            Charms_Change_Radios(false);
            Files.Configuration.Filters.Charms.Amount = Amount.All;
        }

        private void Do_MI_Load()
        {
            Group_MI.Header = Files.Language["category01"];

            MI_Radio_None.Content = Files.Language["amount01"];
            MI_Radio_All.Content = Files.Language["amount02"];
            MI_Radio_Any.Content = Files.Language["amount03"];
            MI_Radio_Some.Content = Files.Language["amount04"];

            MI_Check_Normal.Content = Files.Language["level04"];
            MI_Check_Epic.Content = Files.Language["level05"];
            MI_Check_Legendary.Content = Files.Language["level06"];

            if (Files.Configuration.Filters.MonsterInfrequent.Amount == Amount.None)
            {
                MI_Radio_None.IsChecked = true;
                MI_Change_Radios(false);
            }
            else if (Files.Configuration.Filters.MonsterInfrequent.Amount == Amount.Some)
            {
                MI_Radio_Some.IsChecked = true;
                MI_Change_Radios(true);
                MI_Check_Normal.IsChecked = Files.Configuration.Filters.MonsterInfrequent.Normal;
                MI_Check_Epic.IsChecked = Files.Configuration.Filters.MonsterInfrequent.Epic;
                MI_Check_Legendary.IsChecked = Files.Configuration.Filters.MonsterInfrequent.Legendary;
            }
            else if (Files.Configuration.Filters.MonsterInfrequent.Amount == Amount.Any)
            {
                MI_Radio_Any.IsChecked = true;
                MI_Change_Radios(false);
            }
            else if (Files.Configuration.Filters.MonsterInfrequent.Amount == Amount.All)
            {
                MI_Radio_All.IsChecked = true;
                MI_Change_Radios(false);
            }
        }

        private void Do_Uniques_Load()
        {
            Group_Uniques.Header = Files.Language["category06"];

            Unique_Radio_None.Content = Files.Language["amount01"];
            Unique_Radio_All.Content = Files.Language["amount02"];
            Unique_Radio_Some.Content = Files.Language["amount04"];

            Unique_Check_Epic.Content = Files.Language["level05"];
            Unique_Check_Legendary.Content = Files.Language["level06"];

            if (Files.Configuration.Filters.Uniques.Amount == Amount.None)
            {
                Unique_Radio_None.IsChecked = true;
                Uniques_Change_Radios(false);
            }
            else if (Files.Configuration.Filters.Uniques.Amount == Amount.Some)
            {
                Unique_Radio_Some.IsChecked = true;
                Uniques_Change_Radios(true);
                Unique_Check_Epic.IsChecked = Files.Configuration.Filters.Uniques.Epic;
                Unique_Check_Legendary.IsChecked = Files.Configuration.Filters.Uniques.Legendary;
            }
            else if (Files.Configuration.Filters.Uniques.Amount == Amount.All)
            {
                Unique_Radio_All.IsChecked = true;
                Uniques_Change_Radios(false);
            }
        }

        private void Uniques_Change_Radios(bool b)
        {
            Unique_Check_Epic.IsEnabled = b;
            Unique_Check_Legendary.IsEnabled = b;
        }

        private void Do_Relics_Load()
        {
            Group_Relics.Header = Files.Language["category02"];

            Relics_Radio_None.Content = Files.Language["amount01"];
            Relics_Radio_All.Content = Files.Language["amount02"];
            Relics_Radio_Some.Content = Files.Language["amount04"];

            Relics_Check_Ess.Content = Files.Language["level07"];
            Relics_Check_Embod.Content = Files.Language["level08"];
            Relics_Check_Inc.Content = Files.Language["level09"];

            if (Files.Configuration.Filters.Relics.Amount == Amount.None)
            {
                Relics_Radio_None.IsChecked = true;
                Relics_Change_Radios(false);
            }
            else if (Files.Configuration.Filters.Relics.Amount == Amount.Some)
            {
                Relics_Radio_Some.IsChecked = true;
                Relics_Change_Radios(true);
                Relics_Check_Ess.IsChecked = Files.Configuration.Filters.Relics.Normal;
                Relics_Check_Embod.IsChecked = Files.Configuration.Filters.Relics.Epic;
                Relics_Check_Inc.IsChecked = Files.Configuration.Filters.Relics.Legendary;
            }
            else if (Files.Configuration.Filters.Relics.Amount == Amount.All)
            {
                Relics_Radio_All.IsChecked = true;
                Relics_Change_Radios(false);
            }
        }

        private void Do_Charms_Load()
        {
            Group_Charms.Header = Files.Language["category03"];

            Charms_Radio_None.Content = Files.Language["amount01"];
            Charms_Radio_All.Content = Files.Language["amount02"];
            Charms_Radio_Some.Content = Files.Language["amount04"];

            Charms_Check_Normal.Content = Files.Language["level04"];
            Charms_Check_Epic.Content = Files.Language["level05"];
            Charms_Check_Legendary.Content = Files.Language["level06"];

            if (Files.Configuration.Filters.Charms.Amount == Amount.None)
            {
                Charms_Radio_None.IsChecked = true;
                Charms_Change_Radios(false);
            }
            else if (Files.Configuration.Filters.Charms.Amount == Amount.Some)
            {
                Charms_Radio_Some.IsChecked = true;
                Charms_Change_Radios(true);
                Charms_Check_Normal.IsChecked = Files.Configuration.Filters.Charms.Normal;
                Charms_Check_Epic.IsChecked = Files.Configuration.Filters.Charms.Epic;
                Charms_Check_Legendary.IsChecked = Files.Configuration.Filters.Charms.Legendary;
            }
            else if (Files.Configuration.Filters.Charms.Amount == Amount.All)
            {
                Charms_Radio_All.IsChecked = true;
                Charms_Change_Radios(false);
            }
        }

        private void Do_Artifacts_Load()
        {
            Group_Artifacts.Header = Files.Language["category04"];

            Arti_Radio_None.Content = Files.Language["amount01"];
            Arti_Radio_All.Content = Files.Language["amount02"];
            Arti_Radio_Some.Content = Files.Language["amount04"];

            Arti_Check_Lesser.Content = Files.Language["level01"];
            Arti_Check_Greater.Content = Files.Language["level02"];
            Arti_Check_Divine.Content = Files.Language["level03"];

            if (Files.Configuration.UseIT || Files.Configuration.UseAE)
            {
                if (Files.Configuration.Filters.Artifacts.Amount == Amount.None)
                {
                    Arti_Radio_None.IsChecked = true;
                    Arti_Change_Radios(false);
                }
                else if (Files.Configuration.Filters.Artifacts.Amount == Amount.Some)
                {
                    Arti_Radio_Some.IsChecked = true;
                    Arti_Change_Radios(true);
                    Arti_Check_Lesser.IsChecked = Files.Configuration.Filters.Artifacts.Normal;
                    Arti_Check_Greater.IsChecked = Files.Configuration.Filters.Artifacts.Epic;
                    Arti_Check_Divine.IsChecked = Files.Configuration.Filters.Artifacts.Legendary;
                }
                else if (Files.Configuration.Filters.Artifacts.Amount == Amount.All)
                {
                    Arti_Radio_All.IsChecked = true;
                    Arti_Change_Radios(false);
                }
            }
            else
            {
                //No IT, no Artifacts
                Arti_Change_Radios(false);
                Arti_Radio_None.IsEnabled = false;
                Arti_Radio_All.IsEnabled = false;
                Arti_Radio_Some.IsEnabled = false;
            }
        }

        private void Do_Sets_Load()
        {
            Group_Sets.Header = Files.Language["category05"];

            Sets_Radio_Count.Content = Files.Language["amount06"];

            Sets_Radio_None.Content = Files.Language["amount01"];
            Sets_Radio_All.Content = Files.Language["amount02"];
            Sets_Radio_Some.Content = Files.Language["amount04"];

            Sets_Check_Epic.Content = Files.Language["level05"];
            Sets_Check_Legendary.Content = Files.Language["level06"];

            if (Files.Configuration.Filters.Sets.Amount == Amount.None)
            {
                if (Files.Configuration.Filters.Sets.Count == true)
                {
                    Sets_Radio_Count.IsChecked = true;
                    Sets_Change_Radios(false);
                }
                else
                {
                    Sets_Radio_None.IsChecked = true;
                    Sets_Change_Radios(false);
                }
            }
            else if (Files.Configuration.Filters.Sets.Amount == Amount.All)
            {
                Sets_Radio_All.IsChecked = true;
                Sets_Change_Radios(false);
            }
            else if (Files.Configuration.Filters.Sets.Amount == Amount.Some)
            {
                Sets_Radio_Some.IsChecked = true;
                Sets_Change_Radios(true);
                Sets_Check_Epic.IsChecked = Files.Configuration.Filters.Sets.Epic;
                Sets_Check_Legendary.IsChecked = Files.Configuration.Filters.Sets.Legendary;
            }

        }

        private void Do_Parchments_Load()
        {
            Group_Parchments.Header = Files.Language["category07"];

            Parchments_Check_All.Content = Files.Language["amount02"];

            if (Files.Configuration.UseIT || Files.Configuration.UseAE)
            {
                if (Files.Configuration.Filters.Parchments.Amount == Amount.None)
                {
                    Parchments_Check_All.IsChecked = false;
                }
                else
                {
                    Parchments_Check_All.IsChecked = true;
                }
            }
            else
            {
                Parchments_Check_All.IsEnabled = false;
            }
        }

        private void Do_Formulae_Load()
        {
            Group_Formulae.Header = Files.Language["category08"];

            Formulae_Radio_None.Content = Files.Language["amount01"];
            Formulae_Radio_All.Content = Files.Language["amount02"];
            Formulae_Radio_Some.Content = Files.Language["amount04"];

            Formulae_Check_Lesser.Content = Files.Language["level01"];
            Formulae_Check_Greater.Content = Files.Language["level02"];
            Formulae_Check_Divine.Content = Files.Language["level03"];

            if (Files.Configuration.UseIT || Files.Configuration.UseAE)
            {
                if (Files.Configuration.Filters.Formulae.Amount == Amount.None)
                {
                    Formulae_Radio_None.IsChecked = true;
                    Formulae_Change_Radios(false);
                }
                else if (Files.Configuration.Filters.Formulae.Amount == Amount.All)
                {
                    Formulae_Radio_All.IsChecked = true;
                    Formulae_Change_Radios(false);
                }
                else
                {
                    Formulae_Radio_Some.IsChecked = true;
                    Formulae_Change_Radios(true);
                }
            }
            else
            {
                Formulae_Change_Radios(false);
                Formulae_Radio_All.IsEnabled = false;
                Formulae_Radio_None.IsEnabled = false;
                Formulae_Radio_Some.IsEnabled = false;
            }
        }

        private void Do_Misc_Load()
        {
            Group_Misc.Header = Files.Language["category09"];

            Misc_Check_Socketed.Content = Files.Language["misc01"];
            Misc_Check_Bold.Content = Files.Language["misc02"];
            Misc_Check_ItemOwned.Content = Files.Language["misc03"];
            Misc_Check_ItemNameCount.Content = Files.Language["misc04"];

            Misc_Check_Bold.IsChecked = Files.Configuration.UseCheckBox;
            Misc_Check_Socketed.IsChecked = Files.Configuration.UseSocketed;
            Misc_Check_ItemOwned.IsChecked = Files.Configuration.UseItemOwned;
            Misc_Check_ItemNameCount.IsChecked = Files.Configuration.UseItemNameCount;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Files.SaveConfig();
            this.Close();
        }

        private void MI_Check_Normal_Checked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.MonsterInfrequent.Normal = true;
        }

        private void MI_Check_Normal_Unchecked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.MonsterInfrequent.Normal = false;
        }

        private void MI_Check_Epic_Checked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.MonsterInfrequent.Epic = true;
        }

        private void MI_Check_Epic_Unchecked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.MonsterInfrequent.Epic = false;
        }

        private void MI_Check_Legendary_Checked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.MonsterInfrequent.Legendary = true;
        }

        private void MI_Check_Legendary_Unchecked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.MonsterInfrequent.Legendary = false;
        }

        private void Arti_Check_Lesser_Checked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Artifacts.Normal = true;
        }

        private void Arti_Check_Lesser_Unchecked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Artifacts.Normal = false;
        }

        private void Arti_Check_Greater_Checked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Artifacts.Epic = true;
        }

        private void Arti_Check_Greater_Unchecked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Artifacts.Epic = false;
        }

        private void Arti_Check_Divine_Checked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Artifacts.Legendary = true;
        }

        private void Arti_Check_Divine_Unchecked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Artifacts.Legendary = false;
        }

        private void Charms_Check_Normal_Checked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Charms.Normal = true;
        }

        private void Charms_Check_Normal_Unchecked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Charms.Normal = false;
        }

        private void Charms_Check_Epic_Checked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Charms.Epic = true;
        }

        private void Charms_Check_Epic_Unchecked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Charms.Epic = false;
        }

        private void Charms_Check_Legendary_Checked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Charms.Legendary = true;
        }

        private void Charms_Check_Legendary_Unchecked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Charms.Legendary = false;
        }

        private void Relics_Check_Ess_Checked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Relics.Normal = true;
        }

        private void Relics_Check_Ess_Unchecked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Relics.Normal = false;
        }

        private void Relics_Check_Embod_Checked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Relics.Epic = true;
        }

        private void Relics_Check_Embod_Unchecked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Relics.Epic = false;
        }

        private void Relics_Check_Inc_Checked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Relics.Legendary = true;
        }

        private void Relics_Check_Inc_Unchecked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Relics.Legendary = false;
        }

        private void Sets_Check_Epic_Checked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Sets.Epic = true;
        }

        private void Sets_Check_Epic_Unchecked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Sets.Epic = false;
        }

        private void Sets_Check_Legendary_Checked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Sets.Legendary = true;
        }

        private void Sets_Check_Legendary_Unchecked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Sets.Legendary = false;
        }

        private void Parchments_Check_All_Checked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Parchments.Amount = Amount.All;
        }

        private void Parchments_Check_All_Unchecked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Parchments.Amount = Amount.None;
        }

        private void Unique_Radio_None_Checked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Uniques.Amount = Amount.None;
            Uniques_Change_Radios(false);
        }

        private void Unique_Radio_All_Checked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Uniques.Amount = Amount.All;
            Uniques_Change_Radios(false);
        }

        private void Unique_Radio_Some_Checked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Uniques.Amount = Amount.Some;
            Uniques_Change_Radios(true);
        }

        private void Unique_Check_Epic_Checked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Uniques.Epic = true;
        }

        private void Unique_Check_Epic_Unchecked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Uniques.Epic = false;
        }

        private void Unique_Check_Legendary_Checked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Uniques.Legendary = true;
        }

        private void Unique_Check_Legendary_Unchecked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Uniques.Legendary = false;
        }

        private void Formulae_Check_Lesser_Checked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Formulae.Normal = true;
        }

        private void Formulae_Check_Lesser_Unchecked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Formulae.Normal = false;
        }

        private void Formulae_Check_Greater_Checked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Formulae.Epic = true;
        }

        private void Formulae_Check_Greater_Unchecked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Formulae.Epic = false;
        }

        private void Formulae_Check_Divine_Checked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Formulae.Legendary = true;
        }

        private void Formulae_Check_Divine_Unchecked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Formulae.Legendary = false;
        }

        private void Formulae_Radio_None_Checked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Formulae.Amount = Amount.None;
            Formulae_Change_Radios(false);
        }

        private void Formulae_Radio_All_Checked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Formulae.Amount = Amount.All;
            Formulae_Change_Radios(false);
        }

        private void Formulae_Radio_Some_Checked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Filters.Formulae.Amount = Amount.Some;
            Formulae_Change_Radios(true);
        }

        private void Misc_Check_Bold_Checked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.UseCheckBox = true;
        }

        private void Misc_Check_Bold_Unchecked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.UseCheckBox = false;
        }

        private void Misc_Check_Socketed_Checked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.UseSocketed = true;
        }

        private void Misc_Check_Socketed_Unchecked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.UseSocketed = false;
        }
        private void Misc_Check_ItemOwned_Checked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.UseItemOwned = true;
        }

        private void Misc_Check_ItemOwned_Unchecked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.UseItemOwned = false;
        }

        private void Misc_Check_ItemNameCount_Checked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.UseItemNameCount = true;
        }

        private void Misc_Check_ItemNameCount_Unchecked(object sender, RoutedEventArgs e)
        {
            Files.Configuration.UseItemNameCount = false;
        }
    }
}
