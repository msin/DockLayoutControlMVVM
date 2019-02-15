using DevExpress.Mvvm;

namespace DockLayoutControlMVVM.ViewModels
{
    public class ViewModel1 : ISupportParentViewModel
    {
        public string Text => "ViewModel 1";
        
        public object ParentViewModel { get; set; }
    }
}