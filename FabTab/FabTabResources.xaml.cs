using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace FabTab
{
    public partial class FabTabResources : ResourceDictionary
    {

        public FabTabResources()
        {
            InitializeComponent();
        }
        
        public ContentControl DefaultContentTabHeader
        {
            get
            {
                return this["DefaultContentTabHeader"] as ContentControl;
            }
        }
    }
}
