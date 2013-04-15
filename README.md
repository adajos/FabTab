FabTab
======

WPF custom tab control
------
FabTab is a subclass of the WPF TabControl with many extra features: tab close buttons, IE-style "QuickTab" with screenshots of all tabitem's content, tab ToolTips are live screenshots of tab content, drag and drop to reorder tabs, transition animations selected tab changes.

FabTab originally lived on Codeplex, and to see screenshots of the sample app in action and older forum discussions, you can go [there.](http://fabtab.codeplex.com)

Most WPF apps that use the standard TabControl can just swap that out for the FabTabControl since it is a subclass of the WPF TabControl. The FabTabControl supports directly adding to the Items collection, either by adding WPF elements directly, or inside of a FabTabItem (which is a subclass of WPF TabItem). Additionally, you can bind a collection of data to the ItemsSource of the FabTabControl--however, *this will only work if you are binding to an ObservableCollection* (so that FabTab receives change notification to know when to show or hide it's ContentTab).

There is a sample project that comes in the download with the FabTab source itself, this sample shows how to use FabTab and demonstrates all the unique features to FabTab. Using the SampleProject is the fastest way to get a feel for what FabTab can do.

Feature Explanation
------

###Close Tabs
By default the FabTabControl will display a close button in the upper right corner of each FabTabItem that is rendered inside of it. This behavior can be turned off, simply by setting the ShowTabCloseButtons property on the FabTabControl to false. The Close button is completely styleable and the default style can be changed by setting the CloseButtonStyle property on the FabTabControl. When the close button on a FabTabItem header is clicked a TabClosing event is fired. Any subscribers to this event will have their event handler called before the tab actually closes, and can choose to cancel the closing event. To cancel the event, e.Handled must be set to True in the event handler. This is useful for a common scenario where a user has changed some data on the screen and then tries to close the tab without saving it. In such a case the Window that contains the tab, or perhaps even the content of the FabTabItem itself could subscribe to the TabClosing event, and display a dialog box that allows the user to click "Yes" to close the tab without saving their changes or "No", to cancel the TabClosing event and save their changes first. 

This feature is fully demonstrated in SampleProject in the CancelCloseEventWindow.xaml and codebehind. Cancelling a TabClosing event requires a reference to the FabTabItem--which can be in done in XAML or codebehind. If done on a FabTabControl via the ItemsSource it would need to be done in codebehind, likely via the ItemContainerGenerator.

###ContentTab, similar to Internet Explorer "QuickTab"
FabTabControl supports a "ContentTab", which is special tab that is visible only when two or more TabItems are in the FabTabControl. The ContentTab provides a polished and easy way for users to view, close, or find a specific tab. In the header of the ContentTab there is a dropdownlist that displays the contents of all the tab headers. Selecting an item from the dropdownlist immediately selects that tab so it's content is visible. Additionally, clicking on the ContentTab's header takes the user to a special view that is essentially a wrap panel with ImageButtons inside of it. Each ImageButton's content is a smallish screenshot of each FabTabItem's content. The user has the option of clicking on the ImageButton to instantly select that tab, or click on the close button in the upper right corner of the ImageButton to close the tab (Note that turning off setting ShowTabCloseButton to false on the FabTabControl hides not only the close button in the tab headers, but also the close buttons on the ContentTab view). 

The Style (including size) of the ImageButtons in the ContentTabView has a reasonable default but can be specified by setting the ContentTabImageButtonStyle property on the FabTabControl. Additionally the image in the ContentTab's header as well as the ComboBox in its header have reasonable default styles, but can be customized by setting the ContentTabHeaderContent and ContentTabHeaderComboBoxStyle properties respectively on the FabTabControl. Additionally, the ContentTab can be turned off altogether by setting the ShowContentsTab property on FabTabControl to false. 

One additional item of note is that when using the ItemsSource property of the FabTabControl to populate the tabs, sometimes the screenshots are a little bit different than they are actually displayed. For instance, the LastChildFill property on a DockPanel doesn't necessarily get reflected. This can usually be circumvented by setting reasonable MinWidth and MinHeight sizes on the WPF elements displayed within a FabTabItem's content field--this ensures a certain amount of similarity between the screenshots when using ItemsSource. See Window1.xaml and it's codebehind in the sample project to see this in action.

![ContentTab](http://i3.codeplex.com/Download?ProjectName=fabtab&DownloadId=86256 "ContentTab")

###Single row or multi-row tab header layout
The FabTabControl supports a single row tab header layout by default. This is because using the standard multi-row tab header layout out the WPF TabControl provides can be kind of confusing with a ContentTab because the location of the ContentTab changes when the number of tab header rows changes. The AllowMultiLineTabHeaders property on the FabTabControl can be set to true to get the typical WPF TabControl behavior however. The single row tab header behavior is only noticeable when are more tabheaders than there is room to fully display. It narrows the width of all FabTabItem headers except for the ContentTab header and the header of the currently-selected tab. This results in a truncation of the content in most of the FabTabItem headers, but this is mitigated by the fact that the full headers are shown in the dropdown list of the ContentTab header.

![NarrowTab](http://i3.codeplex.com/Download?ProjectName=fabtab&DownloadId=86258 "Narrow Tabs")

###Rich FabTabItem ToolTips
Hovering the cursor over the header for a FabTabItem that is not selected (except for the ContentTab header) displays a ToolTip which contains a screenshot of the contents for that particular tab. This feature can be turned off if desired by setting the ShowToolTipImages on FabTabControl to false.

![RichToolTip](http://i3.codeplex.com/Download?ProjectName=fabtab&DownloadId=86259 "Rich ToolTip")

###Drag and Drop To Reorder Tabs
By default drag and drop is turned on for all FabTabItems (but not the ContentTab) in FabTabControl. This can be turned off by setting the AllowDragAndDropTabReordering property on the FabTabControl to false or by customizing the ControlTemplate for FabTabItem to not set the DragSource and DropTarget advisors. Drag and Drop may not play well with fancy custom transition animations because it may cause the transition animation(s) to fire since it affects the selected tab. Major thanks to Pavan Podila and his book WPF Control Development Unleashed for guidance on how to implement drag and drop.

###Transitional Animations When the Selected Tab Changes
By default a simple "Fade In" transitional animation is used when the user selects a different FabTabItem. In SampleProject this animation is shown in all windows except for the initial window and the ItemsSource window (AKA Window1.xaml). To turn this animation off, set the ShowDefaultTransitionAnimation property on FabTabControl to false. You will certainly want to do this if you will be using your own custom transition animation.

Custom animations can be easily done in two ways: Apply an OpacityMask animation on FabTabControl's new ContentOpacityMask property (this is ultimately set to the OpacityMask of the FabTabControl's ContentPresenter) or by setting an animation to some property (say the Margin) in a style trigger for FabTabItem when it is selected. An example of the former method is used in TabItemsWindow.xaml (main window of the sample project) and the latter method is shown in Window1.xaml (ItemsSource window of the sample project).

Also of note are a couple of new Easing animation that provide non-linear more natural animations. There is an EasingDoubleAnimation and an EasingMarginAnimation that can be used to this end. The Equation property on these classes can be set to the various enum values on the EasingEquation class to produce different effects. Major thanks to Josh Smith and Dr Wpf and their Thriple project for showing me how to do this in WPF 3 (WPF 4 has this support built in.) Also major thanks to the Transitionals project for its inspiration on adding this feature to my control.
