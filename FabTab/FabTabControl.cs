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
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Controls.Primitives;
using FabTab.DragDrop;
using FabTab.Extensions;

namespace FabTab
{
    [TemplatePart(Name = "PART_SelectedContentHost", Type = typeof(ContentPresenter))]
    [TemplatePart(Name = "PART_HiddenContentHost", Type = typeof(ContentPresenter))]
    [TemplatePart(Name = "HeaderPanel", Type = typeof(TabPanel))]
    public class FabTabControl : TabControl
    {

        public static readonly DependencyProperty CloseButtonStyleProperty = DependencyProperty.Register(
                        "CloseButtonStyle", typeof(Style), typeof(FabTabControl));

        public static readonly DependencyProperty ContentTabImageButtonStyleProperty = DependencyProperty.Register(
                        "ContentTabImageButtonStyle", typeof(Style), typeof(FabTabControl));

        public static readonly DependencyProperty ContentTabHeaderContentProperty = DependencyProperty.Register(
                        "ContentTabHeaderContent", typeof(FrameworkElement), typeof(FabTabControl));

        public static readonly DependencyProperty ContentTabHeaderComboBoxStyleProperty = DependencyProperty.Register(
                        "ContentTabHeaderComboBoxStyle", typeof(Style), typeof(FabTabControl));

        public static readonly DependencyProperty HiddenContentProperty = DependencyProperty.Register("HiddenContent", typeof(ItemsControl),
                                                                                                            typeof(FabTabControl));

        public static readonly DependencyProperty ShowToolTipImagesProperty = DependencyProperty.Register("ShowToolTipImages", typeof(bool),
                        typeof(FabTabControl), new PropertyMetadata(true));

        public static readonly DependencyProperty ShowDefaultTransitionAnimationProperty = DependencyProperty.Register("ShowDefaultTransitionAnimation", typeof(bool),
            typeof(FabTabControl), new PropertyMetadata(true));

        public static readonly DependencyProperty ContentOpacityMaskProperty = DependencyProperty.Register("ContentOpacityMask", typeof(Brush),
                        typeof(FabTabControl));

        public static readonly DependencyProperty DidSelectionChangeProperty = DependencyProperty.Register("DidSelectionChange", typeof(bool),
                        typeof(FabTabControl));

        private ContentTabView _contentTabView = null;
        private bool _showContentsTab = true;
        private bool _contentsTabAdded;
        private ComboBox _contentsTabComboBox;
        private bool _showTabCloseButtons = true;
        private bool _allowMultiLineTabHeaders = false;
        private bool _itemsChanging = false;
        private bool _allowDragAndDropTabReordering = true;
        private IFabTabItemsStrategy _itemsStrategy;

        
        public bool ShowContentsTab
        {
            get { return _showContentsTab; }
            set { _showContentsTab = value; }
        }

        public bool ShowTabCloseButtons
        {
            get { return _showTabCloseButtons; }
            set { _showTabCloseButtons = value; }
        }

        public bool AllowMultiLineTabHeaders
        {
            get { return _allowMultiLineTabHeaders; }
            set { _allowMultiLineTabHeaders = value; }
        }

        public bool AllowDragAndDropTabReordering
        {
            get { return _allowDragAndDropTabReordering; }
            set { _allowDragAndDropTabReordering = value; }
        }

        public int ItemsCountWithoutContentTab
        {
            get
            {
                if (!ShowContentsTab)
                {
                    return this.Items.Count;
                }

                if (this.Items.Count >= 3)
                {
                    return this.Items.Count - 1;
                }

                return this.Items.Count;
                
            }
        }

        //TODO: rename this to something more meaningful
        internal ItemsControl HiddenContent
        {
            get { return (ItemsControl)this.GetValue(HiddenContentProperty); }
            set
            {
                this.SetValue(HiddenContentProperty, value);
            }
        }

        public Style CloseButtonStyle
        {
            get { return (Style)this.GetValue(CloseButtonStyleProperty); }
            set
            {
                this.SetValue(CloseButtonStyleProperty, value);

            }
        }

