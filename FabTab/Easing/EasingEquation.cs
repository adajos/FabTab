using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FabTab
{
    /// <summary>
    /// Identifiers for the easing equations made available by methods in the EasingEquations class.
    /// Thanks to Josh Smith and Dr WPF for demonstrating how to do this in the Thriple source code
    /// </summary>
    public enum EasingEquation
    {
        Linear = 0,

        BackEaseIn,
        BackEaseInOut,
        BackEaseOut,

        BounceEaseIn,
        BounceEaseInOut,
        BounceEaseOut,

        CircEaseIn,
        CircEaseInOut,
        CircEaseOut,
        CircEaseOutIn,

        CubicEaseIn,
        CubicEaseInOut,
        CubicEaseOut,

        ElasticEaseIn,
        ElasticEaseInOut,
        ElasticEaseOut,

        ExpoEaseIn,
        ExpoEaseInOut,
        ExpoEaseOut,

        QuadEaseIn,
        QuadEaseInOut,
        QuadEaseOut,

        QuartEaseIn,
        QuartEaseInOut,
        QuartEaseOut,

        QuintEaseIn,
        QuintEaseInOut,
        QuintEaseOut,

        SineEaseIn,
        SineEaseInOut,
        SineEaseOut,
    }
}
