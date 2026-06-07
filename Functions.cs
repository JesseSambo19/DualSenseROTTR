// namespace DualSenseROTTR
// {
//     public static class Functions
//     {
//         public static bool IsValidMemory(
//             IntPtr aimWeaponPointer,
//             IntPtr mainAmmoPointer,
//             IntPtr backupAmmoPointer,
//             IntPtr totalAmmoCapacityPointer,
//             IntPtr grenadeAmmoPointer,
//             IntPtr arrowRopeAmmoPointer,
//             IntPtr pauseStatesPointer,
//             IntPtr menuAndLoadingScreensPointer,
//             IntPtr shootWeaponCounterPointer,
//             IntPtr shootArrowCounterPointer,
//             IntPtr shootGrenadeCounterPointer,
//             IntPtr shootHandgunCounterPointer,
//             IntPtr shootShotgunCounterPointer,
//             IntPtr shootWeaponStatePointer,
//             IntPtr edgePointer,
//             IntPtr dualPistolsPointer,
//             IntPtr traversalPointer,
//             IntPtr isHoldingWeapon2Pointer,
//             IntPtr defaultWeaponPointer,
//             IntPtr cavePointer,
//             IntPtr quickTimeEventPointer,
//             IntPtr locationPointer,
//             IntPtr secondCavePointer,
//             IntPtr isHoldingFireStrikerPointer1,
//             IntPtr isHoldingFireStrikerPointer3,
//             IntPtr isHoldingFireStrikerPointer4,
//             IntPtr isHoldingFireStrikerPointer5,
//             IntPtr actionPointer,
//             IntPtr secondIsHoldingWeapon2Pointer,
//             IntPtr secondIsHoldingFireStrikerPointer1,
//             IntPtr secondIsHoldingFireStrikerPointer3,
//             IntPtr secondIsHoldingFireStrikerPointer4,
//             IntPtr secondIsHoldingFireStrikerPointer5,
//             IntPtr secondActionPointer,
//             MemoryReader mem)
//         {
//             try
//             {
//                 int aimWeapon = mem.ReadInt(aimWeaponPointer);
//                 uint default_weapon_type = mem.ReadUInt(defaultWeaponPointer);
//                 int mainAmmo = mem.ReadInt(mainAmmoPointer);
//                 int backupAmmo = mem.ReadInt(backupAmmoPointer);
//                 int totalAmmoCapacity = mem.ReadInt(totalAmmoCapacityPointer);
//                 int grenadeAmmo = mem.ReadInt(grenadeAmmoPointer);
//                 int pauseStates = mem.ReadInt(pauseStatesPointer);
//                 int menuAndLoadingScreens = mem.ReadInt(menuAndLoadingScreensPointer);
//                 int arrowRopeAmmo = mem.ReadInt(arrowRopeAmmoPointer);
//                 int shootWeaponCounter = mem.ReadInt(shootWeaponCounterPointer);
//                 int shootArrowCounter = mem.ReadInt(shootArrowCounterPointer);
//                 int shootGrenadeCounter = mem.ReadInt(shootGrenadeCounterPointer);
//                 int shootHandgunCounter = mem.ReadInt(shootHandgunCounterPointer);
//                 int shootShotgunCounter = mem.ReadInt(shootShotgunCounterPointer);
//                 int shootWeaponState = mem.ReadInt(shootWeaponStatePointer);
//                 int edge = mem.ReadInt(edgePointer);
//                 int dualPistols = mem.ReadInt(dualPistolsPointer);
//                 int traversal = mem.ReadInt(traversalPointer);
//                 int isHoldingWeapon2 = mem.ReadInt(isHoldingWeapon2Pointer);
//                 int cave = mem.SafeReadInt(cavePointer);
//                 int quickTimeEvent = mem.SafeReadInt(quickTimeEventPointer);
//                 int location = mem.SafeReadInt(locationPointer);
//                 int secondCave = mem.SafeReadInt(secondCavePointer);
//                 int isHoldingFireStriker1 = mem.ReadInt(isHoldingFireStrikerPointer1);
//                 int isHoldingFireStriker3 = mem.ReadInt(isHoldingFireStrikerPointer3);
//                 uint isHoldingFireStriker4 = mem.ReadUInt(isHoldingFireStrikerPointer4);
//                 int isHoldingFireStriker5 = mem.ReadInt(isHoldingFireStrikerPointer5);
//                 int action = mem.ReadInt(actionPointer);

