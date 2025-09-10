using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace ConsoleShootEmUp
{
    internal class SaveFile
    {
        public static List<bool> UnlockedLevel { get => s_instance.unlockedLevel; }
#if DEBUG
        public List<bool> unlockedLevel { get; set; } = new List<bool> { true, true, true, true, true };
#else
        public List<bool> unlockedLevel { get; set; } = new List<bool> { true, false, false, false, false };
#endif
        public static List<int> Highscores { get => s_instance.highScores; }
        public List<int> highScores { get; set; } = new List<int> { 0, 0, 0, 0, 0 };
        public static List<int> Times { get => s_instance.times; }
        public List<int> times { get; set; } = new List<int> { 999999, 999999, 999999, 999999, 999999 };
        public static bool Debug { get => s_instance.debug; set => s_instance.debug = value; }
#if DEBUG
        public bool debug = true;
#else
        public bool debug = false;
#endif
        private static SaveFile s_instance = new();
        private static string s_path = Path.Combine(Directory.GetCurrentDirectory(), "SaveFile.json");
        public static void Save()
        {
            if (s_instance == null) return;
            JsonSerializerOptions options = new JsonSerializerOptions();
            string jsonText = JsonSerializer.Serialize(s_instance, SaveFileContext.Default.SaveFile);
            File.WriteAllTextAsync(s_path, jsonText);
        }
        public static void Load()
        {
            try
            {
                if (!File.Exists(s_path))
                {
                    s_instance = new SaveFile();
                    return;
                }
                string content = File.ReadAllText(s_path);
                s_instance = JsonSerializer.Deserialize(content, SaveFileContext.Default.SaveFile)!;
            }
            catch
            {
                s_instance = new();
            }
        }
        public static void Delete()
        {
            if (File.Exists(s_path))
                File.Delete(s_path);
        }
    }
    [JsonSourceGenerationOptions(WriteIndented = true)]
    [JsonSerializable(typeof(SaveFile))]
    internal partial class SaveFileContext : JsonSerializerContext
    {
    }

}
