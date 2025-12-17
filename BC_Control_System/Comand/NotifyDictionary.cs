using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BC_Control_System.Converters
{
    public class NotifyDictionary<TKey, TValue> : Dictionary<TKey, TValue>, INotifyPropertyChanged
    {
        //private readonly Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();

        public new TValue this[TKey key]
        {
            get
            {
                // 如果不存在则返回默认值（或你指定的默认值）
                return base.TryGetValue(key, out TValue value) ? value : default(TValue);
            }
            set
            {
                base[key] = value;
                OnPropertyChanged(Binding.IndexerName);
            }
        }
        public new void Add(TKey key, TValue value)
        {
            base.Add(key, value);
            OnPropertyChanged(Binding.IndexerName);
        }

        // 可选：重写 Remove 方法以确保触发通知
        public new bool Remove(TKey key)
        {
            bool result = base.Remove(key);
            if (result) OnPropertyChanged(Binding.IndexerName);
            return result;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
