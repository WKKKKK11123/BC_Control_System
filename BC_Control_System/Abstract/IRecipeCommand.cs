using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC_Control_Models.RecipeModel;

namespace BC_Control_System.Interface
{
    public interface IRecipeCommand<T>
    {
        BindingList<T> SCClasses { get; set; }
        DelegateCommand OpenFileCommand { get; set; }
        DelegateCommand DeleteFileCommand { get; set; }
        DelegateCommand SaveFileCommand { get; set; }
        DelegateCommand AddStepCommand { get; set; }
        DelegateCommand AddPreStepCommand { get; set; }
        DelegateCommand AddPostStepCommand { get; set; }
        DelegateCommand<T> InsertStepCommand { get; set; }
        DelegateCommand<T> DeleteStepCommand { get; set; }
        DelegateCommand NewRecipeCommand { get; set; }
    }
}
