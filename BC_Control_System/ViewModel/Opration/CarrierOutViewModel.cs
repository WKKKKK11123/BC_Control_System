using CommunityToolkit.Mvvm.Input;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BC_Control_BLL.Services;
using BC_Control_DAL;
using ZC_Control_EFAM;
using BC_Control_Models;
using Prism.Common;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ZC_Control_System.ViewModel.Opration
{
    public partial class CarrierOutViewModel : ObservableObject, IDialogAware
    {
        public string Title { get; set; }="Carrier Out";

        public event Action<IDialogResult> RequestClose;
        [ObservableProperty]
        private string _CarrierID;
        [ObservableProperty]
        private int _Location;
        [ObservableProperty]
        private StationID _StorageStationID;
        public ICommand CancelCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }
        public CarrierOutViewModel()
        {
            ConfirmCommand=new RelayCommand(Confirm);   
            CancelCommand = new DelegateCommand(Cancel);
        }
        private void Confirm()
        {

            DialogParameters keys = new DialogParameters();
            keys.Add("Value1", CarrierID);
            keys.Add("Value2", Location);
            keys.Add("Value3", Location);
            //var result = PLCSelect.Instance.CommonWrite(value, NewSiteValue);         
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, keys));

        }
        private void Cancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.No));
        }
        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            try
            {
                //CanChangeMessage = parameters.GetValue<bool>("Param1");
                CarrierID = parameters.GetValue<string>("Param1").Trim();
                Location = parameters.GetValue<int>("Param2")-19;
            }
            catch (Exception ee)
            {


            }
        }
    }
}
