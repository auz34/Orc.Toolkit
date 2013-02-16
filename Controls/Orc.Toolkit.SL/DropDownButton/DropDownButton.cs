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
    ﻿using System;
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

        #region DP

        /// <summary>
        /// The popup placement property.
        /// </summary>
        public static readonly DependencyProperty PopupPlacementProperty =
            DependencyProperty.Register("PopupPlacement", typeof(PlacementMode), typeof(DropDownButton), new PropertyMetadata(PlacementMode.Bottom));

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

        #endregion

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
#if (SILVERLIGHT)
            this.contentPresenter.SizeChanged += this.contentPresenter_SizeChanged;
#endif
        }

        #endregion

        #region private

#if (SILVERLIGHT)

        /// <summary>
        /// The content presenter_ size changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void contentPresenter_SizeChanged(object sender, SizeChangedEventArgs e)
        {
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
        }

#endif

        #endregion
    }
}