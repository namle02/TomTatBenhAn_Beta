using System.Net.Http;
using System.Text;
using System.Text.Json;
using TomTatBenhAn_WPF.Repos._Model;
using TomTatBenhAn_WPF.Services.Interface;

namespace TomTatBenhAn_WPF.Services.Implement
{
    public class BenhNhanService : IBenhNhanService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfigServices _configServices;
        private readonly string _baseUrl;

        public BenhNhanService(IConfigServices configServices)
        {
            _configServices = configServices;
            _baseUrl = _configServices.GetApiBaseUrl();
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<ApiResponse<PatientAllData>> SaveBenhNhanAsync(PatientAllData benhNhanData)
        {
            try
            {
                var json = JsonSerializer.Serialize(benhNhanData, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("/benhnhan/save", content);
                
                var responseContent = await response.Content.ReadAsStringAsync();
                
                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<ApiResponse<PatientAllData>>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                    return result ?? new ApiResponse<PatientAllData> { Success = false, Message = "Lỗi khi parse response" };
                }
                else
                {
                    return new ApiResponse<PatientAllData>
                    {
                        Success = false,
                        Message = $"API Error: {response.StatusCode} - {responseContent}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<PatientAllData>
                {
                    Success = false,
                    Message = $"Lỗi khi lưu bệnh nhân: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<PatientAllData>> GetBenhNhanBySoBenhAnAsync(string soBenhAn)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/benhnhan/soBenhAn/{Uri.EscapeDataString(soBenhAn)}");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<ApiResponse<PatientAllData>>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                    return result ?? new ApiResponse<PatientAllData> { Success = false, Message = "Lỗi khi parse response" };
                }
                else
                {
                    var errorResult = JsonSerializer.Deserialize<ApiResponse<PatientAllData>>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                    return errorResult ?? new ApiResponse<PatientAllData>
                    {
                        Success = false,
                        Message = $"API Error: {response.StatusCode}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<PatientAllData>
                {
                    Success = false,
                    Message = $"Lỗi khi tìm kiếm bệnh nhân: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<List<PatientAllData>>> GetAllBenhNhanAsync(int page = 1, int limit = 10)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/benhnhan/all?page={page}&limit={limit}");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<ApiResponse<List<PatientAllData>>>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                    return result ?? new ApiResponse<List<PatientAllData>> { Success = false, Message = "Lỗi khi parse response" };
                }
                else
                {
                    return new ApiResponse<List<PatientAllData>>
                    {
                        Success = false,
                        Message = $"API Error: {response.StatusCode} - {responseContent}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<PatientAllData>>
                {
                    Success = false,
                    Message = $"Lỗi khi lấy danh sách bệnh nhân: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<bool>> DeleteBenhNhanAsync(string id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/benhnhan/{Uri.EscapeDataString(id)}");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<ApiResponse<bool>>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                    return result ?? new ApiResponse<bool> { Success = false, Message = "Lỗi khi parse response" };
                }
                else
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = $"API Error: {response.StatusCode} - {responseContent}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Lỗi khi xóa bệnh nhân: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<List<PatientAllData>>> SearchBenhNhanByNameAsync(string tenBN)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/benhnhan/search?tenBN={Uri.EscapeDataString(tenBN)}");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<ApiResponse<List<PatientAllData>>>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                    return result ?? new ApiResponse<List<PatientAllData>> { Success = false, Message = "Lỗi khi parse response" };
                }
                else
                {
                    return new ApiResponse<List<PatientAllData>>
                    {
                        Success = false,
                        Message = $"API Error: {response.StatusCode} - {responseContent}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<PatientAllData>>
                {
                    Success = false,
                    Message = $"Lỗi khi tìm kiếm bệnh nhân theo tên: {ex.Message}"
                };
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