//                 // second
//                 int secondIsHoldingWeapon2 = mem.ReadInt(secondIsHoldingWeapon2Pointer);
//                 int secondIsHoldingFireStriker1 = mem.ReadInt(secondIsHoldingFireStrikerPointer1);
//                 int secondIsHoldingFireStriker3 = mem.ReadInt(secondIsHoldingFireStrikerPointer3);
//                 int secondIsHoldingFireStriker4 = mem.ReadInt(secondIsHoldingFireStrikerPointer4);
//                 int secondIsHoldingFireStriker5 = mem.ReadInt(secondIsHoldingFireStrikerPointer5);
//                 int secondAction = mem.ReadInt(secondActionPointer);

//                 return aimWeapon != 0 || default_weapon_type != 0 || mainAmmo != 0 || backupAmmo != 0 || totalAmmoCapacity != 0 || grenadeAmmo != 0 || pauseStates != 0 || menuAndLoadingScreensPointer != 0 || arrowRopeAmmo != 0 || shootWeaponCounter != 0 || shootArrowCounter != 0 || shootGrenadeCounter != 0 || shootHandgunCounter != 0 || shootShotgunCounter != 0 || shootWeaponState != 0 || edge != 0 || dualPistols != 0 || traversal != 0 || isHoldingWeapon2 != 0 || cave != 0 || quickTimeEvent != 0 || location != 0 || secondCave != 0 || isHoldingFireStriker1 != 0 || isHoldingFireStriker3 != 0 || isHoldingFireStriker4 != 0 || isHoldingFireStriker5 != 0 || secondIsHoldingWeapon2 != 0 || secondIsHoldingFireStriker1 != 0 || secondIsHoldingFireStriker3 != 0 || secondIsHoldingFireStriker4 != 0 || secondIsHoldingFireStriker5 != 0 || action != 0 || secondAction != 0;
//             }
//             catch
//             {
//                 // Likely invalid pointer format or unreadable memory
//                 return false;
//             }
//         }

