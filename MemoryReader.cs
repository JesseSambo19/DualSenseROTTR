// using System;
using System.Diagnostics;
// using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DualSenseROTTR
{
    // this code works on .Net 7+ or later
    // I had to add this line in csproj under <PropertyGroup> in order for the code below to work: 
    // <AllowUnsafeBlocks>true</AllowUnsafeBlocks>
    public partial class MemoryReader
    {
        [LibraryImport("kernel32.dll", SetLastError = true)]
        internal static partial IntPtr OpenProcess(
            int dwDesiredAccess,
            [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle,
            int processId
        );

        [LibraryImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        // [SuppressMessage("Interoperability", "SYSLIB1092", Justification = "Explicit [Out] used on buffer.")]
        internal static partial bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            [Out] byte[] buffer,
            int size,
            out int bytesRead
        );

        private const int PROCESS_VM_READ = 0x0010;
        private const int PROCESS_QUERY_INFORMATION = 0x0400;

        private IntPtr processHandle;
        private Process? process;

        public bool OpenProcess(string processName)
        {
            process = Process.GetProcessesByName(processName).FirstOrDefault();
            if (process == null)
                return false;

            processHandle = OpenProcess(PROCESS_VM_READ | PROCESS_QUERY_INFORMATION, false, process.Id);
            return processHandle != IntPtr.Zero;
        }

        public IntPtr GetModuleBase(string moduleName)
        {
            foreach (ProcessModule module in process!.Modules)
            {
                if (module.ModuleName.Equals(moduleName, StringComparison.OrdinalIgnoreCase))
                {
                    return module.BaseAddress;
                }
            }
            return IntPtr.Zero;
        }

        public int ReadInt(Tuple<IntPtr, int[]> address)
        {
            IntPtr resolvedAddress = ResolvePointer(address.Item1, address.Item2);
            byte[] buffer = new byte[4];
            ReadProcessMemory(processHandle, resolvedAddress, buffer, buffer.Length, out _);
            return BitConverter.ToInt32(buffer, 0);
        }

        public uint ReadUInt(Tuple<IntPtr, int[]> address)
        {
            IntPtr resolvedAddress = ResolvePointer(address.Item1, address.Item2);
            byte[] buffer = new byte[4];
            ReadProcessMemory(processHandle, resolvedAddress, buffer, buffer.Length, out _);
            return BitConverter.ToUInt32(buffer, 0);
        }

        public float ReadFloat(Tuple<IntPtr, int[]> address)
        {
            IntPtr resolvedAddress = ResolvePointer(address.Item1, address.Item2);
            byte[] buffer = new byte[4];
            ReadProcessMemory(processHandle, resolvedAddress, buffer, buffer.Length, out _);
            return BitConverter.ToSingle(buffer, 0);
        }

        public double ReadDouble(Tuple<IntPtr, int[]> address)
        {
            IntPtr resolvedAddress = ResolvePointer(address.Item1, address.Item2);
            byte[] buffer = new byte[8];
            ReadProcessMemory(processHandle, resolvedAddress, buffer, buffer.Length, out _);
            return BitConverter.ToDouble(buffer, 0);
        }

        public string ReadString(Tuple<IntPtr, int[]> address, int maxLength = 32)
        {
            IntPtr resolvedAddress = ResolvePointer(address.Item1, address.Item2);
            byte[] buffer = new byte[maxLength];
            ReadProcessMemory(processHandle, resolvedAddress, buffer, buffer.Length, out _);
            int length = Array.IndexOf(buffer, (byte)0);
            if (length < 0) length = maxLength;
            return Encoding.UTF8.GetString(buffer, 0, length);
        }

        public IntPtr ResolvePointer(IntPtr baseAddress, params int[] offsets)
        {
            IntPtr address = baseAddress;
            byte[] buffer = new byte[8]; // 64-bit pointer

            foreach (int offset in offsets)
            {
                ReadProcessMemory(processHandle, address, buffer, buffer.Length, out _);
                long nextAddress = BitConverter.ToInt64(buffer, 0) + offset;

                address = checked((IntPtr)nextAddress);
            }

            return address;
        }

        public int SafeReadInt(Tuple<IntPtr, int[]> pointer, int fallback = 0)
        {
            try
            {
                int? value = ReadInt(pointer);
                return value <= -1 || value == null ? 0 : (int)value;
            }
            catch { return fallback; }
        }

        public uint SafeReadUInt(Tuple<IntPtr, int[]> pointer, uint fallback = 0)
        {
            try
            {
                uint? value = ReadUInt(pointer);
                return value <= -1 || value == null ? 0 : (uint)value;
            }
            catch { return fallback; }
        }

        public float SafeReadFloat(Tuple<IntPtr, int[]> pointer, float fallback = 0)
        {
            try
            {
                float? value = ReadFloat(pointer);
                return value <= -1 || value == null ? 0 : (float)value;
            }
            catch { return fallback; }
        }

        public string SafeReadString(Tuple<IntPtr, int[]> pointer)
        {
            try
            {
                string? value = ReadString(pointer);
                return value ?? "";
            }
            catch { return ""; }
        }
    }
}