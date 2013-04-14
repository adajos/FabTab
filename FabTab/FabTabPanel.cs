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
using System.Windows.Controls.Primitives;

namespace FabTab
{
    //this class is a modified version of the excellent SqueezeTabPanel 
    //by Ron Dunant on codeproject at http://www.codeproject.com/KB/WPF/WpfSqueezeTabPanel.aspx
    public class FabTabPanel : TabPanel
    {
        private double _rowHeight;
        private double _scaleFactor;
        private bool _allowMultiLineTabHeaders = false;

        static FabTabPanel()
        {
            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(FabTabPanel), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
            KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(FabTabPanel), new FrameworkPropertyMetadata(KeyboardNavigationMode.Cycle));
        }

        public bool AllowMultiLineTabHeaders
        {
            get { return _allowMultiLineTabHeaders; }
            set { _allowMultiLineTabHeaders = value; }
        }

        // This Panel lays its children out horizontally.
        // If all children cannot fit in the allocated space,
        // the available space is divided proportionally between them.
        protected override Size MeasureOverride(Size availableSize)
        {
            if (!this.AllowMultiLineTabHeaders)
            {
                // See how much room the children want
                double width = 0.0;
                this._rowHeight = 0.0;
                foreach (UIElement element in this.Children)
                {
                    element.Measure(availableSize);
                    Size size = this.GetDesiredSizeLessMargin(element);
                    this._rowHeight = Math.Max(this._rowHeight, size.Height);
                    width += size.Width;
                }

                // If not enough room, scale the
                // children to the available width
                if (width > availableSize.Width)
                {

                    ContentTabItem contentTabItem = this.Children[0] as ContentTabItem;
                    if (contentTabItem != null)
                    {
                        //there's a content tab, we don't want to narrow it, so take that into account in calculating
                        //the scalefactor
                        this._scaleFactor = (availableSize.Width - contentTabItem.DesiredSize.Width) / width;

                    }
                    else
                    {
                        //scale factor calculated based upon all tabs, there's no contenttab whose width
                        //we want to preserve
                        this._scaleFactor = availableSize.Width / width;
                    }
                    width = 0.0;

                    foreach (UIElement element in this.Children)
                    {
                        if (!(element is ContentTabItem) && (!(element as TabItem).IsSelected))
                        {
                            element.Measure(new Size(element.DesiredSize.Width * this._scaleFactor, availableSize.Height));
                        }
                        else
                        {
                            //give the full width to the ContentTab and the currently selected tab
                            element.Measure(new Size(element.DesiredSize.Width, availableSize.Height));
                        }

                        width += element.DesiredSize.Width;
                    }
                }
                else
                    this._scaleFactor = 1.0;

                return new Size(width, this._rowHeight);

            }
            else
            {
                return base.MeasureOverride(availableSize);
            }
        }

        // Perform arranging of children based on the final size
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            if (!this.AllowMultiLineTabHeaders)
            {
                Point point = new Point();
                foreach (UIElement element in this.Children)
                {
                    Size size1 = element.DesiredSize;
                    Size size2 = this.GetDesiredSizeLessMargin(element);
                    Thickness margin = (Thickness)element.GetValue(FrameworkElement.MarginProperty);
                    double width = size2.Width;
                    if (element.DesiredSize.Width != size2.Width)
                        width = arrangeSize.Width - point.X; // Last-tab-selected "fix"
                    element.Arrange(new Rect(
                        point,
                        new Size(Math.Min(width, size2.Width), this._rowHeight)));
                    double leftRightMargin = Math.Max(0.0, -(margin.Left + margin.Right));
                    point.X += size1.Width + (leftRightMargin * this._scaleFactor);
                }

                return arrangeSize;
            }
            else
            {
                return base.ArrangeOverride(arrangeSize);
            }
        }

        // Return element's size
        // after subtracting margin
        private Size GetDesiredSizeLessMargin(UIElement element)
        {
            Thickness margin = (Thickness)element.GetValue(FrameworkElement.MarginProperty);
            Size size = new Size();
            size.Height = Math.Max(0.0, element.DesiredSize.Height - (margin.Top + margin.Bottom));
            size.Width = Math.Max(0.0, element.DesiredSize.Width - (margin.Left + margin.Right));
            return size;
        }
    }
}
