﻿using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.IO;

namespace JCorePanel
{
    public static class ConfigMenager
    {
        public static JCConfig PanelConfig = new JCConfig();

        public static void LoadSettings()
        {
            PanelConfig.SteamPath = (string)Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Valve\Steam", "SteamExe", "null");
            try
            {
                string filePath = "Config.json";
                string jsonString = File.ReadAllText(filePath);

                PanelConfig = JsonConvert.DeserializeObject<JCConfig>(jsonString);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Config.json not found");
            }
        }
        public static void SaveSettings()
        {
            try
            {
                string Settings = JsonConvert.SerializeObject(PanelConfig);
                File.WriteAllText("Config.json", Settings);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Config.json not found");
            }
        }
    }
}
