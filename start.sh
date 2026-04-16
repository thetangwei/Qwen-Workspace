#!/bin/bash

# AR互动地球仪 - 一键启动脚本
# 支持自动检测环境并启动服务器

echo "🌍 AR互动地球仪 - 启动中..."

# 获取本机局域网IP地址
if command -v hostname &> /dev/null; then
    LOCAL_IP=$(hostname -I | awk '{print $1}')
else
    LOCAL_IP="localhost"
fi

PORT=8080

# 检测可用的启动方式
if command -v python3 &> /dev/null; then
    echo "✅ 检测到 Python3，使用 Python HTTP 服务器启动"
    echo ""
    echo "=========================================="
    echo "🌍 AR互动地球仪已启动！"
    echo "=========================================="
    echo "📱 本地访问：http://localhost:$PORT"
    echo "📱 手机/平板访问：http://$LOCAL_IP:$PORT"
    echo ""
    echo "📋 使用说明："
    echo "1. 在手机/电脑浏览器打开上述地址"
    echo "2. 允许摄像头权限"
    echo "3. 显示此图片进行AR识别："
    echo "   https://raw.githubusercontent.com/AR-js-org/AR.js/master/data/images/hiro.png"
    echo "4. 将摄像头对准Hiro标记即可看到3D地球"
    echo ""
    echo "按 Ctrl+C 停止服务器"
    echo "=========================================="
    echo ""
    python3 -m http.server $PORT
    
elif command -v python &> /dev/null; then
    echo "✅ 检测到 Python，使用 Python HTTP 服务器启动"
    echo ""
    echo "=========================================="
    echo "🌍 AR互动地球仪已启动！"
    echo "=========================================="
    echo "📱 本地访问：http://localhost:$PORT"
    echo "📱 手机/平板访问：http://$LOCAL_IP:$PORT"
    echo ""
    echo "📋 使用说明："
    echo "1. 在手机/电脑浏览器打开上述地址"
    echo "2. 允许摄像头权限"
    echo "3. 显示此图片进行AR识别："
    echo "   https://raw.githubusercontent.com/AR-js-org/AR.js/master/data/images/hiro.png"
    echo "4. 将摄像头对准Hiro标记即可看到3D地球"
    echo ""
    echo "按 Ctrl+C 停止服务器"
    echo "=========================================="
    echo ""
    python -m SimpleHTTPServer $PORT
    
elif command -v npx &> /dev/null; then
    echo "✅ 检测到 Node.js，使用 http-server 启动"
    echo ""
    echo "=========================================="
    echo "🌍 AR互动地球仪已启动！"
    echo "=========================================="
    echo "📱 本地访问：http://localhost:$PORT"
    echo "📱 手机/平板访问：http://$LOCAL_IP:$PORT"
    echo ""
    echo "📋 使用说明："
    echo "1. 在手机/电脑浏览器打开上述地址"
    echo "2. 允许摄像头权限"
    echo "3. 显示此图片进行AR识别："
    echo "   https://raw.githubusercontent.com/AR-js-org/AR.js/master/data/images/hiro.png"
    echo "4. 将摄像头对准Hiro标记即可看到3D地球"
    echo ""
    echo "按 Ctrl+C 停止服务器"
    echo "=========================================="
    echo ""
    npx --yes http-server -p $PORT
    
else
    echo "❌ 错误：未检测到 Python 或 Node.js"
    echo ""
    echo "请先安装以下任一环境："
    echo "  - Python: https://www.python.org/downloads/"
    echo "  - Node.js: https://nodejs.org/"
    echo ""
    exit 1
fi
