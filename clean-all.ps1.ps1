Write-Host "===============================" -ForegroundColor Cyan
Write-Host "LIMPEZA TOTAL DA SOLUTION" -ForegroundColor Cyan
Write-Host "===============================" -ForegroundColor Cyan

Write-Host "`nRemovendo pastas bin e obj..." -ForegroundColor Yellow

Get-ChildItem -Recurse -Directory -Include bin,obj |
ForEach-Object {
    Write-Host "Removendo $($_.FullName)" -ForegroundColor DarkGray
    Remove-Item -Recurse -Force -Path $_.FullName
}

Write-Host "`nLimpando cache do NuGet..." -ForegroundColor Yellow
dotnet nuget locals all --clear

Write-Host "`nExecutando dotnet clean..." -ForegroundColor Yellow
dotnet clean

Write-Host "`nExecutando dotnet restore..." -ForegroundColor Yellow
dotnet restore

Write-Host "`nExecutando dotnet build..." -ForegroundColor Yellow
dotnet build

Write-Host "`n===============================" -ForegroundColor Green
Write-Host "LIMPEZA FINALIZADA COM SUCESSO" -ForegroundColor Green
Write-Host "===============================" -ForegroundColor Green