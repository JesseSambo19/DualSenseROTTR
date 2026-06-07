using System.Runtime.InteropServices;

namespace DualSenseROTTR
{
    // XInput structures and functions for reading controller state
    [StructLayout(LayoutKind.Sequential)]
    public struct XINPUT_GAMEPAD
    {
        public ushort wButtons;
        public byte bLeftTrigger;
        public byte bRightTrigger;
        public short sThumbLX;
        public short sThumbLY;
        public short sThumbRX;
        public short sThumbRY;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct XINPUT_STATE
    {
        public uint dwPacketNumber;
        public XINPUT_GAMEPAD Gamepad;
    }

    public partial class XInputReader
    {
        [LibraryImport("xinput1_4.dll")]
        public static partial uint XInputGetState(uint dwUserIndex, ref XINPUT_STATE pState);

        public static uint SafeXInputGetState(uint controllerIndex, ref XINPUT_STATE controllerState)
        {
            try
            {
                return XInputGetState(controllerIndex, ref controllerState);
            }
            catch (Exception ex)
            {
                // Log the exception
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string errorMessage = $"[{timestamp}] XInput exception: {ex.Message}";
                // Functions.WriteLog(errorMessage);
                Console.WriteLine($"XInput exception: {ex.Message}");
                
                // Return a failure code (1167 = ERROR_DEVICE_NOT_CONNECTED)
                return 1167;
            }
        }

        // Button bitmasks (wButtons)
        public const ushort XINPUT_GAMEPAD_DPAD_UP        = 0x0001;
        public const ushort XINPUT_GAMEPAD_DPAD_DOWN      = 0x0002;
        public const ushort XINPUT_GAMEPAD_DPAD_LEFT      = 0x0004;
        public const ushort XINPUT_GAMEPAD_DPAD_RIGHT     = 0x0008;
        public const ushort XINPUT_GAMEPAD_START          = 0x0010;
        public const ushort XINPUT_GAMEPAD_BACK           = 0x0020;
        public const ushort XINPUT_GAMEPAD_LEFT_THUMB     = 0x0040;
        public const ushort XINPUT_GAMEPAD_RIGHT_THUMB    = 0x0080;
        public const ushort XINPUT_GAMEPAD_LEFT_SHOULDER  = 0x0100;
        public const ushort XINPUT_GAMEPAD_RIGHT_SHOULDER = 0x0200;
        public const ushort XINPUT_GAMEPAD_A              = 0x1000;
        public const ushort XINPUT_GAMEPAD_B              = 0x2000;
        public const ushort XINPUT_GAMEPAD_X              = 0x4000;
        public const ushort XINPUT_GAMEPAD_Y              = 0x8000;

        // helper functions
        public static bool IsButtonPressed(ref XINPUT_STATE state, ushort button)
        {
            return (state.Gamepad.wButtons & button) != 0;
        }

        public static bool IsLeftTriggerPressed(ref XINPUT_STATE state, byte threshold = 40)
        {
            return state.Gamepad.bLeftTrigger > threshold;
        }

        public static bool IsRightTriggerPressed(ref XINPUT_STATE state, byte threshold = 40)
        {
            return state.Gamepad.bRightTrigger > threshold;
        }
    }
}