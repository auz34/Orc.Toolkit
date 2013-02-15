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

namespace Orc.Toolkit
{
    [TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
    [TemplatePart(Name = "PART_ToggleDropDown", Type = typeof(ToggleButton))]
    [TemplatePart(Name = "PART_ContentPresenter", Type = typeof(FrameworkElement))]
    public class DropDownButton : HeaderedContentControl
    {
        FrameworkElement contentPresenter;
        Popup popup;
        ToggleButton button;

        public DropDownButton()
        {
            this.DefaultStyleKey = typeof(DropDownButton);
        }

        #region OVERRIDE
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            button = (ToggleButton)GetTemplateChild("PART_ToggleDropDown");
            popup = (Popup)GetTemplateChild("PART_Popup");
            contentPresenter = (FrameworkElement)GetTemplateChild("PART_ContentPresenter");
            #if (SILVERLIGHT)
            contentPresenter.SizeChanged += contentPresenter_SizeChanged;
            #endif
        }
        #endregion

        #region DP
        public PlacementMode PopupPlacement
        {
            get { return (PlacementMode)GetValue(PopupPlacementProperty); }
            set { SetValue(PopupPlacementProperty, value); }
        }
        public static readonly DependencyProperty PopupPlacementProperty =
           DependencyProperty.Register("PopupPlacement", typeof(PlacementMode), typeof(DropDownButton), new PropertyMetadata(PlacementMode.Bottom));
        #endregion

        #region private
        #if (SILVERLIGHT)
        void contentPresenter_SizeChanged(object sender, SizeChangedEventArgs e)
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
