using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
// using Memory;
// using SharpDX.DirectInput;

namespace DualSenseROTTR
{
    internal class Program
    {
        static UdpClient? client;

        static IPEndPoint? endPoint;

        static DateTime TimeSent;

        static Process[] gameProcessName = Process.GetProcessesByName("ROTTR");

        static FileVersionInfo? versionInfo;

        static string? fileVersion;

        static string platform = "";

        public static IntPtr baseAddress;

        // public static string weaponTypePointer;
        // public static string isHoldingWeaponPointer;
        // public static string aimWeaponPointer;
        // public static string aimWeapon2Pointer;
        // public static string pauseStatesPointer;

        public static IntPtr weaponTypePointer;
        public static IntPtr isHoldingWeaponPointer;
        public static IntPtr aimWeaponPointer;
        public static IntPtr aimWeapon2Pointer;
        public static IntPtr pauseStatesPointer;


        static void Connect()
        {
            client = new UdpClient();
            var portNumber = File.ReadAllText(@"C:\Temp\DualSenseX\DualSenseX_PortNumber.txt");
            // var portNumber = DSXPortHelper.GetPortNumber();
            endPoint = new IPEndPoint(Triggers.localhost, Convert.ToInt32(portNumber));
            Console.WriteLine($"Port number found is: {portNumber}\n");
        }

        static void Send(Packet data)
        {
            var RequestData = Encoding.ASCII.GetBytes(Triggers.PacketToJson(data));
            client!.Send(RequestData, RequestData.Length, endPoint);
            TimeSent = DateTime.Now;
        }

        static void CheckGameProcess()
        {
            if (gameProcessName.Length == 0)
            {
                Console.WriteLine("ROTTR.exe not found. Waiting for the game to start...\n");
            }

            while (gameProcessName.Length == 0)
            {
                // Check if the game process is running
                gameProcessName = Process.GetProcessesByName("ROTTR");

                // If the game process is not found, wait for a while and check again
                if (gameProcessName.Length == 0)
                {
                    Thread.Sleep(1000); // Wait for 1 second before checking again
                }
            }

            Console.WriteLine("========================================");
            Console.WriteLine(" Rise of the Tomb Raider DualSense Mod v1.0.0 by Jexar");
            Console.WriteLine(" Enhancing DualSense support for Rise of the Tomb Raider");
            Console.WriteLine("========================================\n");

            string? exePath = gameProcessName[0].MainModule!.FileName;
            versionInfo = FileVersionInfo.GetVersionInfo(exePath);
            fileVersion = $"{versionInfo.FileMajorPart}.{versionInfo.FileMinorPart}.{versionInfo.FileBuildPart}.{versionInfo.FilePrivatePart}";

            Console.WriteLine($"Game Name: {gameProcessName[0].ProcessName}");
            Console.WriteLine($"Product Version: {versionInfo.ProductVersion}");
            // Console.WriteLine($"File Version: {versionInfo.FileVersion}\n");
            Console.WriteLine($"File Version: {fileVersion}\n");

            string exeDirectory = Path.GetDirectoryName(exePath)!;

            var gogInfoFile = Directory.GetFiles(exeDirectory, "goggame-*.info");
            var gogFiles = Directory.GetFiles(exeDirectory, "goggame-*.*");

            if (File.Exists(Path.Combine(exeDirectory, "steam_api64.dll")) && !File.Exists(Path.Combine(exeDirectory, "Galaxy64.dll")) || gogInfoFile.Length == 0 || gogFiles.Length == 0)
            {
                platform = "Steam";
            }
            else if (File.Exists(Path.Combine(exeDirectory, "EOSSDK-Win64-Shipping.dll"))
                     && !File.Exists(Path.Combine(exeDirectory, "steam_api64.dll"))
                     && !File.Exists(Path.Combine(exeDirectory, "Galaxy64.dll"))
                     && gogInfoFile.Length == 0
                     && gogFiles.Length == 0
                     && !File.Exists(Path.Combine(exeDirectory, "xgameruntime.dll"))
                     && !File.Exists(Path.Combine(exeDirectory, "MicrosoftGame.Config"))
                     && !File.Exists(Path.Combine(exeDirectory, "appxmanifest.xml")))
            {
                platform = "Epic Games Store";
            }
            else if (File.Exists(Path.Combine(exeDirectory, "Galaxy64.dll")) || gogInfoFile.Length > 0 || gogFiles.Length > 0)
            {
                platform = "GOG";
            }
            else if (File.Exists(Path.Combine(exeDirectory, "xgameruntime.dll"))
                     || File.Exists(Path.Combine(exeDirectory, "MicrosoftGame.Config"))
                     || File.Exists(Path.Combine(exeDirectory, "appxmanifest.xml")))
            {
                platform = "Microsoft Store";
            }
            else
            {
                platform = "Unknown";
            }

            Console.WriteLine($"Platform: {platform}\n");
        }

