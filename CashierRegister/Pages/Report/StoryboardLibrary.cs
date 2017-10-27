using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CashierRegister.Pages.Report
{
    public static class StoryboardLibrary
    {
        public enum MenuAnimOption
        {
            Hide,
            Show
        }
        public enum MoveDirection
        {
            UpDown,
            RightLeft,
        }

        public static List<EasingFunctionBase> ListOfFunctions = new List<EasingFunctionBase>() {
            new BackEase(),
            new BounceEase(),
            new CircleEase(),
            new CubicEase(),
            new ElasticEase(),
            new ExponentialEase(),
            new PowerEase(),
            new QuadraticEase(),
            new QuarticEase(),
            new QuinticEase(),
            new SineEase()
        };

        public static List<EasingMode> ListOfModes = new List<EasingMode>() {
            EasingMode.EaseIn,
            EasingMode.EaseInOut,
            EasingMode.EaseOut };

        public static Storyboard MenuAnim(UIElement obj, MenuAnimOption option, double size, EasingFunctionBase function, EasingMode mode, MoveDirection moveDirection )
        {
            Storyboard storyboard = new Storyboard();
            DoubleAnimation doubleAnim = new DoubleAnimation();
            obj.RenderTransform = new TranslateTransform();

            if (option == MenuAnimOption.Hide)
            {
                doubleAnim.From = 0;
                doubleAnim.To = size;
            }

            if (option == MenuAnimOption.Show)
            {
                doubleAnim.From = size;
                doubleAnim.To = 0;
            }

            doubleAnim.Duration = new Duration(TimeSpan.FromSeconds(0));
            function.EasingMode = mode;
            doubleAnim.EasingFunction = function;

            Storyboard.SetTarget(doubleAnim, obj);
            if (moveDirection == MoveDirection.RightLeft)
                Storyboard.SetTargetProperty(doubleAnim, new PropertyPath("(FrameworkElement.RenderTransform).(TranslateTransform.X)"));
            else if (moveDirection == MoveDirection.UpDown)
                Storyboard.SetTargetProperty(doubleAnim, new PropertyPath("(FrameworkElement.RenderTransform).(TranslateTransform.Y)"));

            storyboard.Children.Add(doubleAnim);

           return storyboard;
        }

    }
}
