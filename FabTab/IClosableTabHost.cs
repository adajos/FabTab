using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace FabTab
{
    internal interface IClosableTabHost
    {
        void CloseTab(TabItem tab);

        bool ShowTabCloseButtons { get; }
    }
}
