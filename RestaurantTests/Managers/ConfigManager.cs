using Newtonsoft.Json;
using Utils.Configuration;

namespace RestaurantTests.Managers
{
    public class ConfigManager
    {
        private static ConfigManager _instance;
        public RestaurantConfiguration RestaurantConfiguration { get; private set; }

        private ConfigManager()
        {
            RestaurantConfiguration = GetRestaurantConfiguration(ReadConfigFile("RestaurantConfig.json"));
        }

        public static ConfigManager Instance => _instance ??= new ConfigManager();

        private static RestaurantConfiguration GetRestaurantConfiguration(string config)
        {
            return JsonConvert.DeserializeObject<RestaurantConfiguration>(config);
        }

        private string ReadConfigFile(string fileName)
        {
            var filePath = Path.GetFullPath(Path.Combine(GetType().Assembly.Location, "..", $"Configuration\\{fileName}"));

            string file;
            using (var stream = File.OpenRead(filePath))
            {
                using (var sr = new StreamReader(stream))
                {
                    file = sr.ReadToEnd();
                }
            }

            if (file.Length == 0)
            {
                throw new Exception($"Could not read config file with path {filePath}");
            }

            return file;
        }
    }
}