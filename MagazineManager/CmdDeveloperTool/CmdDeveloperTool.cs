using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazineManager.CmdDeveloperTool
{
    internal class CmdDeveloperTool
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll")] //Support Console
        private static extern bool AllocConsole(); //Support Console

        public CmdDeveloperTool()
        {
            AllocConsole(); //Support Console
        }
    }
}
