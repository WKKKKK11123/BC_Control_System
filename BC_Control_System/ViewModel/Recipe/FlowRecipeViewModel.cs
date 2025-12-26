using BC_Control_System.View.Opration;
using BC_Control_System.View.Recipe;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using BC_Control_Helper;
using BC_Control_Models;
using BC_Control_Models.RecipeModel;
using CommunityToolkit.Mvvm.ComponentModel;
using BC_Control_Models.RecipeModel.RecipeBase;
using EnumsNET;
using NPOI.SS.Formula.Functions;
using SqlSugar;
using MathNet.Numerics.Differentiation;

namespace BC_Control_System.ViewModel.Recipe
{
    public partial class FlowRecipeViewModel : ObservableObject
    {
        private IDialogService _dialogService;
        private string filepath = @"C:\212Recipe\Tool";
        [ObservableProperty]
        private FlowRecipeClass _FlowRecipeModel=new FlowRecipeClass();
        [ObservableProperty]
        private BindingList<FlowStepClass> _FlowSteps = new BindingList<FlowStepClass>();

        public DelegateCommand OpenFileCommand { get; set; }
        public DelegateCommand DeleteFileCommand { get; set; }
        public DelegateCommand SaveFileCommand { get; set; }
        public DelegateCommand NewRecipeCommand { get; set; }
        public DelegateCommand AddStepCommand { get; set; }
        public DelegateCommand<FlowStepClass> InsertStepCommand { get; set; }
        public DelegateCommand<FlowStepClass> DeleteStepCommand { get; set; }
        public DelegateCommand<FlowStepClass> ChangeStepCommand { get; set; }



        public FlowRecipeViewModel(IDialogService dialogService)
        {            
            _dialogService = dialogService;
            OpenFileCommand = new DelegateCommand(OpenFile);
            DeleteFileCommand = new DelegateCommand(DeleteFile);
            SaveFileCommand = new DelegateCommand(SaveFile);
            AddStepCommand = new DelegateCommand(AddStep);
            InsertStepCommand = new DelegateCommand<FlowStepClass>(InsStep);
            DeleteStepCommand = new DelegateCommand<FlowStepClass>(DelStep);
            NewRecipeCommand = new DelegateCommand(NewRecipe);
            ChangeStepCommand = new DelegateCommand<FlowStepClass>(ChangeStep);
            updateStepList();

        }

        private void ChangeStep(FlowStepClass @class)
        {
            if (@class == null)
            {
                return;
            }
            DialogParameters keyValuePairs = new DialogParameters();
            IDialogResult dialogResult = new DialogResult();
            keyValuePairs.Add("FlowStep", @class);
            _dialogService.ShowDialog(nameof(FlowRecipeStepView), keyValuePairs, r => dialogResult = r);
            if (dialogResult.Result == ButtonResult.OK)
            {
                var temp = dialogResult.Parameters.GetValue<FlowStepClass>("FlowStep");
                FlowSteps.Where(filter => filter.FlowStep == temp.FlowStep).Select(p => p = temp);
            }
        }

        private void updateStepList()
        {
            FlowSteps = new BindingList<FlowStepClass>(FlowRecipeModel.FlowStepList);
        }
        private void NewRecipe()
        {
            FlowRecipeModel = new FlowRecipeClass();
            updateStepList();
        }

        private void DelStep(FlowStepClass @class)
        {
            if (@class == null)
            {
                return;
            }
            FlowSteps.Remove(@class);
            FlowSteps.Where(x => x.FlowStep > @class.FlowStep).Select(x => x.FlowStep = x.FlowStep - 1).ToList();
            var temp2 = FlowSteps.OrderBy(p => p.FlowStep).ToList();
            FlowSteps.Clear();
            FlowSteps.AddRange(temp2);
        }

