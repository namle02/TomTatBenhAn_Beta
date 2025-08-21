using CommunityToolkit.Mvvm.Messaging;
using System.IO;
using System.Reflection;
using System.Text;
using TomTatBenhAn_WPF.Services.Interface;

namespace TomTatBenhAn_WPF.Services.Implement
{
    public class FileServices : IFileServices
    {
        // hàm giải mã thông tin trong file config
        public string Decrypt(string Base64Input, string key)
        {
            var inputBytes = Convert.FromBase64String(Base64Input);
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var result = new byte[inputBytes.Length];

            for (int i = 0; i < inputBytes.Length; i++)
            {
                result[i] = (byte)(inputBytes[i] ^ keyBytes[i % keyBytes.Length]);
            }

            return Encoding.UTF8.GetString(result);
        }

        // Hàm đọc file resources và lấy câu truy vấn
        public string GetQuery(string FileName)
        {
            try
            {
                using Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"TomTatBenhAn_WPF.SqlScripts.{FileName}");
                if(stream == null)
                {
                    throw new FileNotFoundException($"Không tìm thấy file sql: {FileName}");
                }
                using StreamReader strReader = new StreamReader(stream);
                string query = strReader.ReadToEnd();
                return query;
            }
            catch
            {
                throw;
            }

        }

        public string GetPromt(string FileName)
        {
            try
            {
                using (Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"TomTatBenhAn_WPF.Promt.{FileName}")) 
                {
                    if(stream == null)
                    {
                        throw new FileNotFoundException($"Không tìm thấy file promt: {FileName}");
                    }
                    using StreamReader strReader = new StreamReader(stream);
                    string promt = strReader.ReadToEnd();
                    return promt;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
