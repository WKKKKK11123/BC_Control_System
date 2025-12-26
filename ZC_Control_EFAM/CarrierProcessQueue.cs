using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PropertyChanged;

namespace ZC_Control_EFAM
{
    [AddINotifyPropertyChangedInterface]
    public class CarrierProcessQueue
    {
        public CarrierProcessQueue()
        {
            BatchidCollection.CollectionChanged += BatchidCollection_CollectionChanged;
        }

        private void BatchidCollection_CollectionChanged(
            object sender,
            System.Collections.Specialized.NotifyCollectionChangedEventArgs e
        )
        {
            OnPropertyChanged(string.Empty);
        }

        public CarrierProcessQueueModel CurrentBatchid { get; set; } =
            new CarrierProcessQueueModel();

        public ObservableCollection<CarrierProcessQueueModel> BatchidCollection { get; set; } =
            new ObservableCollection<CarrierProcessQueueModel>();

        public void OnPropertyChanged(string propertyName)
        {
            File.WriteAllText(
                System.IO.Path.Combine(
                    Environment.CurrentDirectory,
                    "Status",
                    "CarrierProcessQueue.json"
                ),
                JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented)
            );
        }
    }

    [AddINotifyPropertyChangedInterface]
    public class CarrierProcessQueueModel
    {
        public string Batchid { get; set; } = string.Empty;

        public string RecipeName { get; set; } = string.Empty;
        public BatchState BatchState { get; set; }

        public int Priority { get; set; }
    }
}
