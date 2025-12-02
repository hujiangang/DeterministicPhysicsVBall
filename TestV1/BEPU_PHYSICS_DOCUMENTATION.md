# BEPU Physics 项目文档

## 1. 项目概述

本项目使用 BEPU Physics 引擎实现确定性物理模拟，主要用于台球游戏开发。项目采用模块化设计，支持传统脚本挂载和集中式物理管理两种方式。

## 2. 标签系统

### 2.1 必须创建的标签

| 标签名称 | 用途 | 适用对象 |
|---------|------|----------|
| `MainCamera` | 主摄像机 | 场景中的主摄像机 |
| `Cuestick` | 球杆 | 球杆游戏对象 |
| `WhiteBall` | 白球 | 白球游戏对象 |
| `Table` | 桌子 | 台球桌游戏对象 |
| `Ball` | 普通球 | 除白球外的其他台球 |

### 2.2 标签创建方法

1. 在 Unity 编辑器中，选择需要添加标签的游戏对象
2. 在 Inspector 面板顶部，点击标签下拉菜单
3. 选择 `Add Tag...`
4. 在标签管理器中点击 `+` 按钮添加新标签
5. 输入标签名称，点击 `Save` 保存
6. 返回游戏对象，在标签下拉菜单中选择刚刚创建的标签

## 3. 物理系统架构

### 3.1 传统脚本挂载方式

- **PhyBaseEntity**：物理实体基类，所有物理实体脚本的父类
- **PhyBoxEntity**：单个 BoxCollider 的物理实体脚本
- **PhyCompoundBoxEntity**：多个 BoxCollider 的复合物理实体脚本
- **PhySphereEntity**：SphereCollider 的物理实体脚本

### 3.2 集中式物理管理方式

- **PhyEntityManager**：集中式物理实体管理器，自动处理场景中的物理对象
- **BEPUPhyMgr**：BEPU 物理世界管理器，负责物理世界的创建和更新

### 3.3 球杆系统

- **CuestickManager**：集中式球杆管理器，处理球杆的瞄准、蓄力和击打逻辑

## 4. 管理器使用说明

### 4.1 BEPUPhyMgr（BEPU 物理世界管理器）

#### 功能
- 创建和管理 BEPU 物理世界
- 配置重力、时间步长等物理参数
- 处理物理实体的更新和休眠

#### 使用方法
- 系统自动创建，无需手动操作
- 单例模式，通过 `BEPUPhyMgr.Instance` 访问

### 4.2 PhyEntityManager（物理实体管理器）

#### 功能
- 自动扫描场景中的物理对象
- 为有碰撞体的对象创建物理实体
- 处理物理变换和 Unity 变换的双向同步

#### 使用方法
1. 点击菜单 `Tools/BEPU Physics/Physics Entity/Scan Scene for Physics Objects` 扫描场景
2. 点击菜单 `Tools/BEPU Physics/Physics Entity/Clear All Physics Entities` 清除所有物理实体
3. 点击菜单 `Tools/BEPU Physics/Physics Entity/Toggle Physics Manager` 切换管理器状态

### 4.3 CuestickManager（球杆管理器）

#### 功能
- 自动查找和管理球杆对象
- 处理瞄准、蓄力和击打逻辑
- 绘制瞄准线

#### 使用方法
1. 点击菜单 `Tools/BEPU Physics/Cuestick/Create Cuestick Manager` 创建球杆管理器
2. 点击菜单 `Tools/BEPU Physics/Cuestick/Toggle Cuestick Manager` 切换管理器状态
3. 为球杆对象添加 `Cuestick` 标签
4. 为白球对象添加 `WhiteBall` 标签

## 5. 编辑器工具说明

### 5.1 物理实体工具

| 菜单项 | 功能 |
|-------|------|
| `Auto Attach Physics Scripts` | 自动为场景中的对象挂载物理脚本 |
| `Clear All Physics Scripts` | 清除场景中所有对象的物理脚本 |
| `Scan Scene for Physics Objects` | 扫描场景，创建集中式物理实体 |
| `Clear All Physics Entities` | 清除所有集中式物理实体 |
| `Toggle Physics Manager` | 切换集中式物理管理器状态 |

