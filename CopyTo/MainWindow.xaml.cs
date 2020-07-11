using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CopyTo
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {        
        public MainWindow()
        {
            InitializeComponent();
            
            CopyFiles.loadConfig(out string from, out string to);
            tbFrom.Text = from;
            tbTo.Text = to;

        }


        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            CopyFiles.execCopy(tbFrom.Text, tbTo.Text);
        }

        private void btnAnal_Click(object sender, RoutedEventArgs e)
        {
            dgFiles.ItemsSource = CopyFiles.analizeDir(tbFrom.Text);
        }

        private void lvFiles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CopyFiles.copyOneFile((dgFiles.SelectedItem as MyFileClass).Name, tbFrom.Text, tbTo.Text);

        }
    }
}
