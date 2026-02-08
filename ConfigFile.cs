using System.Text.Json;
using System.Text.Json.Nodes;
using TShockAPI;

namespace ShockMP
{
    public class ConfigFile
    {
        private readonly string path;
        private JsonObject data;

        public ConfigFile(string fileName)
        {
            path = Path.Combine(TShock.SavePath, fileName + ".json");
            data = new();

            load();
        }

        public void load()
        {
            if (File.Exists(path)) data = JsonNode.Parse(File.ReadAllText(path))!.AsObject(); else data = new JsonObject();
        }

        public void save()
        {
            File.WriteAllText(path, data.ToJsonString(new JsonSerializerOptions
            {
                WriteIndented = true
            }));
        }

        public bool has(string key)
        {
            return data.ContainsKey(key);
        }

        public void set(string key, string value)
        {
            data[key] = JsonValue.Create(value);
            save();
        }

        public void set(string key, bool value)
        {
            data[key] = JsonValue.Create(value);
            save();
        }

        public void set(string key, int value)
        {
            data[key] = JsonValue.Create(value);
            save();
        }

        public void setIfAbsent(string key, string value)
        {
            if (!has(key)) set(key, value);
        }

        public void setIfAbsent(string key, bool value)
        {
            if (!has(key)) set(key, value);
        }

        public void setIfAbsent(string key, int value)
        {
            if (!has(key)) set(key, value);
        }

        public string get(string key, string def)
        {
            if (!has(key)) return def;
            
            return data[key]!.GetValue<string>();
        }

        public bool get(string key, bool def)
        {
            if (!has(key)) return def;

            return data[key]!.GetValue<bool>();
        }

        public int get(string key, int def)
        {
            if (!has(key)) return def;

            return data[key]!.GetValue<int>();
        }
    }
}