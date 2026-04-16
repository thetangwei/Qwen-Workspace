@echo off
chcp 65001 >nul
title AR互动地球仪启动器

echo ========================================
echo    AR 互动地球仪 - 一键启动服务
echo ========================================
echo.

:: 检测 Python
where python >nul 2>nul
if %errorlevel% == 0 (
    echo [√] 检测到 Python 环境
    echo.
    echo 正在启动服务器...
    echo 访问地址：http://localhost:8080
    echo.
    echo 按 Ctrl+C 可停止服务器
    echo.
    python -m http.server 8080
    goto :end
)

:: 检测 Python3
where python3 >nul 2>nul
if %errorlevel% == 0 (
    echo [√] 检测到 Python3 环境
    echo.
    echo 正在启动服务器...
    echo 访问地址：http://localhost:8080
    echo.
    echo 按 Ctrl+C 可停止服务器
    echo.
    python3 -m http.server 8080
    goto :end
)

:: 检测 Node.js
where node >nul 2>nul
if %errorlevel% == 0 (
    echo [√] 检测到 Node.js 环境
    echo.
    echo 正在安装 http-server...
    call npm install -g http-server --silent
    echo.
    echo 正在启动服务器...
    echo 访问地址：http://localhost:8080
    echo.
    echo 按 Ctrl+C 可停止服务器
    echo.
    npx http-server -p 8080
    goto :end
)

echo [×] 未检测到 Python 或 Node.js 环境！
echo.
echo 请先安装以下任一环境：
echo   1. Python (推荐): https://www.python.org/downloads/
echo   2. Node.js: https://nodejs.org/
echo.
pause

:end