        private void InsStep(FlowStepClass @class)
        {
            if (@class == null)
            {
                return;
            }
            var temp = new FlowStepClass()
            {
                FlowStep = @class.FlowStep + 1,
                BathName = @class.BathName,
                UnitRecipeName = @class.UnitRecipeName,
            };
            DialogParameters keyValuePairs = new DialogParameters();
            IDialogResult dialogResult = new DialogResult();
            keyValuePairs.Add("FlowStep", temp);
            _dialogService.ShowDialog(nameof(FlowRecipeStepView), keyValuePairs, r => dialogResult = r);
            if (dialogResult.Result == ButtonResult.OK)
            {
                var result = dialogResult.Parameters.GetValue<FlowStepClass>("FlowStep");
                FlowSteps.Where(x => x.FlowStep >= result.FlowStep).Select(x => x.FlowStep = x.FlowStep + 1).ToList();
                FlowSteps.Add(result);
                var temp2 = FlowSteps.OrderBy(p => p.FlowStep).ToList();
                FlowSteps.Clear();
                FlowSteps.AddRange(temp2);
            }
        }
        private void SaveFile()
        {
            bool hasMGD9 = FlowSteps.Any(step => step.BathName == BathNameEnum.LPD_1);
            
            if(!CheckUnitStepTime())
            {
                return;
            }
            if (!hasMGD9)
            {
                MessageBox.Show("保存失败：必须至少包含一个LPD的步骤", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            bool hasQDR = FlowSteps.Any(step => step.BathName == BathNameEnum.QDR_1) || FlowSteps.Any(step => step.BathName == BathNameEnum.QDR_2)
            || FlowSteps.Any(step => step.BathName == BathNameEnum.QDR_3) || FlowSteps.Any(step => step.BathName == BathNameEnum.QDR_4)
            || FlowSteps.Any(step => step.BathName == BathNameEnum.QDR_5);

            if (!hasQDR)
            {
                MessageBox.Show("保存失败：必须至少包含一个QDR的步骤", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if ((int)MessageBox.Show("Save this Recipe ?", "Confirm Message", MessageBoxButton.OKCancel) == 1)
            {
                FlowRecipeModel.FlowStepList = FlowSteps.ToList();
                string json = JSONHelper.EntityToJSON(FlowRecipeModel);
                string path = Path.Combine(filepath, $"{FlowRecipeModel.Name}.json");
                File.WriteAllText(path, json);
                
            }
        }
        private bool CheckUnitStepTime()
        {
            try
            {
                string recipePath = @"C:\212Recipe";

                for (int i = 0; i < FlowSteps.Count - 1; i++)
                {
                    var current = FlowSteps[i];
                    var next = FlowSteps[i + 1];

                    // 当前步配方
                    var currentPath = Path.Combine(
                        recipePath,
                        current.BathName.GetName()!,
                        current.UnitRecipeName
                    );

                    var currentRecipeJson = File.ReadAllText(currentPath);
                    var currentRecipe =
                        JSONHelper.JSONToEntity<ModuleRecipeClassBase<TotalStep>>(currentRecipeJson);

                    int totalTime = currentRecipe.RecipeStepCollection
                        .Sum(x => x.Time);

                    // 下一步配方（只算 PreStep）
                    var nextPath = Path.Combine(
                        recipePath,
                        next.BathName.GetName()!,
                        next.UnitRecipeName
                    );

                    var nextRecipeJson = File.ReadAllText(nextPath);
                    var nextRecipe =
                        JSONHelper.JSONToEntity<ModuleRecipeClassBase<TotalStep>>(nextRecipeJson);

                    int preTime = nextRecipe.RecipeStepCollection
                        .Where(x => x.StepType == ProcessStepEnum.PreStep)
                        .Sum(x => x.Time);

                    if (preTime > totalTime)
                    {
                        MessageBox.Show(
                            $"第{current.FlowStep}步 {current.BathName.GetName()} 的配方总时间 {totalTime} S，" +
                            $"小于下一步提前步时间 {preTime} S"
                        );
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

        }
        private void GetPreTime()
        { 
        
        }
        private void AddStep()
        {
            var temp = new FlowStepClass()
            {
                FlowStep = FlowSteps.Count() + 1,
                BathName = BathNameEnum.Ag_1,
            };
            DialogParameters keyValuePairs = new DialogParameters();
            IDialogResult dialogResult = new DialogResult();
            keyValuePairs.Add("FlowStep", temp);
            _dialogService.ShowDialog(nameof(FlowRecipeStepView), keyValuePairs, r => dialogResult = r);
            if (dialogResult.Result == ButtonResult.OK)
            {
                var result = dialogResult.Parameters.GetValue<FlowStepClass>("FlowStep");
                FlowSteps.Add(result);
            }
        }

        private void DeleteFile()
        {
            DialogParameters key = new DialogParameters();
            key.Add("FilePath", filepath);
            _dialogService.ShowDialog(nameof(OpenFileView), key, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    var tempresult = result.Parameters.GetValue<string>("Result1");
                    FlowRecipeClass tempRecipe = JSONHelper.JSONToEntity<FlowRecipeClass>(tempresult);
                    string path1 = Path.Combine(filepath, $"{tempRecipe.Name}.json");
                    File.Delete(path1);// 3.2、删除文件
                                       //string s=FormHelper.deleteOneFile(path);
                                       //MessageBox.Show(s);
                }
            });
        }

        private void OpenFile()
        {
            DialogParameters key = new DialogParameters();
            key.Add("FilePath", filepath);
            _dialogService.ShowDialog(nameof(OpenFileView), key, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    var tempresult = result.Parameters.GetValue<string>("Result1");
                    FlowRecipeModel = JSONHelper.JSONToEntity<FlowRecipeClass>(tempresult);
                    updateStepList();
                }
            });
        }
    }
}
