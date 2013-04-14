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
using System.Windows.Controls.Primitives;
using System.IO;
using System.Windows.Forms.Integration;

namespace FabTab
{
    /// <summary>
    /// Interaction logic for ContentTabView.xaml
    /// </summary>
    partial class ContentTabView : UserControl, IContentTabView
    {
        private Dictionary<object, object> _views;
        private FabTabControl _tabControl;
        private BitmapSourceBuilder _bitmapBuilder = new BitmapSourceBuilder();
        private ImageButtonFactory _imageButtonFactory;
        private ToolTipBuilder _toolTipBuilder;

        public Dictionary<object, object> Views
        {
            get { return _views; }
        }

        public UIElementCollection ChildButtonCollection
        {
            get { return this.wrapPanel.Children; }
        }
                
        public ContentTabView(FabTabControl tabControl)
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ContentTabView_Loaded);
            _tabControl = tabControl;
            _imageButtonFactory = new ImageButtonFactory(_tabControl, this);
            _toolTipBuilder = new ToolTipBuilder(_tabControl);
        }

        internal void SetViews(Dictionary<object, object> views)
        {
            _views = views;
            UpdateViews();

        }

        void ContentTabView_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateViews();
        }

        private void UpdateViews()
        {
            if (_views != null)
            {
                this.wrapPanel.Children.Clear();
                UpdateViewSnapshots();
            }
        }

        private void UpdateViewSnapshots()
        {
            foreach (object view in _views.Keys)
            {
                FrameworkElement element = view as FrameworkElement;
                element = CheckForHiddenElement(view, element);
                if (ShouldTakeSnapShot(element))
                {
                    BitmapSource bitmapSource = _bitmapBuilder.GetBitmapSourceFromElement(GetElementForSnapshot(element));
                    _toolTipBuilder.SetImageScreenshotOnFabTabItemAttachedProperty(GetElementForSnapshot(element), bitmapSource);
                    wrapPanel.Children.Add(_imageButtonFactory.CreateImageButton(view, bitmapSource));
                }
            }
        }

        private FrameworkElement CheckForHiddenElement(object view, FrameworkElement element)
        {
            if (element == null)
            {
                element = this._tabControl.HiddenContent.ItemContainerGenerator.ContainerFromItem(view) as FrameworkElement;
            }
            return element;
        }


        private bool ShouldTakeSnapShot(FrameworkElement element)
        {
            if (element != null && !(element is ContentTabView))
                return true;
            return false;
        }

        private FrameworkElement GetElementForSnapshot(FrameworkElement element)
        {
            //if we've got a tabitem, then grab it's content for the bitmap
            if (element is FabTabItem)
            {
                FabTabItem fabTab = element as FabTabItem;
                //thanks for the patch for WinForms interop butters877!
                if (fabTab.Content is WindowsFormsHost)
                {
                    return fabTab;
                }
                else
                {
                    return (FrameworkElement)(element as FabTabItem).Content;
                }
            }
            else
            {
                return element;
            }
        }

       
    }
}
