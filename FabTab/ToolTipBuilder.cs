using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Imaging;

namespace FabTab
{
    internal class ToolTipBuilder
    {
        private FabTabControl _tabControl;

        public ToolTipBuilder(FabTabControl tabControl)
        {
            _tabControl = tabControl;
        }

        public void SetImageScreenshotOnFabTabItemAttachedProperty(FrameworkElement element, BitmapSource bitmap)
        {
            Image tooltipImage = CreateImageForToolTip(bitmap);

            FabTabItem item = element.Parent as FabTabItem;
            if (item == null)
            {
                item = element as FabTabItem;
            }
            item = GetItemContainer(element, item);
            
            if (item != null)
            {
                FabTabItemProperties.SetFabTabItemImage(item, tooltipImage);
            }
            else
            {
                throw new InvalidOperationException("Cannot create rich tooltip because FabTabItem cannot be resolved");
            }
        }

        private FabTabItem GetItemContainer(FrameworkElement element, FabTabItem item)
        {
            if (item == null)
            {
                item = _tabControl.ItemContainerGenerator.ContainerFromItem(element) as FabTabItem;
            }
            return GetItemFromHiddenListViewIfNecessary(element, item);
        }

        private FabTabItem GetItemFromHiddenListViewIfNecessary(FrameworkElement element, FabTabItem item)
        {
            //handle the case where they're binding tab's itemsource property to model object using datatemplates
            if (element is ListViewItem)
            {
                item = _tabControl.ItemContainerGenerator.ContainerFromItem((element as ListViewItem).Content) as FabTabItem;
            }
            return item;
        }

        private static Image CreateImageForToolTip(BitmapSource bitmap)
        {
            //TODO: Make the size of the tooltip image user-configurable?
            Image tooltipImage = new Image();
            tooltipImage.Source = bitmap;
            tooltipImage.Height = 300;
            tooltipImage.Width = 400;
            //changed to UniformToFill because Uniform caused weird behavior if entire windows
            //were resized
            tooltipImage.Stretch = Stretch.UniformToFill;
            return tooltipImage;
        }
    }
}
