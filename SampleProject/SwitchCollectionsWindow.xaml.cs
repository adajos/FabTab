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
using System.Collections.ObjectModel;

namespace SampleProject
{
    /// <summary>
    /// Interaction logic for SwitchCollectionsWindow.xaml
    /// </summary>
    public partial class SwitchCollectionsWindow : Window
    {
        public SwitchCollectionsWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            myTab.Items.Clear();
            ObservableCollection<UserControl> views = new ObservableCollection<UserControl>();
            
            myTab.ItemsSource = views;
            for (int counter = 0; counter < 3; counter++)
            {
                SampleView newView = new SampleView();
                newView.LabelText = "View #" + counter.ToString();
                views.Add(newView);

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            myTab.ItemsSource = null;
            SampleView view = new SampleView();
            view.LabelText = "Items Collection View";
            myTab.Items.Add(view);
            view = new SampleView();
            view.LabelText = "Items Collection View 2";
            myTab.Items.Add(view);
            view = new SampleView();
            view.LabelText = "Items Collection View 3";
            myTab.Items.Add(view);

        }
    }
}
