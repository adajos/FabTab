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
using System.ComponentModel;

namespace SampleProject
{
    /// <summary>
    /// Interaction logic for MVVMWindow.xaml
    /// </summary>
    public partial class MVVMWindow : Window
    {
        private ObservableCollection<object> testViews = new ObservableCollection<object>();

        public ObservableCollection<object> TestViews
        {
            get { return testViews; }
            set { testViews = value; }
        }
        
        public MVVMWindow()
        {
            InitializeComponent();
            LoadDummyButtonData();
            this.tabControl.DataContext = TestViews;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ModelObject mo = new ModelObject();
            mo.Name = "Model View # " + (this.tabControl.Items.OfType<ModelObject>().Count()).ToString();
            mo.AllowClose = true;
            testViews.Add(mo);
            
        }

        private void LoadDummyButtonData()
        {
            for (int counter = 0; counter < 2; counter++)
            {
                SampleView newView = new SampleView();
                newView.LabelText = "View #" + counter.ToString();
                this.testViews.Add(newView);

            }

            for (int counter = 0; counter < 2; counter++)
            {
                ModelObject model = new ModelObject();
                model.Name = "Model View #" + counter.ToString();
                if (model.Name == "Model View #1")
                    model.AllowClose = true;
                else
                    model.AllowClose = false;
                this.testViews.Add(model);

            }


        }
    }

    public class ModelObject : INotifyPropertyChanged
    {
        private string _name;
        private string _value;

        //added this just a passthrough so that the style that's sets the FabTabItem
        //header text will work the same as for the tabs that contain a sample view
        public string LabelText
        {
            get { return Name; }
            set { Name = value; }
        }
        
        public string Name 
        {
            get { return _name; }
            set
            {
                if (_name == value)
                    return;
                _name = value;
                OnPropertyChanged("Name");
                

            }
        }
        public string Value
        {
            get { return _value; }
            set
            {
                if (_value == value)
                    return;
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        public bool AllowClose { get; set; }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        } 


        #endregion
    }
}
