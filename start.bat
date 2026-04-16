@echo off
chcp 65001 >nul
title AR互动地球仪 - 启动服务器

echo ========================================
echo    AR 互动地球仪 - 一键启动脚本
echo ========================================
echo.

:: 获取局域网 IP
for /f "tokens=2 delims=:" %%a in ('ipconfig ^| findstr /c:"IPv4"') do (
    for %%b in (%%a) do set LOCALIP=%%b
    goto :got_ip
)
:got_ip

:: 检测 Python
where python >nul 2>&1
if %errorlevel% equ 0 (
    echo [√] 检测到 Python 环境
    echo.
    echo 正在启动服务器...
    echo.
    echo ========================================
    echo  服务器已启动！
    echo.
    echo  【电脑浏览器访问】
    echo  http://localhost:8080
    echo.
    echo  【手机浏览器访问】(需同一 WiFi)
    echo  http://%LOCALIP%:8080
    echo.
    echo  ⚠️ 重要提示：
    echo  1. 手机和电脑必须连接同一 WiFi
    echo  2. 如手机无法访问，请检查 Windows 防火墙
    echo  3. 部分手机需要 HTTPS 才能调用摄像头
    echo     如遇到问题，建议在电脑浏览器测试
    echo ========================================
    echo.
    echo 按 Ctrl+C 可停止服务器
    echo.
    python -m http.server 8080
    goto :end
)

:: 检测 Python3
where python3 >nul 2>&1
if %errorlevel% equ 0 (
    echo [√] 检测到 Python3 环境
    echo.
    echo 正在启动服务器...
    echo.
    echo ========================================
    echo  服务器已启动！
    echo.
    echo  【电脑浏览器访问】
    echo  http://localhost:8080
    echo.
    echo  【手机浏览器访问】(需同一 WiFi)
    echo  http://%LOCALIP%:8080
    echo.
    echo  ⚠️ 重要提示：
    echo  1. 手机和电脑必须连接同一 WiFi
    echo  2. 如手机无法访问，请检查 Windows 防火墙
    echo  3. 部分手机需要 HTTPS 才能调用摄像头
    echo     如遇到问题，建议在电脑浏览器测试
    echo ========================================
    echo.
    echo 按 Ctrl+C 可停止服务器
    echo.
    python3 -m http.server 8080
    goto :end
)

:: 检测 Node.js
where node >nul 2>&1
if %errorlevel% equ 0 (
    echo [√] 检测到 Node.js 环境
    echo.
    echo 正在启动服务器...
    echo.
    echo ========================================
    echo  服务器已启动！
    echo.
    echo  【电脑浏览器访问】
    echo  http://localhost:8080
    echo.
    echo  【手机浏览器访问】(需同一 WiFi)
    echo  http://%LOCALIP%:8080
    echo.
    echo  ⚠️ 重要提示：
    echo  1. 手机和电脑必须连接同一 WiFi
    echo  2. 如手机无法访问，请检查 Windows 防火墙
    echo  3. 部分手机需要 HTTPS 才能调用摄像头
    echo     如遇到问题，建议在电脑浏览器测试
    echo ========================================
    echo.
    echo 按 Ctrl+C 可停止服务器
    echo.
    npx http-server -p 8080 -c-1
    goto :end
)

echo [×] 未检测到 Python 或 Node.js 环境！
echo.
echo 请安装以下任一环境：
echo 1. Python (推荐): https://www.python.org/downloads/
echo    安装时请勾选 "Add to PATH"
echo.
echo 2. Node.js: https://nodejs.org/
echo.
pause

:end
