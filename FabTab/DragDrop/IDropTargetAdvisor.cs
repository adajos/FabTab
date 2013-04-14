using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace FabTab.DragDrop
{
    internal interface IDropTargetAdvisor
    {
        UIElement TargetUI { get; set; }

        bool ApplyMouseOffset { get; }
        bool IsValidDataObject(IDataObject obj);
        void OnDropCompleted(IDataObject obj, UIElement sender);
        UIElement GetVisualFeedback(IDataObject obj);
        UIElement GetTopContainer();
    }
}
