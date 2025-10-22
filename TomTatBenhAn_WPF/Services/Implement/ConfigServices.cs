using System.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using TomTatBenhAn_WPF.Services.Interface;

namespace TomTatBenhAn_WPF.Services.Implement
{
    public class ConfigServices : IConfigServices
    {
        private readonly IFileServices _fileServices;

        public Dictionary<string, string> configDict = new Dictionary<string, string>();


        public ConfigServices(IFileServices fileServices)
        {
            _fileServices = fileServices;
        }

        public async Task GetConfigFromSheet()
        {
            try
            {
                string encryptedUrl = ConfigurationManager.AppSettings["ApiSheet"]!;
                //string key = ConfigurationManager.AppSettings["KeyDecrypt"]!;

                //if (string.IsNullOrWhiteSpace(encryptedUrl) || string.IsNullOrWhiteSpace(key))
                //    throw new ConfigurationErrorsException("ApiSheet hoặc KeyDecrypt bị thiếu trong App.config.");

                //string decryptedUrl = _fileServices.Decrypt(encryptedUrl, key);

                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(encryptedUrl);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(json);
                var values = doc.RootElement.GetProperty("values");

                foreach (var row in values.EnumerateArray())
                {
                    if (row.GetArrayLength() >= 2)
                    {
                        var k = row[0].GetString() ?? "";
                        var v = row[1].GetString() ?? "";
                        configDict[k] = v;
                    }
                }

            }
            catch (Exception ex)
            {
                // Có thể log ra file nếu cần :
                MessageBox.Show("🛑 Lỗi khi lấy config từ Google Sheet: " + ex.Message);
                throw; // hoặc return null / default fallback
            }
        }

        public string? Get(string key)
        {
            configDict.TryGetValue(key, out var value);
            return value;
        }

        public string GetApiBaseUrl()
        {
            // Trước tiên thử lấy từ config dictionary
            var baseUrl = Get("API_BASE_URL");
            
            // Nếu không có trong config, sử dụng default từ App.config
            if (string.IsNullOrEmpty(baseUrl))
            {
                baseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];
            }
            
            // Nếu vẫn không có, sử dụng default localhost
            if (string.IsNullOrEmpty(baseUrl))
            {
                baseUrl = "http://localhost:3000";
            }
            
            return baseUrl.TrimEnd('/');
        }
    }
}
