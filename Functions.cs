// using Memory;

namespace DualSenseROTTR
{
    public static class Functions
    {
        public static bool IsValidMemory(
            IntPtr weaponTypePointer,
            IntPtr isHoldingWeaponPointer,
            IntPtr aimWeaponPointer,
            IntPtr aimWeapon2Pointer,
            IntPtr pauseStatesPointer,
            MemoryReader mem)
        {
            try
            {
                int weapon_type = mem.SafeReadInt(weaponTypePointer);
                int isHoldingWeapon = mem.SafeReadInt(isHoldingWeaponPointer);
                int isAimingWeapon = mem.SafeReadInt(aimWeaponPointer);
                int pauseStates = mem.SafeReadInt(pauseStatesPointer);
                float isAimingWeapon2 = mem.SafeReadFloat(aimWeapon2Pointer);

                Console.WriteLine($"Debug: weapon_type={weapon_type}, isHoldingWeapon={isHoldingWeapon}, isAimingWeapon={isAimingWeapon}, isAimingWeapon2={isAimingWeapon2}, pauseStates={pauseStates}");

                return weapon_type != 0 || isHoldingWeapon != 0 || isAimingWeapon != 0 || isAimingWeapon2 != 0 || pauseStates != 0;
            }
            catch
            {
                // Likely invalid pointer format or unreadable memory
                return false;
            }
        }

