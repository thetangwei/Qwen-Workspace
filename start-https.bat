@echo off
chcp 65001 >nul
title AR 地球仪 - HTTPS 安全启动模式

echo ==========================================
echo   AR 互动地球仪 - HTTPS 安全启动脚本
echo   (解决手机摄像头权限问题)
echo ==========================================
echo.

:: 1. 检查是否安装了 choco (Chocolatey 包管理器)
where choco >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo [错误] 未检测到 Chocolatey 包管理器。
    echo.
    echo 为了自动生成可信的 HTTPS 证书，我们需要安装两个小工具：mkcert 和 caddy。
    echo 请先以【管理员身份】运行 PowerShell，输入以下命令安装 Chocolatey：
    echo.
    echo Set-ExecutionPolicy Bypass -Scope Process -Force; ^
    echo [System.Net.ServicePointManager]::SecurityProtocol = ^
    echo [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; ^
    echo iex ((New-Object System.Net.WebClient).DownloadString('https://community.chocolatey.org/install.ps1'))
    echo.
    echo 安装完成后，请重新双击此脚本。
    echo.
    pause
    exit /b
)

:: 2. 检查并安装 mkcert 和 caddy
echo [检查] 正在检查必要组件...

where mkcert >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo [安装] 正在安装 mkcert (用于生成可信证书)...
    choco install mkcert -y
    if %ERRORLEVEL% NEQ 0 (
        echo [错误] mkcert 安装失败，请检查网络连接或手动安装。
        pause
        exit /b
    )
    echo [操作] 正在创建本地证书授权机构 (首次运行需确认)...
    mkcert -install
)

where caddy >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo [安装] 正在安装 Caddy (支持 HTTPS 的轻量服务器)...
    choco install caddy -y
    if %ERRORLEVEL% NEQ 0 (
        echo [错误] Caddy 安装失败。
        pause
        exit /b
    )
)

:: 3. 获取本机局域网 IP
for /f "tokens=2 delims=:" %%a in ('ipconfig ^| findstr /r "IPv4.*192\."') do (
    set "LOCAL_IP=%%a"
    goto :found_ip
)
:found_ip
if "%LOCAL_IP%"=="" (
    for /f "tokens=2 delims=:" %%a in ('ipconfig ^| findstr /r "IPv4.*10\."') do (
        set "LOCAL_IP=%%a"
    )
)
set "LOCAL_IP=%LOCAL_IP: =%"

if "%LOCAL_IP%"=="" (
    echo [警告] 未检测到局域网 IP，将尝试使用 localhost (手机可能无法访问)
    set "LOCAL_IP=localhost"
)

:: 4. 生成证书
echo [生成] 正在为 %LOCAL_IP% 生成 HTTPS 证书...
mkcert -cert-file cert.pem -key-file key.pem %LOCAL_IP% localhost 127.0.0.1

if not exist cert.pem (
    echo [错误] 证书生成失败。
    pause
    exit /b
)

:: 5. 创建 Caddy 配置文件
echo [配置] 正在创建 Caddyfile...
(
    echo %LOCAL_IP%:8443 {
    echo     root * .
    echo     file_server
    echo     tls ./cert.pem ./key.pem
    echo     encode gzip
    echo }
) > Caddyfile

echo.
echo ==========================================
echo   ✅ 服务器启动成功！
echo ==========================================
echo.
echo   🔒 协议：HTTPS (已启用摄像头权限支持)
echo   💻 电脑访问：https://localhost:8443
echo   📱 手机访问：https://%LOCAL_IP%:8443
echo.
echo   ⚠️ 重要提示：
echo   1. 确保手机和电脑连接在【同一个 WiFi】下。
echo   2. 如果手机无法打开，请检查 Windows 防火墙，允许 "caddy.exe" 通过专用网络。
echo   3. 第一次访问手机可能会提示证书风险，请选择"继续访问" (因为我们是本地自签证书)。
echo.
echo   按 Ctrl+C 或关闭窗口停止服务器。
echo ==========================================
echo.

:: 6. 启动 Caddy
caddy run --config Caddyfile

pause