//         public static void SetToolLEDColor(int isHoldingWeapon2, int secondIsHoldingWeapon2, bool isHoldingTorch, bool isHoldingTorchAndPistol, int controllerIndex, Packet p, bool isHoldingTorchAndNoWeapons = false)
//         {
//             // Touchpad LED
//             if (isHoldingWeapon2 == 69 || secondIsHoldingWeapon2 == 69) // Pick Axe
//             {
//                 p.instructions![4].type = InstructionType.RGBUpdate;
//                 // p.instructions[4].parameters = [controllerIndex, 255, 32, 255]; // original
//                 // p.instructions[4].parameters = [controllerIndex, 191, 32, 191]; // 75% dimmer original
//                 p.instructions[4].parameters = [controllerIndex, 128, 0, 255]; // original
//                 // p.instructions[4].parameters = [controllerIndex, 109, 0, 217]; // 85% dimmer
//                 // p.instructions[4].parameters = [controllerIndex, 96, 0, 191]; // 75% dimmer
//             }
//             else if (isHoldingWeapon2 == 704 || secondIsHoldingWeapon2 == 704) // Motorized Rope Ascender
//             {
//                 // when using zip line
//                 p.instructions![4].type = InstructionType.RGBUpdate;
//                 // p.instructions[4].parameters = new object[] { controllerIndex, 135, 206, 250 }; // Sky Blue
//                 p.instructions[4].parameters = [controllerIndex, 0, 191, 255]; // original Electric Blue
//                 // p.instructions[4].parameters = [controllerIndex, 0, 162, 217]; // 85% dimmer Electric Blue
//                 // p.instructions[4].parameters = [controllerIndex, 0, 143, 191]; // 75% dimmer Electric Blue
//             }
//             else if ((isHoldingTorch || isHoldingTorchAndNoWeapons) && !isHoldingTorchAndPistol) // Fire striker
//             {
//                 p.instructions![4].type = InstructionType.RGBUpdate;
//                 p.instructions[4].parameters = [controllerIndex, 255, 255, 255];
//                 // p.instructions[4].parameters = [controllerIndex, 217, 217, 217]; // 85% dimmer
//                 // p.instructions[4].parameters = [controllerIndex, 191, 191, 191]; // 75% dimmer
//             }
//             else if (isHoldingTorchAndPistol) // Fire striker and pistol
//             {
//                 p.instructions![4].type = InstructionType.RGBUpdate;
//                 // p.instructions[4].parameters = [controllerIndex, 255, 255, 38]; // 85% yellow dominance (15% white)
//                 // p.instructions[4].parameters = [controllerIndex, 217, 217, 32]; // 85% brightness version
//                 // p.instructions[4].parameters = [controllerIndex, 191, 191, 29]; // 75% brightness version
//                 // p.instructions[4].parameters = [controllerIndex, 255, 255, 51]; // 80% yellow dominance (20% white)
//                 p.instructions[4].parameters = [controllerIndex, 255, 255, 64]; // 75% yellow dominance (25% white)
//                 // p.instructions[4].parameters = [controllerIndex, 217, 217, 54]; // 75% yellow dominance 85% brightness version
//                 // p.instructions[4].parameters = [controllerIndex, 191, 191, 48]; // 75% yellow dominance 75% brightness version
//                 // p.instructions[4].parameters = [controllerIndex, 255, 255, 85]; // 67% yellow dominance (33% white)
//                 // p.instructions[4].parameters = [controllerIndex, 255, 255, 107]; // 58% yellow dominance (42% white)
//                 // p.instructions[4].parameters = [controllerIndex, 255, 255, 128]; // 50% balanced blend (50% white)
//                 // p.instructions[4].parameters = [controllerIndex, 255, 255, 255]; // 100% white and 0% yellow
//             }
//         }

//         public static void CheckPlatform(string platform, string fileVersion, string productVersion)
//         {
//             if (platform == "Steam" && productVersion == "1.0" && fileVersion == "1.0.505.0")
//             {
//                 Program.weaponPointer = Program.baseAddress + 0x20D0020;
//                 Program.defaultWeaponPointer = Program.baseAddress + 0x20CFAE4;
//                 Program.aimWeaponNamePointer = Program.baseAddress + 0x20CFDDC;
//                 Program.aimWeaponPointer = Program.baseAddress + 0x20CFB00;
//                 Program.mainAmmoPointer = Program.baseAddress + 0x20CFFFC;
//                 Program.backupAmmoPointer = Program.baseAddress + 0x20D0000;
//                 Program.totalAmmoCapacityPointer = Program.baseAddress + 0x20CE6C8;
//                 Program.grenadeAmmoPointer = Program.baseAddress + 0x20D0014;
//                 // IntPtr pointerBase = IntPtr.Add(Program.baseAddress, 0x01FD17DC);
//                 // Program.grenadeAmmoPointer = Program.mem.ResolvePointer(pointerBase, [0x20, 0x8]);
//                 Program.pauseStatesPointer = Program.baseAddress + 0xB021A4;
//                 Program.menuAndLoadingScreensPointer = Program.baseAddress + 0x1B21290;
//                 Program.arrowRopeAmmoPointer = Program.baseAddress + 0x20CFE44;
//                 Program.shootWeaponCounterPointer = Program.baseAddress + 0x20625B4;
//                 Program.shootArrowCounterPointer = Program.baseAddress + 0x20625C8;
//                 Program.shootGrenadeCounterPointer = Program.baseAddress + 0x20625E8;
//                 Program.shootHandgunCounterPointer = Program.baseAddress + 0x20625F8;
//                 Program.shootShotgunCounterPointer = Program.baseAddress + 0x20625D8;
//                 Program.shootWeaponStatePointer = Program.baseAddress + 0x10C46C4; // 0, 3XXXXXXXXX
//                 Program.edgePointer = Program.baseAddress + 0x1113840; // 145, 177
//                 Program.dualPistolsPointer = Program.baseAddress + 0xEC3F9C; // 588
//                 Program.isHoldingWeapon2Pointer = Program.baseAddress + 0x1F7B704;
//                 Program.traversalPointer = Program.baseAddress + 0x1113844; // 0, 4, 20, 512
//                 Program.cavePointer = Program.baseAddress + 0x1F7B6C8;
//                 Program.quickTimeEventPointer = Program.baseAddress + 0x1FD4B90;
//                 Program.locationPointer = Program.baseAddress + 0x2016390;
//                 Program.secondCavePointer = Program.baseAddress + 0x1F7B788;
//                 Program.isHoldingFireStrikerPointer1 = Program.baseAddress + 0x1F7B720;
//                 Program.isHoldingFireStrikerPointer3 = Program.baseAddress + 0x1F7B728;
//                 Program.isHoldingFireStrikerPointer4 = Program.baseAddress + 0x1F7B72C; // 0, 4XXXXXXXX
//                 Program.isHoldingFireStrikerPointer5 = Program.baseAddress + 0x1F7B730;
//                 Program.actionPointer = Program.baseAddress + 0x1F7B718;

