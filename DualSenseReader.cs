using System;
using SharpDX.DirectInput;
using System.Threading;

namespace DualSenseROTTR
{
    public class DualSenseState
    {
        public bool[] Buttons = new bool[20];
        public int Pov;
        public int LX, LY, RX, RY;
        public int L2, R2;
        public bool Connected;
        // Face buttons
        public bool Square => Buttons.Length > 0 && Buttons[0];
        public bool Cross => Buttons.Length > 1 && Buttons[1];
        public bool Circle => Buttons.Length > 2 && Buttons[2];
        public bool Triangle => Buttons.Length > 3 && Buttons[3];

        // Shoulders
        public bool L1 => Buttons.Length > 4 && Buttons[4];
        public bool R1 => Buttons.Length > 5 && Buttons[5];

        // Trigger buttons (digital click)
        public bool L2Button => Buttons.Length > 6 && Buttons[6];
        public bool R2Button => Buttons.Length > 7 && Buttons[7];

        // Share / Options
        public bool Share => Buttons.Length > 8 && Buttons[8];
        public bool Options => Buttons.Length > 9 && Buttons[9];

        // Stick clicks
        public bool L3 => Buttons.Length > 10 && Buttons[10];
        public bool R3 => Buttons.Length > 11 && Buttons[11];

        // PS + Touchpad
        public bool PS => Buttons.Length > 12 && Buttons[12];
        public bool Touchpad => Buttons.Length > 13 && Buttons[13];

        // Trigger analog press helpers
        public bool L2Pressed => L2 > 10280;
        public bool R2Pressed => R2 > 10280;

        // D-pad helpers
        public bool DPadUp => Pov == 0;
        public bool DPadRight => Pov == 9000;
        public bool DPadDown => Pov == 18000;
        public bool DPadLeft => Pov == 27000;

        public bool DPadUpRight => Pov == 4500;
        public bool DPadDownRight => Pov == 13500;
        public bool DPadDownLeft => Pov == 22500;
        public bool DPadUpLeft => Pov == 31500;
    }

    public class DualSenseReader : IDisposable
    {
        private readonly DirectInput directInput;
        private Joystick? joystick;
        private Guid joystickGuid = Guid.Empty;

        public DualSenseReader()
        {
            directInput = new DirectInput();
        }

        public bool TryConnect()
        {
            try
            {
                joystickGuid = Guid.Empty;

                foreach (var device in directInput.GetDevices(SharpDX.DirectInput.DeviceType.Gamepad, DeviceEnumerationFlags.AttachedOnly))
                {
                    string name = device.ProductName.ToLowerInvariant();
                    string productGuid = device.ProductGuid.ToString().ToLowerInvariant();

                    if (name.Contains("xbox") || !productGuid.Contains("05c4054c")) // this will only read DS4 controllers
                        continue;

                    // Console.WriteLine(device.ProductGuid);
                    // Console.WriteLine(device.InstanceName);
                    // Console.WriteLine(device.ProductName);

                    joystickGuid = device.InstanceGuid;
                    break;
                }

                if (joystickGuid == Guid.Empty)
                    return false;

                joystick = new Joystick(directInput, joystickGuid);
                joystick.Acquire();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public DualSenseState GetState()
        {
            var state = new DualSenseState();

            try
            {
                if (joystick == null)
                {
                    state.Connected = TryConnect();
                    return state;
                }

                joystick.Poll();
                var s = joystick.GetCurrentState();

                if (s == null)
                {
                    state.Connected = false;
                    joystick = null;
                    return state;
                }

                state.Connected = true;

                // Buttons
                var buttons = s.Buttons;
                for (int i = 0; i < buttons.Length && i < state.Buttons.Length; i++)
                    state.Buttons[i] = buttons[i];

                // Sticks
                state.LX = s.X;
                state.LY = s.Y;
                state.RX = s.Z;
                state.RY = s.RotationZ;

                // Triggers (DirectInput mapping varies by driver/emulation)
                state.L2 = s.RotationX;
                state.R2 = s.RotationY;

                // D-pad
                // state.Pov = s.PointOfViewControllers[0];
                state.Pov = s.PointOfViewControllers.Length > 0 ? s.PointOfViewControllers[0] : -1;

                // Console.WriteLine($"Cross: {state.Cross} | Square: {state.Square} | Triangle: {state.Triangle} | Circle: {state.Circle} | L1: {state.L1} | R1: {state.R1} | L3: {state.L3} | R3: {state.R3} | Options: {state.Options} | Touchpad: {state.Touchpad} | Share: {state.Share} | PS: {state.PS} | DPadUp: {state.DPadUp} | DPadDown: {state.DPadDown} | DPadLeft: {state.DPadLeft} | DPadRight: {state.DPadRight} | L2Pressed: {state.L2Pressed} | R2Pressed: {state.R2Pressed} | Pov: {state.Pov}");
                // Console.WriteLine($"L2: {state.L2}");
                // Console.WriteLine($"R2: {state.R2}");
                // Console.WriteLine($"LX: {state.LX} LY: {state.LY}");
            }
            catch
            {
                // if anything breaks → force reconnect next frame
                joystick = null;
                state.Connected = false;
            }

            return state;
        }

        public void Dispose()
        {
            joystick?.Dispose();
            directInput?.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}