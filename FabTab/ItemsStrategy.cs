using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FabTab.Extensions;
using System.Windows.Controls;

namespace FabTab
{
    public class ItemsStrategy : IFabTabItemsStrategy 
    {
        private FabTabControl _tabControl;

        public ItemsStrategy(FabTabControl tabControl)
        {
            _tabControl = tabControl;
        }
        #region IFabTabItemsStrategy Members

        public void CloseTab(System.Windows.Controls.TabItem tab)
        {
            //remove the tab item if that's actually in the collection
            if (_tabControl.Items.Contains(tab))
            {
                _tabControl.Items.Remove(tab);
            }
            //otherwise remove the view
            else
            {
                _tabControl.Items.Remove(tab.Content);
            }

        }

        public void AddContentsTab()
        {
            //presumably using items collection, modify that collection to contain TOC Tab
            _tabControl.Items.Insert(0, _tabControl.GetContentsTab());
        }

        public void RemoveContentsTab()
        {
            _tabControl.Items.RemoveAt(0);
        }

        public void ForceItemsRender()
        {
            //loop through everything and force it to render to the visual tree so the bitmaps
            //for the content tab look correct.
            int indexToSelect = _tabControl.SelectedIndex;
            SelectEachTabAndUpdateItsLayout();
            ReselectCorrectTab(indexToSelect);
        }

        private void SelectEachTabAndUpdateItsLayout()
        {
            _tabControl.Items.ForEach(delegate(object item)
            {
                FabTabItem tabItem = _tabControl.FindFabTabFromItem(item);
                if (tabItem != null)
                {
                    tabItem.IsSelected = true;
                    _tabControl.UpdateLayout();
                }
            });
                        
        }

        private void ReselectCorrectTab(int indexToSelect)
        {
            if (indexToSelect == -1 && _tabControl.Items.Count > 0)
            {
                indexToSelect = 0;
            }
            _tabControl.SelectedIndex = indexToSelect;
        }

        public void JuggleHiddenContent(SelectionChangedEventArgs e, Action<SelectionChangedEventArgs> updateAfterAdd)
        {
            //Nothing necessary.
        }

        #endregion
    }
}
