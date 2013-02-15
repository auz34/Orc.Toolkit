using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Orc.Toolkit
{
    public class HeaderedContentControl : ContentControl
    {
        public HeaderedContentControl()
        {
            this.DefaultStyleKey = typeof(HeaderedContentControl);
        }

        #region DP
        [System.ComponentModel.Bindable(true)]
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(HeaderedContentControl), new PropertyMetadata(null));
        #endregion
    }
}
