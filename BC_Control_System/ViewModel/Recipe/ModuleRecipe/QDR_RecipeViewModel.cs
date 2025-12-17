using BC_Control_Models.RecipeModel.RecipeBase;
using BC_Control_Models.RecipeModel;
using BC_Control_System.ViewModel.Recipe.ModuleRecipeBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC_Control_Models.RecipeModel.RecipeEntity;
using BC_Control_Models;
using BC_Control_System.Abstract;
using Prism.Services.Dialogs;

namespace BC_Control_System.ViewModel.Recipe.ModuleRecipe
{
    public class QDR_RecipeViewModel: ModuleRecipeBaseViewModel<QDRTankModuleRecipeStep, QDRRecipeClassBase>
    {
        public QDR_RecipeViewModel(IDialogService dialogService, ILogOpration logOpration, IRecipeFileCommand recipeFileCommand)
            : base(dialogService, logOpration, recipeFileCommand)
        {

        }
    }
}
