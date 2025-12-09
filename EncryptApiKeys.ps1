# Script để mã hóa API keys trước khi commit lên Git
# Sử dụng: .\EncryptApiKeys.ps1

param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey,
    
    [Parameter(Mandatory=$false)]
    [string]$Key = "TomTatBenhAn"
)

function Encrypt-String {
    param(
        [string]$input,
        [string]$key
    )
    
    $inputBytes = [System.Text.Encoding]::UTF8.GetBytes($input)
    $keyBytes = [System.Text.Encoding]::UTF8.GetBytes($key)
    $result = New-Object byte[] $inputBytes.Length
    
    for ($i = 0; $i -lt $inputBytes.Length; $i++) {
        $result[$i] = $inputBytes[$i] -bxor $keyBytes[$i % $keyBytes.Length]
    }
    
    return [Convert]::ToBase64String($result)
}

Write-Host "Mã hóa API Key..." -ForegroundColor Green
$encrypted = Encrypt-String -input $ApiKey -key $Key
Write-Host ""
Write-Host "API Key gốc: $ApiKey" -ForegroundColor Yellow
Write-Host "API Key đã mã hóa: $encrypted" -ForegroundColor Cyan
Write-Host ""
Write-Host "Copy giá trị đã mã hóa vào App.config:" -ForegroundColor Green
Write-Host "  <add key=`"API_gemini_X`" value=`"$encrypted`"/>" -ForegroundColor White
Write-Host ""

