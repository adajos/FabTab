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
    /// Interaction logic for TabItemsWindow.xaml
    /// </summary>
    public partial class TabItemsWindow : Window
    {
        public TabItemsWindow()
        {
            InitializeComponent();
                         
        }

        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SampleView view = new SampleView();
            
            view.LabelText = "A new view #" + (this.tabControl.Items.Count).ToString();
            this.tabControl.Items.Add(view);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CancelCloseEventWindow newWindow = new CancelCloseEventWindow();
            newWindow.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Window1 newWindow = new Window1();
            newWindow.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            SimpleClosableTabWindow newWindow = new SimpleClosableTabWindow();
            newWindow.Show();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            NoTabCloseWindow newWindow = new NoTabCloseWindow();
            newWindow.Show();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            NoXamlWindow newWindow = new NoXamlWindow();
            newWindow.Show();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            MVVMWindow newWindow = new MVVMWindow();
            newWindow.Show();
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            
            SampleView view = new SampleView();
            view.LabelText = "A new view #" + (this.tabControl.Items.Count).ToString();

            FabTabItem item = new FabTabItem();
            
            Uri uri = new Uri("pack://application:,,/SampleProject;component/Graphics/tab_icon.gif", UriKind.RelativeOrAbsolute);
            BitmapImage bitmap = new BitmapImage(uri);
            
            //need to create these element factories to build up the datatemplates completely in code.
            //This would be much easier to define in XAML, but doing it here for demonstration purposes.
            //For more info on how this works see: http://stackoverflow.com/questions/248362/how-do-i-build-a-datatemplate-in-c-code
            FrameworkElementFactory spFactory = new FrameworkElementFactory(typeof(StackPanel));
            spFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
     
            FrameworkElementFactory imgFactory = new FrameworkElementFactory(typeof(Image));
            imgFactory.SetValue(Image.SourceProperty, bitmap);
            
            FrameworkElementFactory tbFactory = new FrameworkElementFactory(typeof(TextBlock));
            //Very important to bind something in our datatemplate to the actual header so that it will show up.
            tbFactory.SetBinding(TextBlock.TextProperty, new Binding("."));

            spFactory.AppendChild(imgFactory);
            spFactory.AppendChild(tbFactory);
            
            DataTemplate headerTemplate = new DataTemplate();
            //set the datatemplate's visual tree to the root frameworkelementfactory.
            headerTemplate.VisualTree = spFactory;

            //build the visual tree for the HeaderTemplate, but the header itself needs to remain a string
            //if we want a meaningful string to show up for the screenshot headers on the ContentTabView
            //and meaningful tab header names to show up in the ContentTab dropdown.
            item.HeaderTemplate = headerTemplate;
            item.Header = view.LabelText;
            item.Content = view;

            this.tabControl.Items.Add(item);

        }

        
    }
}
