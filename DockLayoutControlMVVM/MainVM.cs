using System;
using System.Collections.Generic;
using System.Windows;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.LayoutControl;

namespace DockLayoutControlMVVM
{
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

}