        // Represents the lifecycle of the L2 trigger when aiming.
        // We use an explicit state machine instead of booleans because
        // weapon equips can happen while the trigger is already held,
        // which causes edge-detection logic to desync over time.
        enum TriggerArmState
        {
            Idle,           // No valid weapon, or trigger logic inactive
            WaitingRelease, // Weapon equipped, waiting for trigger to be fully released
            Armed           // Next trigger pull should use the higher threshold
        }

        static TriggerArmState triggerState = TriggerArmState.Idle;

        static void ControllerLogic()
        {
            Connect();

            gameProcessName = Process.GetProcessesByName("ROTTR");

            Console.WriteLine("Monitoring game process...\n");

            MemoryReader mem = new();

            // Open the game process
            mem.OpenProcess("ROTTR");
            // Get the base address from game process
            baseAddress = mem.GetModuleBase("ROTTR.exe");

            Console.WriteLine("Rise of the Tomb Raider DualSense Mod Initializing...\n");

            Functions.CheckPlatform(platform, fileVersion!, versionInfo!.ProductVersion!, mem);

            Packet p = new()
            {
                // Set how many instructions you want to send at one time
                instructions = new Instruction[7]
            };

            int controllerIndex = 0;

            int previousWeaponTypeId = 0;

            while (!Functions.IsValidMemory(
                weaponTypePointer,
            isHoldingWeaponPointer,
            aimWeaponPointer,
            aimWeapon2Pointer,
            pauseStatesPointer,
                mem))
            {
                gameProcessName = Process.GetProcessesByName("ROTTR");

                if (gameProcessName.Length == 0)
                {
                    Console.WriteLine("ROTTR.exe not found. Exiting...\n");
                    Environment.Exit(1); // Stop the mod or script from continuing
                }

                p.instructions[4].type = InstructionType.RGBUpdate;
                p.instructions[4].parameters = [controllerIndex, 0, 0, 0]; // Off // Black

                p.instructions[2].type = InstructionType.TriggerUpdate;
                p.instructions[2].parameters = [controllerIndex, Trigger.Left, TriggerMode.Normal];

                p.instructions[3].type = InstructionType.TriggerUpdate;
                p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.Normal];

                Send(p);
                Console.WriteLine("Addresses are invalid or zero. Waiting...\n");
                Thread.Sleep(1000);
            }

