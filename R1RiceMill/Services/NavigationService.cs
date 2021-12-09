using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using MahApps.Metro.Controls;

namespace R1RiceMill.Services
{
    public class NavigationService
    {
        public event NavigatedEventHandler Navigated;

        public event NavigationFailedEventHandler NavigationFailed;

        private Frame _frame;

        public Frame Frame
        {
            get
            {
                if (this._frame == null)
                {
                    this._frame = new Frame() { NavigationUIVisibility = NavigationUIVisibility.Hidden };
                    this.RegisterFrameEvents();
                }

                return this._frame;
            }
            set
            {
                this.UnregisterFrameEvents();
                this._frame = value;
                this.RegisterFrameEvents();
            }
        }

        public bool CanGoBack => this.Frame.CanGoBack;

        public bool CanGoForward => this.Frame.CanGoForward;

        public void GoBack() => this.Frame.GoBack();

        public void GoForward() => this.Frame.GoForward();

        public bool Navigate(Uri sourcePageUri, object extraData = null)
        {
            if (this.Frame.CurrentSource != sourcePageUri)
            {
                return this.Frame.Navigate(sourcePageUri, extraData);
            }

            return false;
        }

        public bool Navigate(Type sourceType)
        {
            if (this.Frame.NavigationService?.Content?.GetType() != sourceType)
            {
                return this.Frame.Navigate(Activator.CreateInstance(sourceType));
            }

            return false;
        }

        private void RegisterFrameEvents()
        {
            if (this._frame != null)
            {
                this._frame.Navigated += this.Frame_Navigated;
                this._frame.NavigationFailed += this.Frame_NavigationFailed;
            }
        }

        private void UnregisterFrameEvents()
        {
            if (this._frame != null)
            {
                this._frame.Navigated -= this.Frame_Navigated;
                this._frame.NavigationFailed -= this.Frame_NavigationFailed;
            }
        }

        private void Frame_NavigationFailed(object sender, NavigationFailedEventArgs e) => this.NavigationFailed?.Invoke(sender, e);

        private void Frame_Navigated(object sender, NavigationEventArgs e) => this.Navigated?.Invoke(sender, e);
    }

    public class FrameAnimator
    {
        public static readonly DependencyProperty FrameNavigationStoryboardProperty
            = DependencyProperty.RegisterAttached(
                "FrameNavigationStoryboard",
                typeof(Storyboard),
                typeof(FrameAnimator),
                new FrameworkPropertyMetadata(null, OnFrameNavigationStoryboardChanged));

        private static void OnFrameNavigationStoryboardChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Frame frame && e.OldValue != e.NewValue)
            {
                frame.Navigating -= Frame_Navigating;
                if (e.NewValue is Storyboard)
                {
                    frame.Navigating += Frame_Navigating;
                }
            }
        }

        private static void Frame_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            if (sender is Frame frame)
            {
                var sb = GetFrameNavigationStoryboard(frame);
                if (sb != null)
                {
                    var presenter = frame.FindChild<ContentPresenter>();
                    sb.Begin((FrameworkElement)presenter ?? frame);
                }
            }
        }

        /// <summary>Helper for setting <see cref="FrameNavigationStoryboardProperty"/> on <paramref name="control"/>.</summary>
        /// <param name="control"><see cref="DependencyObject"/> to set <see cref="FrameNavigationStoryboardProperty"/> on.</param>
        /// <param name="storyboard">FrameNavigationStoryboard property value.</param>
        public static void SetFrameNavigationStoryboard(DependencyObject control, Storyboard storyboard)
        {
            control.SetValue(FrameNavigationStoryboardProperty, storyboard);
        }

        /// <summary>Helper for getting <see cref="FrameNavigationStoryboardProperty"/> from <paramref name="control"/>.</summary>
        /// <param name="control"><see cref="DependencyObject"/> to read <see cref="FrameNavigationStoryboardProperty"/> from.</param>
        /// <returns>FrameNavigationStoryboard property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(DependencyObject))]
        public static Storyboard GetFrameNavigationStoryboard(DependencyObject control)
        {
            return (Storyboard)control.GetValue(FrameNavigationStoryboardProperty);
        }
    }
}
