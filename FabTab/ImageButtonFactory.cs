using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FabTab
{
    internal class ImageButtonFactory
    {
        private FabTabControl _tabControl;
        private IContentTabView _contentTabView;

        public ImageButtonFactory(FabTabControl tabControl, IContentTabView contentTabView)
        {
            _tabControl = tabControl;
            _contentTabView = contentTabView;
        }

        public ImageButton CreateImageButton(object view, BitmapSource bitmapSource)
        {
            ImageButton imageButton = null;
            Image newImage = CreateImageForButton(bitmapSource);
            if (newImage != null)
            {
                imageButton = new ImageButton();
                imageButton.Content = newImage;

                imageButton.Style = _tabControl.ContentTabImageButtonStyle;
                object title = null;
                if (_contentTabView.Views.TryGetValue(view, out title))
                {
                    imageButton.Title = title.ToString();
                }

                imageButton.Click += new RoutedEventHandler(imageButton_Click);
                imageButton.ApplyTemplate();
                SetupCloseButton(view, imageButton);
            }
            return imageButton;

        }

        private Image CreateImageForButton(BitmapSource bitmapSource)
        {
            Image image = new Image();
            image.Source = bitmapSource;
            image.Height = 150;
            //changed to UniformToFill because Uniform caused weird behavior if entire windows
            //were resized
            image.Stretch = Stretch.UniformToFill;
            return image;
        }


        private void SetupCloseButton(object view, ImageButton imageButton)
        {
            Button closeButton = imageButton.Template.FindName("PART_CloseButton", imageButton) as Button;
            FabTabItem currentItem = view as FabTabItem;
            if (currentItem == null)
            {
                currentItem = _tabControl.ItemContainerGenerator.ContainerFromItem(view) as FabTabItem;
            }
            if (currentItem.ShowCloseButton)
            {
                closeButton.Click += new RoutedEventHandler(closeButton_Click);
                closeButton.Command = new RoutedCommand();
                CommandBinding b = new CommandBinding(closeButton.Command, new ExecutedRoutedEventHandler(imageButtonCommandExecuted));
                imageButton.CommandBindings.Add(b);
            }
            else
            {
                closeButton.Visibility = Visibility.Hidden;
            }
        }

        void imageButtonCommandExecuted(object sender, ExecutedRoutedEventArgs args)
        {
            ImageButton imageButton = sender as ImageButton;
            if (imageButton != null)
            {
                int indexOfClickedButton = _contentTabView.ChildButtonCollection.IndexOf(imageButton);
                FabTabItem tabItem = _tabControl.ItemContainerGenerator.ContainerFromIndex(indexOfClickedButton + 1) as FabTabItem;
                if (tabItem != null)
                {
                    tabItem.OnRaiseTabClosingEvent(new RoutedEventArgs(FabTabItem.TabClosingEvent, tabItem));
                }
            }
        }

        void closeButton_Click(object sender, RoutedEventArgs e)
        {
            //set handled equal to tree so it doesn't keep going up the visual tree
            //and hit the imageButton_Click handler
            e.Handled = true;
        }

        void imageButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = e.OriginalSource as Button;
            if (clickedButton != null)
            {
                int indexOfClickedButton = _contentTabView.ChildButtonCollection.IndexOf(clickedButton);
                //add one to the index to account for the ContentTab being an item
                //in the tab control itself.
                _tabControl.SelectedIndex = indexOfClickedButton + 1;

            }

        }
    }
}
