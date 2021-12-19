using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Numerics;
using UnrealSharp;
using SDK.Script.fnaf9SDK;

namespace Fnaf99
{
    internal class Program
    {

        String staticGameName => "fnaf9-Win64-Shipping";

        string ArtText = @"_________  ___ _     
|  ___|  \/  || |    
| |_  | .  . || |    
|  _| | |\/| || |    
| |   | |  | || |____
\_|   \_|  |_/\_____/
                     
                     ";

        Process process;
        List<UEObject> Players = new List<UEObject>();


        //public UEObject GetLevelManager()
        //{
        //    var World = new UEObject(UnrealEngine.Memory.ReadProcessMemory<UInt64>(UnrealEngine.GWorldPtr)).As<SDK.Script.EngineSDK.World>(); if (World == null || !World.IsA("Class /Script/Engine.World")) return null;
        //    var PersistentLevel = World["PersistentLevel"];
        //    var Levels = World["Levels"];
        //    var OwningGameInstance = World["OwningGameInstance"]; if (OwningGameInstance == null || !OwningGameInstance.IsA("Class /Script/Engine.GameInstance")) return null;
        //    var LocalPlayers = OwningGameInstance["LocalPlayers"]; if (LocalPlayers == null) return null;
        //    var PlayerController = LocalPlayers[0]["PlayerController"].As<SDK.Script.EngineSDK.PlayerController>();
        //    for (var levelIndex = 0u; levelIndex < Levels.Num; levelIndex++)
        //    {
        //        var Level = Levels[levelIndex];
        //        var Actors = new UEObject(Level.Address + 0xA8); // todo fix hardcoded 0xA8 offset...
        //        var y = 0;
        //        for (var i = 0u; i < Actors.Num; i++)
        //        {
        //            if (Actors[i].ClassName.Contains("FNAFLevelManager"))
        //            {
        //                Players.Add(Actors[i]);
        //            }
        //        }
        //    }
        //}

        public void UpdateAllGregorys()
        {

            try
            {
                var World = new UEObject(UnrealEngine.Memory.ReadProcessMemory<UInt64>(UnrealEngine.GWorldPtr)).As<SDK.Script.EngineSDK.World>(); if (World == null || !World.IsA("Class /Script/Engine.World")) return;
                var PersistentLevel = World["PersistentLevel"];
                var Levels = World["Levels"];
                var OwningGameInstance = World["OwningGameInstance"]; if (OwningGameInstance == null || !OwningGameInstance.IsA("Class /Script/Engine.GameInstance")) return;
                var LocalPlayers = OwningGameInstance["LocalPlayers"]; if (LocalPlayers == null) return;
                var PlayerController = LocalPlayers[0]["PlayerController"].As<SDK.Script.EngineSDK.PlayerController>();
                for (var levelIndex = 0u; levelIndex < Levels.Num; levelIndex++)
                {
                    var Level = Levels[levelIndex];
                    var Actors = new UEObject(Level.Address + 0xA8); // todo fix hardcoded 0xA8 offset...
                    var y = 0;
                    for (var i = 0u; i < Actors.Num; i++)
                    {
                        if (Actors[i].ClassName.Contains("Gregory_C"))
                        {
                            Players.Add(Actors[i]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        void ToggleDebugMenu()
        {
            try
            {
                UpdateAllGregorys();
                for (var i = 0; i < Players.Count; i++)
                {
                    Players[i].As<FNAFBasePlayerCharacter>().Controller.As<FNAFBasePlayerController>().OnToggleDevUI();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }

        void EnableCheatsForAll()
        {
            try
            {
                UpdateAllGregorys();
                for (var i = 0; i < Players.Count; i++)
                {
                    Players[i].As<FNAFBasePlayerCharacter>().Controller.As<FNAFBasePlayerController>().CheatManager.BugItGo(3096f,47993f,1524f,1,1,1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        private static bool MainMenu()
        {

            LOOP:

            Program program = new Program();

            Console.Clear();
            Console.WriteLine(program.ArtText);
            Console.WriteLine("FNaF Menu Loader (v1.2)");
            Console.WriteLine("1) COMING SOON");
            Console.WriteLine("2) ToggleDebugMenu");
            Console.WriteLine("3) Exit");
            Console.Write("\r\nSelect an option: ");

            

            switch (Console.ReadLine())
            {
                case "1":
                    
                    goto LOOP;
                    return true;
                case "2":
                    program.ToggleDebugMenu();
                    goto LOOP;
                    return true;
                case "3":
                    return false;
                default:
                    return true;
            }
        }

        private void DumpScene()
        {
            var World = new UEObject(UnrealEngine.Memory.ReadProcessMemory<UInt64>(UnrealEngine.GWorldPtr));
            var Levels = World["Levels"];
            for (var levelIndex = 0u; levelIndex < Levels.Num; levelIndex++)
            {
                var Level = Levels[levelIndex];
                var Actors = new UEObject(Level.Address + 0xA8); // todo fix hardcoded 0xA8 offset...
                for (var i = 0u; i < Actors.Num; i++)
                {
                    var Actor = Actors[i];
                    if (Actor.Address == 0) continue;
                    if (Actor.IsA("Class /Script/Engine.Actor"));
                }
            }
        }

        void GetProcess()
        {
            while (true)
            {
                if (staticGameName != "autogenerate") process = Process.GetProcesses().FirstOrDefault(p => p.ProcessName.Contains(staticGameName) && p.MainWindowHandle != IntPtr.Zero);
                if (process != null) break;
                Thread.Sleep(500);
            }
        }

        Object sync = new Object();
        public void Initlize()
        {
            if (process == null)
            {
                GetProcess();
                new UnrealEngine(new Memory(process)).UpdateAddresses();
            }
            else
            {
                DumpScene();
            }
        }
        static void Main(string[] args)
        {

            Console.Title = "Made by ItzNotSpookyy#5139, With help by uSkizzik#3576";

            Program mainP = new Program();

            mainP.Initlize();

            MainMenu();
        }
    }
}
