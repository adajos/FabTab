using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace FabTab
{
    public interface IContentTabView
    {
        Dictionary<object, object> Views { get; }
        UIElementCollection ChildButtonCollection { get; }
        
    }
}
