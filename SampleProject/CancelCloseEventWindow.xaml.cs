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
using FabTab;

namespace SampleProject
{
    /// <summary>
    /// Interaction logic for CancelCloseEvent.xaml
    /// </summary>
    public partial class CancelCloseEventWindow : Window
    {
        public CancelCloseEventWindow()
        {
            InitializeComponent();
            
            myConcreteTabItem.TabClosing += new RoutedEventHandler(myConcreteTabItem_TabClosing);
            this.Loaded += new RoutedEventHandler(CancelCloseEventWindow_Loaded);
        }
                              

        void CancelCloseEventWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= CancelCloseEventWindow_Loaded;
            FabTabItem tabItem = mySplitTabControl.ItemContainerGenerator.ContainerFromItem(mySplitTabControl.Items[0]) as FabTabItem;
            if (tabItem != null)
            {
                tabItem.TabClosing += new RoutedEventHandler(tabItem_TabClosing);
            }
        }

        void tabItem_TabClosing(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You can never close this tab");
            e.Handled = true;
        }

        void myConcreteTabItem_TabClosing(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("got here");
        }

        private void ClosableTabItem_TabClosing(object sender, RoutedEventArgs e)
        {
            if (mySplitTabControl.SelectedItem == myConcreteTabItem)
            {
                MessageBox.Show("Cannot close this tab when it is selected.  Select another tab, then close it");
                e.Handled = true;

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SampleView view = new SampleView();
            view.LabelText = "A new view #" + (this.mySplitTabControl.Items.Count + 1).ToString();
            this.mySplitTabControl.Items.Add(view);
        }
    }
}
