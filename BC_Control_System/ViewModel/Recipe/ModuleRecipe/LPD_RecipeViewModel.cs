using BC_Control_Models;
using BC_Control_Models.RecipeModel;
using BC_Control_Models.RecipeModel.RecipeBase;
using BC_Control_System.Abstract;
using BC_Control_System.ViewModel.Recipe.ModuleRecipeBase;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_System.ViewModel.Recipe.ModuleRecipe
{
    public class LPD_RecipeViewModel : ModuleRecipeBaseViewModel<LPDTankModuleRecipeBase, LPDRecipeClassBase>
    {
        public LPD_RecipeViewModel(IDialogService dialogService, ILogOpration logOpration, IRecipeFileCommand recipeFileCommand) 
            : base(dialogService, logOpration, recipeFileCommand)
        { 
        
        }
    }
}
