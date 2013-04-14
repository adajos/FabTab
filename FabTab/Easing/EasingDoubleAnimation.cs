using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace FabTab
{
    /// <summary>
	/// A DoubleAnimation with support for using Penner easing equations to provide
	/// a more natural transition between the From and To values.
    /// Thanks to Josh Smith and Dr WPF for demonstrating how to do this in the Thriple source code
	/// </summary>
    public class EasingDoubleAnimation : DoubleAnimation
    {
        public EasingEquation Equation { get; set; }

        protected override Freezable CreateInstanceCore()
        {
            return new EasingDoubleAnimation
            {
                Equation = this.Equation
            };
        }

        protected override double GetCurrentValueCore(double defaultOriginValue, double defaultDestinationValue, AnimationClock animationClock)
        {
            double time = animationClock.CurrentTime.HasValue ? animationClock.CurrentTime.Value.TotalMilliseconds : 0.0;
            double from = this.From.HasValue ? this.From.Value : defaultOriginValue;
            double to = this.To.HasValue ? this.To.Value : defaultDestinationValue;
            double delta = to - from;
            double duration = this.Duration.TimeSpan.TotalMilliseconds;
            return EasingUtilities.CalculateCurrentValue(time, from, delta, duration, this.Equation);
        }
    }
}
