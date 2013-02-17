// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BrowseForFolderDialog.cs" company="ORC">
//   MS-PL
// </copyright>
// <summary>
//   Commands for ExtendedColorLegend control.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Orc.Toolkit
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows.Input;

    using Orc.Toolkit.Commands;

    /// <summary>
    /// Control to show color legend with checkboxes for each color
    /// </summary>
    public class ExtendedColorLegend : HeaderedContentControl
    {
        #region Dependency properties

        /// <summary>
        /// Property for colors list
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable<IColorProvider>), typeof(ExtendedColorLegend), new PropertyMetadata(null));

        /// <summary>
        /// Property for colors list
        /// </summary>
        public static readonly DependencyProperty FilteredItemsSourceProperty = DependencyProperty.Register("FilteredItemsSource", typeof(IEnumerable<IColorProvider>), typeof(ExtendedColorLegend), new PropertyMetadata(null));

        /// <summary>
        /// The selected items property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.RegisterAttached(
            "SelectedColorItems",
            typeof(ObservableCollection<IColorProvider>),
            typeof(ExtendedColorLegend),
            new PropertyMetadata(null));

        #if(!SILVERLIGHT)
        /// <summary>
        /// Property indicating whether search box is shown or not
        /// </summary>
        public static readonly DependencyProperty ShowSearchBoxProperty = DependencyProperty.Register("ShowSearchBox", typeof(bool), typeof(ExtendedColorLegend), new UIPropertyMetadata(true));

        /// <summary>
        /// Property indicating whether search box is shown or not
        /// </summary>
        public static readonly DependencyProperty ShowToolBoxProperty = DependencyProperty.Register("ShowToolBox", typeof(bool), typeof(ExtendedColorLegend), new UIPropertyMetadata(true));

        /// <summary>
        /// Property indicating whether search box is shown or not
        /// </summary>
        public static readonly DependencyProperty ShowColorVisibilityControlsProperty = DependencyProperty.Register("ShowColorVisibilityControls", typeof(bool), typeof(ExtendedColorLegend), new UIPropertyMetadata(true));

        /// <summary>
        /// Property indicating whether search box is shown or not
        /// </summary>
        public static readonly DependencyProperty ShowSettingsProperty = DependencyProperty.Register("ShowSettings", typeof(bool), typeof(ExtendedColorLegend), new UIPropertyMetadata(true));

        /// <summary>
        /// Expose filter property
        /// </summary>
        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register("Filter", typeof(string), typeof(ExtendedColorLegend), new UIPropertyMetadata(null, OnFilterChanged));

        /// <summary>
        /// Property for filter watermark
        /// </summary>
        public static readonly DependencyProperty FilterWatermarkProperty = DependencyProperty.Register("FilterWatermark", typeof(string), typeof(ExtendedColorLegend), new UIPropertyMetadata("Search"));
        
        #else
        /// <summary>
        /// Property indicating whether search box is shown or not
        /// </summary>
        public static readonly DependencyProperty ShowSearchBoxProperty = DependencyProperty.Register("ShowSearchBox", typeof(bool), typeof(ExtendedColorLegend), new PropertyMetadata(true));

        /// <summary>
        /// Property indicating whether search box is shown or not
        /// </summary>
        public static readonly DependencyProperty ShowToolBoxProperty = DependencyProperty.Register("ShowToolBox", typeof(bool), typeof(ExtendedColorLegend), new PropertyMetadata(true));

        /// <summary>
        /// Property indicating whether search box is shown or not
        /// </summary>
        public static readonly DependencyProperty ShowColorVisibilityControlsProperty = DependencyProperty.Register("ShowColorVisibilityControls", typeof(bool), typeof(ExtendedColorLegend), new PropertyMetadata(true));

        /// <summary>
        /// Property indicating whether search box is shown or not
        /// </summary>
        public static readonly DependencyProperty ShowSettingsProperty = DependencyProperty.Register("ShowSettingsBox", typeof(bool), typeof(ExtendedColorLegend), new PropertyMetadata(true));

        /// <summary>
        /// Expose filter property
        /// </summary>
        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register("Filter", typeof(string), typeof(ExtendedColorLegend), new PropertyMetadata(null, OnFilterChanged));

        /// <summary>
        /// Property for filter watermark
        /// </summary>
        public static readonly DependencyProperty FilterWatermarkProperty = DependencyProperty.Register("FilterWatermark", typeof(string), typeof(ExtendedColorLegend), new PropertyMetadata("Search"));
