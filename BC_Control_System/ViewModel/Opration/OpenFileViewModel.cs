using CommunityToolkit.Mvvm.ComponentModel;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.IO;
using System.Windows;

namespace BC_Control_System.ViewModel.Opration
{
    public partial class OpenFileViewModel : ObservableObject, IDialogAware
    {
        private string filepath;
        [ObservableProperty]
        
        private List<string> _OpenFiles = new List<string>();
        [ObservableProperty]
        private string _SelectFile = "";
        public string Title { get; set; } = "Open Folder";

        public event Action<IDialogResult> RequestClose;
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand ConfirmCommand { get; set; }
        public OpenFileViewModel()
        {
            CancelCommand = new DelegateCommand(Cancel);
            ConfirmCommand = new DelegateCommand(Confirm);
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
            filepath = parameters.GetValue<string>("FilePath");
            OpenFiles = new List<string>();
            DirectoryInfo TheFolder = new DirectoryInfo(filepath);
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
        private void Cancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.No));
        }

        private void Confirm()
        {
            if (string.IsNullOrEmpty(SelectFile))
            {
                MessageBox.Show("未选择文件");
                return;
            }
            string result = "";
            string temppath = Path.Combine(filepath, SelectFile);
            result = File.ReadAllText(temppath);
            DialogParameters keys = new DialogParameters();
            keys.Add("Result1", result);
            keys.Add("Result2", SelectFile);
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, keys));
        }
    }
}
