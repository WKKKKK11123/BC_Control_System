using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BC_Control_System.View.Recipe.ModelRecipe;

namespace BC_Control_System.Command
{
    public class OpenRecipeEditorViewCommand : ICommand
    {
        private readonly IRegionManager _regionManager;
        public static OpenRecipeEditorViewCommand Instance { get; private set; }
        public OpenRecipeEditorViewCommand(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            Instance = this;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            NavigationParameters keys = new NavigationParameters();
            keys.Add("ModuleName", parameter);
            _regionManager.Regions["ContentRegion"].RequestNavigate(nameof(LPD_RecipeView), keys);
        }
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
