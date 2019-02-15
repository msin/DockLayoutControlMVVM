# DockLayoutControlMVVM
Compose several UserControls in one complex multiregion View powered by DevExpress DockLayoutControl

The using is quite simple

**MainWindow:**
```
    <dxlc:DockLayoutControl AllowItemSizing="True" Margin="0" Padding="0" ItemSpace="5">
        <dxmvvm:Interaction.Behaviors>
            <local:DockItemLayoutBehavior ItemsSource="{Binding LayoutItems}" />
        </dxmvvm:Interaction.Behaviors>
    </dxlc:DockLayoutControl>
```
**MainViewModel:**
```
    public class MainVM
    {
        public virtual IList<LayoutItemVM> LayoutItems { get; set; }

        [Command(false)]
        public void OnLoaded()
        {
            LayoutItems = new List<LayoutItemVM>
            {
                LayoutItemVM.Create("View 1", "View1", 250d, true, Dock.Left),
                LayoutItemVM.Create("View 2", "View2", 250d, true, Dock.Right),
                LayoutItemVM.Create("View 3", "View3", 150d, true, Dock.Top),
                LayoutItemVM.Create("View 4", "View4", 150d, true, Dock.Bottom),
                LayoutItemVM.Create("View 5", "View5"),
            };
        }
    }
```

**Region definition:**
```
LayoutItemVM Create(string header, string content, double size, bool isCollapsible, Dock dock)
LayoutItemVM Create(string header, string content, double size = 0d, Dock dock = Dock.Client)
```
- **Header** is a region header
- **Content** is a region UserControl name
- **Size** is a region width for horizontal docking and region height for vertical docking
