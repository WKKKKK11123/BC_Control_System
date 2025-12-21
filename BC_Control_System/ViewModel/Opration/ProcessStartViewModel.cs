using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using PropertyChanged;
using BC_Control_BLL.Services;
using ZC_Control_EFAM;
using BC_Control_Helper;
using BC_Control_Models;
using BC_Control_Models.Personal;
using BC_Control_Models.RecipeModel;
using BC_Control_System.View.Opration;

namespace BC_Control_System.ViewModel.Opration
{
    [AddINotifyPropertyChangedInterface]
    public class ProcessStartViewModel : BindableBase, IDialogAware
    {
        private IDialogService _dialogService;
        private string filepath = @"C:\212Recipe\Tool";

        //public List<StorageCollection> carrierTrackingClasses { get; set; }
        public List<StorageStation> carrierTrackingClasses { get; set; }
        public string Title { get; set; } = "Process Start View";
        public string BatchID { get; set; }
        public string RecipeName { get; set; }

        public event Action<IDialogResult> RequestClose;
        public DelegateCommand OpenfileDelegateCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand ConfirmCommand { get; set; }

        public ProcessStartViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            carrierTrackingClasses = new List<StorageStation>();
            OpenfileDelegateCommand = new DelegateCommand(OpenFile);
            ConfirmCommand = new DelegateCommand(Confirm);
            CancelCommand = new DelegateCommand(Cancel);
        }

        private void OpenFile()
        {
            DialogParameters key = new DialogParameters();
            key.Add("FilePath", filepath);
            _dialogService.ShowDialog(
                nameof(OpenFileView),
                key,
                result =>
                {
                    if (result.Result == ButtonResult.OK)
                    {
                        var tempresult = result.Parameters.GetValue<string>("Result2");
                        RecipeName = tempresult;
                    }
                }
            );
        }

        private void Cancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.No));
        }

        private void Confirm()
        {
            DialogParameters keys = new DialogParameters();
            keys.Add("Value1", BatchID);
            keys.Add("Value2", RecipeName);
            //var result = _plcHelper.CommonWrite(value, NewSiteValue);
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, keys));
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed() { }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            try
            {
                carrierTrackingClasses = parameters.GetValue<List<StorageStation>>(
                    "CarrierTrackClasses"
                );
            }
            catch (Exception ee) { }
        }
    }
}
