using Memory;

namespace DualSenseROTTR
{
    public static class Functions
    {
        public static bool IsValidMemory(
            string weaponTypePointer,
            string isHoldingWeaponPointer,
            string aimWeaponPointer,
            string aimWeapon2Pointer,
            string pauseStatesPointer,
            Mem mem)
        {
            try
            {
                int? value;
                try
                {
                    value = mem.ReadInt(weaponTypePointer);
                }
                catch
                {
                    value = 0;
                }
                int weapon_type = value <= -1 || value == null ? 0 : (int)value;
                int? value2;
                try
                {
                    value2 = mem.ReadInt(isHoldingWeaponPointer);
                }
                catch
                {
                    value2 = 0;
                }
                int isHoldingWeapon = value2 <= -1 || value2 == null ? 0 : (int)value2;
                int? value3;
                try
                {
                    value3 = mem.ReadInt(aimWeaponPointer);
                }
                catch
                {
                    value3 = 0;
                }
                int isAimingWeapon = value3 <= -1 || value3 == null ? 0 : (int)value3;
                int? value4;
                try
                {
                    value4 = mem.ReadInt(pauseStatesPointer);
                }
                catch
                {
                    value4 = 0;
                }
                int pauseStates = value4 <= -1 || value4 == null ? 0 : (int)value4;

                float? value8;
                try
                {
                    value8 = mem.ReadFloat(aimWeapon2Pointer);
                }
                catch
                {
                    value8 = 0;
                }
                float isAimingWeapon2 = value8 <= -1 || value8 == null ? 0 : (float)value8;



                return weapon_type != 0 || isHoldingWeapon != 0 || isAimingWeapon != 0 || isAimingWeapon2 != 0 || pauseStates != 0;
            }
            catch
            {
                // Likely invalid pointer format or unreadable memory
                return false;
            }
        }

        public static void CheckPlatform(string platform, string fileVersion, string productVersion)
        {
            if (platform == "Steam" && productVersion == "1.0" && fileVersion == "1.0.0.0")
            {
                Program.weaponTypePointer = "ROTTR.exe+02D8C758,2E8,140,398,8,390,8,2E0";
                Program.isHoldingWeaponPointer = "ROTTR.exe+02D9A3F8,2B0,658,0,2C0,8,5A0";
                Program.aimWeaponPointer = "ROTTR.exe+0161E0C8,978,958,960,148,20,88,28";
                Program.aimWeapon2Pointer = "ROTTR.exe+0161E0C8,978,148,48,50,18,960,110";
                Program.pauseStatesPointer = "ROTTR.exe+02E6C0A8,190,68,0,10,0,0,DF8";
            }
            else if (platform == "Epic Games Store" && productVersion == "1.0" && fileVersion == "1.0.1027.0")
            {
                Program.weaponTypePointer = "ROTTR.exe+02C196F0,3F0,3A0,48,3D0,3A0,8,2E0"; // find this
                Program.isHoldingWeaponPointer = "ROTTR.exe+010E1A40,158,280,18,8,2A8,8,5A0"; // find this
                Program.aimWeaponPointer = "ROTTR.exe+014AB068,960,148,38,30,20,88,28"; // find this
                Program.aimWeapon2Pointer = "ROTTR.exe+014AB068,138,978,958,148,50,840,110"; // find this
                Program.pauseStatesPointer = "ROTTR.exe+02CF8910,78,118,28,B8,1D0,0,CB8"; // find this
            }
            // else if (platform == "Steam" && productVersion == "1.0" && fileVersion == "1.0.1026.0")
            // {
            //     Program.weaponTypePointer = "ROTTR.exe+02D8C758,2E8,140,398,8,390,8,2E0"; // find this
                // Program.isHoldingWeaponPointer = "ROTTR.exe+02D9A3F8,2B0,658,0,2C0,8,5A0"; // find this
                // Program.aimWeaponPointer = "ROTTR.exe+0161E0C8,978,958,960,148,20,88,28"; // find this
                // Program.aimWeapon2Pointer = "ROTTR.exe+0161E0C8,978,148,48,50,18,960,110"; // find this
                // Program.pauseStatesPointer = "ROTTR.exe+02E6C0A8,190,68,0,10,0,0,DF8"; // find this

            // }
            // else if (platform == "GOG" && productVersion == "1.0" && fileVersion == "1.0.1.2")
            // {
            //     Program.weaponTypePointer = "ROTTR.exe+02D8C758,2E8,140,398,8,390,8,2E0"; // find this
                // Program.isHoldingWeaponPointer = "ROTTR.exe+02D9A3F8,2B0,658,0,2C0,8,5A0"; // find this
                // Program.aimWeaponPointer = "ROTTR.exe+0161E0C8,978,958,960,148,20,88,28"; // find this
                // Program.aimWeapon2Pointer = "ROTTR.exe+0161E0C8,978,148,48,50,18,960,110"; // find this
                // Program.pauseStatesPointer = "ROTTR.exe+02E6C0A8,190,68,0,10,0,0,DF8"; // find this

            // }
            else if (platform == "Microsoft Store")
            {
                Console.WriteLine("Rise of the Tomb Raider DualSense Mod initialization failed.");
                Console.WriteLine($"Platform: {platform}");
                Console.WriteLine("This platform of Tomb Raider is not supported by the mod.");
                Console.WriteLine($"This {platform} version of Tomb Raider (product version: {productVersion}, file version: {fileVersion}) could not be identified by the mod.");
                Console.WriteLine("This mod only works on Tomb Raider (base game) and Tomb Raider GOTY Edition from the following platforms:");
                Console.WriteLine("  - Steam");
                Console.WriteLine("  - Epic Games Store");
                // Console.WriteLine("  - GOG");
                Console.WriteLine("Supported versions:");
                Console.WriteLine("  Steam:");
                Console.WriteLine("    - product version: 1.0, file version: 1.0.0.0 (latest build)");
                // Console.WriteLine("    - product version: 1.1.748.0, file version: 1.1.748.0 (older build, same as GOG)");
                Console.WriteLine("  Epic Games Store:");
                Console.WriteLine("    - product version: 1.0, file version: 1.0.1027.0");
                // Console.WriteLine("  GOG:");
                // Console.WriteLine("    - product version: 1.1.748.0, file version: 1.1.748.0");
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
                // Console.WriteLine("    - product version: 1.1.748.0, file version: 1.1.748.0 (older build, same as GOG)");
                Console.WriteLine("  Epic Games Store:");
                Console.WriteLine("    - product version: 1.0, file version: 1.0.1027.0");
                // Console.WriteLine("  GOG:");
                // Console.WriteLine("    - product version: 1.1.748.0, file version: 1.1.748.0");
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
                // Console.WriteLine("    - product version: 1.1.748.0, file version: 1.1.748.0 (older build, same as GOG)");
                // Console.WriteLine("    - product version: 1.1.743.0, file version: 1.1.743.0 (even older build)");
                Console.WriteLine("  Epic Games Store:");
                Console.WriteLine("    - product version: 1.0, file version: 1.0.1027.0");
                // Console.WriteLine("  GOG:");
                // Console.WriteLine("    - product version: 1.1.748.0, file version: 1.1.748.0");
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