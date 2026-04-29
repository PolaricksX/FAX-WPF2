using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace FAX_WPF
{
    public interface IMainView
    {
        string StatusMessage { get; set; }
    }
}