            try
            {
                // Controller state for trigger reading
                XINPUT_STATE controllerState = new();

                while (gameProcessName.Length > 0)
                {
                    gameProcessName = Process.GetProcessesByName("ROTTR");

                    // Poll controller state from XInput (Windows Xbox controller API)
                    // Since DSX makes DualSense appear as Xbox controller, we can read trigger states
                    uint xinputResult = XInputReader.SafeXInputGetState((uint)controllerIndex, ref controllerState); // controllerIndex = 0 = first controller
                    // xinputResult: Windows error code (0 = success, 1167 = no controller, etc.)
                    
                    bool leftTriggerPressed = (xinputResult == 0) && (controllerState.Gamepad.bLeftTrigger > 40); // Higher threshold to avoid noise

                    // bool holdingLeftTrigger = xinputResult == 0 ? (xinputResult == 0) && (controllerState.Gamepad.bLeftTrigger > 140) : true;
                    bool holdingLeftTrigger = xinputResult != 0 || leftTriggerPressed;

                    Functions.CheckPlatform(platform, fileVersion!, versionInfo!.ProductVersion!, mem);

                    int weapon_type = mem.SafeReadInt(weaponTypePointer);
                    int isHoldingWeapon = mem.SafeReadInt(isHoldingWeaponPointer);
                    int isAimingWeapon = mem.SafeReadInt(aimWeaponPointer);
                    int pauseStates = mem.SafeReadInt(pauseStatesPointer);
                    float isAimingWeapon2 = mem.SafeReadFloat(aimWeapon2Pointer);

                    Console.WriteLine($"Weapon Type: {weapon_type} | Is Holding Weapon: {isHoldingWeapon} | Is Aiming Weapon: {isAimingWeapon} | Is Aiming Weapon 2: {isAimingWeapon2} | Pause States: {pauseStates}");
                    if (
                        // false
                        // default_weapon_type == 0 || isAimingWeapon == 1 || 
                        weapon_type == 0 || (weapon_type != 1 && weapon_type != 2 && weapon_type != 3 && weapon_type != 4) ||
                        // pauseStates == 0 || 
                        pauseStates == 10 || pauseStates == 13 // 12 - play, 10 - pause/start menu, 13 - loading screen
                        ) // isAimingWeapon == 1 for cutscenes, pauseStates == 0 for pause and map menus
                    {

                        p.instructions[4].type = InstructionType.RGBUpdate;
                        // p.instructions[4].parameters = [controllerIndex, 50, 150, 250]; // original
                        p.instructions[4].parameters = [controllerIndex, 50, 75, 250]; // original

                        if (
                        // weapon_type == "" &&
                        // default_weapon_type == 0
                        weapon_type == 0 || pauseStates == 13
                        ) // Off
                        {
                            // p.instructions[4].type = InstructionType.RGBUpdate;
                            // p.instructions[4].parameters = [controllerIndex, 0, 0, 0];

                            previousWeaponTypeId = 0;
                        }

                        // // Adaptive triggers for the escape from collapsing cave QTE at the start of the game on L2/LT and R2/RT
                        // if (default_weapon_type == 0 && !isHoldingTorch && !isHoldingTorchAndNoWeapons && dualPistols >= 590 && dualPistols <= 595 && dualPistols != 594 && (cave == 5 || secondCave == 5) && quickTimeEvent == 1 && location == 112 && pauseStates == 1 && menuAndLoadingScreens == 1 && isAimingWeapon != 1)
                        // // if (default_weapon_type == 0 && !isHoldingTorch && !isHoldingTorchAndNoWeapons && (dualPistols == 590 || dualPistols == 595) && dualPistols != 594 && (cave == 5 || secondCave == 5) && quickTimeEvent == 1 && location == 112 && pauseStates == 1 && menuAndLoadingScreens == 1 && isAimingWeapon != 1) // decide if to use this later
                        // {
                        //     // Reset left trigger threshold to 0
                        //     p.instructions[0].type = InstructionType.TriggerThreshold;
                        //     p.instructions[0].parameters = [controllerIndex, Trigger.Left, 0];

                        //     // Reset right trigger threshold to 0
                        //     p.instructions[1].type = InstructionType.TriggerThreshold;
                        //     p.instructions[1].parameters = [controllerIndex, Trigger.Right, 0];

                        //     // Reset right trigger to off
                        //     p.instructions[2].type = InstructionType.TriggerUpdate;
                        //     // p.instructions[2].parameters = [controllerIndex, Trigger.Left, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.Rigid, 0, 50, 0, 0, 0, 0];
                        //     // p.instructions[2].parameters = new object[] {controllerIndex, Trigger.Left, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.Rigid, 0, 0, 0, 0, 0, 0, 0};
                        //     p.instructions[2].parameters = new object[] {controllerIndex, Trigger.Left, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.Rigid, 0, 10, 0, 0, 0, 0, 0};
                        //     // p.instructions[2].parameters = new object[] {controllerIndex, Trigger.Left, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.Rigid, 0, 20, 0, 0, 0, 0, 0};

                        //     // Reset right trigger to off
                        //     p.instructions[3].type = InstructionType.TriggerUpdate;
                        //     // p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.Rigid, 0, 50, 0, 0, 0, 0];
                        //     // p.instructions[3].parameters = new object[] {controllerIndex, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.Rigid, 0, 0, 0, 0, 0, 0, 0};
                        //     p.instructions[3].parameters = new object[] {controllerIndex, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.Rigid, 0, 10, 0, 0, 0, 0, 0};
                        //     // p.instructions[3].parameters = new object[] {controllerIndex, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.Rigid, 0, 20, 0, 0, 0, 0, 0};
                        // }
                        // else
                        // {
                        // Reset left trigger threshold to 0
                        p.instructions[0].type = InstructionType.TriggerThreshold;
                        p.instructions[0].parameters = [controllerIndex, Trigger.Left, 0];

                        // Reset right trigger threshold to 0
                        p.instructions[1].type = InstructionType.TriggerThreshold;
                        p.instructions[1].parameters = [controllerIndex, Trigger.Right, 0];

                        // Reset right trigger to off
                        p.instructions[2].type = InstructionType.TriggerUpdate;
                        p.instructions[2].parameters = [controllerIndex, Trigger.Left, TriggerMode.Normal];

                        // Reset right trigger to off
                        p.instructions[3].type = InstructionType.TriggerUpdate;
                        p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.Normal];
                        // }

                    }
                    else
                    {
                        p.instructions[4].type = InstructionType.RGBUpdate;
                        // p.instructions[4].parameters = [controllerIndex, 50, 150, 250]; // original
                        p.instructions[4].parameters = [controllerIndex, 50, 75, 250]; // original
                        // // Determine if Lara currently has a valid weapon equipped.
                        // // Certain weapon IDs are excluded, and an action state must be active.
                        // bool hasWeaponEquipped = (isHoldingWeapon2 > 0 && isHoldingWeapon2 != 69 && isHoldingWeapon2 != 704 && action > 0) || (secondIsHoldingWeapon2 > 0 && secondIsHoldingWeapon2 != 69 && secondIsHoldingWeapon2 != 704 && secondAction > 0);

                        // // bool hasDualPistolsEquipped = default_weapon_type == 31985484 && isAimingWeapon == 16 && (edge == 177 || dualPistols == 588) && ((isHoldingWeapon2 > 0 && isHoldingWeapon2 != 69 && isHoldingWeapon2 != 704 && action == 0) || (secondIsHoldingWeapon2 > 0 && secondIsHoldingWeapon2 != 69 && secondIsHoldingWeapon2 != 704 && secondAction == 0)); // original
                        // bool hasDualPistolsEquipped = default_weapon_type == 31985484 && isAimingWeapon == 16 && ((edge == 177 && traversal == 512) || dualPistols == 588) && location == 33 && ((isHoldingWeapon2 > 0 && isHoldingWeapon2 != 69 && isHoldingWeapon2 != 704 && (action == 0 || action > 0 && isHoldingFireStriker1 > 0)) || (secondIsHoldingWeapon2 > 0 && secondIsHoldingWeapon2 != 69 && secondIsHoldingWeapon2 != 704 && (secondAction == 0 || secondAction > 0 && secondIsHoldingFireStriker1 > 0)));

                        if (xinputResult == 0)
                        // if (xinputResult == 0 || joystickGuid != Guid.Empty)
                        {
                            // Check if weapon changed this frame
                            if (weapon_type != previousWeaponTypeId && previousWeaponTypeId != 0)
                            {
                                triggerState = TriggerArmState.Idle; // reset state on weapon switch
                            }
                            previousWeaponTypeId = weapon_type; // update for next frame

                            // Update trigger arming state.
                            // This logic ensures the trigger threshold is only armed after
                            // a clean release, preventing mid-pull threshold changes and
                            // long-session desynchronization.
                            if (isHoldingWeapon != 65537)
                            {
                                // No weapon: reset to a safe baseline.
                                triggerState = TriggerArmState.Idle;
                            }
                            else
                            {
                                switch (triggerState)
                                {
                                    case TriggerArmState.Idle:
                                        // Weapon just became available.
                                        // Wait for the trigger to be fully released before arming.
                                        if (!leftTriggerPressed)
                                            triggerState = TriggerArmState.WaitingRelease;
                                        break;

                                    case TriggerArmState.WaitingRelease:
                                        // Trigger is idle with a weapon equipped.
                                        // This arms the higher threshold for the next pull.
                                        if (!leftTriggerPressed)
                                            triggerState = TriggerArmState.Armed;
                                        break;

                                    case TriggerArmState.Armed:
                                        // Once the player starts pulling the trigger again,
                                        // consume the armed state and wait for the next release.
                                        if (leftTriggerPressed)
                                            triggerState = TriggerArmState.WaitingRelease;
                                        break;
                                }
                            }

                            // Apply trigger threshold.
                            // Armed = resistance bump at the aim threshold.
                            // Otherwise, keep the trigger fully responsive.
                            int threshold = (triggerState == TriggerArmState.Armed) ? 140 : 0;

                            p.instructions[0].type = InstructionType.TriggerThreshold;
                            p.instructions[0].parameters = [controllerIndex, Trigger.Left, threshold];
                        }
                        else
                        {
                            if (isHoldingWeapon == 65537)
                            {
                                p.instructions[0].type = InstructionType.TriggerThreshold;
                                p.instructions[0].parameters = [controllerIndex, Trigger.Left, 140];
                            }
                            else
                            {
                                p.instructions[0].type = InstructionType.TriggerThreshold;
                                p.instructions[0].parameters = [controllerIndex, Trigger.Left, 0];
                            }
                        }

                        if (
                            // weapon_type.Contains("arrow") ||
                            // weapon_type == 3743
                            // weapon_type == 35
                            weapon_type == 1
                         //  || weapon_type == 5273748904 long
                         )
                        {
                           
                            // 256 is for aiming a weapon
                            // if ((isAimingWeapon == 16 || isAimingWeapon > 0 || isAimingWeapon2 == 0.25) && isHoldingWeapon == 65537)
                            if (isAimingWeapon == 16 || isAimingWeapon > 0 || isAimingWeapon2 == 0.25)
                            {
                                p.instructions[1].type = InstructionType.TriggerThreshold;
                                p.instructions[1].parameters = [controllerIndex, Trigger.Right, 160];
                            }
                            else
                            {
                                p.instructions[1].type = InstructionType.TriggerThreshold;
                                p.instructions[1].parameters = [controllerIndex, Trigger.Right, 0];
                            }

                            
                            // Reset left trigger to off
                            p.instructions[2].type = InstructionType.TriggerUpdate;
                            p.instructions[2].parameters = [controllerIndex, Trigger.Left, TriggerMode.Normal];


                            // if ((isAimingWeapon == 16 || isAimingWeapon > 0 || isAimingWeapon2 == 0.25) && isHoldingWeapon == 65537)
                            if (isAimingWeapon == 16 || isAimingWeapon > 0 || isAimingWeapon2 == 0.25)
                            {
                                // Bow effect
                                p.instructions[3].type = InstructionType.TriggerUpdate;
                                // p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.PulseA, 155, 130, 0, 0, 0, 0, 0]; // default
                                p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.PulseA, 155, 130, 10, 0, 0, 0, 0]; // strongest // was 30
                                                                                                                                                                                          // p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.Rigid, 0, 100, 0, 0, 0, 0, 0]; // was 50 // 40 // 30

                            }
                            else
                            {
                                // Reset right trigger to off
                                p.instructions[3].type = InstructionType.TriggerUpdate;
                                p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.Normal];
                            }

                        }