//                 // Second
//                 Program.secondIsHoldingWeapon2Pointer = Program.baseAddress + 0x1F7B7C4;
//                 Program.secondIsHoldingFireStrikerPointer1 = Program.baseAddress + 0x1F7B7E0;
//                 Program.secondIsHoldingFireStrikerPointer3 = Program.baseAddress + 0x1F7B7E8;
//                 Program.secondIsHoldingFireStrikerPointer4 = Program.baseAddress + 0x1F7B7EC;
//                 Program.secondIsHoldingFireStrikerPointer5 = Program.baseAddress + 0x1F7B7F0;
//                 Program.secondActionPointer = Program.baseAddress + 0x1F7B7D8;
//             }
//             else if (platform == "Epic Games Store" && productVersion == "1.01" && fileVersion == "1.1.838.0")
//             {
//                 Program.weaponPointer = Program.baseAddress + 0x20CE6F0;
//                 Program.defaultWeaponPointer = Program.baseAddress + 0x20CE1B4;
//                 Program.aimWeaponNamePointer = Program.baseAddress + 0x20CE4AC;
//                 Program.aimWeaponPointer = Program.baseAddress + 0x20CE1D0;
//                 Program.mainAmmoPointer = Program.baseAddress + 0x20CE6CC;
//                 Program.backupAmmoPointer = Program.baseAddress + 0x20CE6D0;
//                 Program.totalAmmoCapacityPointer = Program.baseAddress + 0x20CE6C8;
//                 Program.grenadeAmmoPointer = Program.baseAddress + 0x20CE6E4;
//                 // IntPtr pointerBase = IntPtr.Add(Program.baseAddress, 0x01FD17DC);
//                 // Program.grenadeAmmoPointer = Program.mem.ResolvePointer(pointerBase, [0x20, 0x8]);
//                 Program.pauseStatesPointer = Program.baseAddress + 0xB01124;
//                 Program.menuAndLoadingScreensPointer = Program.baseAddress + 0x1B1FC10;
//                 Program.arrowRopeAmmoPointer = Program.baseAddress + 0x20CE514;
//                 Program.shootWeaponCounterPointer = Program.baseAddress + 0x2060C94;
//                 Program.shootArrowCounterPointer = Program.baseAddress + 0x2060CA8;
//                 Program.shootGrenadeCounterPointer = Program.baseAddress + 0x2060CC8;
//                 Program.shootHandgunCounterPointer = Program.baseAddress + 0x2060CD8;
//                 Program.shootShotgunCounterPointer = Program.baseAddress + 0x2060CB8;
//                 Program.shootWeaponStatePointer = Program.baseAddress + 0x10C3094; // 0, 3XXXXXXXXX
//                 Program.edgePointer = Program.baseAddress + 0x1112220; // 145, 177
//                 Program.dualPistolsPointer = Program.baseAddress + 0xEC2ABC; // 588
//                 Program.isHoldingWeapon2Pointer = Program.baseAddress + 0x1F79DD4;
//                 Program.cavePointer = Program.baseAddress + 0x1F79E58;
//                 Program.quickTimeEventPointer = Program.baseAddress + 0x1FD3260;
//                 Program.locationPointer = Program.baseAddress + 0x2014A60;
//                 Program.secondCavePointer = Program.baseAddress + 0x1F79D98;
//                 Program.isHoldingFireStrikerPointer1 = Program.baseAddress + 0x1F79DF0;
//                 Program.isHoldingFireStrikerPointer3 = Program.baseAddress + 0x1F79DF8;
//                 Program.isHoldingFireStrikerPointer4 = Program.baseAddress + 0x1F79DFC; // 0, 4XXXXXXXX
//                 Program.traversalPointer = Program.baseAddress + 0x1112224; // 0, 4, 20, 512
//                 Program.isHoldingFireStrikerPointer5 = Program.baseAddress + 0x1F79E00;
//                 Program.actionPointer = Program.baseAddress + 0x1F79DE8;

