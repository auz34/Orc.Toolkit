// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DropDownButton.cs" company="ORC">
//   MS-PL
// </copyright>
// <summary>
//   The drop down button.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.Toolkit
{
    using System;
    using System.Net;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Documents;
    using System.Windows.Ink;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;

    /// <summary>
    /// The drop down button.
    /// </summary>
    [TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
    [TemplatePart(Name = "PART_ToggleDropDown", Type = typeof(ToggleButton))]
    [TemplatePart(Name = "PART_Content", Type = typeof(ContentControl))]
    public class DropDownButton : HeaderedContentControl
    {
        /// <summary>
        /// The content.
        /// </summary>
        private ContentControl content;

        /// <summary>
        /// The popup.
        /// </summary>
        private Popup popup;

        /// <summary>
        /// The button.
        /// </summary>
        private ToggleButton button;

        /// <summary>
        /// Initializes a new instance of the <see cref="DropDownButton"/> class.
        /// </summary>
        public DropDownButton()
        {
            this.DefaultStyleKey = typeof(DropDownButton);
        }

        #region OVERRIDE

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.button = (ToggleButton)this.GetTemplateChild("PART_ToggleDropDown");
            this.popup = (Popup)this.GetTemplateChild("PART_Popup");
            this.content = (ContentControl)this.GetTemplateChild("PART_Content");
                        
            this.SizeChanged += this.DropDownButton_SizeChanged;

#if(SILVERLIGHT)
            UIElement root = Application.Current.RootVisual;
            if (root != null)
            {
                root.MouseLeftButtonDown += (s, ee) =>
                {
                    if (popup.IsOpen)
                        popup.IsOpen = false;
                };
            }
#endif

#if (!SILVERLIGHT)
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                System.Windows.Window window = System.Windows.Window.GetWindow(this);
                window.LocationChanged += window_LocationChanged;
                window.SizeChanged += window_SizeChanged;
                LayoutUpdated += DropDownButton_LayoutUpdated;
            }

            popup.Opened += popup_Opened;
            popup.IsKeyboardFocusWithinChanged += popup_IsKeyboardFocusWithinChanged;
            FindTopLevelElement(popup).MouseDown += Outside_MouseDown;
#endif
        }

        

#if (!SILVERLIGHT)
        void Outside_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (popup.IsOpen)
            {
                Point p = e.GetPosition(popup);
                if (!new Rect(0, 0, (popup.Child as FrameworkElement).ActualWidth, (popup.Child as FrameworkElement).ActualHeight).Contains(p))
                    popup.IsOpen = false;
            }
        }
#endif




        #endregion

        #region DP

#if(SILVERLIGHT)
        public PlacementMode PopupPlacement
        {
            get
            {
                return (PlacementMode)this.GetValue(PopupPlacementProperty);
            }

            set
            {
                this.SetValue(PopupPlacementProperty, value);
            }
        }
        public static readonly DependencyProperty PopupPlacementProperty =
            DependencyProperty.Register("PopupPlacement", typeof(PlacementMode), typeof(DropDownButton), new PropertyMetadata(PlacementMode.Bottom));
#endif
        #endregion

        #region private

#if (!SILVERLIGHT)
        void popup_Opened(object sender, EventArgs e)
        {
            if (popup.Child != null)
                popup.Child.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            else
                popup.Focus();
        }
        void popup_IsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!popup.IsKeyboardFocusWithin)
                popup.IsOpen = false;
        }

        private static FrameworkElement FindTopLevelElement(Popup popup)
        {
            FrameworkElement iterator, nextUp = popup;
            do
            {
                iterator = nextUp;
                nextUp = VisualTreeHelper.GetParent(iterator) as FrameworkElement;
            } while (nextUp != null);
            return iterator;
        }

        void UpdatePopupPosition()
        {
            if (popup != null)
            {
                if (popup.IsOpen)
                {
                    popup.HorizontalOffset += 0.1;
                    popup.HorizontalOffset -= 0.1;
                }
            }
        }

        void window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdatePopupPosition();
        }

        void window_LocationChanged(object sender, EventArgs e)
        {
            UpdatePopupPosition();
        }

        void DropDownButton_LayoutUpdated(object sender, EventArgs e)
        {
            UpdatePopupPosition();
        }
#endif

        private void DropDownButton_SizeChanged(object sender, SizeChangedEventArgs e)
        {
#if (SILVERLIGHT)
            if (this.PopupPlacement == PlacementMode.Bottom)
            {
                this.popup.VerticalOffset = this.ActualHeight;
            }

            if (this.PopupPlacement == PlacementMode.Top)
            {
                this.popup.VerticalOffset = -1 * this.content.ActualHeight;
            }

            if (this.PopupPlacement == PlacementMode.Right)
            {
                this.popup.HorizontalOffset = this.ActualWidth;
            }

            if (this.PopupPlacement == PlacementMode.Left)
            {
                this.popup.HorizontalOffset = -1 * this.content.ActualWidth;
            }

#endif
#if(!SILVERLIGHT)
            if (popup != null)
            {
                UpdatePopupPosition();
            }
#endif
        }

        #endregion
    }
}