        public Style ContentTabImageButtonStyle
        {
            get { return (Style)this.GetValue(ContentTabImageButtonStyleProperty); }
            set
            {
                this.SetValue(ContentTabImageButtonStyleProperty, value);

            }
        }

        public FrameworkElement ContentTabHeaderContent
        {
            get { return (FrameworkElement)this.GetValue(ContentTabHeaderContentProperty); }
            set
            {
                this.SetValue(ContentTabHeaderContentProperty, value);

            }
        }

        public Style ContentTabHeaderComboBoxStyle
        {
            get { return (Style)this.GetValue(ContentTabHeaderComboBoxStyleProperty); }
            set
            {
                this.SetValue(ContentTabHeaderComboBoxStyleProperty, value);

            }
        }

        public bool ShowDefaultTransitionAnimation
        {
            get { return (bool)this.GetValue(ShowDefaultTransitionAnimationProperty); }
            set { this.SetValue(ShowDefaultTransitionAnimationProperty, value); }
        }
        
        /// <summary>
        /// This exposes the OpacityMask of the ContentPresenter in which the FabTabItem.Content is displayed.
        /// Exposing this allows some cool animations to be done on the OpacityMask.
        /// </summary>
        public Brush ContentOpacityMask
        {
            get { return (Brush)this.GetValue(ContentOpacityMaskProperty); }
            set { this.SetValue(ContentOpacityMaskProperty, value); }
        }

        public bool DidSelectionChange
        {
            get { return (bool)this.GetValue(DidSelectionChangeProperty); }
            set { this.SetValue(DidSelectionChangeProperty, value); }
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            if (_itemsStrategy == null)
            {
                SetupItemsStrategy();
            }
            //need the second check only for .NET 4.0 because this method gets called way more often for some reason.
            if (item is ContentTabView && !(this.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated))
            {
                _contentTabView = item as ContentTabView;
            }
            return (item is FabTabItem);
        }
                        

        public bool ShowToolTipImages
        {
            get
            {
                return (bool)this.GetValue(ShowToolTipImagesProperty);
            }
            set
            {
                this.SetValue(ShowToolTipImagesProperty, value);
            }

        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            //Added some extra _contentTabview stuff in this method in the if conditional
            //as well as nulling it out outside of the if conditional.  Also had to 
            //null out _contentTabView in OnSelectionChanged.  All these changes were necessary
            //because of some differing behavior in .NET 4.0 beta for when IsItemsItsOwnContainer
            //gets called which caused me state issues with_contentTabView.
            if (_itemsStrategy == null)
            {
                SetupItemsStrategy();
            }
            
            if (_contentTabView != null)
            {
                ContentTabItem tabItem = CreateContentTabItem();
                _contentTabView = null;
                return tabItem;
            }

            return CreateNewFabTabItemAndWireUpClosingEvent();
        }

        private DependencyObject CreateNewFabTabItemAndWireUpClosingEvent()
        {
            FabTabItem newClosableTabItem = new FabTabItem();
            newClosableTabItem.TabClosing += new RoutedEventHandler(newClosableTabItem_TabClosing);
            return newClosableTabItem;
        }

        private ContentTabItem CreateContentTabItem()
        {
            ContentTabItem tabItem = new ContentTabItem();
            if (this.ContentTabHeaderContent != null)
            {
                tabItem.Header = this.ContentTabHeaderContent;
            }
            else
            {
                FabTabResources rd = new FabTabResources();
                tabItem.Header = rd.DefaultContentTabHeader;
            }
            return tabItem;
        }

        void newClosableTabItem_TabClosing(object sender, RoutedEventArgs e)
        {
            //TODO: see if this check is redundant
            if (!e.Handled)
            {
                FabTabItem closingTab = e.OriginalSource as FabTabItem;
                GiveOtherEventHandlersAChanceToCancelTabClose(e, closingTab);

                if (closingTab != null && !e.Handled)
                {
                    this.CloseTab(closingTab);
                    e.Handled = true;
                    ResetDragDropAdvisors();
                }
            }
        }

