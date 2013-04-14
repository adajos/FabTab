using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FabTab
{
    /// <summary>
    /// This class contains easing equations created by Robert Penner.
    /// </summary>
    public static class EasingEquations
    {
        public static double EaseOutElastic(double time, double startVal, double newVal, double duration)
        {
            if ((time /= duration) == 1) return startVal + newVal;
            double p = duration * .3;
            double s = p / 4;
            return (newVal * Math.Pow(2, -10 * time) * Math.Sin((time * duration - s) * (2 * Math.PI) / p) + newVal + startVal);
        }

        public static double EaseInElastic(double time, double startVal, double newVal, double duration)
        {
            if ((time /= duration) == 1) return startVal + newVal;
            double p = duration * .3;
            double s = p / 4;
            return -(newVal * Math.Pow(2, 10 * (time -= 1)) * Math.Sin((time * duration - s) * (2 * Math.PI) / p)) + startVal;
        }

        public static double EaseInOutElastic(double time, double startVal, double newVal, double duration)
        {
            if ((time /= duration / 2) == 2) return startVal + newVal;
            double p = duration * (.3 * 1.5);
            double s = p / 4;
            if (time < 1) return -.5 * (newVal * Math.Pow(2, 10 * (time -= 1)) * Math.Sin((time * duration - s) * (2 * Math.PI) / p)) + startVal;
            return newVal * Math.Pow(2, -10 * (time -= 1)) * Math.Sin((time * duration - s) * (2 * Math.PI) / p) * .5 + newVal + startVal;
        }

        public static double EaseOutBounce(double time, double startVal, double newVal, double duration)
        {
            if ((time /= duration) < (1 / 2.75)) return newVal * (7.5625 * time * time) + startVal;
            else if (time < (2 / 2.75)) return newVal * (7.5625 * (time -= (1.5 / 2.75)) * time + .75) + startVal;
            else if (time < (2.5 / 2.75)) return newVal * (7.5625 * (time -= (2.25 / 2.75)) * time + .9375) + startVal;
            else return newVal * (7.5625 * (time -= (2.625 / 2.75)) * time + .984375) + startVal;
        }

        public static double EaseInBounce(double time, double startVal, double newVal, double duration)
        {
            if ((time /= duration) < (1 / 2.75)) return newVal * (7.5625 * time * time) + startVal;
            else if (time < (2 / 2.75)) return newVal * (7.5625 * (time -= (1.5 / 2.75)) * time + .75) + startVal;
            else if (time < (2.5 / 2.75)) return newVal * (7.5625 * (time -= (2.25 / 2.75)) * time + .9375) + startVal;
            else return newVal * (7.5625 * (time -= (2.625 / 2.75)) * time + .984375) + startVal;
        }

        public static double EaseInOutBounce(double time, double startVal, double newVal, double duration)
        {
            if (time < duration / 2) return EaseInBounce(time * 2, 0, newVal, duration) * .5 + startVal;
            else return EaseOutBounce(time * 2 - duration, 0, newVal, duration) * .5 + newVal * .5 + startVal;
        }

        public static double EaseOutExpo(double time, double startVal, double newVal, double duration)
        {
            return (time == duration) ? startVal + newVal : newVal * (-Math.Pow(2, -10 * time / duration) + 1) + startVal;
        }

        public static double EaseInExpo(double time, double startVal, double newVal, double duration)
        {
            return (time == 0) ? startVal : newVal * Math.Pow(2, 10 * (time / duration - 1)) + startVal;
        }

        public static double EaseInOutExpo(double time, double startVal, double newVal, double duration)
        {
            if (time == 0) return startVal;
            if (time == duration) return startVal + newVal;
            if ((time /= duration / 2) < 1) return newVal / 2 * Math.Pow(2, 10 * (time - 1)) + startVal;
            return newVal / 2 * (-Math.Pow(2, -10 * --time) + 2) + startVal;
        }

        public static double EaseOutQuad(double time, double startVal, double newVal, double duration)
        {
            return -newVal * (time /= duration) * (time - 2) + startVal;
        }

        public static double EaseInQuad(double time, double startVal, double newVal, double duration)
        {
            return newVal * (time /= duration) * time + startVal;
        }

        public static double EaseInOutQuad(double time, double startVal, double newVal, double duration)
        {
            if ((time /= duration / 2) < 1) return newVal / 2 * time * time + startVal;
            return -newVal / 2 * ((--time) * (time - 2) - 1) + startVal;
        }

        public static double EaseOutSine(double time, double startVal, double newVal, double duration)
        {
            return newVal * Math.Sin(time / duration * (Math.PI / 2)) + startVal;
        }

        public static double EaseInSine(double time, double startVal, double newVal, double duration)
        {
            return -newVal * Math.Cos(time / duration * (Math.PI / 2)) + newVal + startVal;
        }

        public static double EaseInOutSine(double time, double startVal, double newVal, double duration)
        {
            if ((time /= duration / 2) < 1) return newVal / 2 * (Math.Sin(Math.PI * time / 2)) + startVal;
            return -newVal / 2 * (Math.Cos(Math.PI * --time / 2) - 2) + startVal;
        }

        public static double EaseOutCirc(double time, double startVal, double newVal, double duration)
        {
            return newVal * Math.Sqrt(1 - (time = time / duration - 1) * time) + startVal;
        }

        public static double EaseInCirc(double time, double startVal, double newVal, double duration)
        {
            return -newVal * (Math.Sqrt(1 - (time /= duration) * time) - 1) + startVal;
        }

        public static double EaseInOutCirc(double time, double startVal, double newVal, double duration)
        {
            if ((time /= duration / 2) < 1) return -newVal / 2 * (Math.Sqrt(1 - time * time) - 1) + startVal;
            return newVal / 2 * (Math.Sqrt(1 - (time -= 2) * time) + 1) + startVal;
        }

        public static double EaseOutCubic(double time, double startVal, double newVal, double duration)
        {
            return newVal * ((time = time / duration - 1) * time * time + 1) + startVal;
        }

        public static double EaseInCubic(double time, double startVal, double newVal, double duration)
        {
            return newVal * (time /= duration) * time * time + startVal;
        }

        public static double EaseInOutCubic(double time, double startVal, double newVal, double duration)
        {
            if ((time /= duration / 2) < 1) return newVal / 2 * time * time * time + startVal;
            return newVal / 2 * ((time -= 2) * time * time + 2) + startVal;
        }

        public static double EaseOutQuint(double time, double startVal, double newVal, double duration)
        {
            return newVal * ((time = time / duration - 1) * time * time * time * time + 1) + startVal;
        }

        public static double EaseInQuint(double time, double startVal, double newVal, double duration)
        {
            return newVal * (time /= duration) * time * time * time * time + startVal;
        }

        public static double EaseInOutQuint(double time, double startVal, double newVal, double duration)
        {
            if ((time /= duration / 2) < 1) return newVal / 2 * time * time * time * time * time + startVal;
            return newVal / 2 * ((time -= 2) * time * time * time * time + 2) + startVal;
        }

        public static double EaseOutBack(double time, double startVal, double newVal, double duration)
        {
            return newVal * ((time = time / duration - 1) * time * ((1.70158 + 1) * time + 1.70158) + 1) + startVal;
        }

        public static double EaseInBack(double time, double startVal, double newVal, double duration)
        {
            return newVal * (time /= duration) * time * ((1.70158 + 1) * time - 1.70158) + startVal;
        }

        public static double EaseInOutBack(double time, double startVal, double newVal, double duration)
        {
            double s = 1.70158;
            if ((time /= duration / 2) < 1) return newVal / 2 * (time * time * (((s *= (1.525)) + 1) * time - s)) + startVal;
            return newVal / 2 * ((time -= 2) * time * (((s *= (1.525)) + 1) * time + s) + 2) + startVal;
        }

        public static double EaseOutQuart(double time, double startVal, double newVal, double duration)
        {
            return -newVal * ((time = time / duration - 1) * time * time * time - 1) + startVal;
        }

        public static double EaseInQuart(double time, double startVal, double newVal, double duration)
        {
            return newVal * (time /= duration) * time * time * time + startVal;
        }

        public static double EaseInOutQuart(double time, double startVal, double newVal, double duration)
        {
            if ((time /= duration / 2) < 1) return newVal / 2 * time * time * time * time + startVal;
            return -newVal / 2 * ((time -= 2) * time * time * time - 2) + startVal;
        }

        public static double Linear(double time, double startVal, double newVal, double duration)
        {
            return newVal * time / duration + startVal;
        }
    }
}