### 5.2 球杆工具

| 菜单项 | 功能 |
|-------|------|
| `Create Cuestick Manager` | 创建球杆管理器 |
| `Toggle Cuestick Manager` | 切换球杆管理器状态 |

### 5.3 摄像机工具

| 菜单项 | 功能 |
|-------|------|
| `Set Main Camera to Top View` | 设置主摄像机为顶视角 |
| `Adjust Camera to See Table` | 调整摄像机位置以更好地看到桌子 |

## 6. 输入控制说明

### 6.1 球杆控制

| 输入 | 功能 |
|------|------|
| **鼠标右键** | 按住并移动鼠标进行瞄准 |
| **空格键** | 按住蓄力，松开后自动衰减 |
| **鼠标左键** | 松开时击打白球（在蓄力状态下） |

## 7. 文件结构

```
Assets/
├── 3D/                    # 3D 模型资源
│   ├── Cuestick/          # 球杆模型
│   └── Table/             # 桌子和球模型
├── Fbx/                   # FBX 模型文件
├── Mat/                   # 材质文件
├── Physic Materials/      # 物理材质
├── Prefab/                # 预制体
├── Scenes/                # 场景文件
└── Scripts/               # 脚本文件
    ├── 3rd/              # 第三方库
    │   └── BEPU/         # BEPU Physics 引擎
    ├── Framework/         # 框架代码
    │   ├── BEPUWrapper/   # BEPU 包装器
    │   ├── ConversionHelper/ # 转换助手
    │   └── Editor/       # 编辑器工具
    └── GamePlay/          # 游戏逻辑
```

## 8. 常见问题及解决方案

### 8.1 物理实体不响应

**可能原因**：
- 未添加正确的标签
- 物理管理器未启用
- 碰撞体未正确配置

**解决方案**：
- 检查并添加正确的标签
- 确保物理管理器已启用
- 检查碰撞体的大小、位置和材质设置

### 8.2 球杆不跟随瞄准

**可能原因**：
- 球杆未添加 `Cuestick` 标签
- 白球未添加 `WhiteBall` 标签
- 球杆管理器未启用

**解决方案**：
- 为球杆添加 `Cuestick` 标签
- 为白球添加 `WhiteBall` 标签
- 确保球杆管理器已启用

### 8.3 摄像机视角不正确

**可能原因**：
- 未设置主摄像机
- 摄像机位置和旋转不正确

**解决方案**：
- 确保场景中有一个带有 `MainCamera` 标签的摄像机
- 使用编辑器工具设置摄像机为顶视角

## 9. 性能优化建议

1. **使用静态实体**：对于静止的物体，设置 `isStatic = true`
2. **合理设置休眠参数**：调整 `VelocityLowerLimit` 和 `LowVelocityTimeMinimum`
3. **减少碰撞体数量**：合并相邻的碰撞体，减少物理计算开销
4. **使用集中式管理**：对于大量物理对象，优先使用集中式物理管理
5. **优化物理材质**：合理设置摩擦系数和弹性系数

## 10. 版本控制注意事项

1. **统一换行符**：使用 LF 换行符，避免跨平台问题
2. **忽略临时文件**：在 `.gitignore` 中添加临时文件和缓存目录
3. **使用 .editorconfig**：统一代码风格和格式
4. **定期更新文档**：及时更新文档，保持与代码同步

## 11. 扩展开发指南

### 11.1 添加新的物理实体类型

1. 创建新的物理实体脚本，继承自 `PhyBaseEntity`
2. 实现 `Start` 方法，创建对应的 BEPU 物理实体
3. 调用 `AddSelfToPhyWorld` 将实体添加到物理世界
4. 调用 `SyncPhyTransformWithUnityTransform` 同步初始变换

### 11.2 扩展集中式物理管理

1. 修改 `PhyEntityManager` 类，添加新的碰撞体类型支持
2. 在 `CreatePhyEntityForObject` 方法中添加新的碰撞体处理逻辑
3. 确保新的物理实体类型能够正确同步变换

## 12. 联系方式

如有任何问题或建议，请联系项目负责人。

---

**文档版本**：1.0
**创建日期**：2025-12-02
**更新日期**：2025-12-02
