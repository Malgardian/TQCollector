using System.Windows;
using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Threading;

namespace TQCollector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        //To stop Files.reloadFiles() and refreshDisplay() being called when initially setting the toggles.
        private bool loaded = false;

        private int refreshTimerSeconds = 0;

        SynchronizationContext uiContext = SynchronizationContext.Current;
        System.Timers.Timer refreshTimer;

        public MainWindow()
        {
            InitializeComponent();

            //Load config.xml, itemdb.xml, vaults, characters...
            if (Files.LoadFiles())
            {
                this.Title = "TQ Collector v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

                //Set toolbar toggle buttons
                LoadToggles();
                LoadLanguageList();
            }
            else
            {
                this.Close();
                return;
            }

            loaded = true;
            refreshDisplay();
            
            if(Files.Configuration.RefreshTimer!=null) {
                refreshTimerSeconds = Int32.Parse(Files.Configuration.RefreshTimer);
                if (refreshTimerSeconds >= 120) {
                    refreshTimer = new System.Timers.Timer(refreshTimerSeconds*1000);
                    refreshTimer.Elapsed += new System.Timers.ElapsedEventHandler(refreshTimerHandler);
                    refreshTimer.Start();
                }
            }
        }

        private void refreshTimerHandler(object sender, System.Timers.ElapsedEventArgs e)
        {
            uiContext.Send(new SendOrPostCallback(
                delegate (object state)
                {
                    Files.reloadFiles();
                    refreshDisplay();
                }
            ), null);
        }

        private void LoadToggles()
        {
            Button_ToggleBG.IsChecked = Files.Configuration.UseBG;
            Button_ToggleBG.IsEnabled = Files.Configuration.UseBG;
            Button_ToggleBG.ToolTip = Files.Language["tooltip16"];
            Button_ToggleIT.IsChecked = Files.Configuration.UseIT;
            Button_ToggleIT.IsEnabled = Files.Configuration.UseIT || Files.Configuration.UseAE;
            Button_ToggleIT.ToolTip = Files.Language["tooltip04"];
            Button_ToggleSP.IsChecked = Files.Configuration.UseSP;
            Button_ToggleSP.IsEnabled = Files.Configuration.UseIT || Files.Configuration.UseAE;
            Button_ToggleSP.ToolTip = Files.Language["tooltip05"];
            Button_ToggleR.IsChecked = Files.Configuration.UseR;
            Button_ToggleR.IsEnabled = Files.Configuration.UseAE;
            Button_ToggleR.ToolTip = Files.Language["tooltip13"];
            Button_ToggleAtl.IsChecked = Files.Configuration.UseAtl;
            Button_ToggleAtl.IsEnabled = Files.Configuration.UseAE;
            Button_ToggleAtl.ToolTip = Files.Language["tooltip14"];
            Button_ToggleEE.IsChecked = Files.Configuration.UseEE;
            Button_ToggleEE.IsEnabled = Files.Configuration.UseAE;
            Button_ToggleEE.ToolTip = Files.Language["tooltip15"];
            Button_ToggleCaravan.IsChecked = Files.Configuration.UseCaravan;
            Button_ToggleCaravan.ToolTip = Files.Language["tooltip01"];
            Button_ToggleCaravan.IsEnabled = Files.Configuration.UseIT || Files.Configuration.UseAE;
            Button_ToggleInventory.IsChecked = Files.Configuration.UseInventories;
            Button_ToggleInventory.ToolTip = Files.Language["tooltip03"];
            Button_ToggleItemCount.IsChecked = Files.Configuration.UseItemCount;
            Button_ToggleItemCount.ToolTip = Files.Language["tooltip06"];
            Button_About.ToolTip = Files.Language["tooltip07"];
            Button_CustomDirectory.ToolTip = Files.Language["tooltip08"];
            Button_ExportDataAs.ToolTip = Files.Language["tooltip09"];
            Button_Refresh.ToolTip = Files.Language["tooltip10"];
            Button_Filters.ToolTip = Files.Language["tooltip11"];
        }

        private void Button_Export_Click(object sender, RoutedEventArgs e)
        {
            SummaryExport f = new SummaryExport(Filterer.generateSummary());
            f.ShowDialog();
        }

        private void Button_Filters_Click(object sender, RoutedEventArgs e)
        {
            FilterOptions Form_FilterOptions = new FilterOptions();
            Form_FilterOptions.ShowDialog();
            refreshDisplay();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (loaded)
            {
                Files.SaveConfig();
            }
        }

        private void Button_ToggleItemCount_Checked(object sender, RoutedEventArgs e)
        {
            if (loaded)
            {
                Files.Configuration.UseItemCount = true;
                refreshDisplay();
            }
        }

        private void Button_ToggleItemCount_Unchecked(object sender, RoutedEventArgs e)
        {
            if (loaded)
            {
                Files.Configuration.UseItemCount = false;
                refreshDisplay();
            }
        }

        private int[] getSelectedIndexes(Grid grid)
        {
            //get selected tabs (2 levels, top tabcontrol and tabcontrol inside tabitem)
            int[] ret = new int[2];

            foreach (var obj in grid.Children)
            {
                if (obj.GetType() == typeof(TabControl))
                {
                    TabControl tabControl = (TabControl)obj;
                    ret[0] = tabControl.SelectedIndex;
                    TabItem tabItemOld = (TabItem)tabControl.Items[ret[0]];
                    if (tabItemOld.Content.GetType() == typeof(TabControl))
                    {
                        TabControl subControl = (TabControl)tabItemOld.Content;
                        ret[1] = subControl.SelectedIndex;
                    }
                }
            }
            return ret;
        }

        private void setSelectedIndexes(int[] selected, TabControl masterControl)
        {
            //set selected tabs (2 levels, top tabcontrol and tabcontrol inside tabitem)
            masterControl.SelectedIndex = selected[0];
            TabItem tabItem = null;
            if (masterControl.Items[selected[0]].GetType() == typeof(TabItem))
            {
                tabItem = (TabItem)masterControl.Items[selected[0]];
                if (tabItem.Content.GetType() == typeof(TabControl))
                {
                    TabControl innerTabControl = (TabControl)tabItem.Content;
                    innerTabControl.SelectedIndex = selected[1];
                }
            }
        }

        private void refreshDisplay()
        {
            int[] selected = getSelectedIndexes(myGrid);

            myGrid.Children.Clear();
            TabControl masterControl = Filterer.Display();

            setSelectedIndexes(selected, masterControl);

            myGrid.Children.Add(masterControl);

            Filterer.resizeLists();
            countLabel.Content = Files.Language["count01"] + Filterer.ItemsCount + "/" + Filterer.ItemsTotal + " (" + (((double)Filterer.ItemsCount) / Filterer.ItemsTotal * 100).ToString("N2") + "%)";
        }

        private void Button_About_Click(object sender, RoutedEventArgs e)
        {
            About ab = new About();
            ab.ShowDialog();
        }

        private void Button_CustomDirectory_Click(object sender, RoutedEventArgs e)
        {
            Directories d = new Directories();
            if ((bool)d.ShowDialog())
            {
                Files.reloadFiles();
                refreshDisplay();
            }
        }

        private void Button_ToggleCaravan_Checked(object sender, RoutedEventArgs e)
        {
            if (loaded)
            {
                Files.Configuration.UseCaravan = true;
                Files.reloadFiles();
                refreshDisplay();
            }
        }

        private void Button_ToggleCaravan_Unchecked(object sender, RoutedEventArgs e)
        {
            if (loaded)
            {
                Files.Configuration.UseCaravan = false;
                Files.reloadFiles();
                refreshDisplay();
            }
        }

        private void Button_ToggleInventory_Checked(object sender, RoutedEventArgs e)
        {
            if (loaded)
            {
                Files.Configuration.UseInventories = true;
                Files.reloadFiles();
                refreshDisplay();
            }
        }

        private void Button_ToggleInventory_Unchecked(object sender, RoutedEventArgs e)
        {
            if (loaded)
            {
                Files.Configuration.UseInventories = false;
                Files.reloadFiles();
                refreshDisplay();
            }
        }

        private void Button_Refresh_Click(object sender, RoutedEventArgs e)
        {
            Files.reloadFiles();
            refreshDisplay();
        }

        private void Button_ToggleBG_Checked(object sender, RoutedEventArgs e)
        {
            if (loaded)
            {
                Files.Configuration.UseBG = true;
                Files.reloadFiles();
                refreshDisplay();
            }
        }

        private void Button_ToggleBG_Unchecked(object sender, RoutedEventArgs e)
        {
            if (loaded)
            {
                Files.Configuration.UseBG = false;
                Files.reloadFiles();
                refreshDisplay();
            }
        }

        private void Button_ToggleIT_Checked(object sender, RoutedEventArgs e)
        {
            if (loaded)
            {
                Files.Configuration.UseIT = true;
                Files.reloadFiles();
                refreshDisplay();
            }
        }

        private void Button_ToggleIT_Unchecked(object sender, RoutedEventArgs e)
        {
            if (loaded)
            {
                Files.Configuration.UseIT = false;
                Files.reloadFiles();
                refreshDisplay();
            }
        }

        private void Button_ToggleSP_Checked(object sender, RoutedEventArgs e)
        {
            if (loaded)
            {
                Files.Configuration.UseSP = true;
                Files.reloadFiles();
                refreshDisplay();
            }
        }

        private void Button_ToggleSP_Unchecked(object sender, RoutedEventArgs e)
        {
            if (loaded)
            {
                Files.Configuration.UseSP = false;
                Files.reloadFiles();
                refreshDisplay();
            }
        }

        private void Button_ToggleR_Checked(object sender, RoutedEventArgs e)
        {
            if (loaded)
            {
                Files.Configuration.UseR = true;
                Files.reloadFiles();
                refreshDisplay();
            }
        }

        private void Button_ToggleR_Unchecked(object sender, RoutedEventArgs e)
        {
            if (loaded)
            {
                Files.Configuration.UseR = false;
                Files.reloadFiles();
                refreshDisplay();
            }
        }

        private void Button_ToggleAtl_Checked(object sender, RoutedEventArgs e)
        {
            if (loaded)
            {
                Files.Configuration.UseAtl = true;
                Files.reloadFiles();
                refreshDisplay();
            }
        }

        private void Button_ToggleAtl_Unchecked(object sender, RoutedEventArgs e)
        {
            if (loaded)
            {
                Files.Configuration.UseAtl = false;
                Files.reloadFiles();
                refreshDisplay();
            }
        }

        private void Button_ToggleEE_Checked(object sender, RoutedEventArgs e)
        {
            if (loaded)
            {
                Files.Configuration.UseEE = true;
                Files.reloadFiles();
                refreshDisplay();
            }
        }

        private void Button_ToggleEE_Unchecked(object sender, RoutedEventArgs e)
        {
            if (loaded)
            {
                Files.Configuration.UseEE = false;
                Files.reloadFiles();
                refreshDisplay();
            }
        }

        private void LoadLanguageList()
        {
            //Get list of languages in TQC folder.
            DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory);
            FileInfo[] rgFiles = di.GetFiles("text_*.xml");
            List<string> sl = new List<string>();

            foreach (FileInfo f in rgFiles)
            {
                string s = f.Name.Substring(5, 2);
                //Make sure we compare all lowercase
                s = s.ToLowerInvariant();
                sl.Add(s);
            }

            //Compare to available IT ones
            if (Files.Configuration.UseIT)
            {
                DirectoryInfo di2 = new DirectoryInfo(Files.Configuration.Directories.IT + "\\Resources\\");
                FileInfo[] rgFiles2 = di2.GetFiles("text_*.arc");
                List<string> sl2 = new List<string>();

                foreach (FileInfo f in rgFiles2)
                {
                    string s = f.Name.Substring(5, 2);
                    //Make sure we compare all lowercase
                    s = s.ToLowerInvariant();
                    sl2.Add(s);
                }

                foreach (string st in sl)
                {
                    if (sl2.Contains(st))
                    {
                        //Add to Language list
                        ComboBoxItem cb = new ComboBoxItem();
                        cb.Content = st;
                        cb.Selected += Language_Combo_Change;
                        Language_Combo.Items.Add(cb);
                        if(st.Equals(Files.Configuration.Language)) Language_Combo.SelectedItem = cb;
                    }
                }
            }
            // Anniversary Edition
            else if(Files.Configuration.UseAE)
            {
                DirectoryInfo di2 = new DirectoryInfo(Files.Configuration.Directories.AE + "\\Text\\");
                FileInfo[] rgFiles2 = di2.GetFiles("text_*.arc");
                List<string> sl2 = new List<string>();

                foreach (FileInfo f in rgFiles2)
                {
                    string s = f.Name.Substring(5, 2);
                    //Make sure we compare all lowercase
                    s = s.ToLowerInvariant();
                    sl2.Add(s);
                }

                foreach (string st in sl)
                {
                    if (sl2.Contains(st))
                    {
                        //Add to Language list
                        ComboBoxItem cb = new ComboBoxItem();
                        cb.Content = st;
                        cb.Selected += Language_Combo_Change;
                        Language_Combo.Items.Add(cb);
                        if (st.Equals(Files.Configuration.Language)) Language_Combo.SelectedItem = cb;
                    }
                }
            }
            //TQ instead of IT.
            else
            {
                DirectoryInfo di2 = new DirectoryInfo(Files.Configuration.Directories.TQ + "\\Text\\");
                FileInfo[] rgFiles2 = di.GetFiles("text_*.arc");
                List<string> sl2 = new List<string>();

                foreach (FileInfo f in rgFiles2)
                {
                    string s = f.Name.Substring(5, 2);
                    //Make sure we compare all lowercase
                    s = s.ToLowerInvariant();
                    sl2.Add(s);
                }

                foreach (string st in sl)
                {
                    if (sl2.Contains(st))
                    {
                        //Add to Language list
                        ComboBoxItem cb = new ComboBoxItem();
                        cb.Content = st;
                        Language_Combo.Items.Add(cb);
                        if (st.Equals(Files.Configuration.Language)) Language_Combo.SelectedItem = cb;
                    }
                }
            }
        }

        private void Language_Combo_Change(object sender, RoutedEventArgs e)
        {
            ComboBoxItem c = sender as ComboBoxItem;
            string s = c.Content.ToString();
            Files.Configuration.Language = s;
            Files.SaveConfig();
            if (loaded)
            {
                Files.reloadFiles();
                refreshDisplay();
            }
        }

        private void MWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (loaded) Filterer.resizeLists();
        }
    }
}
