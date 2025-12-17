
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using BC_Control_Helper;
using BC_Control_Models;
using BC_Control_Models.RecipeModel;
using BC_Control_System.Interface;
using BC_Control_System.View.Opration;

namespace BC_Control_System.ViewModel.Recipe.ModuleRecipe
{
    [AddINotifyPropertyChangedInterface]
    public class SYS9070_1RecipeViewModel : BindableBase, INavigationAware, IRecipeCommand<SCClass>
    {
        private string filePath;
        private string moduleName;
        private IDialogService _dialogService;
        public string ModuleName
        {
            get { return moduleName; }
            set { moduleName = value; RaisePropertyChanged(); }
        }
        public SYSRecipeClass sYSRecipeClass { get; set; }
        [OnChangedMethod("UpdateBaseRecipeList")]
        public BindingList<SCClass> SCClasses { get; set; }
        public DelegateCommand OpenFileCommand { get; set; }
        public DelegateCommand DeleteFileCommand { get; set; }
        public DelegateCommand SaveFileCommand { get; set; }
        public DelegateCommand AddStepCommand { get; set; }
        public DelegateCommand AddPreStepCommand { get; set; }
        public DelegateCommand AddPostStepCommand { get; set; }
        public DelegateCommand<SCClass> InsertStepCommand { get; set; }
        public DelegateCommand<SCClass> DeleteStepCommand { get; set; }
        public DelegateCommand NewRecipeCommand { get; set; }
        
        // 添加模块类型标识属性
        private bool _isType1Module;
        public bool IsType1Module
        {
            get => _isType1Module;
            set => SetProperty(ref _isType1Module, value);
        }
        public bool IsTypeNull { get; set; }
        public SYS9070_1RecipeViewModel(IDialogService dialogService)
        {
            _dialogService= dialogService;
            OpenFileCommand=new DelegateCommand(OpenFile);
            DeleteFileCommand=new DelegateCommand(DeleteFile);
            SaveFileCommand = new DelegateCommand(SaveFile);
            AddStepCommand = new DelegateCommand(AddStep);
            AddPreStepCommand = new DelegateCommand(AddPreStep);
            AddPostStepCommand = new DelegateCommand(AddPostStep);
            InsertStepCommand = new DelegateCommand<SCClass>(InsStep);
            DeleteStepCommand = new DelegateCommand<SCClass>(DelStep);
            SCClasses = new BindingList<SCClass>();
            sYSRecipeClass = new SYSRecipeClass();
            NewRecipeCommand = new DelegateCommand(NewRecipe);
            SCClasses.ListChanged += List_ListChanged;

            sYSRecipeClass = new SYSRecipeClass();
            
            UpdateRecipeList();
        }
        private void List_ListChanged(object sender, ListChangedEventArgs e)
        {

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            ModuleName=navigationContext.Parameters.GetValue<string>("ModuleName");
            filePath = Path.Combine(@"D:\Recipe",ModuleName);
            sYSRecipeClass = new SYSRecipeClass();

            // 根据模块名称设置DSM列的标题和最大值
            SetModuleType(ModuleName);
        }

        private void SetModuleType(string moduleName)
        {
            // 判断模块类型
            IsType1Module = false;
            IsTypeNull = false;
        }
        
