using DevExpress.Mvvm;

namespace DockLayoutControlMVVM.ViewModels
{
    public class ViewModel2 : ISupportParentViewModel
    {
        public string Text => "ViewModel 2";
        
        public object ParentViewModel { get; set; }
    }
}