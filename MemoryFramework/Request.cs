using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MemoryTool
{
    public partial class Driver
    {
        private static readonly uint IOCTL_GET_MODULE_BASE = CTL_CODE(0x00008FFF, 0x800, 0, 1 | 2);
        private static readonly uint IOCTL_READ            = CTL_CODE(0x00008FFF, 0x801, 0, 1 | 2);
        private static readonly uint IOCTL_WRITE           = CTL_CODE(0x00008FFF, 0x802, 0, 1 | 2);

        private static uint CTL_CODE(uint deviceType, uint function, uint method, uint access)
        {
            return ((deviceType << 16) | (access << 14) | (function << 2) | method);
        }

        private struct GetModuleBaseRequest
        {
            private int _pid;
            public readonly IntPtr ModuleName;
            public IntPtr ResultBuffer;
            private bool _isWow64;

            public GetModuleBaseRequest(int pid, string moduleName = "UnityPlayer.dll")
            {
                _pid = pid;
                _isWow64 = false;
                ModuleName = Marshal.StringToHGlobalUni(moduleName);
                ResultBuffer = IntPtr.Zero;
            }
        }

        private struct ReadRequest
        {
            private int _size;
            private int _pid;
            private ulong _targetAddress;
            public IntPtr ResultBuffer;

            public ReadRequest(int pid, ulong address, int size)
            {
                _pid = pid;
                _size = size;
                _targetAddress = address;
                ResultBuffer = IntPtr.Zero;
            }
        }

        private struct WriteRequest
        {
            public int ProcessId;
            public ulong Address;
            public IntPtr Buffer;
            public int Size;

            public WriteRequest(int processId, ulong address, IntPtr buffer, int size)
            {
                ProcessId = processId;
                Address = address;
                Buffer = buffer;
                Size = size;
            }
        }
    }
}
