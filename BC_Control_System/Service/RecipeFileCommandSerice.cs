using BC_Control_System.Abstract;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC_Control_Models;

namespace BC_Control_System.Service
{
    public class RecipeFileCommandSerice : IRecipeFileCommand
    {
        public event Action SaveCommand;
        public event Action NewCommand;
        public event Action LoadCommand;
        public event Action DeleteCommand;
        public event Action<ProcessStepEnum> AddStepCommand;
        public event Action DeleteStepFuncCommand;
        public event Action ChangeStepCommand;
        public event Action InsertStepCommand;
        public RecipeFileCommandSerice()
        {
            
        }
        public void AddStepFunc(ProcessStepEnum processStepEnum)
        {
            AddStepCommand?.Invoke(processStepEnum);
        }

        public void ChangeStepFunc()
        {
            ChangeStepCommand?.Invoke();
        }

        public void DeleteFunc()
        {
            DeleteCommand?.Invoke();
        }

        public void DeleteStepFunc()
        {
           DeleteStepFuncCommand?.Invoke();
        }

        public void InsertStepFunc()
        {
            InsertStepCommand?.Invoke();
        }

        public void LoadFunc()
        {
            LoadCommand?.Invoke();
        }

        public void NewFunc()
        {
            NewCommand?.Invoke();
        }

        public void SaveFunc()
        {
            SaveCommand?.Invoke();
        }
    }
}
