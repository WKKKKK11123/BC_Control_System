using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
using System.ComponentModel;
using BC_Control_Models;
using System.Collections.Concurrent;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BC_Control_System.ViewModels
{

    public partial class IOViewModel : ObservableObject, IDialogAware
    {
        // 新增标题属性
        private string _displayTitle = "IO View";
        private IPLCHelper _plcHelper;
        [ObservableProperty]
        private string displayTitle;


        public enum Input
        {
            X00 = 0,
            X01 = 16,
            X02 = 32,
            X03 = 48,
            X04 = 64,
            X05 = 80,
            X06 = 96,
        }

        public enum Output
        {
            Y00 = 0,
            Y01 = 16,
            Y02 = 32,
            Y03 = 48,
            Y04 = 64,
            Y05 = 80,
            Y06 = 96,
        }

        private readonly Dictionary<PlcEnum, (Input[] inputs, Output[] outputs)> PlcIoRanges = new Dictionary<PlcEnum, (Input[] inputs, Output[] outputs)>
        {
            {PlcEnum.PLC1,(new[] { Input.X00, Input.X01, Input.X02, Input.X03 }, new[]{Output.Y00, Output.Y01, Output.Y02, Output.Y03, Output.Y04, Output.Y05}) },
            {PlcEnum.PLC2,(new[] { Input.X00, Input.X01, Input.X02, Input.X03, Input.X04}, new[]{Output.Y00, Output.Y01, Output.Y02 }) },
            {PlcEnum.PLC3,(new[] { Input.X00, Input.X01, Input.X02, Input.X03 }, new[]{Output.Y00, Output.Y01, Output.Y02 }) },
            {PlcEnum.PLC4,(new[] { Input.X00, Input.X01, Input.X02, Input.X03, Input.X04, Input.X05, Input.X06 }, new[]{Output.Y00, Output.Y01, Output.Y02, Output.Y03}) },
            {PlcEnum.PLC5,(new[] { Input.X00, Input.X01, Input.X02, Input.X03, Input.X04, Input.X05, Input.X06 }, new[]{Output.Y00, Output.Y01, Output.Y02}) },
            {PlcEnum.PLC6,(new[] { Input.X00, Input.X01, Input.X02, Input.X03, Input.X04, Input.X05, Input.X06 }, new[]{Output.Y00, Output.Y01, Output.Y02 }) },
        };
        private Device _device;

        [ObservableProperty]
        public Input inputMenuItem;
        [ObservableProperty]
        public Output outputMenuItem;

        public BindingList<Input> InputEnumValues { get; }

        public ObservableCollection<Output> OutputEnumValues { get; }

        public string Title { get; set; } = "IO View";
        [ObservableProperty]
        public BindingList<Variable> inputOberver;
        [ObservableProperty]
        public BindingList<Variable> outputOberver;

        public event Action<IDialogResult> RequestClose;

        private CancellationTokenSource cts = new CancellationTokenSource();
        public IOViewModel(IRegionManager regionManager, IPLCHelper pLCHelper)
        {
            _plcHelper = pLCHelper;
            InputEnumValues = new BindingList<Input>();
            OutputEnumValues = new ObservableCollection<Output>();
        }

        public void EnquiryState()
        {
            Task t = new Task(() =>
            {
                var tocken = cts.Token;
                while (!tocken.IsCancellationRequested)
                {
                    try
                    {
                        Thread.Sleep(500);
                        if (!_device.IsConnected)
                        {
                            continue;
                        }
                        foreach (var variable in InputOberver)
                        {

                            variable.VarValue = _device[variable.VarName];
                        }
                        foreach (var variable in OutputOberver)
                        {
                            variable.VarValue = _device[variable.VarName];
                        }
                    }
                    catch (Exception)
                    {

                    }

                }
            }, cts.Token);
            t.Start();


        }
        partial void OnInputMenuItemChanged(Input value)
        {

            try
            {
                List<Variable> templist = _device.GroupList.FirstOrDefault(P => P.GroupName == "Input").VarList.
                Where(P => P.Start >= (int)InputMenuItem & P.Start < (int)InputMenuItem + 16).ToList();
                InputOberver = new BindingList<Variable>(templist);
            }
            catch (Exception ee)
            {

            }
        }
        partial void OnOutputMenuItemChanged(Output value)
        {

            try
            {
                List<Variable> templist = _device.GroupList.FirstOrDefault(P => P.GroupName == "Output").VarList.
                               Where(P => P.Start >= (int)OutputMenuItem & P.Start < (int)OutputMenuItem + 16).ToList();
                OutputOberver = new BindingList<Variable>(templist);

            }
            catch (Exception ee)
            {

            }
        }
       
        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            cts.Cancel();
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            // 新增：设置显示标题
            if (parameters.TryGetValue("DisplayText", out string text))
                DisplayTitle = text;

            string plcEnumStr = parameters.GetValue<string>("PLCEnum");
            var plcEnum = (PlcEnum)Enum.Parse(typeof(PlcEnum), plcEnumStr);
            _device = _plcHelper.SelectDevice(plcEnum);

            if (PlcIoRanges.TryGetValue(plcEnum, out var ranges))
            {
                InputEnumValues.Clear();
                foreach (var input in ranges.inputs)
                    InputEnumValues.Add(input);

                OutputEnumValues.Clear();
                foreach (var output in ranges.outputs)
                    OutputEnumValues.Add(output);

            }

            InputMenuItem = InputEnumValues.FirstOrDefault();
            OutputMenuItem = OutputEnumValues.FirstOrDefault();

            InputOberver = new BindingList<Variable>(_device.GroupList.FirstOrDefault(P => P.GroupName == "Input").VarList
                .Where(P => P.Start >= (int)InputMenuItem & P.Start < (int)InputMenuItem + 16).ToList());

            OutputOberver = new BindingList<Variable>(_device.GroupList.FirstOrDefault(P => P.GroupName == "Output").VarList
                .Where(P => P.Start >= (int)OutputMenuItem & P.Start < (int)OutputMenuItem + 16).ToList());

            EnquiryState();
        }
    }
}
