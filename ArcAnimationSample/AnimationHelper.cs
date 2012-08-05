using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace ArcAnimationSample
{
    public class AnimationHelper
    {
        protected AnimationHelper() { }

        public static void AnimatePath(Windows.UI.Xaml.Shapes.Path progressPath, double radius, Point initialPoint, double finalAngle = 180, double timeStep = 0.01)
        {
            var storyboard = new Storyboard();

            var progressAnimation = new ObjectAnimationUsingKeyFrames();
            Storyboard.SetTarget(progressAnimation, progressPath);
            Storyboard.SetTargetProperty(progressAnimation, "(Path.Data)");

            // for a smoother animation
            double multiplier = 3;

            Point center = new Point(radius, radius);
            for (int i = 0; i <= finalAngle * multiplier; i++)
            {
                var discreteObjectKeyFrame = new DiscreteObjectKeyFrame();
                discreteObjectKeyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(i * timeStep));

                // create points for each ArcSegment
                Point firstArcPoint = new Point(radius, 0);
                Point secondArcPoint = new Point(radius, 0);
                Point calculatedPoint = new Point()
                {
                    X = Math.Cos(Math.PI * (270 - i / multiplier) / 180.0) * radius + center.X,
                    Y = Math.Sin(Math.PI * (270 - i / multiplier) / 180.0) * radius + center.Y
                };

                if (i < 180 * multiplier)
                {
                    // use the calculated point for the first and second arc segments
                    firstArcPoint = calculatedPoint;
                    secondArcPoint = calculatedPoint;
                }
                else
                {
                    // leave the first arc segment static and use the calculated point for the second
                    firstArcPoint = new Point() { X = radius, Y = radius * 2 };
                    secondArcPoint = calculatedPoint;
                }

                // for instance, a complete circle with a radius of 150: "m 150,0 A 150,150 0 0 0 150,300 A 150,150 0 0 0 150,0"
                string dataValue = "m {0},{1} A {2},{2} 0 0 0 {3},{4} A {2},{2} 0 0 0 {5},{6}";
                discreteObjectKeyFrame.Value = string.Format(dataValue, initialPoint.X, initialPoint.Y, radius, firstArcPoint.X, firstArcPoint.Y, secondArcPoint.X, secondArcPoint.Y);
                progressAnimation.KeyFrames.Add(discreteObjectKeyFrame);
            }   

            storyboard.Children.Add(progressAnimation);
            storyboard.Begin();
        }

        public static void HideElement(FrameworkElement element, double duration = 0.5)
        {
            var animation = AnimationHelper.CreateHideOrDisplayElementAnimation(element, Visibility.Collapsed, 1, 0, TimeSpan.FromSeconds(duration));
            animation.Begin();
        }

        public static void DisplayElement(FrameworkElement element, double duration = 0.5)
        {
            var animation = AnimationHelper.CreateHideOrDisplayElementAnimation(element, Visibility.Visible, 0, 1, TimeSpan.FromSeconds(duration));
            animation.Begin();
        }

        private static Storyboard CreateHideOrDisplayElementAnimation(FrameworkElement element, Visibility visibility, double initialOpacity, double finalOpacity, TimeSpan duration)
        {
            Storyboard storyboard = new Storyboard();

            // animation for visibility
            ObjectAnimationUsingKeyFrames visibilityAnimation = new ObjectAnimationUsingKeyFrames();
            visibilityAnimation.BeginTime = (visibility == Visibility.Visible) ? TimeSpan.FromSeconds(0) : duration;
            visibilityAnimation.KeyFrames.Add(new DiscreteObjectKeyFrame()
            { 
                KeyTime = (visibility == Visibility.Visible) ? TimeSpan.FromSeconds(0) : duration, 
                Value = visibility 
            });
            Storyboard.SetTarget(visibilityAnimation, element);
            Storyboard.SetTargetProperty(visibilityAnimation, "(UIElement.Visibility)");

            // animation for opacity
            DoubleAnimation opacityAnimation = new DoubleAnimation { From = initialOpacity, To = finalOpacity, Duration = new Duration(duration) };
            Storyboard.SetTarget(opacityAnimation, element);
            Storyboard.SetTargetProperty(opacityAnimation, "Opacity");

            storyboard.Children.Add(visibilityAnimation);
            storyboard.Children.Add(opacityAnimation);
            storyboard.Duration = new Duration(duration);
            
            return storyboard;
        }
    }
}
