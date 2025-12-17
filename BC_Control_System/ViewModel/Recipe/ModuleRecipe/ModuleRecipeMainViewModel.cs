using BC_Control_Models;
using BC_Control_Models.BenchConfig;
using BC_Control_Models.RecipeModel;
using BC_Control_System.Abstract;
using BC_Control_System.Service;
using BC_Control_System.View.Recipe.ModelRecipe;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Prism.Commands;
using Prism.Common;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BC_Control_System.ViewModel.Recipe.ModuleRecipe
{
    public partial class ModuleRecipeMainViewModel : ObservableObject, INavigationAware
    {
        private readonly IRecipeFileCommand _recipeFileCommand;
        private readonly ViewTransitionNavigator _viewTransitionManager;
        private IStationConfig _stationEntity;
        #region
        public ICommand OpenFileCommand { get; set; }
        public ICommand DeleteFileCommand { get; set; }
        public ICommand SaveFileCommand { get; set; }
        public ICommand AddStepCommand { get; set; }
        public ICommand AddPreStepCommand { get; set; }
        public ICommand AddPostStepCommand { get; set; }
        public ICommand InsertStepCommand { get; set; }
        public ICommand DeleteStepCommand { get; set; }
        public ICommand NewRecipeCommand { get; set; }
        #endregion
        public ModuleRecipeMainViewModel(IRecipeFileCommand recipeFileCommand, ViewTransitionNavigator viewTransitionManager)
        {
            _viewTransitionManager= viewTransitionManager;
            _stationEntity = new StationCollection();
            _recipeFileCommand = recipeFileCommand;
            OpenFileCommand = new RelayCommand(()=> _recipeFileCommand.LoadFunc());
            DeleteFileCommand=new RelayCommand(() => _recipeFileCommand.DeleteFunc());
            SaveFileCommand = new RelayCommand(() => _recipeFileCommand.SaveFunc());
            AddStepCommand = new RelayCommand(() => _recipeFileCommand.AddStepFunc(ProcessStepEnum.Step));
            AddPreStepCommand = new RelayCommand(() => _recipeFileCommand.AddStepFunc(ProcessStepEnum.PreStep));
            AddPostStepCommand = new RelayCommand(() => _recipeFileCommand.AddStepFunc(ProcessStepEnum.PostStep));
            InsertStepCommand = new RelayCommand(() => _recipeFileCommand.InsertStepFunc());
            DeleteStepCommand = new RelayCommand(() => _recipeFileCommand.DeleteStepFunc());
            NewRecipeCommand = new RelayCommand(() => _recipeFileCommand.NewFunc());
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _stationEntity = navigationContext.Parameters.GetValue<IStationConfig>("Module");
            int moduleNo = _stationEntity.StationNo;
            var keys = new NavigationParameters();
            keys.Add("Module", _stationEntity);
            switch (moduleNo)
            {
                case 1:
                    _viewTransitionManager.RecipeLogViewNavigation(nameof(ETCHRecipeView), keys);
                    break;
                case 2:
                    _viewTransitionManager.RecipeLogViewNavigation(nameof(QDRRecipeView), keys);
                    break;
                case 3:
                    _viewTransitionManager.RecipeLogViewNavigation(nameof(ETCHRecipeView), keys);
                    break;
                case 4:
                    _viewTransitionManager.RecipeLogViewNavigation(nameof(QDRRecipeView), keys);
                    break;
                case 5:
                    _viewTransitionManager.RecipeLogViewNavigation(nameof(ETCHRecipeView), keys);
                    break;
                case 6:
                    _viewTransitionManager.RecipeLogViewNavigation(nameof(QDRRecipeView), keys);
                    break;
                case 7:
                    _viewTransitionManager.RecipeLogViewNavigation(nameof(ETCHRecipeView), keys);
                    break;
                case 8:
                    _viewTransitionManager.RecipeLogViewNavigation(nameof(QDRRecipeView), keys);
                    break;
                case 9:
                    _viewTransitionManager.RecipeLogViewNavigation(nameof(ETCHRecipeView), keys);
                    break;
                case 10:
                    _viewTransitionManager.RecipeLogViewNavigation(nameof(QDRRecipeView), keys);
                    break;
                case 11:
                    _viewTransitionManager.RecipeLogViewNavigation(nameof(LPD_RecipeView), keys);
                    break;
                default:
                    break;
            }
        }
    }
}
