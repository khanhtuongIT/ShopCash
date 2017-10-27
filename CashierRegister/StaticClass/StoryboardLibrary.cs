using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows;

namespace CashierRegister.StaticClass
{
    public static class StoryboardLibrary
    {
        public enum MenuAnimationOption { Hide, Show }
        public enum MoveDirection { UpDown, RightLeft }

        public static List<EasingFunctionBase> ListEasingFunctionBase = new List<EasingFunctionBase>()
        {
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

        public static List<EasingMode> ListEasingMode = new List<EasingMode>() { EasingMode.EaseIn, EasingMode.EaseInOut, EasingMode.EaseOut, };

        public static Storyboard MenuAnimation(UIElement _ui_element, MenuAnimationOption _option, double _size, EasingFunctionBase _easing_function_base, EasingMode _easing_mode, MoveDirection _move_direction)
        {
            Storyboard storyboard = new Storyboard();
            DoubleAnimation double_animation = new DoubleAnimation();
            _ui_element.RenderTransform = new TranslateTransform();
            if (_option == MenuAnimationOption.Hide)
            {
                //hide to right
                double_animation.From = 0;
                double_animation.To = _size;

                ////hight to left
                //double_animation.From = 0;
                //double_animation.To = -_size;
            }
            if (_option == MenuAnimationOption.Show)
            {
                //show to left
                double_animation.From = _size;
                double_animation.To = 0;
                //double_animation.From = -_size;
                //double_animation.To = 0;
            }
            double_animation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            _easing_function_base.EasingMode = _easing_mode;
            double_animation.EasingFunction = _easing_function_base;

            Storyboard.SetTarget(double_animation, _ui_element);
            if (_move_direction == MoveDirection.RightLeft)
                Storyboard.SetTargetProperty(double_animation, new PropertyPath("(FrameworkElement.RenderTransform).(TranslateTransform.X)"));
            else if (_move_direction == MoveDirection.UpDown)
                Storyboard.SetTargetProperty(double_animation, new PropertyPath("(FrameworkElement.RenderTransform).(TranslateTransform.Y)"));
            storyboard.Children.Add(double_animation);
            return storyboard;
        }
    }
}
