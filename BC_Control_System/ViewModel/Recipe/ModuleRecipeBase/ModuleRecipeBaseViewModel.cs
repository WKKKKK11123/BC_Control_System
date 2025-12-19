using CommunityToolkit.Mvvm.ComponentModel;
using Prism.Regions;
using Prism.Services.Dialogs;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using BC_Control_BLL;
using BC_Control_Helper;
using BC_Control_Models;
using BC_Control_Models.BenchConfig;
using BC_Control_Models.RecipeModel;
using BC_Control_Models.RecipeModel.RecipeBase;
using BC_Control_System.Abstract;
using BC_Control_System.View.Opration;
using System.ComponentModel;

namespace BC_Control_System.ViewModel.Recipe.ModuleRecipeBase
{
    public partial class ModuleRecipeBaseViewModel<T, TEntity> : ObservableObject, INavigationAware, IDialogAware
        where T : class, IRecipeStep, new()
        where TEntity : ModuleRecipeClassBase<T>, new()
    {
        private readonly IRecipeFileCommand _recipeFileCommand;
        private IStationConfig _stationConfig;
        private readonly IDialogService _dialogService;
        private string _filePath = "";
        private readonly ILogOpration _logOpration;
        private RecipeStepManager<T> _recipeStepManager;
        public event Action<IDialogResult> RequestClose;
        [ObservableProperty]
        private string moduleName="";
        [ObservableProperty]
        private BindingList<T> recipeStep;
        [ObservableProperty]
        public T selectStepEntity;
        [ObservableProperty]
        public bool isReadOnly;
        [ObservableProperty]
        public TEntity moduleRecipeEntity;

        public string Title { get; set; } = "Module Recipe View";

        public ModuleRecipeBaseViewModel(IDialogService dialogService, ILogOpration logOpration, IRecipeFileCommand recipeFileCommand)
        {
            _recipeFileCommand = recipeFileCommand;
            BindingCommand();
            _logOpration = logOpration;
            SelectStepEntity = new T();
            _dialogService = dialogService;
            ModuleRecipeEntity = new TEntity();
            RecipeStep = new BindingList<T>(ModuleRecipeEntity.RecipeStepCollection);
            _recipeStepManager = new RecipeStepManager<T>(RecipeStep);
        }
        #region 私有方法
        public virtual void BindingCommand()
        {
            try
            {
                _recipeFileCommand.AddStepCommand += AddStep;
                _recipeFileCommand.InsertStepCommand += InsertStep;
                _recipeFileCommand.DeleteStepFuncCommand += DeleteStep;
                _recipeFileCommand.DeleteCommand += DeleteEntity;
                _recipeFileCommand.SaveCommand += SaveEntity;
                _recipeFileCommand.LoadCommand += LoadEntity;
                _recipeFileCommand.NewCommand += NewEntity;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public virtual void UnBindCommand()
        {
            try
            {
                _recipeFileCommand.AddStepCommand -= AddStep;
                _recipeFileCommand.ChangeStepCommand -= InsertStep;
                _recipeFileCommand.DeleteStepFuncCommand -= DeleteStep;
                _recipeFileCommand.DeleteCommand -= DeleteEntity;
                _recipeFileCommand.SaveCommand -= SaveEntity;
                _recipeFileCommand.LoadCommand -= LoadEntity;
                _recipeFileCommand.NewCommand -= NewEntity;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public virtual void SaveEntity()
        {
            try
            {
                if ((int)MessageBox.Show("Save this Recipe ?", "Confirm Message", MessageBoxButton.OKCancel) == 1)
                {
                    string json = JSONHelper.EntityToJSON(ModuleRecipeEntity);
                    string path = Path.Combine(_filePath, $"{ModuleRecipeEntity.Name}.json");
                    File.WriteAllText(path, json);
                }
            }
            catch (Exception ex)
            {
                _logOpration.WriteError(ex);
            }

        }
        public virtual void NewEntity()
        {
            ModuleRecipeEntity = new TEntity();
            RecipeStep = new BindingList<T>(ModuleRecipeEntity.RecipeStepCollection);
            _recipeStepManager = new RecipeStepManager<T>(RecipeStep);
        }
        public virtual void LoadEntity()
        {
            try
            {
                DialogParameters key = new DialogParameters();
                key.Add("FilePath", _filePath);
                _dialogService.ShowDialog(nameof(OpenFileView), key, result =>
                {
                    if (result.Result == ButtonResult.OK)
                    {
                        var tempresult = result.Parameters.GetValue<string>("Result1");
                        ModuleRecipeEntity = JSONHelper.JSONToEntity<TEntity>(tempresult);
                        RecipeStep = new BindingList<T>(ModuleRecipeEntity.RecipeStepCollection);
                        _recipeStepManager = new RecipeStepManager<T>(RecipeStep);
                    }
                });
            }
            catch (Exception ex)
            {
                _logOpration.WriteError(ex);
            }
        }
        public virtual void DeleteEntity()
        {
            try
            {
                DialogParameters key = new DialogParameters();
                key.Add("FilePath", _filePath);
                _dialogService.ShowDialog(nameof(OpenFileView), key, result =>
                {
                    if (result.Result == ButtonResult.OK)
                    {
                        var tempresult = result.Parameters.GetValue<string>("Result1");
                        var temprecipe = JSONHelper.JSONToEntity<TEntity>(tempresult);
                        string path1 = Path.Combine(_filePath, $"{temprecipe.Name}.json");
                        File.Delete(path1);// 3.2、删除文件
                                           //string s=FormHelper.deleteOneFile(path);
                                           //MessageBox.Show(s);
                    }
                });
            }
            catch (Exception ex)
            {
                _logOpration.WriteError(ex);
            }

        }
        public virtual void AddStep(ProcessStepEnum processStepEnum)
        {
            try
            {
                if (!CanAddStep(processStepEnum))
                {
                    MessageBox.Show("禁止进行当前操作");
                    return;
                }
                if (!_recipeStepManager.CreateAndAddStep(processStepEnum))
                {
                    _logOpration.WriteError("Add配方步骤异常");
                }
            }
            catch (Exception ex)
            {

                _logOpration.WriteError(ex);
            }
        }
        public virtual void InsertStep()
        {
            try
            {
                if (SelectStepEntity == null)
                {
                    MessageBox.Show("未选择有效的配方步");
                    return;
                }
                if (SelectStepEntity.StepNo == 0)
                {
                    MessageBox.Show("未选择有效的配方步");
                    return;
                }

                if (!CanAddStep(SelectStepEntity.StepType))
                {
                    MessageBox.Show("禁止进行当前操作");
                    return;
                }
                var newStepEntity = new T();
                newStepEntity.StepNo = SelectStepEntity.StepNo + 1;
                newStepEntity.StepType = SelectStepEntity.StepType;
                if (!_recipeStepManager.InsertStep(newStepEntity))
                {
                    _logOpration.WriteError("Insert配方步骤异常");
                }
            }
            catch (Exception ex)
            {
                _logOpration.WriteError(ex);
            }
        }
        public virtual void DeleteStep()
        {
            try
            {
                if (SelectStepEntity == null)
                {
                    MessageBox.Show("未选择有效的配方步");
                    return;
                }
                if (!_recipeStepManager.DeleteStep(SelectStepEntity.StepType, SelectStepEntity.StepNo))
                {
                    _logOpration.WriteError("删除配方步骤异常");
                }

            }
            catch (Exception ex)
            {
                _logOpration.WriteError(ex);
            }

        }
        public virtual bool CanAddStep(ProcessStepEnum processStepEnum)
        {
            try
            {
                int t = ModuleRecipeEntity.RecipeStepCollection.Where(filter => filter.StepType == processStepEnum).Count();
                switch (processStepEnum)
                {
                    case ProcessStepEnum.PostStep:
                        return t < 5;
                    case ProcessStepEnum.PreStep:
                        return t < 5;
                    case ProcessStepEnum.Step:
                        return t < 40;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
        #region INavigationAware
        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }
        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
            UnBindCommand();

        }
        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                _stationConfig = navigationContext.Parameters.GetValue<IStationConfig>("Module");
                _filePath = Path.Combine(@"C:\212Recipe", _stationConfig.StationName);
                ModuleName = $"Tank{_stationConfig.StationNo}__{_stationConfig.StationName}";
            }
            catch (Exception ex)
            {
                _logOpration.WriteError(ex);
            }

        }
        #endregion
        #region IDialogAware
        public bool CanCloseDialog()
        {
            return true;
        }
        public void OnDialogClosed()
        {

        }
        public void OnDialogOpened(IDialogParameters parameters)
        {
            ModuleRecipeEntity = parameters.GetValue<TEntity>("RecipeEntity");
            IsReadOnly = true;
        }
        #endregion
    }
}