//                 // Second
//                 Program.secondIsHoldingWeapon2Pointer = Program.baseAddress + 0x1F79E94;
//                 Program.secondIsHoldingFireStrikerPointer1 = Program.baseAddress + 0x1F79EB0;
//                 Program.secondIsHoldingFireStrikerPointer3 = Program.baseAddress + 0x1F79EB8;
//                 Program.secondIsHoldingFireStrikerPointer4 = Program.baseAddress + 0x1FD1834; // 0, 4XXXXXXXX // find this
//                 Program.secondIsHoldingFireStrikerPointer5 = Program.baseAddress + 0x1F79EC0;
//                 Program.secondActionPointer = Program.baseAddress + 0x1F79EA8;

//             }
//             else if ((platform == "Steam" || platform == "GOG") && ((productVersion == "1.1.748.0" && fileVersion == "1.1.748.0") || (productVersion == "1.1.743.0" && fileVersion == "1.1.743.0")))
//             {
//                 Program.weaponPointer = Program.baseAddress + 0x2120690;
//                 Program.defaultWeaponPointer = Program.baseAddress + 0x2120154;
//                 Program.aimWeaponNamePointer = Program.baseAddress + 0x212044C;
//                 Program.aimWeaponPointer = Program.baseAddress + 0x2120170;
//                 Program.mainAmmoPointer = Program.baseAddress + 0x212066C;
//                 Program.backupAmmoPointer = Program.baseAddress + 0x2120670;
//                 Program.totalAmmoCapacityPointer = Program.baseAddress + 0x2120668;
//                 Program.grenadeAmmoPointer = Program.baseAddress + 0x2120684;
//                 // IntPtr pointerBase = IntPtr.Add(Program.baseAddress, 0x01FD17DC);
//                 // Program.grenadeAmmoPointer = Program.mem.ResolvePointer(pointerBase, [0x20, 0x8]);
//                 Program.pauseStatesPointer = Program.baseAddress + 0xB6D728;
//                 Program.menuAndLoadingScreensPointer = Program.baseAddress + 0x1B7C810;
//                 Program.arrowRopeAmmoPointer = Program.baseAddress + 0x21204B4;
//                 Program.shootWeaponCounterPointer = Program.baseAddress + 0x20B41FC;
//                 Program.shootArrowCounterPointer = Program.baseAddress + 0x20B4210;
//                 Program.shootGrenadeCounterPointer = Program.baseAddress + 0x20B4230;
//                 Program.shootHandgunCounterPointer = Program.baseAddress + 0x20B4240;
//                 Program.shootShotgunCounterPointer = Program.baseAddress + 0x20B4220;
//                 Program.shootWeaponStatePointer = Program.baseAddress + 0x112E5D4; // 0, 3XXXXXXXXX // original
//                 // Program.shootWeaponStatePointer = Program.baseAddress + 0x212048C; // 0, 3XXXXXXXXX // alternative
//                 // Program.shootWeaponStatePointer = Program.baseAddress + 0x21204A4; // 0, 3XXXXXXXXX // alternative only use for grenade launcher
//                 Program.edgePointer = Program.baseAddress + 0x117D208; // 145, 177
//                 Program.dualPistolsPointer = Program.baseAddress + 0xF2E05C; // 588
//                 Program.isHoldingWeapon2Pointer = Program.baseAddress + 0x1FD180C;
//                 Program.traversalPointer = Program.baseAddress + 0x117D20C; // 0, 4, 20, 512
//                 Program.cavePointer = Program.baseAddress + 0x1FD17D0;
//                 Program.quickTimeEventPointer = Program.baseAddress + 0x2029398;
//                 Program.locationPointer = Program.baseAddress + 0x1F0D768;
//                 Program.secondCavePointer = Program.baseAddress + 0x1FD1890;
//                 Program.isHoldingFireStrikerPointer1 = Program.baseAddress + 0x1FD1828;
//                 Program.isHoldingFireStrikerPointer3 = Program.baseAddress + 0x1FD1830;
//                 Program.isHoldingFireStrikerPointer4 = Program.baseAddress + 0x1FD1834; // 0, 4XXXXXXXX
//                 Program.isHoldingFireStrikerPointer5 = Program.baseAddress + 0x1FD1838;
//                 Program.actionPointer = Program.baseAddress + 0x1FD1820;

