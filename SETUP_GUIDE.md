# GeoSphere AR 设置指南

## 前置要求

### 软件要求
- **Unity**: 2022.3 LTS 或更高版本
- **iOS**: Xcode 14.0+ (需要 macOS)
- **Android**: Android Studio, JDK 11+, Android SDK 8.0+ (API 26)

### 硬件要求
- **iOS 设备**: iPhone XS 或更新机型 (支持 ARKit)
- **Android 设备**: 支持 ARCore 的设备

## 安装步骤

### 1. 克隆/下载项目
```bash
cd /workspace/GeoSphereAR
```

### 2. 打开 Unity 项目
1. 启动 Unity Hub
2. 点击 "Add" 按钮
3. 选择 `/workspace/GeoSphereAR` 文件夹
4. 等待 Unity 导入项目和安装依赖包

### 3. 安装必要的包
Unity 会自动从 `Packages/manifest.json` 安装以下包：
- AR Foundation 5.x
- ARKit XR Plugin (iOS)
- ARCore XR Plugin (Android)
- Universal Render Pipeline (URP)
- Input System
- TextMeshPro

如果自动安装失败，请通过 Window > Package Manager 手动安装。

### 4. 配置场景
1. 打开 `Assets/Scenes/MainScene.unity` (需要创建)
2. 确保场景中包含以下 GameObject:
   - AR Session
   - AR Session Origin
   - Directional Light
   - Main Camera
   - Canvas (UI)

### 5. 配置 Player Settings

#### iOS 配置
1. File > Build Settings > iOS > Switch Platform
2. Player Settings:
   - Bundle Identifier: `com.geosphere.ar`
   - Minimum iOS Version: 14.0
   - Camera Usage Description: "需要摄像头权限以进行 AR 体验"
   - Enable ARKit Support: Yes

#### Android 配置
1. File > Build Settings > Android > Switch Platform
2. Player Settings:
   - Package Name: `com.geosphere.ar`
   - Minimum API Level: Android 8.0 (API 26)
   - Camera Permission: Required
   - Enable ARCore Support: Yes

### 6. 构建和运行

#### iOS
1. File > Build Settings > Build
2. 在 Xcode 中打开生成的项目
3. 配置签名证书
4. 连接到设备并运行

#### Android
1. File > Build Settings > Build And Run
2. 连接 Android 设备
3. 等待安装和启动

## 脚本组件说明

### 核心脚本

| 脚本 | 位置 | 功能 |
|------|------|------|
| ARMainController | Assets/Scripts/AR/ | 主控制器，协调整个 AR 流程 |
| ARSessionManager | Assets/Scripts/AR/ | AR 会话管理 |
| AREarthPlacer | Assets/Scripts/AR/ | 地球仪放置 |
| HandGestureDetector | Assets/Scripts/Gesture/ | 手势检测 |
| GestureInputManager | Assets/Scripts/Gesture/ | 手势输入管理 |
| EarthController | Assets/Scripts/Earth/ | 地球控制 |
| POIManager | Assets/Scripts/Earth/ | POI 标记管理 |
| ARUIManager | Assets/Scripts/UI/ | UI 管理 |
| KalmanFilter | Assets/Scripts/Utils/ | 数据平滑滤波 |

### 可选脚本

| 脚本 | 位置 | 功能 |
|------|------|------|
| MediaPipeHandTracker | Assets/Scripts/Gesture/ | MediaPipe 手部追踪 (需额外插件) |
| EarthShaderController | Assets/Scripts/Earth/ | 地球着色器效果 |

## 常见问题

### Q: AR 无法启动
A: 检查以下几点：
- 设备是否支持 ARKit/ARCore
- 摄像头权限是否已授予
- 环境光线是否充足
- 是否有可识别的平面

### Q: 手势识别不灵敏
A: 尝试调整：
- `HandGestureDetector` 中的 `deadZoneThreshold`
- `rotationSensitivity` 和 `zoomSensitivity`
- 确保有足够的光线

### Q: 性能问题
A: 优化建议：
- 降低地球贴图分辨率
- 关闭阴影和后期处理效果
- 减少 POI 标记数量
- 启用 URP 的性能模式

## 资源准备

### 地球贴图
推荐从以下来源获取：
- NASA Visible Earth: https://visibleearth.nasa.gov/
- Mapbox: https://www.mapbox.com/

需要的贴图：
- 日间地表贴图 (2048x1024 或更高)
- 夜间灯光贴图
- 云层贴图
- 地形法线贴图 (可选)

### 音效资源
- 触摸反馈音效
- UI 交互音效
- 背景环境音 (可选)

## 下一步

1. 创建 Unity 场景并配置预制体
2. 导入地球贴图和模型
3. 配置材质和着色器
4. 测试手势交互
5. 添加更多 POI 数据
6. 优化性能和用户体验

## 许可证

MIT License
