using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace TQCollector
{
    /// <summary>
    /// Interaction logic for Directories.xaml
    /// </summary>
    public partial class Directories : Window
    {
        private bool isVaultsValid = false;
        private bool isTQValid = false;
        private bool isITValid = false;
        private bool isAEValid = false;

        private bool isVaultsEnabled = false;
        private bool isTQEnabled = false;
        private bool isITEnabled = false;
        private bool isAEEnabled = false;

        private bool loaded = false;

        public Directories(bool firstLoad = false)
        {
            InitializeComponent();

            Files.Configuration.Directories.Vaults = changeDirectory(ref isVaultsValid, "*.vault*", ref Text_Vault, "", Files.Configuration.Directories.Vaults);
            Files.Configuration.Directories.TQ = changeDirectory(ref isTQValid, "text_*.arc", ref Text_TQ, "\\text", Files.Configuration.Directories.TQ);
            Files.Configuration.Directories.IT = changeDirectory(ref isITValid, "text_*.arc", ref Text_IT, "\\text", Files.Configuration.Directories.IT);
            Files.Configuration.Directories.AE = changeDirectory(ref isAEValid, "text_*.arc", ref Text_AE, "\\text", Files.Configuration.Directories.AE);

            button5.Content = Files.Language["button01"];
            button4.Content = Files.Language["button02"];

            Default_Vault.Content = Files.Language["button03"];
            Default_IT.Content = Files.Language["button03"];
            Default_TQ.Content = Files.Language["button03"];
            Default_AE.Content = Files.Language["button03"];
            checkBox.Content = Files.Language["misc05"];

            label1.Content = Files.Language["path01"];
            label2.Content = Files.Language["path02"];
            label3.Content = Files.Language["path03"];
            label4.Content = Files.Language["path04"];

            this.Title = Files.Language["window02"];

            button5.IsEnabled = false;
            if(!firstLoad)
            {
                if (Files.Configuration.UseAE)
                {
                    button2.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
                }
                else if (Files.Configuration.UseIT)
                {
                    button1.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
                }
                else
                {
                    button.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
                }

                checkBox.IsEnabled = true;
                checkBox.IsChecked = Files.Configuration.UseVaults;
            }

            loaded = true;
    }

        private void setVaults(bool enabled)
        {
            label1.IsEnabled = enabled;
            Text_Vault.IsEnabled = enabled;
            Browse_Vault.IsEnabled = enabled;
            Default_Vault.IsEnabled = enabled;
            isVaultsEnabled = enabled;
        }

        private void setTQ(bool enabled)
        {
            label2.IsEnabled = enabled;
            Text_TQ.IsEnabled = enabled;
            Browse_TQ.IsEnabled = enabled;
            Default_TQ.IsEnabled = enabled;
            isTQEnabled = enabled;
        }

        private void setIT(bool enabled)
        {
            label3.IsEnabled = enabled;
            Text_IT.IsEnabled = enabled;
            Browse_IT.IsEnabled = enabled;
            Default_IT.IsEnabled = enabled;
            isITEnabled = enabled;
        }

        private void setAE(bool enabled)
        {
            label4.IsEnabled = enabled;
            Text_AE.IsEnabled = enabled;
            Browse_AE.IsEnabled = enabled;
            Default_AE.IsEnabled = enabled;
            isAEEnabled = enabled;
        }

        private string changeDirectory(ref bool valid, string filter, ref TextBox textBox, string appendPath = "", string path = "notset")
        {
            DirectoryInfo di;
            FileInfo[] rgFiles = null;

            if (path.Equals("notset"))
            {
                System.Windows.Forms.FolderBrowserDialog f = new System.Windows.Forms.FolderBrowserDialog();

                f.ShowDialog();
                path = f.SelectedPath;
            }

            if (path.Length > 0)
            {
                textBox.Text = path;

                bool dirExists = Directory.Exists(path + appendPath);

                if (dirExists)
                {
                    di = new DirectoryInfo(path + appendPath);
                    rgFiles = di.GetFiles(filter);
                }

                if (dirExists && rgFiles.Length > 0)
                {
                    // We're good
                    textBox.Background = System.Windows.Media.Brushes.Green;
                    valid = true;

                    button5.IsEnabled = checkValidity();
                }
                else
                {
                    textBox.Background = System.Windows.Media.Brushes.Red;
                    valid = false;
                }

                return path + (path[path.Length - 1].Equals('\\') ? "" : "\\");
            }

            return "null";
        }

        private bool checkValidity()
        {
            bool tempValid = true;
            if (isVaultsEnabled) tempValid &= isVaultsValid;
            if (isTQEnabled) tempValid &= isTQValid;
            if (isITEnabled) tempValid &= isITValid;
            if (isAEEnabled) tempValid &= isAEValid;

            return tempValid;
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            Files.SaveConfig();
            this.DialogResult = true;
            this.Close();
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Browse_Vault_Click(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Directories.Vaults = changeDirectory(ref isVaultsValid, "*.vault", ref Text_Vault);
        }

        private void Browse_TQ_Click(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Directories.TQ = changeDirectory(ref isTQValid, "text_*.arc", ref Text_TQ, "\\text");
        }

        private void Browse_IT_Click(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Directories.IT = changeDirectory(ref isITValid, "text_*.arc", ref Text_IT, "\\text");
        }

        private void Browse_AE_Click(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Directories.AE = changeDirectory(ref isAEValid, "text_*.arc", ref Text_AE, "\\text");
        }

        private void Default_Vault_Click(object sender, RoutedEventArgs e)
        {
            string vaultPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "My Games\\Titan Quest\\TQVaultData\\");
            Files.Configuration.Directories.Vaults = changeDirectory(ref isVaultsValid, "*.vault", ref Text_Vault, "", vaultPath);
        }

        private void Default_TQ_Click(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Directories.TQ = changeDirectory(ref isTQValid, "text_*.arc", ref Text_TQ, "\\text", "C:\\Program Files\\THQ\\Titan Quest\\");
        }

        private void Default_IT_Click(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Directories.IT = changeDirectory(ref isITValid, "text_*.arc", ref Text_IT, "\\text", "C:\\Program Files\\THQ\\Titan Quest Immortal Throne\\");
        }

        private void Default_AE_Click(object sender, RoutedEventArgs e)
        {
            Files.Configuration.Directories.AE = changeDirectory(ref isAEValid, "text_*.arc", ref Text_AE, "\\text", "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Titan Quest Anniversary Edition\\");
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            // Titan Quest selected
            Files.Configuration.UseAE = false;
            Files.Configuration.UseIT = false;
            
            setTQ(true);
            setIT(false);
            setAE(false);

            if(!checkBox.IsEnabled)
            {
                checkBox.IsEnabled = true;
                setVaults(true);
            }

            button5.IsEnabled = checkValidity();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            // Immortal Throne selected
            Files.Configuration.UseIT = true;
            Files.Configuration.UseAE = false;
            
            setTQ(true);
            setIT(true);
            setAE(false);

            if (!checkBox.IsEnabled)
            {
                checkBox.IsEnabled = true;
                setVaults(true);
            }

            button5.IsEnabled = checkValidity();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            // Anniversary Edition selected
            Files.Configuration.UseIT = false;
            Files.Configuration.UseAE = true;
            
            setTQ(false);
            setIT(false);
            setAE(true);

            if (!checkBox.IsEnabled)
            {
                checkBox.IsEnabled = true;
                setVaults(true);
            }

            button5.IsEnabled = checkValidity();
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            if (loaded)
            {
                Files.Configuration.UseVaults = true;
            }
            setVaults(true);

            button5.IsEnabled = checkValidity();
        }

        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (loaded)
            {
                Files.Configuration.UseVaults = false;
            }
            setVaults(false);

            button5.IsEnabled = checkValidity();
        }
    }
}