        public static void CheckPlatform(string platform, string fileVersion, string productVersion, MemoryReader mem)
        {
            if (platform == "Steam" && productVersion == "1.0" && fileVersion == "1.0.0.0")
            {
                // Program.weaponTypePointer = "ROTTR.exe+02D8C758,2E8,140,398,8,390,8,2E0";
                // Program.isHoldingWeaponPointer = "ROTTR.exe+02D9A3F8,2B0,658,0,2C0,8,5A0";
                // Program.aimWeaponPointer = "ROTTR.exe+0161E0C8,978,958,960,148,20,88,28";
                // Program.aimWeapon2Pointer = "ROTTR.exe+0161E0C8,978,148,48,50,18,960,110";
                // Program.pauseStatesPointer = "ROTTR.exe+02E6C0A8,190,68,0,10,0,0,DF8";

                Program.weaponTypePointer = mem.ResolvePointer(Program.baseAddress + 0x02D8C758, 0x2E8, 0x140, 0x398, 0x8, 0x390, 0x8, 0x2E0);
                Program.isHoldingWeaponPointer = mem.ResolvePointer(Program.baseAddress + 0x02D9A3F8, 0x2B0, 0x658, 0x0, 0x2C0, 0x8, 0x5A0);
                Program.aimWeaponPointer = mem.ResolvePointer(Program.baseAddress + 0x0161E0C8, 0x978, 0x958, 0x960, 0x148, 0x20, 0x88, 0x28);
                Program.aimWeapon2Pointer = mem.ResolvePointer(Program.baseAddress + 0x0161E0C8, 0x978, 0x148, 0x48, 0x50, 0x18, 0x960, 0x110);
                Program.pauseStatesPointer = mem.ResolvePointer(Program.baseAddress + 0x02E6C0A8, 0x190, 0x68, 0x0, 0x10, 0x0, 0x0, 0xDF8);
            }
            else if (platform == "Epic Games Store" && productVersion == "1.0" && fileVersion == "1.0.1027.0")
            {
                // Program.weaponTypePointer = "ROTTR.exe+010E1CD0,48,50,10,B0,2D0,80,2F8";
                // Program.isHoldingWeaponPointer = "ROTTR.exe+018DDCA8,60,50,10,B8,2A8,8,5A0";
                // Program.aimWeaponPointer = "ROTTR.exe+014AB068,960,148,38,30,20,88,28";
                // Program.aimWeapon2Pointer = "ROTTR.exe+014AB068,138,978,958,148,50,840,110";
                // Program.pauseStatesPointer = "ROTTR.exe+02CF8910,78,118,28,B8,1D0,0,CB8";

                Program.weaponTypePointer = mem.ResolvePointer(Program.baseAddress + 0x010E1CD0, 0x48, 0x50, 0x10, 0xB0, 0x2D0, 0x80, 0x2F8);
                Program.isHoldingWeaponPointer = mem.ResolvePointer(Program.baseAddress + 0x018DDCA8, 0x60, 0x50, 0x10, 0xB8, 0x2A8, 0x8, 0x5A0);
                Program.aimWeaponPointer = mem.ResolvePointer(Program.baseAddress + 0x014AB068, 0x960, 0x148, 0x38, 0x30, 0x20, 0x88, 0x28);
                Program.aimWeapon2Pointer = mem.ResolvePointer(Program.baseAddress + 0x014AB068, 0x138, 0x978, 0x958, 0x148, 0x50, 0x840, 0x110);
                Program.pauseStatesPointer = mem.ResolvePointer(Program.baseAddress + 0x02CF8910, 0x78, 0x118, 0x28, 0xB8, 0x1D0, 0x0, 0xCB8);
            }
            else if (platform == "Steam" && productVersion == "1.0" && fileVersion == "1.0.1026.0")
            {
                // Program.weaponTypePointer = "ROTTR.exe+02D583E8,10,388,300,48,870,B0,2E0";
                // Program.isHoldingWeaponPointer = "ROTTR.exe+01105378,60,870,B8,2A8,8,5A0";
                // Program.aimWeaponPointer = "ROTTR.exe+0161F0B0,978,148,38,20,68,88,28";
                // Program.aimWeapon2Pointer = "ROTTR.exe+0161F0B0,148,50,858,138,960,970,110";
                // Program.pauseStatesPointer = "ROTTR.exe+02E6D018,450,58,208,0,28,1E0,DF8";

                Program.weaponTypePointer = mem.ResolvePointer(Program.baseAddress + 0x02D583E8, 0x10, 0x388, 0x300, 0x48, 0x870, 0xB0, 0x2E0);
                Program.isHoldingWeaponPointer = mem.ResolvePointer(Program.baseAddress + 0x01105378, 0x60, 0x870, 0xB8, 0x2A8, 0x8, 0x5A0);
                Program.aimWeaponPointer = mem.ResolvePointer(Program.baseAddress + 0x0161F0B0, 0x978, 0x148, 0x38, 0x20, 0x68, 0x88, 0x28);
                Program.aimWeapon2Pointer = mem.ResolvePointer(Program.baseAddress + 0x0161F0B0, 0x148, 0x50, 0x858, 0x138, 0x960, 0x970, 0x110);
                Program.pauseStatesPointer = mem.ResolvePointer(Program.baseAddress + 0x02E6D018, 0x450, 0x58, 0x208, 0x0, 0x28, 0x1E0, 0xDF8);
            }
            else if (platform == "GOG" && productVersion == "1.0" && fileVersion == "1.0.1.0")
            {
                // Program.weaponTypePointer = "ROTTR.exe+02CD5EE8,60,50,10,B0,2D0,280,2F8";
                // Program.isHoldingWeaponPointer = "ROTTR.exe+02CE3BD0,298,0,8,2A8,8,5A0";
                // Program.aimWeaponPointer = "ROTTR.exe+015678D8,138,978,148,20,88,28";
                // Program.aimWeapon2Pointer = "ROTTR.exe+015678D8,148,50,858,138,958,960,110";
                // Program.pauseStatesPointer = "ROTTR.exe+02CD4B50,8,188,10,0,E38";

                Program.weaponTypePointer = mem.ResolvePointer(Program.baseAddress + 0x02CD5EE8, 0x60, 0x50, 0x10, 0xB0, 0x2D0, 0x280, 0x2F8);
                Program.isHoldingWeaponPointer = mem.ResolvePointer(Program.baseAddress + 0x02CE3BD0, 0x298, 0x0, 0x8, 0x2A8, 0x8, 0x5A0);
                Program.aimWeaponPointer = mem.ResolvePointer(Program.baseAddress + 0x015678D8, 0x138, 0x978, 0x148, 0x20, 0x88, 0x28);
                Program.aimWeapon2Pointer = mem.ResolvePointer(Program.baseAddress + 0x015678D8, 0x148, 0x50, 0x858, 0x138, 0x958, 0x960, 0x110);
                Program.pauseStatesPointer = mem.ResolvePointer(Program.baseAddress + 0x02CD4B50, 0x8, 0x188, 0x10, 0x0, 0xE38);
            }
            else if (platform == "Microsoft Store")
            {
                Console.WriteLine("Rise of the Tomb Raider DualSense Mod initialization failed.");
                Console.WriteLine($"Platform: {platform}");
                Console.WriteLine("This platform of Tomb Raider is not supported by the mod.");
                Console.WriteLine($"This {platform} version of Tomb Raider (product version: {productVersion}, file version: {fileVersion}) could not be identified by the mod.");
                Console.WriteLine("This mod only works on Tomb Raider (base game) and Tomb Raider GOTY Edition from the following platforms:");
                Console.WriteLine("  - Steam");
                Console.WriteLine("  - Epic Games Store");
                Console.WriteLine("  - GOG");
                Console.WriteLine("Supported versions:");
                Console.WriteLine("  Steam:");
                Console.WriteLine("    - product version: 1.0, file version: 1.0.0.0 (latest build)");
                Console.WriteLine("    - product version: 1.0, file version: 1.0.1026.0 (older build)");
                Console.WriteLine("  Epic Games Store:");
                Console.WriteLine("    - product version: 1.0, file version: 1.0.1027.0");
                Console.WriteLine("  GOG:");
                Console.WriteLine("    - product version: 1.0, file version: 1.0.1.0");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey(); // Waits for user input before exiting
                Environment.Exit(1); // Stop the mod or script from continuing
            }
            else if (platform == "Unknown")
            {
                Console.WriteLine("Rise of the Tomb Raider DualSense Mod initialization failed.");
                Console.WriteLine($"Platform: {platform}");
                Console.WriteLine("This platform of Tomb Raider could not be identified by the mod.");
                Console.WriteLine("This mod only works on Tomb Raider (base game) and Tomb Raider GOTY Edition from the following platforms:");
                Console.WriteLine("  - Steam");
                Console.WriteLine("  - Epic Games Store");
                Console.WriteLine("  - GOG");
                Console.WriteLine("Supported versions:");
                Console.WriteLine("  Steam:");
                Console.WriteLine("    - product version: 1.0, file version: 1.0.0.0 (latest build)");
                Console.WriteLine("    - product version: 1.0, file version: 1.0.1026.0 (older build)");
                Console.WriteLine("  Epic Games Store:");
                Console.WriteLine("    - product version: 1.0, file version: 1.0.1027.0");
                Console.WriteLine("  GOG:");
                Console.WriteLine("    - product version: 1.0, file version: 1.0.1.0");
                Console.WriteLine("If you think this is a mistake, please verify your game installation files or contact the mod author for assistance.");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey(); // Waits for user input before exiting
                Environment.Exit(1); // Stop the mod or script from continuing
            }
            else
            {
                Console.WriteLine("Rise of the Tomb Raider DualSense Mod initialization failed.");
                Console.WriteLine($"Platform: {platform}");
                Console.WriteLine($"This {platform} version of Tomb Raider (product version: {productVersion}, file version: {fileVersion}) is not supported by the mod.");
                Console.WriteLine("Supported versions:");
                Console.WriteLine("  Steam:");
                Console.WriteLine("    - product version: 1.0, file version: 1.0.0.0 (latest build)");
                Console.WriteLine("    - product version: 1.0, file version: 1.0.1026.0 (older build)");
                // Console.WriteLine("    - product version: 1.1.743.0, file version: 1.1.743.0 (even older build)");
                Console.WriteLine("  Epic Games Store:");
                Console.WriteLine("    - product version: 1.0, file version: 1.0.1027.0");
                Console.WriteLine("  GOG:");
                Console.WriteLine("    - product version: 1.0, file version: 1.0.1.0");
                Console.WriteLine("If you think this is a mistake, please check for updates or contact the mod author for assistance.");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey(); // Waits for user input before exiting
                Environment.Exit(1); // Stop the mod or script from continuing
            }
        }

        public static void WriteLog(string message)
        {
            try
            {
                string currentDir = AppDomain.CurrentDomain.BaseDirectory;
                string root = Directory.GetParent(currentDir)?.Parent?.FullName ?? currentDir;
                string logPath = Path.Combine(root, "DualSenseROTTR.log");

                if (!Directory.Exists(root))
                {
                    Console.WriteLine("Log directory not found.");
                    return;
                }

                File.AppendAllText(logPath, message + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write log: {ex.Message}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                Environment.Exit(1);
            }
        }
    }
}