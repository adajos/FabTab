using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace FabTab
{
    public interface IFabTabItemsStrategy
    {
        void CloseTab(TabItem tab);
        void AddContentsTab();
        void RemoveContentsTab();
        void ForceItemsRender();
        void JuggleHiddenContent(SelectionChangedEventArgs e, Action<SelectionChangedEventArgs> updateAfterRemove);
        
    }
}
