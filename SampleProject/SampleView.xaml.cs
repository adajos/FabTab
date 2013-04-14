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

namespace SampleProject
{
    /// <summary>
    /// Interaction logic for SampleView.xaml
    /// </summary>
    public partial class SampleView : UserControl
    {
        public SampleView()
        {
            InitializeComponent();
        }

        public string LabelText
        {
            get { return this.label1.Content.ToString(); }
            set { this.label1.Content = value; }
        }
    }
}