        private void ResetDragDropAdvisors()
        {
            if (this.SelectedItem != null)
            {
                FabTabItem lastTabItem = this.ItemContainerGenerator.ContainerFromIndex(this.Items.Count - 1) as FabTabItem;
                if (lastTabItem != null)
                {
                    //when a tab closes that happens be the source or target on either of the advisors
                    //it will disallow further drag and drop operations.  therefore whenever a tab closes
                    //reset this to something valid, in this case, the last item in the collection.
                    DragDropManager.GetDragSourceAdvisor(lastTabItem).SourceUI = lastTabItem;
                    DragDropManager.GetDropTargetAdvisor(lastTabItem).TargetUI = lastTabItem;

                }
            }
        }

        private void GiveOtherEventHandlersAChanceToCancelTabClose(RoutedEventArgs e, FabTabItem closingTab)
        {
            //first unwire the handler we are currently in from the event,
            //then raise the event from the tab again so that any subscribers beyond FabTabControl
            //in the chain can have the event handlers invoked first so they can choose to mark it handled
            //thereby cancelling the tab close.
            //Then rewire this handler to the TabClosing event.
            //This seems a bit hackish, but in the case of views that were added to the FabTabControl's
            //Items collection in XAML, by the time in codebehind that the consumer can subscribe to the
            //TabClosing event, the FabTabControl has already wired up this eventhandler.
            closingTab.RemoveHandler(e.RoutedEvent, new RoutedEventHandler(newClosableTabItem_TabClosing));
            closingTab.RaiseEvent(e);
            closingTab.AddHandler(e.RoutedEvent, new RoutedEventHandler(newClosableTabItem_TabClosing));
        }


