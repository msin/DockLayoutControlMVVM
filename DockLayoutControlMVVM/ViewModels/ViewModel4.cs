using DevExpress.Mvvm;

namespace DockLayoutControlMVVM.ViewModels
{
    public class ViewModel4 : ISupportParentViewModel
    {
        public string Text => "ViewModel 4";
        
        public object ParentViewModel { get; set; }
    }
}