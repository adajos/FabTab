using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace FabTab.DragDrop
{
    internal interface IDragSourceAdvisor
    {
        UIElement SourceUI { get; set; }

        DragDropEffects SupportedEffects { get; }

        DataObject GetDataObject(UIElement draggedElt);
        void FinishDrag(UIElement draggedElt, DragDropEffects finalEffects);
        bool IsDraggable(UIElement dragElt);
        UIElement GetTopContainer();
    }
}
