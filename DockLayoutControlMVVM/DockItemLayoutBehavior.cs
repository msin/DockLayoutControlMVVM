using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.POCO;
using DevExpress.Mvvm.UI;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.LayoutControl;
using System;
using System.Collections;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml;
using Dock = DevExpress.Xpf.LayoutControl.Dock;
using GroupBox = DevExpress.Xpf.LayoutControl.GroupBox;

namespace DockLayoutControlMVVM
{
    public class DockItemLayoutBehavior : Behavior<DockLayoutControl>
    {
        private DockLayoutControl LayoutControl => AssociatedObject;
        DataTemplate _normalTemplate;
        DataTemplate _rotatedTemplate;

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IList), typeof(DockItemLayoutBehavior),
                new PropertyMetadata(null, (d, e) => ((DockItemLayoutBehavior)d).OnCreateLayout()));

        public IList ItemsSource
        {
            get => (IList)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public DockItemLayoutBehavior()
        {
            _normalTemplate = NormalTemplate();
            _rotatedTemplate = RotatedTemplate();
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            LayoutControl.Loaded += OnLoaded;
            LayoutControl.Unloaded += OnUnloaded;
        }

        protected override void OnDetaching()
        {
            LayoutControl.Loaded -= OnLoaded;
            LayoutControl.Unloaded -= OnUnloaded;

            base.OnDetaching();
        }

        private void OnLoaded(object sender, RoutedEventArgs e) { }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            foreach (GroupBox groupBox in LayoutControl.Children)
                groupBox.StateChanged -= GroupBox_StateChanged;
        }

        private void OnCreateLayout()
        {
            LayoutControl.Children.Clear();

            foreach (LayoutItemVM item in ItemsSource)
            {
                if (item == null) continue;

                var groupBox = new GroupBox
                {
                    Header = item.Header,
                    Margin = new Thickness(0d),
                    Padding = new Thickness(0d),
                };

                DockLayoutControl.SetDock(groupBox, item.Dock);

                groupBox.MinimizeElementVisibility = item.ShowMinimizeBoxButton ? Visibility.Visible : Visibility.Collapsed;

                groupBox.Content = ViewLocator.Default.ResolveView(item.Content);

                ViewModelExtensions.SetParentViewModel((FrameworkElement)groupBox.Content, LayoutControl.DataContext);

                item.DataContext = ((FrameworkElement)groupBox.Content).DataContext;

                groupBox.HeaderTemplate = _normalTemplate;

                groupBox.StateChanged += GroupBox_StateChanged;

                var myBinding = new Binding
                {
                    Source = item,
                    Path = new PropertyPath("Visibility"),
                    Mode = BindingMode.OneWay
                };
                BindingOperations.SetBinding(groupBox, UIElement.VisibilityProperty, myBinding);

                switch (item.Dock)
                {
                    case Dock.Left:
                        DockLayoutControl.SetAllowHorizontalSizing(groupBox, true);
                        groupBox.Width = item.Size;
                        if (item.ShowMinimizeBoxButton) groupBox.MinimizationDirection = Orientation.Horizontal;
                        break;

                    case Dock.Right:
                        DockLayoutControl.SetAllowHorizontalSizing(groupBox, true);
                        groupBox.Width = item.Size;
                        if (item.ShowMinimizeBoxButton) groupBox.MinimizationDirection = Orientation.Horizontal;
                        break;

                    case Dock.Top:
                        DockLayoutControl.SetAllowVerticalSizing(groupBox, true);
                        groupBox.Height = item.Size;
                        if (item.ShowMinimizeBoxButton) groupBox.MinimizationDirection = Orientation.Vertical;
                        break;

                    case Dock.Bottom:
                        DockLayoutControl.SetAllowVerticalSizing(groupBox, true);
                        groupBox.Height = item.Size;
                        if (item.ShowMinimizeBoxButton) groupBox.MinimizationDirection = Orientation.Vertical;
                        break;

                    case Dock.Client:
                        DockLayoutControl.SetAllowHorizontalSizing(groupBox, true);
                        DockLayoutControl.SetAllowVerticalSizing(groupBox, true);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                LayoutControl.Children.Add(groupBox);
            }
        }

        private void GroupBox_StateChanged(object sender, DevExpress.Xpf.Core.ValueChangedEventArgs<GroupBoxState> e)
        {
            var groupBox = (GroupBox)sender;

            if (groupBox.MinimizationDirection != Orientation.Horizontal) return;

            switch (e.NewValue)
            {
                case GroupBoxState.Normal:
                    groupBox.HeaderTemplate = _normalTemplate;
                    break;

                case GroupBoxState.Minimized:
                    groupBox.HeaderTemplate = _rotatedTemplate;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private DataTemplate NormalTemplate()
        {
            var stringReader = new StringReader(
@"<DataTemplate x:Key=""NotRotated"" xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                                       xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
    <TextBlock Text=""{Binding}"" Margin=""10 -1"" FontSize=""14"" FontWeight=""DemiBold"" />
</DataTemplate>");

            var xmlReader = XmlReader.Create(stringReader);

            return XamlReader.Load(xmlReader) as DataTemplate;
        }

        private DataTemplate RotatedTemplate()
        {
            var stringReader = new StringReader(
@"<DataTemplate x:Key=""Rotated"" xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                                    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
    <TextBlock Text=""{Binding}"" Margin=""-12 0"" FontSize=""14"" FontWeight=""DemiBold"">
        <TextBlock.LayoutTransform>
            <RotateTransform CenterX=""0.5"" CenterY=""0.5"" Angle=""-90"" />
        </TextBlock.LayoutTransform>
    </TextBlock>
</DataTemplate>");

            var xmlReader = XmlReader.Create(stringReader);

            return XamlReader.Load(xmlReader) as DataTemplate;
        }
    }

    public class LayoutItemVM
    {
        private static readonly Func<string, string, double, Dock, LayoutItemVM> Factory4 =
            ViewModelSource.Factory((string header, string content, double size, Dock dock) =>
                new LayoutItemVM(header, content, size, dock));

        private static readonly Func<string, string, double, bool, Dock, LayoutItemVM> Factory5 =
            ViewModelSource.Factory((string header, string content, double size, bool isCollapsible, Dock dock) =>
                new LayoutItemVM(header, content, size, isCollapsible, dock));

        public string Header { get; }
        public bool ShowMinimizeBoxButton { get; }
        public string Content { get; }
        public double Size { get; }
        public Dock Dock { get; }
        public object DataContext { get; set; }
        public virtual Visibility Visibility { get; set; } = Visibility.Visible;

        public LayoutItemVM(string header, string content, double size, Dock dock = Dock.Client)
        {
            Header = header;
            Content = content;
            Size = size;
            Dock = dock;
        }

        public LayoutItemVM(string header, string content, double size, bool isCollapsible, Dock dock)
        {
            Header = header;
            Content = content;
            Size = size;
            ShowMinimizeBoxButton = isCollapsible;
            Dock = dock;
        }

        public static LayoutItemVM Create(string header, string content, double size = 0d, Dock dock = Dock.Client) =>
            Factory4(header, content, size, dock);

        public static LayoutItemVM Create(string header, string content, double size, bool isCollapsible, Dock dock) =>
            Factory5(header, content, size, isCollapsible, dock);

        [Command(false)]
        public void ChangeVisibility()
        {
            if (Dock == Dock.Client) return;

            Visibility = Visibility == Visibility.Collapsed
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
    }
}