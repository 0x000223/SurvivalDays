using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MemoryTool
{
    public class User
    {
        public User()
        {

        }

        public User(int processId)
        {
            ProcessId = processId;
        }

        public static int ProcessId { get; set; }

        public static int GetProcessId(string processName)
        {
            try
            {
                return Process.GetProcessesByName(processName)[0].Id;
            }
            catch { return 0; }
        }

        public static ulong GetModule(int processId, string moduleName)
        {
            var process = Process.GetProcessById(processId);

            for (var i = 0; i < process.Modules.Count; i++)
            {
                if (string.Equals(process.Modules[i].ModuleName, moduleName, StringComparison.CurrentCultureIgnoreCase))
                {
                    return (ulong) process.Modules[i].BaseAddress;
                }
            }

            return 0;
        }

        public static ulong GetMainModule(int processId)
        {
            var process = Process.GetProcessById(processId);

            if (process.MainModule == null)
            {
                return 0;
            }

            return (ulong) process.MainModule.BaseAddress;
        }
    }
}
