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
using System.Windows.Media.Animation;

namespace FabTab
{
    [TemplatePart(Name = "Content", Type = typeof(ContentPresenter))]
    [TemplatePart(Name = "PART_closeButton", Type = typeof(Button))]
    /// <summary>
    /// </summary>
    public class FabTabItem : TabItem
    {

        public static readonly DependencyProperty ShowCloseButtonProperty = DependencyProperty.Register("ShowCloseButton", typeof(bool),
                        typeof(FabTabItem), new PropertyMetadata(true));

        public static readonly RoutedEvent TabClosingEvent = EventManager.RegisterRoutedEvent("TabClosing", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(FabTabItem));

        public event RoutedEventHandler TabClosing
        {
            add { AddHandler(TabClosingEvent, value); }
            remove { RemoveHandler(TabClosingEvent, value); }
        }
        
        private Button closeTabItemButton;

        //public so we can set this guy via binding in XAML
        //also allows more granularity for showing close buttons if so desired.
        public bool ShowCloseButton
        {
            get
            {
                return (bool)this.GetValue(ShowCloseButtonProperty);
            }
            set
            {
                this.SetValue(ShowCloseButtonProperty, value);
            }

        }
        
        static FabTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FabTabItem), new FrameworkPropertyMetadata(typeof(FabTabItem)));
        }

        public FabTabItem()
        {
            this.Loaded += new RoutedEventHandler(FabTabItem_Loaded);
            
        }

        //protected override void OnSelected(RoutedEventArgs e)
        //{
        //    FrameworkElement content = this.Content as FrameworkElement;
        //    base.OnSelected(e);

        //    Storyboard sb = new Storyboard();
        //    sb.AutoReverse = false;

        //    TimeSpan ts = new TimeSpan(0, 0, 0, 0, 750);
        //    Duration d = new Duration(ts);

        //    EasingMarginAnimation marginAnimation;

        //    marginAnimation = new EasingMarginAnimation(new Thickness(content.DesiredSize.Width, 0, 0, 0), new Thickness(0, 0, 0, 0), d);


        //    //could use easing functions but only in WPF4 or higher, so we have to do it via a custom easing animation
        //    marginAnimation.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("Margin"));
        //    marginAnimation.SetValue(Storyboard.TargetProperty, content);
        //    sb.Children.Add(marginAnimation);
        //    marginAnimation.Equation = EasingEquation.BounceEaseIn;
        //    //marginAnimation.Equation = EasingEquation.CircEaseIn;
        //    //marginAnimation.Equation = EasingEquation.ElasticEaseOut;
        //    //marginAnimation.Equation = EasingEquation.SineEaseOut;

        //    sb.Begin();

        //}

      
        void FabTabItem_Loaded(object sender, RoutedEventArgs e)
        {
            //need to unwire loaded otherwise we can wire up our event handler for click
            //on the close button multiple times if we are dragging and dropping tabitems.
            this.Loaded -= new RoutedEventHandler(FabTabItem_Loaded);
            if (ShowCloseButton)
            {
                if (closeTabItemButton != null)
                {
                    //closeTabItemButton.Click -= new RoutedEventHandler(closeTabItemButton_Click);
                    closeTabItemButton.Click += new RoutedEventHandler(closeTabItemButton_Click);
                }
            }
            else
            {
                closeTabItemButton.Visibility = Visibility.Hidden;
            }
        }

        public override void OnApplyTemplate()
        {
            //important to call the base one first
            base.OnApplyTemplate();
            closeTabItemButton = this.Template.FindName("PART_closeButton", this) as Button;
        }
        
        void closeTabItemButton_Click(object sender, RoutedEventArgs e)
        {
            this.OnRaiseTabClosingEvent(new RoutedEventArgs(TabClosingEvent, this));
        }

        internal virtual void OnRaiseTabClosingEvent(RoutedEventArgs e)
        {
            this.RaiseEvent(e);
        }
                
    }

    
}
