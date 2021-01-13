using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using MemoryTool.Native;
using Microsoft.Win32.SafeHandles;

namespace MemoryTool
{
    public partial class Driver
    {
        private const string SymLink = "\\\\.\\MemoryTool";

        private static SafeFileHandle hDriver;

        public Driver()
        {
            hDriver =
                Kernel32.CreateFile(SymLink, FileAccess.Read | FileAccess.Write, FileShare.ReadWrite, IntPtr.Zero,
                    FileMode.Open, 0, IntPtr.Zero);
        }

        public Driver(int processId)
        {
            hDriver =
                Kernel32.CreateFile(SymLink, FileAccess.Read | FileAccess.Write, FileShare.ReadWrite, IntPtr.Zero,
                    FileMode.Open, 0, IntPtr.Zero);

            ProcessId = processId;
        }

        public static int ProcessId { get; set; }

        public static T Read<T>(ulong address)
        {
            var type1 = typeof(T);
            var size = Marshal.SizeOf(type1);
            var buffer = new byte[size];

            // Request initialization
            var kernelRequest = new ReadRequest(ProcessId, address, size);
            var requestSize = (uint) Marshal.SizeOf(kernelRequest);

            var gcHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            kernelRequest.ResultBuffer = gcHandle.AddrOfPinnedObject();

            var bytesReturned = 0;

            var status =
                Kernel32.DeviceIoControl(hDriver, IOCTL_READ, kernelRequest, requestSize, null, 0, ref bytesReturned,
                    IntPtr.Zero);

            var ret = (T) Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(), type1);

            gcHandle.Free();

            return status ? ret : default;
        }

        public static ulong ReadChain(ulong ptr, uint[] offsets)
        {
            var pointer = ptr;

            foreach (var offset in offsets)
            {
                var tempPtr = Read<ulong>(pointer + offset);

                pointer = tempPtr;
            }

            return pointer;
        }

        public static byte[] ReadBytes(ulong address, int size)
        {
            // Input
            var request = new ReadRequest(ProcessId, address, size);
            var requestSize = (uint) Marshal.SizeOf(request);

            // Output
            var buffer = new byte[size];

            var gcHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            request.ResultBuffer = gcHandle.AddrOfPinnedObject();

            var bytesReturned = 0;

            var status =
                Kernel32.DeviceIoControl(
                    hDriver,                // Driver handle
                    IOCTL_READ,             // Control code
                    request,                // Request struct
                    requestSize,            // Request size
                    null,                   // Output buffer
                    0,                      // Output size
                    ref bytesReturned,      // Returned bytes count
                    IntPtr.Zero);           //

            gcHandle.Free();

            return status ? buffer : null;
        }

        public static bool Write<T>(ulong address, T value)
        {
            var request = new WriteRequest()
            {
                ProcessId = ProcessId,
                Address = address,
                Size = Marshal.SizeOf(value),
                Buffer = Marshal.AllocHGlobal(Marshal.SizeOf(value))
            };

            var requestSize = (uint) Marshal.SizeOf(request);

            // Copy struct to unmanaged memory
            Marshal.StructureToPtr(value, request.Buffer, false);

            var bytesReturned = 0;

            var status =
                Kernel32.DeviceIoControl(
                    hDriver,            // Driver handle
                    IOCTL_WRITE,        // Control code
                    request,            // Request struct
                    requestSize,        // Request size
                    null,               // Output buffer
                    0,                  // Output size
                    ref bytesReturned,  // Returned bytes count
                    IntPtr.Zero);       //
            
            // Free unmanaged memory
            Marshal.FreeHGlobal(request.Buffer);

            return status;
        }

        public static ulong GetModuleAddress(string moduleName)
        {
            // Input buffer initialization
            var kernelRequest = new GetModuleBaseRequest(ProcessId, moduleName);
            var nInputbuffer = (uint) Marshal.SizeOf(kernelRequest);

            // Output buffer initialization
            var buffer = new byte[sizeof(ulong)];

            var gcHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            kernelRequest.ResultBuffer = gcHandle.AddrOfPinnedObject();

            var bytesReturned = 0;

            var status =
                Kernel32.DeviceIoControl(hDriver, IOCTL_GET_MODULE_BASE, kernelRequest, nInputbuffer, null, 0,
                    ref bytesReturned, IntPtr.Zero);

            gcHandle.Free();

            Marshal.FreeHGlobal(kernelRequest.ModuleName);

            return status ? BitConverter.ToUInt64(buffer, 0) : 0;
        }
    }
}