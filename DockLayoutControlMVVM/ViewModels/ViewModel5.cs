using DevExpress.Mvvm;

namespace DockLayoutControlMVVM.ViewModels
{
    public class ViewModel5 : ISupportParentViewModel
    {
        public string Text => "ViewModel 5";

        public object ParentViewModel { get; set; }
    }
}