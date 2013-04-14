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
using System.Collections;
using System.Collections.ObjectModel;

namespace SampleProject
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {

        private ObservableCollection<UserControl> testViews = new ObservableCollection<UserControl>();

        public ObservableCollection<UserControl> TestViews
        {
            get { return testViews; }
            set { testViews = value; }
        }
        
        public Window1()
        {
            InitializeComponent();
            
            LoadDummyButtonData();
            this.tabControl.DataContext = TestViews;
            //this.myNormalTabControl.DataContext = TestViews;

        }

        private void LoadDummyButtonData()
        {
            for (int counter = 0; counter < 2; counter++)
            {
                SampleView newView = new SampleView();
                newView.LabelText = "View #" + counter.ToString();
                this.testViews.Add(newView);

            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SampleView newView = new SampleView();
            newView.LabelText = "View #" + testViews.Count.ToString();
            testViews.Add(newView);
        }
    }
}
