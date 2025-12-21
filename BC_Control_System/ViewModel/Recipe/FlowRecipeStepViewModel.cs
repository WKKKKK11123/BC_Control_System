using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC_Control_Models;
using BC_Control_Models.RecipeModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BC_Control_System.View.Recipe
{
    public partial class FlowRecipeStepViewModel :ObservableObject, IDialogAware
    {
        private string path= @"C:\212Recipe";
        #region 视图属性
        [ObservableProperty]
        private FlowStepClass _FlowStep=new FlowStepClass();
        [ObservableProperty]
        public BathNameEnum _BathName=BathNameEnum.Ag_1;
        [ObservableProperty]
        private string _RecipeName="";
        [ObservableProperty]
        private BindingList<string> _OpenFiles=new BindingList<string>();
        #endregion
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand ConfirmCommand { get; set; }       
        public string Title { get; set; } = "";

        public event Action<IDialogResult> RequestClose;
        public FlowRecipeStepViewModel()
        {
            RequestClose=new Action<IDialogResult>(item => { });
            CancelCommand = new DelegateCommand(Cancel);
            ConfirmCommand = new DelegateCommand(Confirm);
            OpenFiles = new BindingList<string>();
            UnitRecipeSelectChanged();
        }
        partial void OnBathNameChanged(BathNameEnum oldValue, BathNameEnum newValue)
        {
            UnitRecipeSelectChanged();
        }
        private void UnitRecipeSelectChanged()
        {
            try
            {
                RecipeName = null;
                OpenFiles.Clear();
                string fullpath = Path.Combine(path, Enum.GetName(typeof(BathNameEnum), BathName));
                if (Directory.Exists(fullpath))
                {
                    DirectoryInfo TheFolder = new DirectoryInfo(fullpath);
                    foreach (FileInfo NextFile in TheFolder.GetFiles())
                    {
                        int i = NextFile.Name.IndexOf(".json");
                        if (i > 0)
                        {
                            OpenFiles.Add(NextFile.Name);
                        }
                        else
                        {
                            //Program.Variable.Faultstr = "Recipe Name Error len<len";
                        }

                    }
                }
            }
            catch (Exception ee)
            {

                throw;
            }
        }     
        private void Cancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.No));
        }
        private void Confirm()
        {
            if (RecipeName == null)
            {
                return;
            }
            FlowStep.UnitRecipeName = RecipeName;
            FlowStep.BathName = BathName;
            DialogParameters keyValuePairs = new DialogParameters();
            keyValuePairs.Add("FlowStep", FlowStep);
            DialogResult dialogResult = new DialogResult(ButtonResult.OK, keyValuePairs);
            RequestClose?.Invoke(dialogResult);
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
            FlowStep = parameters.GetValue<FlowStepClass>("FlowStep");
            BathName = FlowStep.BathName;
            RecipeName = FlowStep.UnitRecipeName;
        }
    }
}