        public void NewRecipe()
        {
            sYSRecipeClass = new SYSRecipeClass();
            SCClasses = new BindingList<SCClass>();
            UpdateRecipeList();
        }
        private void OpenFile()
        {
            DialogParameters key = new DialogParameters();
            key.Add("FilePath", filePath);
            _dialogService.ShowDialog(nameof(OpenFileView), key, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    var tempresult= result.Parameters.GetValue<string>("Result1");
                    sYSRecipeClass = JSONHelper.JSONToEntity<SYSRecipeClass>(tempresult);
                    UpdateRecipeList();
                }
            });
        }
        private void DeleteFile()
        {
            DialogParameters key = new DialogParameters();
            key.Add("FilePath", filePath);
            _dialogService.ShowDialog(nameof(OpenFileView), key, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    var tempresult = result.Parameters.GetValue<string>("Result1");
                    SYSRecipeClass tempsYSRecipeClass = JSONHelper.JSONToEntity<SYSRecipeClass>(tempresult);
                    string path1 = Path.Combine(filePath, $"{tempsYSRecipeClass.Name}.json");
                    File.Delete(path1);// 3.2、删除文件
                                       //string s=FormHelper.deleteOneFile(path);
                                       //MessageBox.Show(s);
                }
            });
        }
        private void SaveFile()
        {
            if ((int)MessageBox.Show("Save this Recipe ?", "Confirm Message", MessageBoxButton.OKCancel) == 1)
            {
                sYSRecipeClass.PreSCRecipeList = SCClasses.Where(filter=>filter.Step.Contains("Pre")).ToList();
                sYSRecipeClass.PostSCRecipeList= SCClasses.Where(filter => filter.Step.Contains("Post")).ToList();//
                sYSRecipeClass.SCRecipeList = SCClasses.Where(filter => Regex.IsMatch(filter.Step, @"^\d+$")).ToList();
                string json = JSONHelper.EntityToJSON(sYSRecipeClass);
                string path = Path.Combine(filePath, $"{sYSRecipeClass.Name}.json");
                File.WriteAllText(path, json);
            }
        }
        public void AddStep()
        {
            try
            {
                if (SCClasses.Where(para => Regex.IsMatch(para.Step, @"^\d+$")).Count() >= 25)
                {
                    MessageBox.Show("添加失败");
                    return;
                }
                var t = SCClasses.ToList().FindLast(para => Regex.IsMatch(para.Step, @"^\d+$"));
                if (t == null)
                {
                    SCClasses.Add(new SCClass() { Step = $"1" });

                }
                else
                {
                    SCClasses.Add(new SCClass() { Step = $"{int.Parse(t.Step) + 1}" });
                }

                AddBindingListStep();
            }
            catch (Exception)
            {

                throw;
            }


        }
        public void AddPreStep()
        {
            try
            {
                if (SCClasses.Where(para => para.Step.Contains("Pre")).Count() >= 5)
                {
                    MessageBox.Show("添加失败");
                    return;
                }
                var t = SCClasses.ToList().FindLast(para => para.Step.Contains("Pre"));
                if (t == null)
                {
                    SCClasses.Add(new SCClass() { Step = $"Pre1" });

                }
                else
                {
                    int t1 = int.Parse(t.Step.Replace("Pre", "")) + 1;
                    SCClasses.Add(new SCClass() { Step = $"Pre{t1}" });
                }

                AddBindingListStep();
            }
            catch (Exception ee)
            {

            }
        }
        public void AddPostStep()
        {
            try
            {
                if (SCClasses.Where(para => para.Step.Contains("Post")).Count() >= 5)
                {
                    MessageBox.Show("添加失败");
                    return;
                }
                var t = SCClasses.ToList().FindLast(para => para.Step.Contains("Post"));
                if (t == null)
                {
                    SCClasses.Add(new SCClass() { Step = $"Post1" });

                }
                else
                {
                    int t1 = int.Parse(t.Step.Replace("Post", "")) + 1;
                    SCClasses.Add(new SCClass() { Step = $"Post{t1}" });
                }

                AddBindingListStep();
            }
            catch (Exception ee)
            {


            }
        }
        public void InsStep(SCClass sCClass)
        {
            try
            {
                if (sCClass == null)
                {
                    return;
                }
                if (sCClass.Step.Contains("Pre"))
                {
                    int t = int.Parse(sCClass.Step.Replace("Pre", "")) + 1;
                    UpdateRecipeListStep(t, "Pre");
                }
                else if (sCClass.Step.Contains("Post"))
                {
                    int t = int.Parse(sCClass.Step.Replace("Post", "")) + 1;
                    UpdateRecipeListStep(t, "Post");
                }
                else
                {
                    int t = int.Parse(sCClass.Step) + 1;
                    UpdateRecipeListStep(t);
                }
                AddBindingListStep();
            }
            catch (Exception)
            {


            }

        }
        public void DelStep(SCClass sCClass)
        {
            try
            {
                if (sCClass == null)
                {
                    return;
                }
                if (sCClass.Step.Contains("Pre"))
                {
                    int t = int.Parse(sCClass.Step.Replace("Pre", ""));
                    DeleteRecipeListStep(t, "Pre", sCClass);
                }
                else if (sCClass.Step.Contains("Post"))
                {
                    int t = int.Parse(sCClass.Step.Replace("Post", ""));
                    DeleteRecipeListStep(t, "Post", sCClass);
                }
                else
                {
                     int t = int.Parse(sCClass.Step);
                      DeleteRecipeListStep(t, "", sCClass);
                }
                AddBindingListStep();
            }
            catch (Exception ee)
            {

            }
        }


        private void AddBindingListStep()
        {
            var sortedItems = SCClasses
            .Select(item => new
            {
                Original = item,
                Prefix = GetPrefix(item.Step), // 获取前缀 (Pre, Post 或空)
                Number = GetNumber(item.Step)  // 获取数字部分
            })
            .OrderBy(x => x.Prefix) // 按前缀排序
            .ThenBy(x => x.Number)                         // 按数字部分排序
            .Select(x => x.Original)                      // 返回原始字符串
            .ToList();
            SCClasses.Clear();
            SCClasses.AddRange(sortedItems);
        }
        private int GetPrefix(string input)
        {
            if (input.StartsWith("Pre"))
                return 0;
            if (input.StartsWith("Post"))
                return 2;
            return 1; // 无前缀
        }

        // 提取数字部分
        private int GetNumber(string input)
        {
            var digits = new string(input.Where(char.IsDigit).ToArray());
            return int.TryParse(digits, out int result) ? result : 0;
        }
        private void UpdateRecipeListStep(int t, string s = "")
        {

            foreach (var item in SCClasses.Where(para => para.Step.Contains(s)))
            {
                //if (s == "")
                //{
                //    int t1 = int.Parse(item.Step);
                //    if (t <= t1)
                //    {
                //        item.Step = $"{int.Parse(item.Step) + 1}";
                //    }
                //}
                //else
                //{
                //    int t1 = int.Parse(item.Step.Replace(s, ""));
                //    if (t <= t1)
                //    {
                //        item.Step = $"{s}{int.Parse(item.Step.Replace(s, "")) + 1}";
                //    }
                //}
                switch (s)
                {
                    case "Pre":
                        int t1 = int.Parse(item.Step.Replace(s, ""));
                        if (t <= t1)
                        {
                            item.Step = $"{s}{int.Parse(item.Step.Replace(s, "")) + 1}";
                        }
                        break;
                    case "Post":
                        int t22 = int.Parse(item.Step.Replace(s, ""));
                        if (t <= t22)
                        {
                            item.Step = $"{s}{int.Parse(item.Step.Replace(s, "")) + 1}";
                        }
                        break;
                    default:
                        int t33;
                        if (!int.TryParse(item.Step, out t33))
                        {
                            break;
                        }
                        if (t <= t33)
                        {
                            item.Step = $"{int.Parse(item.Step) + 1}";
                        }
                        break;
                }

            }
            SCClasses.Add(new SCClass() { Step = $"{s}{t}" });
        }
        private void DeleteRecipeListStep(int t, string s = "", SCClass sCClass = null)
        {
            SCClasses.Remove(sCClass);
            foreach (var item in SCClasses.Where(para => para.Step.Contains(s)))
            {
                try
                {
                    switch (s)
                    {
                        case "Pre":
                            int t1 = int.Parse(item.Step.Replace(s, ""));
                            if (t < t1)
                            {
                                item.Step = $"{s}{int.Parse(item.Step.Replace(s, "")) - 1}";
                            }
                            break;
                        case "Post":
                            int t22 = int.Parse(item.Step.Replace(s, ""));
                            if (t < t22)
                            {
                                item.Step = $"{s}{int.Parse(item.Step.Replace(s, "")) - 1}";
                            }
                            break;
                        default:
                            int t33;
                            if (!int.TryParse(item.Step, out t33))
                            {
                                break;
                            }
                            if (t < t33)
                            {
                                item.Step = $"{int.Parse(item.Step) - 1}";
                            }
                            break;
                    }
                }
                catch (Exception ee)
                {

                    
                }
                
            }
        }
        private void UpdateRecipeList()
        {
            SCClasses.Clear();
            SCClasses.AddRange(sYSRecipeClass.PreSCRecipeList.Select(para => new SCClass() { Step = $"{para.Step}", Time = para.Time, DSM = para.DSM, PumpStop = para.PumpStop, Agination = para.Agination }));
            SCClasses.AddRange(sYSRecipeClass.SCRecipeList);
            SCClasses.AddRange(sYSRecipeClass.PostSCRecipeList.Select(para => new SCClass() { Step = $"{para.Step}", Time = para.Time, DSM = para.DSM, PumpStop = para.PumpStop, Agination = para.Agination }));
        }
        private void UpdateBaseRecipeList()
        {

        }

    }
}