#endif
        #endregion

        /// <summary>
        /// Initializes static members of the <see cref="ExtendedColorLegend" /> class.
        /// </summary>
        static ExtendedColorLegend()
        {
            #if (!SILVERLIGHT)
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExtendedColorLegend), new FrameworkPropertyMetadata(typeof(ExtendedColorLegend)));
            #endif
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedColorLegend" /> class.
        /// </summary>
        public ExtendedColorLegend()
        {
            #if (!SILVERLIGHT)
            CommandBindings.Add(
                new CommandBinding(Commands.ExtendedColorLegendCommands.ClearFilter, this.ClearFilter, this.CanClearFilter));
            #else
            this.ClearFilterCommand = new DelegateCommand(o => this.Filter = string.Empty, o => string.IsNullOrEmpty(this.Filter));
            #endif
        }

        #region Public properties

        #if (SILVERLIGHT)
        /// <summary>
        ///     Gets or sets BrowseCommand.
        /// </summary>
        public ICommand ClearFilterCommand { get; set; }
        #endif

        /// <summary>
        /// Gets or sets a value indicating whether search box is shown or not
        /// </summary>
        public bool ShowSearchBox
        {
            get
            {
                return (bool)GetValue(ShowSearchBoxProperty);
            }

            set
            {
                this.SetValue(ShowSearchBoxProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether tool box is shown or not
        /// </summary>
        public bool ShowToolBox
        {
            get
            {
                return (bool)GetValue(ShowToolBoxProperty);
            }

            set
            {
                this.SetValue(ShowToolBoxProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether settings button is shown or not
        /// </summary>
        public bool ShowSettings
        {
            get
            {
                return (bool)GetValue(ShowSettingsProperty);
            }

            set
            {
                this.SetValue(ShowSettingsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether settings button is shown or not
        /// </summary>
        public bool ShowColorVisibilityControls
        {
            get
            {
                return (bool)GetValue(ShowColorVisibilityControlsProperty);
            }

            set
            {
                this.SetValue(ShowColorVisibilityControlsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets filter for list of color
        /// </summary>
        public string Filter
        {
            get
            {
                return (string)GetValue(FilterProperty);
            }

            set
            {
                this.SetValue(FilterProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets source for color items
        /// </summary>
        public IEnumerable<IColorProvider> ItemsSource
        {
            get
            {
                return (IEnumerable<IColorProvider>)GetValue(ItemsSourceProperty);
            }

            set
            {
                this.SetValue(ItemsSourceProperty, value);
                this.FilteredItemsSource = this.GetFilteredItems();
            }
        }

        /// <summary>
        /// Gets source for color items respecting current filter value
        /// </summary>
        public IEnumerable<IColorProvider> FilteredItemsSource
        {
            get
            {
                return (IEnumerable<IColorProvider>)GetValue(FilteredItemsSourceProperty);
            }

            set
            {
                this.SetValue(FilteredItemsSourceProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets filter watermark string we use in search textbox
        /// </summary>
        public string FilterWatermark
        {
            get
            {
                return (string)this.GetValue(FilterWatermarkProperty);
            }
            set
            {
                this.SetValue(FilterWatermarkProperty, value);
            }
        }

        public ObservableCollection<IColorProvider> SelectedColorItems
        {
            get
            {
                return (ObservableCollection<IColorProvider>)this.GetValue(SelectedItemsProperty);
            }

            set
            {
                this.SetValue(SelectedItemsProperty, value);
            }
        }

        #endregion

        /// <summary>
        /// Process filter changed event
        /// </summary>
        /// <param name="o">Sender dependency object</param>
        /// <param name="e">shows what has been changed</param>
        private static void OnFilterChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ExtendedColorLegend extendedColorLegend = o as ExtendedColorLegend;

            if (extendedColorLegend != null)
            {
                extendedColorLegend.OnFilterChanged((string)e.OldValue, (string)e.NewValue);
            }
        }

        /// <summary>
        /// Gets items filtered by filter
        /// </summary>
        /// <returns>filtered items</returns>
        protected IEnumerable<IColorProvider> GetFilteredItems()
        {
            IEnumerable<IColorProvider> items = (IEnumerable<IColorProvider>)GetValue(ItemsSourceProperty);
            string filter = (string)GetValue(FilterProperty);
            if ((items == null) || string.IsNullOrEmpty(filter))
            {
                return items;
            }

            Regex regex = new Regex(filter);
            return items.Where(cp => regex.IsMatch(cp.Description));
        }


        /// <summary>
        /// Process filter changed event
        /// </summary>
        /// <param name="oldValue">old value</param>
        /// <param name="newValue">new value</param>
        protected virtual void OnFilterChanged(string oldValue, string newValue)
        {
            this.FilteredItemsSource = this.GetFilteredItems();
        }

        #if (!SILVERLIGHT)
        #region Commands

        private void ClearFilter(object sender, ExecutedRoutedEventArgs e)
        {
            this.Filter = string.Empty;
        }

        private void CanClearFilter(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !string.IsNullOrEmpty(Filter);
        }

        #endregion //Commands
        #endif
    }
}
