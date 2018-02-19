using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Handyman
{
    // TODO: when upgrade roslyn version find out how to get this from it

    public interface IHostService
    {
        string GetDefaultNamespace(string projectName);
    }
}
