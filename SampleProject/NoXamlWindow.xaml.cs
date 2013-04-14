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
using System.Windows.Navigation;
using System.Windows.Shapes;
using FabTab;

namespace SampleProject
{
    /// <summary>
    /// Interaction logic for NoXamlWindow.xaml
    /// </summary>
    public partial class NoXamlWindow : Window
    {
        FabTabControl myTabControl;
        public NoXamlWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(NoXamlWindow_Loaded);

        }

        void NoXamlWindow_Loaded(object sender, RoutedEventArgs e)
        {
            myTabControl = new FabTabControl();

            FabTabItem tab1 = new FabTabItem();
            tab1.Content = new SampleView();
            tab1.Header = "Tab 1";

            FabTabItem tab2 = new FabTabItem();
            tab2.Content = new SampleView();
            tab2.Header = "Tab 2";

            myTabControl.Items.Add(tab1);
            myTabControl.Items.Add(tab2);

            grid.Children.Add(myTabControl);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FabTabItem tab3 = new FabTabItem();
            tab3.Content = new SampleView();
            tab3.Header = "Tab 3";

            myTabControl.Items.Add(tab3);
            
        }
    }
}
