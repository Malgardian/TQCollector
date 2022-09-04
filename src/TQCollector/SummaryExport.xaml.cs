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
    /// Interaction logic for SummaryExport.xaml
    /// </summary>
    public partial class SummaryExport : Window
    {
        public SummaryExport(string caption)
        {
            InitializeComponent();
            textBox.Text = caption;
        }
    }
}
