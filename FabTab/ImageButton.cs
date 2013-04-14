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
    [TemplatePart(Name = "PART_CloseButton", Type = typeof(Button))]
    /// <summary>
    /// </summary>
    public class ImageButton : Button
    {
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(object),
                                                                                                            typeof(ImageButton));

        public object Title
        {
            get { return (object)this.GetValue(TitleProperty); }
            set
            {
                this.SetValue(TitleProperty, value);
            }
        }
        
        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
        }
    }
}
