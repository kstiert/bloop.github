using Newtonsoft.Json;
using System.IO;
using System;

namespace Bloop.GitHub
{
    public class PluginSettings
    {
        private static readonly JsonSerializer Serializer = new JsonSerializer();
        private string _path;

        [JsonIgnore]
        public Action SettingsChanged;

        public static PluginSettings Instance { get; set; }

        public static void LoadSettings(string file)
        {
            if (File.Exists(file))
            {
                var j = new JsonTextReader(new StreamReader(file));
                Instance = Serializer.Deserialize<PluginSettings>(j);
                Instance._path = file;
            }
            else
            {
                // Defaults
                Instance = new PluginSettings
                {
                    Token = string.Empty,
                    RepoRoot = "C:\\git",
                    Shell = "Cmd"
                };
                Instance._path = file;
                Instance.Save();
            }
        }

        public void Save()
        {
            var w = new JsonTextWriter(new StreamWriter(_path, false));
            Serializer.Serialize(w, this);
            w.Close();
            SettingsChanged?.Invoke();
        }

        public PluginSettings() { }

        public string Token { get; set; }

        public string RepoRoot { get; set; }

        public string Shell { get; set; }
    }
}
