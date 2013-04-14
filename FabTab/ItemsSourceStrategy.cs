using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections;
using System.Windows.Controls;
using FabTab.Extensions;

namespace FabTab
{
    public class ItemsSourceStrategy : IFabTabItemsStrategy
    {
        private FabTabControl _tabControl;

        public ItemsSourceStrategy(FabTabControl tabControl)
        {
            _tabControl = tabControl;
        }

        
        #region IFabTabItemsStrategy Members

        public void CloseTab(System.Windows.Controls.TabItem tab)
        {
            IList listItemsSource = _tabControl.ItemsSource as IList;
            if (listItemsSource != null)
            {
                int indexToSelect = GetIndexToSelectAfterTabCloses(tab);
                listItemsSource.Remove(tab.Content);
                _tabControl.ItemsSource = null;
                _tabControl.ItemsSource = listItemsSource;
                _tabControl.SelectedIndex = indexToSelect;

            }
        }

        private int GetIndexToSelectAfterTabCloses(TabItem tab)
        {
            int indexToSelect = _tabControl.SelectedIndex;
            if ((IsSelectedTabClosing(tab) && IsLastTabClosing())
                || IsAPreviousTabClosing(tab))
            {
                indexToSelect--;
            }
            return indexToSelect;
        }

        private bool IsSelectedTabClosing(TabItem tab)
        {
            return _tabControl.SelectedItem == tab.Content;
        }

        private bool IsLastTabClosing()
        {
            return _tabControl.SelectedIndex == _tabControl.Items.Count - 1;
        }

        private bool IsAPreviousTabClosing(TabItem tab)
        {
            return _tabControl.Items.IndexOf(tab.Content) < _tabControl.SelectedIndex;
        }

        public void AddContentsTab()
        {
            //using ItemsSource, modify collection to contain TOC Tab
            IList items = _tabControl.ItemsSource as IList;
            if (items != null)
            {
                _tabControl.ItemsSource = null;
                items.Insert(0, _tabControl.GetContentsTab());
                _tabControl.ItemsSource = items;
                (_tabControl.ItemContainerGenerator.ContainerFromItem(items[1]) as TabItem).Focus();
            }
        }

        public void RemoveContentsTab()
        {
            IList items = _tabControl.ItemsSource as IList;
            if (items != null)
            {
                _tabControl.ItemsSource = null;
                items.RemoveAt(0);
                _tabControl.ItemsSource = items;
            }
        }

        public void ForceItemsRender()
        {
            //add all the unselected items into the hidden listview that gets stuffed into
            //the hidden content presenter...without this, snapshots of the other views for the contentstab
            //don't work.
            _tabControl.HiddenContent.Items.Clear();
            _tabControl.Items.ForEach(delegate(object item)
            {
                if (item != _tabControl.SelectedItem)
                {
                    _tabControl.HiddenContent.Items.Add(item);
                }
            });
            
            _tabControl.UpdateLayout();
        }

        public void JuggleHiddenContent(SelectionChangedEventArgs e, Action<SelectionChangedEventArgs> updateAfterRemove)
        {
            //juggle the parents of our hidden listview that gets stuffed into
            //the hidden contentpresenter
            if (e.AddedItems.Count > 0)
            {
                _tabControl.HiddenContent.Items.Remove(e.AddedItems[0]);
            }

            updateAfterRemove.Invoke(e);
            
            if (e.RemovedItems.Count > 0)
            {
                _tabControl.HiddenContent.Items.Add(e.RemovedItems[0]);
            }
        }

        

        #endregion
    }
}
