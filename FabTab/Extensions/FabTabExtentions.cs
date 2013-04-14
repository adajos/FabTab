using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace FabTab.Extensions
{
    public static class FabTabExtentions
    {
        public static FabTabItem FindFabTabFromItem(this FabTabControl control, object item)
        {
            FabTabItem fabTab = item as FabTabItem;
            if (fabTab == null)
            {
                fabTab = control.ItemContainerGenerator.ContainerFromItem(item) as FabTabItem;
            }
            return fabTab;
        }

        public static void ForEach(this ItemCollection collection, Action<object> actionToDo)
        {
            foreach (object item in collection)
            {
                actionToDo(item);
            }
        }
    }
}
