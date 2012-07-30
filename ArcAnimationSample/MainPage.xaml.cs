using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ArcAnimationSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            AnimationHelper.HideElement(this.statusMessage, 0.01);
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            // animate path
            double radius = Convert.ToDouble(this.radius.Text);
            double finalAngle = Convert.ToDouble(this.finalAngle.Text);
            double timeStep = Convert.ToDouble(this.timeStep.Text);
            Point initialPoint = new Point(radius, 0);
            
            AnimationHelper.AnimatePath(this.progressPath, radius, initialPoint, finalAngle, timeStep);

            // display processing field
            AnimationHelper.DisplayElement(this.statusMessage, 1); 
        }
    }
}
