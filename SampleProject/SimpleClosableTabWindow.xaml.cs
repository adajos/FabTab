using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SampleProject
{
    /// <summary>
    /// Interaction logic for SimpleClosableTabWindow.xaml
    /// </summary>
    public partial class SimpleClosableTabWindow : Window
    {
        public SimpleClosableTabWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SampleView view = new SampleView();
            view.LabelText = "A new view #" + (this.tabControl.Items.Count).ToString();
            this.tabControl.Items.Add(view);
        }
    }
}