//                 // Second
//                 Program.secondIsHoldingWeapon2Pointer = Program.baseAddress + 0x1FD18CC;
//                 Program.secondIsHoldingFireStrikerPointer1 = Program.baseAddress + 0x1FD18E8;
//                 Program.secondIsHoldingFireStrikerPointer3 = Program.baseAddress + 0x1FD18F0;
//                 Program.secondIsHoldingFireStrikerPointer4 = Program.baseAddress + 0x1FD18F4; // 0, 4XXXXXXXX
//                 Program.secondIsHoldingFireStrikerPointer5 = Program.baseAddress + 0x1FD18F8;
//                 Program.secondActionPointer = Program.baseAddress + 0x1FD18E0;

//             }
//             else if (platform == "Microsoft Store")
//             {
//                 Console.WriteLine("Tomb Raider DualSense Mod initialization failed.");
//                 Console.WriteLine($"Platform: {platform}");
//                 Console.WriteLine("This platform of Tomb Raider is not supported by the mod.");
//                 Console.WriteLine($"This {platform} version of Tomb Raider (product version: {productVersion}, file version: {fileVersion}) could not be identified by the mod.");
//                 Console.WriteLine("This mod only works on Tomb Raider (base game) and Tomb Raider GOTY Edition from the following platforms:");
//                 Console.WriteLine("  - Steam");
//                 Console.WriteLine("  - Epic Games Store");
//                 Console.WriteLine("  - GOG");
//                 Console.WriteLine("Supported versions:");
//                 Console.WriteLine("  Steam:");
//                 Console.WriteLine("    - product version: 1.01, file version: 1.1.0.0 (latest build)");
//                 Console.WriteLine("    - product version: 1.1.748.0, file version: 1.1.748.0 (older build, same as GOG)");
//                 Console.WriteLine("  Epic Games Store:");
//                 Console.WriteLine("    - product version: 1.01, file version: 1.1.838.0");
//                 Console.WriteLine("  GOG:");
//                 Console.WriteLine("    - product version: 1.1.748.0, file version: 1.1.748.0");
//                 Console.WriteLine("Press any key to exit...");
//                 Console.ReadKey(); // Waits for user input before exiting
//                 Environment.Exit(1); // Stop the mod or script from continuing
//             }
//             else if (platform == "Unknown")
//             {
//                 Console.WriteLine("Tomb Raider DualSense Mod initialization failed.");
//                 Console.WriteLine($"Platform: {platform}");
//                 Console.WriteLine("This platform of Tomb Raider could not be identified by the mod.");
//                 Console.WriteLine("This mod only works on Tomb Raider (base game) and Tomb Raider GOTY Edition from the following platforms:");
//                 Console.WriteLine("  - Steam");
//                 Console.WriteLine("  - Epic Games Store");
//                 Console.WriteLine("  - GOG");
//                 Console.WriteLine("Supported versions:");
//                 Console.WriteLine("  Steam:");
//                 Console.WriteLine("    - product version: 1.01, file version: 1.1.0.0 (latest build)");
//                 Console.WriteLine("    - product version: 1.1.748.0, file version: 1.1.748.0 (older build, same as GOG)");
//                 Console.WriteLine("  Epic Games Store:");
//                 Console.WriteLine("    - product version: 1.01, file version: 1.1.838.0");
//                 Console.WriteLine("  GOG:");
//                 Console.WriteLine("    - product version: 1.1.748.0, file version: 1.1.748.0");
//                 Console.WriteLine("If you think this is a mistake, please verify your game installation files or contact the mod author for assistance.");
//                 Console.WriteLine("Press any key to exit...");
//                 Console.ReadKey(); // Waits for user input before exiting
//                 Environment.Exit(1); // Stop the mod or script from continuing
//             }
//             else
//             {
//                 Console.WriteLine("Tomb Raider DualSense Mod initialization failed.");
//                 Console.WriteLine($"Platform: {platform}");
//                 Console.WriteLine($"This {platform} version of Tomb Raider (product version: {productVersion}, file version: {fileVersion}) is not supported by the mod.");
//                 Console.WriteLine("Supported versions:");
//                 Console.WriteLine("  Steam:");
//                 Console.WriteLine("    - product version: 1.01, file version: 1.1.0.0 (latest build)");
//                 Console.WriteLine("    - product version: 1.1.748.0, file version: 1.1.748.0 (older build, same as GOG)");
//                 Console.WriteLine("    - product version: 1.1.743.0, file version: 1.1.743.0 (even older build)");
//                 Console.WriteLine("  Epic Games Store:");
//                 Console.WriteLine("    - product version: 1.01, file version: 1.1.838.0");
//                 Console.WriteLine("  GOG:");
//                 Console.WriteLine("    - product version: 1.1.748.0, file version: 1.1.748.0");
//                 Console.WriteLine("If you think this is a mistake, please check for updates or contact the mod author for assistance.");
//                 Console.WriteLine("Press any key to exit...");
//                 Console.ReadKey(); // Waits for user input before exiting
//                 Environment.Exit(1); // Stop the mod or script from continuing
//             }
//         }

//         public static void WriteLog(string message)
//         {
//             try
//             {
//                 string currentDir = AppDomain.CurrentDomain.BaseDirectory;
//                 string root = Directory.GetParent(currentDir)?.Parent?.FullName ?? currentDir;
//                 string logPath = Path.Combine(root, "DualSenseROTTR.log");

//                 if (!Directory.Exists(root))
//                 {
//                     Console.WriteLine("Log directory not found.");
//                     return;
//                 }

//                 File.AppendAllText(logPath, message + Environment.NewLine);
//             }
//             catch (Exception ex)
//             {
//                 Console.WriteLine($"Failed to write log: {ex.Message}");
//                 Console.WriteLine("Press any key to exit...");
//                 Console.ReadKey();
//                 Environment.Exit(1);
//             }
//         }
//     }
// }