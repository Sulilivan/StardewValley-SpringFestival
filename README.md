# 🧧 Chinese Spring Festival / 中国春节 — Stardew Valley Mod

为星露谷物语添加一个全新的中国风节日：**除夕烟花大会**（冬季28日）。

Adds a brand-new festival to Stardew Valley: **Chinese Spring Festival (New Year's Eve)** on **Winter 28**.

---

## ⚠️ 重要说明 / Important Note

**本 Mod 包含两个组件，必须同时安装才能正常运行！**

**This mod consists of TWO components that MUST be installed together!**

| 组件 / Component | 类型 / Type | 功能 / Purpose |
|-----------------|-------------|----------------|
| `StardewValley-SpringFestival` | Content Pack | 节日数据、NPC对话、物品、地图 |
| `SpringFestivalHelper` | SMAPI Mod (C#) | 烟花表演、换装系统、商店交互 |

---

## ✨ 功能 / Features

### 🎆 除夕烟花大会 (Winter 28)
- **节日时间**：晚上 6:00 — 次日 0:00
- **节日地点**：鹈鹕镇广场
- **邮件通知**：冬季25日（26日起床后）收到镇长刘易斯的春节邀请函
- **双语支持**：根据游戏语言自动切换中/英文

### 🎇 壮观的烟花表演
- **多样烟花效果**：爱心、星爆、螺旋、经典等多种形态
- **绚丽色彩**：红色、金色、紫色等喜庆配色
- **沉浸式体验**：与镇长刘易斯互动触发，全村人一起观赏
- **自动聚集**：NPC们会自动移动到最佳观看位置

### 👘 换装系统
- **类似沙漠节日**：走进彩色帐篷区域即可更换服装
- **位置提示**：刘易斯会在欢迎词中说明换装地点

### 🛒 节日商店
- **商店老板**：皮埃尔
- **节日食品**：冰糖葫芦（+速度）、饺子（+运气）
- **幸运物品**：红包（惊喜礼物！）
- **节日装饰**：中国结、福字、蝴蝶兰
- **烟花道具**：红色、紫色、绿色烟花

### 🧨 NPC 互动
30+ 位村民都有独特的春节主题对话，包含对中国传统文化的独特反应和感受：
- 刘易斯主持烟花大会 | 皮埃尔经营节日商店
- 艾米丽展示她的红色新衣 | 德米特里讲述春节历史
- 加斯准备了正宗饺子和长寿面 | 莱纳斯从山上欣赏烟花
- ……以及更多惊喜！

---

## 📦 安装 / Installation

### 前置要求 / Requirements
1. [SMAPI](https://smapi.io/) — 星露谷物语 Mod 加载器（v4.0.0+）
2. [Content Patcher](https://www.nexusmods.com/stardewvalley/mods/1915) — 内容补丁框架（v2.0.0+）

### 安装步骤 / Installation Steps
1. 确保已安装 SMAPI 和 Content Patcher
2. 将以下**两个**文件夹放入游戏的 `Mods` 目录：
   - `StardewValley-SpringFestival` — 内容包
   - `SpringFestivalHelper` — C# 辅助模块
3. 启动游戏，等到冬季28日即可体验除夕！

```
Stardew Valley/
└── Mods/
    ├── StardewValley-SpringFestival/    ← 内容包（必装）
    │   ├── manifest.json
    │   ├── content.json
    │   └── assets/
    └── SpringFestivalHelper/            ← C# 模块（必装）
        ├── manifest.json
        └── SpringFestivalHelper.dll
```

---

## 📁 文件结构 / File Structure

### StardewValley-SpringFestival (Content Pack)
```
StardewValley-SpringFestival/
├── manifest.json              # Mod 元数据
├── content.json               # Content Patcher 配置
├── assets/
│   ├── data/
│   │   ├── winter28.json      # 节日数据（英文）
│   │   └── winter28_zh.json   # 节日数据（中文）
│   ├── objects/
│   │   ├── tanghulu.png       # 冰糖葫芦
│   │   ├── dumpling.png       # 饺子
│   │   └── red_envelope.png   # 红包
│   ├── furniture/
│   │   ├── chinese_knot.png   # 中国结
│   │   ├── fu_character.png   # 福字
│   │   └── orchid.png         # 蝴蝶兰
│   └── maps/
│       ├── Town.tmx           # 节日地图
│       └── Town-SVE-Christmas2.tmx  # SVE 兼容地图
├── LICENSE
└── README.md
```

### SpringFestivalHelper (SMAPI Mod)
```
SpringFestivalHelper/
├── manifest.json              # Mod 元数据
└── SpringFestivalHelper.dll   # 编译后的 C# 模块
```

---

## 🔧 兼容性 / Compatibility

- **星露谷物语**：1.6+
- **SMAPI**：4.0.0+
- **Content Patcher**：2.0.0+
- **Stardew Valley Expanded**：✅ 完全兼容（提供专用地图）
- 支持多人联机
- 与大多数其他 Mod 兼容

---

## 🗓️ 游戏日历 / Calendar

- **冬季 25 日**：收到镇长的除夕邀请函
- **冬季 28 日**：除夕烟花大会（18:00 - 24:00）

---

## 🎮 游玩指南 / How to Play

1. **到达冬季28日** — 查看日历或信箱中的邀请函
2. **前往鹈鹕镇** — 节日从晚上6点开始
3. **与刘易斯对话** — 获得欢迎词和提示
4. **换装（可选）** — 前往彩色帐篷更换节日服装
5. **逛市集** — 和皮埃尔对话购买节日商品
6. **和村民聊天** — 每个人都有独特对话
7. **再次与刘易斯对话** — 选择开始烟花表演
8. **观赏烟花** — 全村人聚集一起观看壮观的烟花秀！

---

## 🚀 未来计划 / Roadmap

- [x] ~~添加节日商店（出售红包、饺子、灯笼等节日物品）~~
- [x] ~~添加换装系统（类似沙漠节日）~~
- [x] ~~添加壮观的烟花表演~~
- [ ] 添加更多节日地图装饰（红灯笼、春联等）
- [ ] 添加自定义NPC动画（舞龙、放鞭炮等）
- [ ] 添加节日专属烹饪配方
- [ ] 添加节日小游戏

---

## 🐛 问题反馈 / Bug Reports

如果遇到问题，请在 Nexus Mods 页面留言或提交 GitHub Issue，并附上：
- SMAPI 日志（`%appdata%\StardewValley\ErrorLogs\SMAPI-latest.txt`）
- 已安装的其他 Mod 列表
- 问题描述和复现步骤

---

## 📜 License / 许可证

See [LICENSE](LICENSE) for details.

---

## 🙏 致谢 / Credits

- **ConcernedApe** — 创作了这款美妙的游戏
- **Pathoschild** — SMAPI 和 Content Patcher
- **星露谷物语中文社区** — 灵感与支持
