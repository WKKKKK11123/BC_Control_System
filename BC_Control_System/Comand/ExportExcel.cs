using System.Data;
using System.Windows.Input;

namespace BC_Control_System.Command
{
    public class ExportExcel : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return parameter is DataTable;
        }

        public void Execute(object parameter)
        {
            if (parameter is DataTable dt)
            {
                Execute(dt, null);
            }
        }

        public void Execute(DataTable dt, string filePath)
        {
            //string path = filePath;

            //if (string.IsNullOrEmpty(path))
            //{
            //    using (var sfd = new SaveFileDialog())
            //    {
            //        sfd.Filter = "Excel文件 (*.xlsx)|*.xlsx";
            //        sfd.FileName = $"{dt.TableName}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

            //        if (sfd.ShowDialog() == DialogResult.OK)
            //        {
            //            path = sfd.FileName;
            //        }
            //        else
            //        {
            //            return;
            //        }
            //    }
            //}

            //try
            //{
            //    // 这里用你的 AsposeExcel 或其他导出实现
            //    AsposeExcel outPutExcel = new AsposeExcel(path, "FactoryAffair");
            //    if (outPutExcel.DatatableToExcel(dt))
            //    {
            //    }
            //    else
            //    {
            //        MessageBox.Show("导出失败");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("导出异常: " + ex.Message);
            //}
        }

    }
}
