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

namespace FabTab
{
    [TemplatePart(Name = "PART_tabHeaderComboBox", Type = typeof(ComboBox))]
    [TemplatePart(Name = "Content", Type = typeof(ContentPresenter))]
    /// <summary>
    /// </summary>
    public class ContentTabItem : TabItem
    {
        static ContentTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentTabItem), new FrameworkPropertyMetadata(typeof(ContentTabItem)));
        }

        public ContentTabItem()
        {
            
        }

        
    }
}
