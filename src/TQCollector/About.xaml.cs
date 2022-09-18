using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TQCollector
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Label_Credits.Content = String.Format(Files.Language["credits01"], System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
            Label_JJYing.Content = Files.Language["credits02"];
            Label_Sekkyumu.Content = Files.Language["credits03"];
            Label_Forums.Content = Files.Language["credits04"];
            Label_bman654.Content = Files.Language["credits05"];
            Label_Arperum.Content = Files.Language["credits06"];
            Label_CreatedBy.Content = Files.Language["credits07"];
            Label_Spectre.Content = Files.Language["credits08"];
            Label_CreatedBy2.Content = Files.Language["credits09"];
            Label_Translators.Content = Files.Language["credits10"];
            Label_hguy.Content = Files.Language["credits11"];
        }
    }
}