                        else if (
                        // weapon_type.Contains("handgun") ||
                        // weapon_type == 185
                        // weapon_type == 46
                        weapon_type == 2
                        // || weapon_type == 4326952780 long
                        )
                        {

                            // 256 is for aiming a weapon
                            // if ((isAimingWeapon == 16 || isAimingWeapon > 0 || isAimingWeapon2 == 0.25) && isHoldingWeapon == 65537)
                            if (isAimingWeapon == 16 || isAimingWeapon > 0 || isAimingWeapon2 == 0.25)
                            {
                                p.instructions[1].type = InstructionType.TriggerThreshold;
                                p.instructions[1].parameters = [controllerIndex, Trigger.Right, 200];
                            }
                            else
                            {
                                p.instructions[1].type = InstructionType.TriggerThreshold;
                                p.instructions[1].parameters = [controllerIndex, Trigger.Right, 0];
                            }

                            if (isHoldingWeapon == 65537)
                            {
                                // wasHandgunShot = false;

                                // Single pistol idle
                                // Aiming with one pistol
                                p.instructions[2].type = InstructionType.TriggerUpdate;
                                // p.instructions[2].parameters = [controllerIndex, Trigger.Left, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.PulseA, 7, 0, 0, 0, 0, 0, 0];
                                // p.instructions[2].parameters = [controllerIndex, Trigger.Left, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.PulseA, 7, 0, 20, 0, 0, 0, 0]; // strongest
                                p.instructions[2].parameters = [controllerIndex, Trigger.Left, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.PulseA, 7, 0, 10, 0, 0, 0, 0]; // strongest
                            }
                            else
                            {
                                // Reset left trigger to off
                                p.instructions[2].type = InstructionType.TriggerUpdate;
                                p.instructions[2].parameters = [controllerIndex, Trigger.Left, TriggerMode.Normal];

                                // wasShotgunShot = false;
                            }

                            // if ((isAimingWeapon == 16 || isAimingWeapon > 0 || isAimingWeapon2 == 0.25) && isHoldingWeapon == 65537)
                            if (isAimingWeapon == 16 || isAimingWeapon > 0 || isAimingWeapon2 == 0.25)
                            {
                                // Hand gun or Pistol:
                                p.instructions[3].type = InstructionType.TriggerUpdate;
                                // p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.VerySoft];
                                // p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.Pulse, 100, 148, 32, 0, 0, 0, 0]; // default
                                // p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.Pulse, 125, 148, 160, 0, 0, 0, 0]; // moderate
                                // p.instructions[3].parameters = new object[] { controllerIndex, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.RigidAB, 27, 40, 86, 102, 184, 172, 2 }; // prefered

                                // alternative with a gradual resistance
                                // p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.Pulse, 105, 158, 40, 0, 0, 0, 0]; // moderate
                                // p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.Pulse, 98, 158, 40, 0, 0, 0, 0]; // moderate
                                // p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.Pulse, 101, 158, 40, 0, 0, 0, 0]; // moderate // or 30
                                p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.Pulse, 101, 158, 33, 0, 0, 0, 0]; // moderate
                            }
                            else
                            {
                                // Reset right trigger to off
                                p.instructions[3].type = InstructionType.TriggerUpdate;
                                p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.Normal];
                            }
                        }

                        else if (
                            // weapon_type.Contains("machinegun") ||
                            // weapon_type == 3742
                            // weapon_type == 55
                            weapon_type == 3
                        // || weapon_type == 7526949996 // long
                        )
                        {
                            // 256 is for aiming a weapon
                            // if ((isAimingWeapon == 16 || isAimingWeapon > 0 || isAimingWeapon2 == 0.25) && isHoldingWeapon == 65537)
                            if (isAimingWeapon == 16 || isAimingWeapon > 0 || isAimingWeapon2 == 0.25)
                            {
                                p.instructions[1].type = InstructionType.TriggerThreshold;
                                p.instructions[1].parameters = [controllerIndex, Trigger.Right, 200];
                            }
                            else
                            {
                                p.instructions[1].type = InstructionType.TriggerThreshold;
                                p.instructions[1].parameters = [controllerIndex, Trigger.Right, 0];
                            }

                            if (isHoldingWeapon == 65537)
                            {
                                // wasHandgunShot = false;

                                // Single pistol idle
                                // Aiming with one pistol
                                p.instructions[2].type = InstructionType.TriggerUpdate;
                                // p.instructions[2].parameters = [controllerIndex, Trigger.Left, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.PulseA, 25, 0, 0, 0, 0, 0, 0];
                                // p.instructions[2].parameters = [controllerIndex, Trigger.Left, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.PulseA, 25, 0, 30, 0, 0, 0, 0]; // stronger
                                p.instructions[2].parameters = [controllerIndex, Trigger.Left, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.PulseA, 25, 0, 20, 0, 0, 0, 0];
                            }
                            else
                            {
                                // Reset left trigger to off
                                p.instructions[2].type = InstructionType.TriggerUpdate;
                                p.instructions[2].parameters = [controllerIndex, Trigger.Left, TriggerMode.Normal];

                                // wasShotgunShot = false;
                            }

                            // previousShootWeaponState = currentShootWeaponState;

                            if (isAimingWeapon == 16 && isHoldingWeapon == 65537
                                // && previousWeaponType.Contains("machinegun") && weapon_type.Contains("machinegun") && !previousWeaponType.Contains("grenade")
                                // && !weapon_type.Contains("grenade")

                                )
                            {
                                // if (machinegunAmmo > 0)
                                // {
                                //     p.instructions[3].type = InstructionType.TriggerUpdate;
                                //     // p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.VIBRATION, 0, 9, 7, 7, 10, 0]; // moderate
                                //     // p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.VIBRATION, 3, 8, 9]; // moderate
                                //     // p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.PulseB, 9, 55, 110]; // moderate
                                //     p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.VIBRATION, 4, 8, 11]; // Submachine gun - rapid fire stutter
                                // }
                                // else
                                // {


                                p.instructions[3].type = InstructionType.TriggerUpdate;
                                // p.instructions[3].parameters = new object[] { controllerIndex, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.RigidAB, 60, 40, 86, 102, 184, 172, 2 }; // machine gun
                                // p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.Pulse, 73, 135, 32, 0, 0, 0, 0]; // default
                                // p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.Pulse, 93, 135, 55, 0, 0, 0, 0]; // moderate
                                // p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.Pulse, 93, 135, 160, 0, 0, 0, 0]; // stronger

                                // alternative with a gradual resistance
                                // p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.Pulse, 73, 145, 50, 0, 0, 0, 0]; // moderate
                                // p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.Pulse, 70, 145, 50, 0, 0, 0, 0]; // moderate // preferred
                                // p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.Pulse, 73, 140, 40, 0, 0, 0, 0];
                                p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.Pulse, 73, 140, 38, 0, 0, 0, 0];
                                // }
                            }
                            else
                            {
                                // Reset right trigger to off
                                p.instructions[3].type = InstructionType.TriggerUpdate;
                                p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.Normal];
                            }
                        }

                        else if (
                        // weapon_type.Contains("shotgun") ||
                        // weapon_type == 1596
                        // weapon_type == 61
                        weapon_type == 4
                         //  || weapon_type == 7146889127 long
                         )
                        {
                            // 256 is for aiming a weapon
                            // if ((isAimingWeapon == 16 || isAimingWeapon > 0 || isAimingWeapon2 == 0.25) && isHoldingWeapon == 65537)
                            if (isAimingWeapon == 16 || isAimingWeapon > 0 || isAimingWeapon2 == 0.25)
                            {
                                p.instructions[1].type = InstructionType.TriggerThreshold;
                                p.instructions[1].parameters = [controllerIndex, Trigger.Right, 200];
                            }
                            else
                            {
                                p.instructions[1].type = InstructionType.TriggerThreshold;
                                p.instructions[1].parameters = [controllerIndex, Trigger.Right, 0];
                            }

                            if (isHoldingWeapon == 65537)
                            {
                                // wasHandgunShot = false;

                                // Single pistol idle
                                // Aiming with one pistol
                                p.instructions[2].type = InstructionType.TriggerUpdate;
                                // p.instructions[2].parameters = [controllerIndex, Trigger.Left, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.PulseA, 63, 0, 0, 0, 0, 0, 0];
                                // p.instructions[2].parameters = [controllerIndex, Trigger.Left, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.PulseA, 63, 0, 30, 0, 0, 0, 0]; // stronger
                                p.instructions[2].parameters = [controllerIndex, Trigger.Left, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.PulseA, 63, 0, 20, 0, 0, 0, 0];
                            }
                            else
                            {
                                // Reset left trigger to off
                                p.instructions[2].type = InstructionType.TriggerUpdate;
                                p.instructions[2].parameters = [controllerIndex, Trigger.Left, TriggerMode.Normal];

                                // wasShotgunShot = false;
                            }

                            // previousShootWeaponState = currentShootWeaponState;

                            // Console.WriteLine($"currentShootWeaponState: {currentShootWeaponState}, previousShootWeaponState: {previousShootWeaponState}, shootWeaponState: {shootWeaponState}");

                            // if ((isAimingWeapon == 16 || isAimingWeapon > 0 || isAimingWeapon2 == 0.25) && isHoldingWeapon == 65537)
                            if (isAimingWeapon == 16 || isAimingWeapon > 0 || isAimingWeapon2 == 0.25)
                            {
                                // Shotgun or any heavy gun:
                                p.instructions[3].type = InstructionType.TriggerUpdate;
                                // p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.RigidAB, 95, 36, 38, 133, 186, 217, 129]; // preferred
                                // p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.Pulse, 45, 122, 32, 0, 0, 0, 0];
                                // p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.Pulse, 65, 122, 50, 0, 0, 0, 0]; // default moderate
                                // p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.Pulse, 65, 122, 160, 0, 0, 0, 0]; // stronger
                                // p.instructions[3].parameters = new object[] { controllerIndex, Trigger.Right, TriggerMode.Soft };

                                // alternative with a gradual resistance
                                // p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.Pulse, 45, 132, 50, 0, 0, 0, 0];
                                // p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.Pulse, 52, 132, 50, 0, 0, 0, 0];
                                // p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.Pulse, 52, 139, 50, 0, 0, 0, 0];
                                // p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.Pulse, 52, 136, 50, 0, 0, 0, 0]; // preferred
                                p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.CustomTriggerValue, CustomTriggerValueMode.Pulse, 53, 136, 40, 0, 0, 0, 0];
                            }
                            else
                            {
                                // Reset right trigger to off
                                p.instructions[3].type = InstructionType.TriggerUpdate;
                                p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.Normal];
                            }
                        }
                        // else
                        // {
                        //     p.instructions[1].type = InstructionType.TriggerThreshold;
                        //     p.instructions[1].parameters = [controllerIndex, Trigger.Right, 0];

                        //     // Reset left trigger to off
                        //     p.instructions[2].type = InstructionType.TriggerUpdate;
                        //     p.instructions[2].parameters = [controllerIndex, Trigger.Left, TriggerMode.Normal];
                        //     // Reset right trigger to off
                        //     p.instructions[3].type = InstructionType.TriggerUpdate;
                        //     p.instructions[3].parameters = [controllerIndex, Trigger.Right, TriggerMode.Normal];
                        // }
                    }

                    // Player number
                    p.instructions[5].type = InstructionType.PlayerLED;
                    p.instructions[5].parameters = [controllerIndex, false, false, true, false, false];

                    // Player LED for new revision controllers
                    p.instructions[6].type = InstructionType.PlayerLEDNewRevision;
                    p.instructions[6].parameters = [controllerIndex, PlayerLEDNewRevision.One];

                    // Send UDP commands to DSX
                    Console.WriteLine("Instructions Sent\n");
                    Send(p);

                    // Wait 100ms before sending the next instruction
                    Thread.Sleep(100);

                    Console.WriteLine("Waiting for Server Response...\n");

                    // Make sure you setup some timeout for server response incase DSX has a bug or not running
                    Process[] process1 = Process.GetProcessesByName("DSX");
                    Process[] process2 = Process.GetProcessesByName("DualSenseY");

                    // Checks if either DSX or DualSenseY is running
                    if (process1.Length == 0 && process2.Length == 0)
                    {
                        Console.WriteLine("DSX is not running... \n");
                    }
                    else
                    {
                        try
                        {
                            byte[] bytesReceivedFromServer = client!.Receive(ref endPoint);

                            if (bytesReceivedFromServer.Length > 0)
                            {
                                Console.WriteLine("Rise of the Tomb Raider DualSense Mod Initialized\n");
                                ServerResponse ServerResponseJson = JsonConvert.DeserializeObject<ServerResponse>($"{Encoding.ASCII.GetString(bytesReceivedFromServer, 0, bytesReceivedFromServer.Length)}")!;
                                Console.WriteLine("===================================================================");
                                Console.WriteLine($"Status: {ServerResponseJson!.Status}");
                                DateTime CurrentTime = DateTime.Now;
                                TimeSpan Timespan = CurrentTime - TimeSent;
                                Console.WriteLine($"Time Received: {ServerResponseJson.TimeReceived}, took: {Timespan.TotalMilliseconds} to receive response from DSX");
                                Console.WriteLine($"isControllerConnected: {ServerResponseJson.isControllerConnected}");
                                Console.WriteLine($"BatteryLevel: {ServerResponseJson.BatteryLevel}");
                                Console.WriteLine("===================================================================\n");
                            }
                        }
                        catch (SocketException ex) when (ex.SocketErrorCode == SocketError.ConnectionReset)
                        {
                            Console.WriteLine("Connection reset by DSX (10054). Retrying...");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Unexpected error communicating with DSX: {ex.Message}");
                            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            string errorMessage = $"[{timestamp}] Unexpected error communicating with DSX: {ex.Message}\n{ex.StackTrace}";
                            // Functions.WriteLog(errorMessage);
                        }
                    }
                }
                Console.WriteLine("ROTTR.exe not found. Exiting...\n");
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n==================================================");
                Console.WriteLine("A fatal error occurred:");
                Console.WriteLine(ex.ToString());
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string errorMessage = $"[{timestamp}] Unexpected error: [Loop Crash] {ex.Message}\n{ex.StackTrace}";
                // Functions.WriteLog(errorMessage);
                Console.WriteLine("==================================================");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                Environment.Exit(1);
            }
        }

        static void Main(string[] args)
        {
            try
            {
                CheckGameProcess();
                ControllerLogic();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n==================================================");
                Console.WriteLine("A fatal error occurred:");
                Console.WriteLine(ex.ToString());
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string errorMessage = $"[{timestamp}] Unexpected error: {ex.Message}\n{ex.StackTrace}";
                // Functions.WriteLog(errorMessage);

                if (ex is Win32Exception win32Ex && win32Ex.NativeErrorCode == 5)
                {
                    // Access denied specific handling
                    Console.WriteLine("\nAccess denied while trying to communicate with the game. The game may be running as Administrator.");
                    Console.WriteLine("Fix: Run this mod as Administrator and try again.");
                }

                Console.WriteLine("==================================================");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                Environment.Exit(1);
            }
        }
    }
}