using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Animation;
using System.Windows;

namespace FabTab
{
    /// <summary>
    /// A ThicknessAnimation with support for using Penner easing equations to provide
    /// a more natural transition between the From and To values.
    /// Thanks to Josh Smith and Dr WPF for demonstrating how to do this in the Thriple source code
    /// </summary>
    public class EasingMarginAnimation : ThicknessAnimation
    {
        public EasingEquation Equation { get; set; }

        public EasingMarginAnimation(Thickness fromThickness, Thickness toThickness, Duration duration)
        {
            base.From = fromThickness;
            base.To = toThickness;
            base.Duration = duration;
        }

        public EasingMarginAnimation()
        {

        }

        protected override Freezable CreateInstanceCore()
        {
            return new EasingMarginAnimation
            {
                Equation = this.Equation
            };
        }
             
                

        protected override Thickness GetCurrentValueCore(Thickness defaultOriginValue, Thickness defaultDestinationValue, AnimationClock animationClock)
        {
            double time = animationClock.CurrentTime.HasValue ? animationClock.CurrentTime.Value.TotalMilliseconds : 0.0;
            Thickness from = this.From.HasValue ? this.From.Value : defaultOriginValue;
            Thickness to = this.To.HasValue ? this.To.Value : defaultDestinationValue;

            double leftDelta = to.Left - from.Left;
            double topDelta = to.Top - from.Top;
            double rightDelta = to.Right - from.Right;
            double bottomDelta = to.Bottom - from.Bottom;
            
            double duration = this.Duration.TimeSpan.TotalMilliseconds;

            double leftResult = EasingUtilities.CalculateCurrentValue(time, from.Left, leftDelta, duration, this.Equation);
            double topResult = EasingUtilities.CalculateCurrentValue(time, from.Top, topDelta, duration, this.Equation);
            double rightResult = EasingUtilities.CalculateCurrentValue(time, from.Right, rightDelta, duration, this.Equation);
            double bottomResult = EasingUtilities.CalculateCurrentValue(time, from.Bottom, bottomDelta, duration, this.Equation);

            Thickness returnThickness = new Thickness(leftResult, topResult, rightResult, bottomResult);

            return returnThickness;
        }

        
    }
}
