using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toaster.Interfaces
{
    public interface IToaster : INotifyPropertyChanged
    {
        double Setting { get; set; }
        string Content { get; set; }
        bool Toasting { get; set; }
        string Color { get; set; }
    }
}
