# AR互动地球仪 🌍

一个基于Web的增强现实(AR)互动地球仪应用，使用摄像头和标记卡片在现实世界中展示3D地球。

## 功能特点

- 🎯 **AR实时追踪**: 使用Hiro标记卡片在现实世界中定位3D地球仪
- 🌐 **多种视图模式**: 
  - 地形模式 - 显示真实地球卫星图像
  - 政区模式 - 显示地形拓扑图
  - 夜景模式 - 显示城市灯光效果
- ⏯️ **交互控制**: 暂停/继续地球自转
- ℹ️ **知识科普**: 随机显示地球相关知识
- 🎨 **精美UI**: 现代化渐变设计和毛玻璃效果

## 使用方法

### 1. 启动应用
```bash
# 使用Python内置服务器
python -m http.server 8080

# 或使用Node.js的http-server
npx http-server -p 8080
```

### 2. 访问应用
在浏览器中打开：`http://localhost:8080`

### 3. 准备标记卡片
- 访问 [Hiro标记图片](https://raw.githubusercontent.com/AR-js-org/AR.js/master/data/images/hiro.png)
- 打印该图片或在其他设备上显示

### 4. 开始体验
1. 允许浏览器访问摄像头
2. 将摄像头对准Hiro标记卡片
3. 等待地球仪出现在标记上方
4. 使用控制面板进行互动

## 技术栈

- **A-Frame**: WebVR框架
- **AR.js**: Web端AR库
- **Three.js**: 3D图形渲染
- **HTML5/CSS3**: 现代前端技术

## 浏览器兼容性

- Chrome (推荐)
- Firefox
- Safari (iOS需要HTTPS)
- Edge

## 注意事项

⚠️ **重要提示**:
- 需要摄像头权限
- 建议在光线充足的环境中使用
- 标记卡片需要完整显示在摄像头画面中
- iOS设备需要HTTPS连接才能访问摄像头

## 文件结构

```
/workspace/
├── index.html      # 主应用文件
└── README.md       # 说明文档
```

## 开发说明

本项目完全基于Web技术，无需安装任何原生应用。所有代码都在单个HTML文件中，便于部署和使用。

## 许可证

MIT License