        static FabTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FabTabControl), new FrameworkPropertyMetadata(typeof(FabTabControl)));
        }

        public FabTabControl()
        {
            this.Loaded += new RoutedEventHandler(FabTabControl_Loaded);
            CreateHiddenListViewForRenderingUnselectedItems();
            CreateDefaultContentOpacityMask();
        }

        private void CreateDefaultContentOpacityMask()
        {
            //set default opacity mask if the user hasn't set one,
            //that way there's something there for the default transitional animation to 
            //work on.
            if (ContentOpacityMask == null)
            {
                SolidColorBrush defaultBrush = new SolidColorBrush(Colors.White);
                defaultBrush.Opacity = 1;
                ContentOpacityMask = defaultBrush;
            }
        }

        private void CreateHiddenListViewForRenderingUnselectedItems()
        {
            ItemsControl hiddenItems = new ListView();
            FrameworkElementFactory factory = new FrameworkElementFactory(typeof(StackPanel));
            hiddenItems.ItemsPanel = new ItemsPanelTemplate(factory);
            this.HiddenContent = hiddenItems;
        }


        protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //this is a little hackish, but this boolean is needed for performance reasons.
            //See comments in OnSelectionChanged method
            _itemsChanging = true;

            //If the ItemsSource is set to ObservableCollection<T> by the developer, this method
            //will fire anytime an item is added to the collection.  If it's set to something
            //that's not Obvservable, it will only fire when ItemsSource is nulled out.
            //This matters quite alot in terms of adding ContentsTab when appropriate.
            //Closing tabs does fire this method because we null the itemssource out.
            //This method DOES get fired for just declaring tabitems or usercontrols
            //in the tab control in XAML though, because CollectionView must support notification.
            base.OnItemsChanged(e);

            //need to make sure that newly added or removed views are taken out of their hidden place
            //in the visual tree
            ForceItemRender();
                                    
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                GetAddedFabTabItemAndWireUpClosingEvent(e);
                AddContentsTabIfNecessary();
                UpdateContentsTabDropdownIfNecessary();
                UpdateContentsTabViewsIfNecessary();
                AutoSelectFirstItemAdded(e);
            }

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                HideContentsTabIfNecessary();
            }

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
            {
                if (this.Items.Count > 0 && _itemsStrategy != null)
                {
                    UpdateContentsTabDropdownIfNecessary();
                }
            }

            _itemsChanging = false;
            
        }

        private void SetupItemsStrategy()
        {
            if (this.ItemsSource != null)
            {
                _itemsStrategy = new ItemsSourceStrategy(this);
            }
            else if (this.Items != null)
            {
                _itemsStrategy = new ItemsStrategy(this);
            }
        }

        private void HideContentsTabIfNecessary()
        {
            if (ShouldHideContentsTab())
            {
                _contentsTabAdded = false;
                RemoveContentsTab();
            }
            else
            {
                UpdateContentsTabDropdownIfNecessary();
                UpdateContentsTabViewsIfNecessary();
            }
        }

        private bool ShouldHideContentsTab()
        {
            return ShowContentsTab && _contentsTabAdded && this.Items.Count <= 2;
        }

        private void AutoSelectFirstItemAdded(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //if nothing selected and only one item in the tab, then select it.
            if (this.Items.Count == 1 && this.SelectedItem == null)
            {
                this.SelectedItem = e.NewItems[0];
            }
        }

        private void GetAddedFabTabItemAndWireUpClosingEvent(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            FabTabItem newClosableTabItem = this.FindFabTabFromItem(e.NewItems[0]);
            if (newClosableTabItem != null)
            {
                newClosableTabItem.TabClosing += new RoutedEventHandler(newClosableTabItem_TabClosing);
            }
        }

        private void UpdateContentsTabViewsIfNecessary()
        {
            if (ShowContentsTab && _contentsTabAdded && this.Items.Count > 1)
            {
                ContentTabView view = this.Items[0] as ContentTabView;
                if (view != null)
                {
                    view.SetViews(BuildDictionaryOfViewsAndHeaders());
                }
            }
        }

        private Dictionary<object, object> BuildDictionaryOfViewsAndHeaders()
        {
            Dictionary<object, object> namedContentList = new Dictionary<object, object>();
            foreach (object item in this.Items)
            {
                FabTabItem tabItem = this.FindFabTabFromItem(item);
                if (tabItem != null)
                {
                    namedContentList.Add(item, tabItem.Header);
                }
            }
            return namedContentList;
        }

        private void UpdateContentsTabDropdownIfNecessary()
        {
            //How do we do this if the header isn't merely string content????
            if (ShowContentsTab && _contentsTabAdded)
            {
                IfItemContainersNotGeneratedCallThisMethodAgainLater();
                UpdateContentsTabDropdown();
            }
        }

        private void UpdateContentsTabDropdown()
        {
            ContentTabItem contentTab = this.ItemContainerGenerator.ContainerFromIndex(0) as ContentTabItem;
            if (contentTab != null)
            {
                contentTab.ApplyTemplate();
                _contentsTabComboBox = contentTab.Template.FindName("PART_tabHeaderComboBox", contentTab) as ComboBox;
                _contentsTabComboBox.SelectionChanged -= new SelectionChangedEventHandler(combo_SelectionChanged);
                _contentsTabComboBox.Items.Clear();
                if (_contentsTabComboBox != null)
                {
                    this.Items.ForEach(AddFabTabItemHeaderIntoContentTabDropDown);
                    //now wire up to the combo to select tabs
                    _contentsTabComboBox.SelectionChanged += new SelectionChangedEventHandler(combo_SelectionChanged);
                }
            }
        }

        private void AddFabTabItemHeaderIntoContentTabDropDown(object item)
        {
            if (!(item is ContentTabView || item is ContentTabItem))
            {
                FabTabItem tabItem = this.FindFabTabFromItem(item);
                if (tabItem != null)
                {
                    InsertUniqueHeader(tabItem);
                }
            }
        }

        private void IfItemContainersNotGeneratedCallThisMethodAgainLater()
        {
            if (this.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
            {
                this.ItemContainerGenerator.StatusChanged += new EventHandler(ItemContainerGenerator_StatusChanged);
            }
        }

        void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            if (this.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
            {
                this.ItemContainerGenerator.StatusChanged -= new EventHandler(ItemContainerGenerator_StatusChanged);
                this.UpdateContentsTabDropdownIfNecessary();
            }
        }

        private void InsertUniqueHeader(TabItem tabItem)
        {
            //add a a dictionary entry so combobox.selectedindex returns
            //the correct value, therefore make sure everything is unique to
            //handle the cases of identical headers
            DictionaryEntry e = new DictionaryEntry(Guid.NewGuid(), tabItem.Header);
            _contentsTabComboBox.Items.Add(e);
        }

        void combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (_contentsTabComboBox != null)
            {
                //select the corresponding tab, which will be one off from the combobox
                //because of the ContentTab.
                this.SelectedIndex = _contentsTabComboBox.SelectedIndex + 1;
                _contentsTabComboBox.SelectionChanged -= new SelectionChangedEventHandler(combo_SelectionChanged);
                _contentsTabComboBox.SelectedIndex = -1;
                _contentsTabComboBox.SelectionChanged += new SelectionChangedEventHandler(combo_SelectionChanged);
            }
        }

        private void AddContentsTabIfNecessary()
        {
            if (ShowContentsTab && !_contentsTabAdded && this.Items.Count > 1)
            {
                _contentsTabAdded = true;
                AddContentsTab();
            }
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            if (!_itemsChanging)
            {
                DidSelectionChange = true;
            }
            //have to check _itemsChanging for performance reasons, otherwise this method
            //gets called when we force renders for adding/removing tabs, and then we're constantly'
            //updating the snapshots WAY more than we need to.
            if (this.ShowContentsTab && !_itemsChanging)
            {
                _itemsStrategy.JuggleHiddenContent(e, args => base.OnSelectionChanged(args));
                
                //this only needs to happen if we are in this method because the user actually
                //change the tab they selected, and not when new items are added or removed
                //to the collection and ForceRenderItems  (which forces selection of all items)
                //is called from OnItemsChanged---thus the _itemsChanging check above
                UpdateContentsTabViewsIfNecessary();
            }
            
            base.OnSelectionChanged(e);

            DidSelectionChange = false;
        }

        void FabTabControl_Loaded(object sender, RoutedEventArgs e)
        {
            AddContentsTabIfNecessary();
            if (this.ShowContentsTab)
            {
                ForceItemRender();
                UpdateContentsTabViewsIfNecessary();
            }

        }

        private void ForceItemRender()
        {
            if(_itemsStrategy != null)
                _itemsStrategy.ForceItemsRender();
        }
        
        private void RemoveContentsTab()
        {
            if(_itemsStrategy != null)
                _itemsStrategy.RemoveContentsTab();
        }

        private void AddContentsTab()
        {
            if(_itemsStrategy != null)
                _itemsStrategy.AddContentsTab();
        }

        internal object GetContentsTab()
        {
            ContentTabView contentTabView = new ContentTabView(this);
            return contentTabView;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this.ContentTabHeaderComboBoxStyle == null && BindingOperations.GetBinding(this, FabTabControl.ContentTabHeaderComboBoxStyleProperty) == null)
            {
                this.ContentTabHeaderComboBoxStyle = this.TryFindResource("ComboBoxDefaultStyle") as Style;
            }
            
            //wire up the tabitems wired up directly in xaml
            if (this.Items != null)
            {
                this.Items.ForEach(WireTabClosingEvent);
            }
            SetMultiLineTabHeadersOnHeaderPanel();
        }

        private void WireTabClosingEvent(object item)
        {
            FabTabItem tabItem = item as FabTabItem;
            if (tabItem != null)
            {
                tabItem.TabClosing += new RoutedEventHandler(newClosableTabItem_TabClosing);
            }
        }

        
        private void SetMultiLineTabHeadersOnHeaderPanel()
        {
            //configure the FabTabPanel appropriately, which would mean following the default
            //out of the box WPF TabPanel behavior (which doesn't always look too good with ContentTab
            if (this.AllowMultiLineTabHeaders)
            {
                FabTabPanel tabPanel = this.Template.FindName("HeaderPanel", this) as FabTabPanel;
                if (tabPanel != null)
                {
                    tabPanel.AllowMultiLineTabHeaders = true;
                }

            }
        }

        #region IClosableTabHost Members

        public void CloseTab(TabItem tab)
        {
            if (tab != null && _itemsStrategy != null)
            {
                _itemsStrategy.CloseTab(tab);
            }
        }

        #endregion
    }
}
