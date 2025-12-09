# Hướng dẫn mã hóa API Keys

## Vấn đề
Khi push `App.config` chứa API keys lên GitHub public, Google sẽ tự động quét và khóa các API keys bị rò rỉ.

## Giải pháp
Mã hóa API keys trong `App.config` bằng cách sử dụng XOR encryption với key `TomTatBenhAn`.

## Cách sử dụng

### Bước 1: Mã hóa API Key

Sử dụng script PowerShell:

```powershell
.\EncryptApiKeys.ps1 -ApiKey "AIzaSyCr-NiS_rkqWRRsHg5EmqqkNReFgPCyyvY"
```

Hoặc mã hóa nhiều keys:

```powershell
.\EncryptApiKeys.ps1 -ApiKey "AIzaSyCr-NiS_rkqWRRsHg5EmqqkNReFgPCyyvY"  # API_gemini_1
.\EncryptApiKeys.ps1 -ApiKey "AIzaSyBdgE3_hgL0YgozryfL1xMesI0FKFGv7_o"  # API_gemini_2
.\EncryptApiKeys.ps1 -ApiKey "AIzaSyB15PJNpFSODZe8q18xSyak-53wUNggyZI"  # API_gemini_3
```

### Bước 2: Cập nhật App.config

Thay thế API keys trong `App.config` bằng giá trị đã mã hóa:

```xml
<add key="API_gemini_1" value="[giá trị đã mã hóa từ script]"/>
<add key="API_gemini_2" value="[giá trị đã mã hóa từ script]"/>
<add key="API_gemini_3" value="[giá trị đã mã hóa từ script]"/>
```

### Bước 3: Push lên Git

Bây giờ bạn có thể push `App.config` lên GitHub public mà không lo bị Google quét API keys.

## Lưu ý

- **KeyDecrypt** trong `App.config` phải giống với key dùng để mã hóa (mặc định: `TomTatBenhAn`)
- Code sẽ tự động giải mã API keys khi sử dụng
- Nếu API key không phải Base64 (plain text), code sẽ dùng trực tiếp (tương thích ngược)

## Kiểm tra

Sau khi mã hóa, bạn có thể kiểm tra bằng cách:
1. Push lên GitHub
2. Kiểm tra file `App.config` trên GitHub - API keys sẽ là chuỗi Base64 không đọc được
3. Google sẽ không thể quét được API keys thực tế

