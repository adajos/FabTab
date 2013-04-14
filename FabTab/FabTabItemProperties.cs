using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace FabTab
{
    public static class FabTabItemProperties
    {
        public static Image GetFabTabItemImage(FabTabItem fabTabItem)
        {
            return (Image)fabTabItem.GetValue(FabTabItemImageProperty);
        }

        public static void SetFabTabItemImage(
          FabTabItem fabTabItem, Image value)
        {
            fabTabItem.SetValue(FabTabItemImageProperty, value);
        }

        public static readonly DependencyProperty FabTabItemImageProperty =
            DependencyProperty.RegisterAttached(
            "FabTabItemImage",
            typeof(Image),
            typeof(FabTabItemProperties));



    }
}
