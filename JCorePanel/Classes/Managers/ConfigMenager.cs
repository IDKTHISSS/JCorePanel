using JCorePanelBase;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCorePanel
{
    public static class ConfigMenager
    {
        public static JCConfig PanelConfig = new JCConfig();

        public static void LoadSettings()
        {
            try
            {
                string filePath = "Config.json";
                string jsonString = File.ReadAllText(filePath);

                PanelConfig = JsonConvert.DeserializeObject<JCConfig> (jsonString);
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
