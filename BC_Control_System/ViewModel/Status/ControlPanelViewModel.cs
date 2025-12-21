using BC_Control_BLL.Abstract;
using BC_Control_Helper.interfaceHelper;
using BC_Control_Models;
using BC_Control_Models.BenchConfig;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BC_Control_System.ViewModel.Status
{
    public partial class ControlPanelViewModel : ObservableObject, INavigationAware
    {
        private readonly IPLCControl _plcControl;
        private readonly ILogOpration _logOpration;
        private IStatusCollection statusCollection;
        [ObservableProperty]
        private BindingList<DataClass> controlDataCollections;
        public ControlPanelViewModel(IPLCControl plcControl, ILogOpration logOpration)
        {
            _logOpration = logOpration;
            _plcControl = plcControl;
            statusCollection = new StationCollection();
            ControlDataCollections = new BindingList<DataClass>();
        }
        #region 视图命令
        [RelayCommand]
        private async Task InvetralVarible(DataClass dataClass)
        {
            try
            {
                MessageBoxResult message = MessageBox.Show($"确认操作 {dataClass.ParameterName}", "ConfirmMessage", MessageBoxButton.OKCancel);
                if (message!=MessageBoxResult.OK)
                {
                    return;
                }
                await Task.Run(() =>
                {
                     var oprate=_plcControl.InvetralVarible(dataClass.SettingValueAddress, dataClass.PLC);
                    if (oprate.IsSuccess==false)
                    {
                        MessageBox.Show("操作失败");
                    }
                });
            }
            catch (Exception ex)
            {
                _logOpration.WriteError(ex);
            }

        }
        #endregion
        #region INavigationAware
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                var Collection = navigationContext.Parameters["Param1"];
                statusCollection = (IStatusCollection)Collection;
                ControlDataCollections = new BindingList<DataClass>(statusCollection.ControlDataCollection);
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
    }
}
