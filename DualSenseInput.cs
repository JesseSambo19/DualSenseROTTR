// using System;
// using SharpDX.DirectInput;

// class DualSenseInput
// {
//     static void Main(string[] args)
//     {
//         // Initialize DirectInput
//         var directInput = new DirectInput();

//         // Find a connected DualSense controller
//         var joystickGuid = Guid.Empty;

//         foreach (var deviceInstance in directInput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AttachedOnly))
//         {
//             joystickGuid = deviceInstance.InstanceGuid;
//             break;
//         }

//         if (joystickGuid == Guid.Empty)
//         {
//             Console.WriteLine("No controller found. Please connect a DualSense controller.");
//             return;
//         }

//         // Instantiate the controller
//         var joystick = new Joystick(directInput, joystickGuid);
//         Console.WriteLine("Controller connected: " + joystick.Information.ProductName);

//         // Acquire the device
//         joystick.Acquire();

//         // Poll for input
//         Console.WriteLine("Press Ctrl+C to exit.\n");
//         while (true)
//         {
//             // Poll the controller
//             joystick.Poll();
//             var state = joystick.GetCurrentState();

//             // Get button states
//             var buttons = state.Buttons;

//             Console.Clear();
//             Console.WriteLine("Button States:");
//             Console.WriteLine($"Square: {buttons[0]}");   // Button 0 (Square)
//             Console.WriteLine($"Cross: {buttons[1]}");    // Button 1 (Cross)
//             Console.WriteLine($"Circle: {buttons[2]}");   // Button 2 (Circle)
//             Console.WriteLine($"Triangle: {buttons[3]}"); // Button 3 (Triangle)
//             Console.WriteLine($"L1: {buttons[4]}");       // Button 4 (L1)
//             Console.WriteLine($"R1: {buttons[5]}");       // Button 5 (R1)
//             Console.WriteLine($"L2: {buttons[6]}");       // Button 6 (L2)
//             Console.WriteLine($"R2: {buttons[7]}");       // Button 7 (R2)

//             // Get D-pad button states
//             Console.WriteLine("\nD-pad States:");
//             int pov = state.PointOfViewControllers[0]; // D-pad state
//             if (pov == -1)
//             {
//                 Console.WriteLine("D-pad: None"); // No direction pressed
//             }
//             else
//             {
//                 // Print the actual value to debug the input value
//                 Console.WriteLine($"POV Value: {pov}");

//                 string direction = "None";

//                 // Adjust these ranges to handle D-pad input
//                 if (pov >= 0 && pov < 4500) direction = "Up"; // 0 - 4500
//                 else if (pov >= 4500 && pov < 9000) direction = "Up-Right"; // 4500 - 9000
//                 else if (pov >= 9000 && pov < 13500) direction = "Right"; // 9000 - 13500
//                 else if (pov >= 13500 && pov < 18000) direction = "Down-Right"; // 13500 - 18000
//                 else if (pov >= 18000 && pov < 22500) direction = "Down"; // 18000 - 22500
//                 else if (pov >= 22500 && pov < 27000) direction = "Down-Left"; // 22500 - 27000
//                 else if (pov >= 27000 && pov < 31500) direction = "Left"; // 27000 - 31500
//                 else if (pov >= 31500 && pov < 36000) direction = "Up-Left"; // 31500 - 36000

//                 Console.WriteLine($"D-pad: {direction}");
//             }

//             // Get stick positions
//             Console.WriteLine("\nStick States:");
//             Console.WriteLine($"Left Stick X: {state.X}");
//             Console.WriteLine($"Left Stick Y: {state.Y}");
//             Console.WriteLine($"Right Stick X: {state.Z}");
//             Console.WriteLine($"Right Stick Y: {state.RotationZ}");

//             // Get trigger positions (if supported)
//             Console.WriteLine("\nTrigger States:");
//             Console.WriteLine($"Left Trigger: {state.RotationX}");
//             Console.WriteLine($"Right Trigger: {state.RotationY}");

//             // Throttle the loop
//             System.Threading.Thread.Sleep(100);
//         }
//     }
// }
