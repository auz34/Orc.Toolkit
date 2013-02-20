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
    [TemplatePart(Name = "PART_ContentPresenter", Type = typeof(FrameworkElement))]
    public class DropDownButton : HeaderedContentControl
    {
        /// <summary>
        /// The content presenter.
        /// </summary>
        private FrameworkElement contentPresenter;

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

        /// <summary>
        /// The on apply template.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.button = (ToggleButton)this.GetTemplateChild("PART_ToggleDropDown");
            this.popup = (Popup)this.GetTemplateChild("PART_Popup");
            this.contentPresenter = (FrameworkElement)this.GetTemplateChild("PART_ContentPresenter");

            this.SizeChanged += this.DropDownButton_SizeChanged;

#if (!SILVERLIGHT)
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                Window window = Window.GetWindow(this);
                window.LocationChanged += window_LocationChanged;
                window.SizeChanged += window_SizeChanged;
                LayoutUpdated += DropDownButton_LayoutUpdated;
            }
            #endif
        }

        #endregion

        #region DP

        /// <summary>
        /// Gets or sets the popup placement.
        /// </summary>
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

        /// <summary>
        /// The popup placement property.
        /// </summary>
        public static readonly DependencyProperty PopupPlacementProperty =
            DependencyProperty.Register("PopupPlacement", typeof(PlacementMode), typeof(DropDownButton), new PropertyMetadata(PlacementMode.Bottom));

        #endregion

        #region private

#if (!SILVERLIGHT)
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

        /// <summary>
        /// The drop down button_ size changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void DropDownButton_SizeChanged(object sender, SizeChangedEventArgs e)
        {
#if (SILVERLIGHT)
            if (this.PopupPlacement == PlacementMode.Bottom)
            {
                this.popup.VerticalOffset = this.ActualHeight;
            }

            if (this.PopupPlacement == PlacementMode.Top)
            {
                this.popup.VerticalOffset = -1 * this.contentPresenter.ActualHeight;
            }

            if (this.PopupPlacement == PlacementMode.Right)
            {
                this.popup.HorizontalOffset = this.ActualWidth;
            }

            if (this.PopupPlacement == PlacementMode.Left)
            {
                this.popup.HorizontalOffset = -1 * this.contentPresenter.ActualWidth;
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