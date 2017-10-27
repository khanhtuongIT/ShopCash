using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CashierRegister.Helpers
{
    public static class SetControlFocus
    {
        /*public static readonly DependencyProperty SetFocusProperty = DependencyProperty.RegisterAttached("SetFocus",
                                           typeof(Boolean),
                                           typeof(SetControlFocus),
                                           new PropertyMetadata(OnSetFocusChanged));

        private static void OnSetFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d != null && d is Control)
            {
                if ((bool)e.NewValue)
                {
                    (d as Control).GotFocus += OnLostFocus;
                    (d as Control).Focus();
                }
                else
                {
                    (d as Control).GotFocus -= OnLostFocus;
                }
            }
        }

        private static void OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (sender != null && sender is Control)
            {
                (sender as Control).SetValue(SetFocusProperty, false);
            }
        }

        public static Boolean GetSetFocus(DependencyObject target)
        {
            return (Boolean)target.GetValue(SetFocusProperty);
        }

        public static void SetSetFocus(DependencyObject target, Boolean value)
        {
            target.SetValue(SetFocusProperty, value);
        }*/


        public static readonly DependencyProperty IsFocusedProperty = DependencyProperty.RegisterAttached("IsFocused",
            typeof(bool?),
            typeof(SetControlFocus),
            new FrameworkPropertyMetadata(IsFocusedChanged));

        public static bool? GetIsFocused(DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return (bool?)element.GetValue(IsFocusedProperty);
        }

        public static void SetIsFocused(DependencyObject element, bool? value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            element.SetValue(IsFocusedProperty, value);
        }

        private static void IsFocusedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fe = (FrameworkElement)d;

            if (e.OldValue == null)
            {
                fe.GotFocus += FrameworkElement_GotFocus;
                fe.LostFocus += FrameworkElement_LostFocus;
            }

            if (!fe.IsVisible)
            {
                fe.IsVisibleChanged += new DependencyPropertyChangedEventHandler(fe_IsVisibleChanged);
            }

            if (!fe.IsEnabled)
            {
                fe.IsEnabledChanged += fe_IsEnabledChanged;
            }

            if ((bool)e.NewValue)
            {
                //fe.Focus();
                var action = new Action(() => fe.Dispatcher.BeginInvoke((Action)(() => fe.Focus())));
                Task.Factory.StartNew(action);

            }
        }
        private static void fe_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var fe = (FrameworkElement)sender;
            if (fe.IsVisible && (bool)((FrameworkElement)sender).GetValue(IsFocusedProperty))
            {
                fe.IsVisibleChanged -= fe_IsVisibleChanged;
                fe.Focus();
            }
        }

        private static void fe_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var fe = (FrameworkElement)sender;
            if (fe.IsEnabled && (bool)((FrameworkElement)sender).GetValue(IsFocusedProperty))
            {
                fe.IsEnabledChanged -= fe_IsEnabledChanged;
                fe.Focus();
            }
        }

        private static void FrameworkElement_GotFocus(object sender, RoutedEventArgs e)
        {
            ((FrameworkElement)sender).SetValue(IsFocusedProperty, true);
        }

        private static void FrameworkElement_LostFocus(object sender, RoutedEventArgs e)
        {
            ((FrameworkElement)sender).SetValue(IsFocusedProperty, false);
        }
    }
}
