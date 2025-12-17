using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace BC_Control_System.Comand
{
    public class CommandExtension : MarkupExtension
    {
        public Type CommandType { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (CommandType == null)
                throw new InvalidOperationException("CommandType must be specified");

            // 从 Prism 容器中解析命令实例
            var container = Prism.Ioc.ContainerLocator.Container;
            if (container == null)
                throw new InvalidOperationException("Prism container is not available");

            return container.Resolve(CommandType);
        }
    }
}
