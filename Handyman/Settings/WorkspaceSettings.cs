using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Handyman.Settings
{
    public class WorkspaceSettings : IWorkspaceSettings
    {
        public string DefaultRequestProjectName { get; set; }
    }
}
