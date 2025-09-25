using System.Text.Json.Serialization;

namespace TomTatBenhAn_WPF.Core
{
    /// <summary>
    /// Lớp ApiResponse chung cho tất cả API responses
    /// Đảm bảo format response nhất quán giữa frontend và backend
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của Data</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Trạng thái thành công của request
        /// </summary>
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        /// <summary>
        /// Thông báo từ server
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Dữ liệu trả về từ server
        /// </summary>
        [JsonPropertyName("data")]
        public T? Data { get; set; }

        /// <summary>
        /// Mã trạng thái HTTP
        /// </summary>
        [JsonPropertyName("statusCode")]
        public int StatusCode { get; set; }

        /// <summary>
        /// Thời gian tạo response
        /// </summary>
        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; } = string.Empty;

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public ApiResponse()
        {
            Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        }

        /// <summary>
        /// Constructor với tham số
        /// </summary>
        /// <param name="success">Trạng thái thành công</param>
        /// <param name="message">Thông báo</param>
        /// <param name="data">Dữ liệu</param>
        /// <param name="statusCode">Mã trạng thái HTTP</param>
        public ApiResponse(bool success, string message = "", T? data = default, int statusCode = 200)
        {
            Success = success;
            Message = message;
            Data = data;
            StatusCode = statusCode;
            Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        }

        /// <summary>
        /// Tạo response thành công
        /// </summary>
        /// <param name="data">Dữ liệu trả về</param>
        /// <param name="message">Thông báo thành công</param>
        /// <param name="statusCode">Mã trạng thái HTTP</param>
        /// <returns>ApiResponse thành công</returns>
        public static ApiResponse<T> SuccessResult(T? data = default, string message = "Thành công", int statusCode = 200)
        {
            return new ApiResponse<T>(true, message, data, statusCode);
        }

        /// <summary>
        /// Tạo response lỗi
        /// </summary>
        /// <param name="message">Thông báo lỗi</param>
        /// <param name="statusCode">Mã trạng thái HTTP</param>
        /// <param name="data">Dữ liệu lỗi</param>
        /// <returns>ApiResponse lỗi</returns>
        public static ApiResponse<T> ErrorResult(string message = "Có lỗi xảy ra", int statusCode = 500, T? data = default)
        {
            return new ApiResponse<T>(false, message, data, statusCode);
        }

        /// <summary>
        /// Tạo response không tìm thấy
        /// </summary>
        /// <param name="message">Thông báo không tìm thấy</param>
        /// <param name="data">Dữ liệu</param>
        /// <returns>ApiResponse không tìm thấy</returns>
        public static ApiResponse<T> NotFoundResult(string message = "Không tìm thấy dữ liệu", T? data = default)
        {
            return new ApiResponse<T>(false, message, data, 404);
        }

        /// <summary>
        /// Tạo response dữ liệu không hợp lệ
        /// </summary>
        /// <param name="message">Thông báo lỗi validation</param>
        /// <param name="data">Dữ liệu lỗi validation</param>
        /// <returns>ApiResponse dữ liệu không hợp lệ</returns>
        public static ApiResponse<T> BadRequestResult(string message = "Dữ liệu không hợp lệ", T? data = default)
        {
            return new ApiResponse<T>(false, message, data, 400);
        }

        /// <summary>
        /// Tạo response không có quyền truy cập
        /// </summary>
        /// <param name="message">Thông báo lỗi quyền</param>
        /// <returns>ApiResponse không có quyền</returns>
        public static ApiResponse<T> UnauthorizedResult(string message = "Không có quyền truy cập")
        {
            return new ApiResponse<T>(false, message, default, 401);
        }

        /// <summary>
        /// Tạo response bị cấm
        /// </summary>
        /// <param name="message">Thông báo lỗi cấm</param>
        /// <returns>ApiResponse bị cấm</returns>
        public static ApiResponse<T> ForbiddenResult(string message = "Bị cấm truy cập")
        {
            return new ApiResponse<T>(false, message, default, 403);
        }

        /// <summary>
        /// Kiểm tra xem response có thành công không
        /// </summary>
        /// <returns>True nếu thành công</returns>
        public bool IsSuccess()
        {
            return Success && StatusCode >= 200 && StatusCode < 300;
        }

        /// <summary>
        /// Kiểm tra xem response có lỗi không
        /// </summary>
        /// <returns>True nếu có lỗi</returns>
        public bool IsError()
        {
            return !Success || StatusCode >= 400;
        }

        /// <summary>
        /// Lấy thông báo lỗi chi tiết
        /// </summary>
        /// <returns>Thông báo lỗi</returns>
        public string GetErrorMessage()
        {
            if (IsError())
            {
                return $"Lỗi {StatusCode}: {Message}";
            }
            return Message;
        }

        /// <summary>
        /// Chuyển đổi thành string để debug
        /// </summary>
        /// <returns>String representation của ApiResponse</returns>
        public override string ToString()
        {
            return $"ApiResponse<{typeof(T).Name}>(Success: {Success}, Message: {Message}, StatusCode: {StatusCode})";
        }
    }

    /// <summary>
    /// Lớp ApiResponse không generic cho các trường hợp không cần data
    /// </summary>
    public class ApiResponse : ApiResponse<object>
    {
        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public ApiResponse() : base() { }

        /// <summary>
        /// Constructor với tham số
        /// </summary>
        /// <param name="success">Trạng thái thành công</param>
        /// <param name="message">Thông báo</param>
        /// <param name="data">Dữ liệu</param>
        /// <param name="statusCode">Mã trạng thái HTTP</param>
        public ApiResponse(bool success, string message = "", object? data = null, int statusCode = 200) 
            : base(success, message, data, statusCode) { }

        /// <summary>
        /// Tạo response thành công
        /// </summary>
        /// <param name="message">Thông báo thành công</param>
        /// <param name="statusCode">Mã trạng thái HTTP</param>
        /// <returns>ApiResponse thành công</returns>
        public static ApiResponse SuccessResult(string message = "Thành công", int statusCode = 200)
        {
            return new ApiResponse(true, message, null, statusCode);
        }

        /// <summary>
        /// Tạo response lỗi
        /// </summary>
        /// <param name="message">Thông báo lỗi</param>
        /// <param name="statusCode">Mã trạng thái HTTP</param>
        /// <returns>ApiResponse lỗi</returns>
        public static ApiResponse ErrorResult(string message = "Có lỗi xảy ra", int statusCode = 500)
        {
            return new ApiResponse(false, message, null, statusCode);
        }
    }
}
