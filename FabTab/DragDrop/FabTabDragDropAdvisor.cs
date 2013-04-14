using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Markup;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Controls;
using System.Xml;
using System.IO;
using System.Windows.Input;
using System.Collections;

namespace FabTab.DragDrop
{
    internal class FabTabDragDropAdvisor : IDragSourceAdvisor, IDropTargetAdvisor
    {
        private UIElement _sourceElt;
        private UIElement _targetElt;

        private FabTabControl _tabControl;

        
        #region IDragSourceAdvisor Members

        public UIElement SourceUI
        {
            get { return _sourceElt; }
            set { _sourceElt = value; }
        }

        public DragDropEffects SupportedEffects
        {
            get { return DragDropEffects.Move; }
        }

        public DataObject GetDataObject(UIElement draggedElt)
        {
            if (draggedElt == null)
            {
                return null;
            }

            if (_tabControl == null)
            {
                _tabControl = GetFabTabItemsParent(draggedElt);
            }

            if (_tabControl == null)
            {
                throw new Exception("Can't find parent FabTabControl for FabTabItem");
            }

            FabTabItem fti = draggedElt as FabTabItem;
            DataObject obj = new DataObject("FabTabItem", _tabControl.ItemContainerGenerator.IndexFromContainer(fti).ToString());

            return obj;
        }

        private FabTabControl GetFabTabItemsParent(DependencyObject item)
        {
            FabTabControl fabTabControl;
            DependencyObject parent = VisualTreeHelper.GetParent(item);
            fabTabControl = parent as FabTabControl;
            if (fabTabControl == null)
            {
                return GetFabTabItemsParent(parent);
            }

            return fabTabControl;
        }

        public void FinishDrag(UIElement draggedElt, DragDropEffects finalEffects)
        {
            //if ((finalEffects & DragDropEffects.Move) == DragDropEffects.Move)
            //{
                //TODO: should I have something here????
                
            //}
        }

        public bool IsDraggable(UIElement dragElt)
        {
            if (dragElt is FabTabItem )
            {
                //I wonder if there are performance issues with making this call in a
                //potentially frequently-called method like this
                _tabControl = this.GetFabTabItemsParent(dragElt);
                if (_tabControl.AllowDragAndDropTabReordering)
                {
                    return true;
                }
            }
            return false;
        }

        public UIElement GetTopContainer()
        {
            return _sourceElt;
        }

        #endregion

        #region IDropTargetAdvisor Members

        public UIElement TargetUI
        {
            get { return _targetElt; }
            set { _targetElt = value; }
        }

        public bool ApplyMouseOffset
        {
            get { return true; }
        }

        public bool IsValidDataObject(IDataObject obj)
        {
            return (obj.GetDataPresent("FabTabItem"));
        }

        public UIElement GetVisualFeedback(IDataObject obj)
        {
            int index = ExtractElementIndex(obj);

            Type t = typeof(FabTabItem);

            System.Windows.Shapes.Rectangle rect = new System.Windows.Shapes.Rectangle();
            rect.Width = (double)t.GetProperty("Width").GetValue(_sourceElt, null);
            rect.Height = (double)t.GetProperty("Height").GetValue(_sourceElt, null);
            rect.Fill = new VisualBrush(_sourceElt);
            rect.Opacity = 0.5;
            rect.IsHitTestVisible = false;

            return rect;
        }

        public void OnDropCompleted(IDataObject obj, UIElement sender)
        {
            //FabTabItem fabTabItem = _targetElt as FabTabItem;
            //FabTabControl fabTabControl = this.GetFabTabItemsParent(fabTabItem);

            FabTabItem dropTargetTabItem = sender as FabTabItem;
            FabTabControl fabTabControl = this.GetFabTabItemsParent(sender);
            int indexToInsertAt = fabTabControl.ItemContainerGenerator.IndexFromContainer(dropTargetTabItem);
            if (indexToInsertAt == -1)
            {
                throw new Exception("item not found");
            }

           int index = ExtractElementIndex(obj);

           FabTabItem targetFabTabItem = fabTabControl.ItemContainerGenerator.ContainerFromIndex(index) as FabTabItem;

           if (fabTabControl.Items.Contains(targetFabTabItem))
           {
               fabTabControl.Items.Remove(targetFabTabItem);
               //need a special case here because if there's only 2 items left, removing one
               //removes the contenttab and we have to account for that
               if (fabTabControl.Items.Count == 1 && indexToInsertAt == 2 && fabTabControl.ShowContentsTab)
               {
                   fabTabControl.Items.Add(targetFabTabItem);
               }
               else
               {
                   fabTabControl.Items.Insert(indexToInsertAt, targetFabTabItem);
               }
           }
           else if (fabTabControl.ItemsSource != null)
           {
               IList items = fabTabControl.ItemsSource as IList;
               if (items != null)
               {
                   object content = targetFabTabItem.Content;
                   fabTabControl.ItemsSource = null;
                   items.Remove(content);
                   items.Insert(indexToInsertAt, content);
                   fabTabControl.ItemsSource = items;
               }
           }
           else
           {
               object content = targetFabTabItem.Content;
               fabTabControl.Items.Remove(content);

               //TODO, reduce the duplication with this if block and the one right above.
               if (fabTabControl.Items.Count == 1 && indexToInsertAt == 2 && fabTabControl.ShowContentsTab)
               {
                   fabTabControl.Items.Add(content);
               }
               else
               {
                   //removing the content and re-inserting it causes a new FabTabItem to be generated
                   //therefore anybody subscribing to TabClosing events will no longer receive notifications.
                   //This is only a problem for using Items collection with nonFabTabItems put into it
                   //so that the ItemContainerGenerator itself has to create them.
                   fabTabControl.Items.Insert(indexToInsertAt, content);
               }
           }

           fabTabControl.SelectedIndex = indexToInsertAt;

        }

        #endregion

        private int ExtractElementIndex(IDataObject obj)
        {
            string indexAsString = obj.GetData("FabTabItem") as string;
            return Convert.ToInt32(indexAsString);
        }
    }
}
