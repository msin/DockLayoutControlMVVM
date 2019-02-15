using DevExpress.Mvvm;

namespace DockLayoutControlMVVM.ViewModels
{
    public class ViewModel3 : ISupportParentViewModel
    {
        public string Text => "ViewModel 3";
        
        public object ParentViewModel { get; set; }
    }
}