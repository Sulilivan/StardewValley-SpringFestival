# 📤 Nexus Mods 上传指南 / Upload Guide

本指南将手把手教你如何将这个 Mod 上传到 Nexus Mods。

---

## 📋 上传前准备 / Pre-Upload Checklist

### 1. 确认文件完整
请确保你有以下两个完整的文件夹：

```
✅ StardewValley-SpringFestival/
   ├── manifest.json
   ├── content.json
   ├── README.md
   ├── LICENSE
   └── assets/
       ├── data/
       ├── objects/
       ├── furniture/
       └── maps/

✅ SpringFestivalHelper/
   ├── manifest.json
   ├── SpringFestivalHelper.dll
   └── SpringFestivalHelper.deps.json
```

### 2. 创建发布压缩包

**方法一：分开打包（推荐）**
- 创建 `Chinese Spring Festival v1.0.1.zip`，包含两个文件夹：
  ```
  Chinese Spring Festival v1.0.1.zip
  ├── StardewValley-SpringFestival/
  └── SpringFestivalHelper/
  ```

**打包步骤（Windows）：**
1. 打开 `Mods` 文件夹
2. 同时选中 `StardewValley-SpringFestival` 和 `SpringFestivalHelper` 两个文件夹
3. 右键 → 发送到 → 压缩(zipped)文件夹
4. 将压缩包重命名为 `Chinese Spring Festival v1.0.1.zip`

---

## 🌐 Nexus Mods 上传步骤

### 第一步：注册/登录 Nexus Mods

1. 访问 https://www.nexusmods.com/
2. 点击右上角 **Register** 注册账号（如已有账号则登录）
3. 验证邮箱并完成注册

### 第二步：进入星露谷物语 Mod 页面

1. 访问 https://www.nexusmods.com/stardewvalley
2. 或在首页搜索 "Stardew Valley"

### 第三步：开始创建 Mod 页面

1. 点击页面右上角的用户头像
2. 选择 **Upload a new mod** 或访问：
   https://www.nexusmods.com/stardewvalley/mods/add

### 第四步：填写 Mod 基本信息

#### Name（名称）
```
Chinese Spring Festival - 中国春节
```

#### Summary（简介，150字符以内）
```
Adds a Chinese Spring Festival celebration on Winter 28 with fireworks, NPC dialogues, costume booth, and festive shop!
```

#### Description（详细描述）
复制以下内容（支持BBCode格式）：

```bbcode
[center][size=6][b]🧧 Chinese Spring Festival / 中国春节[/b][/size][/center]

[center][i]为星露谷物语添加一个全新的中国风节日：除夕烟花大会（冬季28日）[/i][/center]

[center][i]Adds a brand-new festival: Chinese Spring Festival (New Year's Eve) on Winter 28[/i][/center]

[line]

[size=5][b]⚠️ 重要 / IMPORTANT[/b][/size]

[b]本 Mod 包含两个组件，必须同时安装！[/b]
[b]This mod has TWO components - both must be installed![/b]

[list]
[*][b]StardewValley-SpringFestival[/b] - Content Pack (节日数据)
[*][b]SpringFestivalHelper[/b] - C# Mod (交互功能)
[/list]

[line]

[size=5][b]✨ 功能 / Features[/b][/size]

[size=4][b]🎆 除夕烟花大会[/b][/size]
[list]
[*]节日时间：冬季28日 18:00-24:00
[*]节日地点：鹈鹕镇广场
[*]邮件通知：冬季25日收到邀请函
[*]双语支持：中/英文自动切换
[/list]

[size=4][b]🎇 壮观的烟花表演[/b][/size]
[list]
[*]多种烟花形态：爱心、星爆、螺旋、经典
[*]绚丽色彩：红色、金色、紫色
[*]与刘易斯互动触发，全村人一起观赏
[*]NPC自动聚集到最佳观看位置
[/list]

[size=4][b]👘 换装系统[/b][/size]
[list]
[*]类似沙漠节日的换装体验
[*]走进彩色帐篷区域即可更换服装
[/list]

[size=4][b]🛒 节日商店[/b][/size]
[list]
[*]商店老板：皮埃尔
[*]节日食品：冰糖葫芦（+速度）、饺子（+运气）
[*]幸运物品：红包（惊喜礼物！）
[*]节日装饰：中国结、福字、蝴蝶兰
[*]烟花道具：红色、紫色、绿色烟花
[/list]

[size=4][b]🧨 NPC互动[/b][/size]
[list]
[*]30+ 位村民都有独特的春节主题对话
[*]每个角色对中国传统文化有不同的反应和感受
[/list]

[line]

[size=5][b]📦 安装 / Installation[/b][/size]

[size=4][b]前置要求[/b][/size]
[list=1]
[*][url=https://smapi.io/]SMAPI[/url] 4.0.0+
[*][url=https://www.nexusmods.com/stardewvalley/mods/1915]Content Patcher[/url] 2.0.0+
[/list]

[size=4][b]安装步骤[/b][/size]
[list=1]
[*]下载并解压 Mod 压缩包
[*]将 [b]两个[/b] 文件夹放入游戏的 Mods 目录：
[code]
Stardew Valley/
└── Mods/
    ├── StardewValley-SpringFestival/
    └── SpringFestivalHelper/
[/code]
[*]启动游戏，等到冬季28日即可体验！
[/list]

[line]

[size=5][b]🔧 兼容性 / Compatibility[/b][/size]

[list]
[*]星露谷物语 1.6+
[*]SMAPI 4.0.0+
[*]Content Patcher 2.0.0+
[*][b]Stardew Valley Expanded[/b]：✅ 完全兼容
[*]支持多人联机
[/list]

[line]

[size=5][b]🎮 游玩指南[/b][/size]

[list=1]
[*]等待冬季28日
[*]前往鹈鹕镇（节日从18:00开始）
[*]与刘易斯对话获得欢迎词
[*]（可选）前往彩色帐篷换装
[*]和皮埃尔对话购买节日商品
[*]和村民们聊天
[*]再次与刘易斯对话开始烟花表演
[*]享受壮观的烟花秀！
[/list]

[line]

[center][size=4][b]新年快乐！Happy New Year! 🎊[/b][/size][/center]
```

### 第五步：设置 Mod 分类

在 **Category** 部分选择：
- **Primary Category**: `Gameplay`
- **Secondary Category**: `Festivals` 或 `Content Patcher`

### 第六步：添加标签 (Tags)

添加以下标签：
- `Festival`
- `Content Patcher`
- `SMAPI`
- `Chinese`
- `Fireworks`
- `Winter`
- `Celebration`

### 第七步：设置图片

#### Main Image（主图）
- 推荐尺寸：1280x720 或 1920x1080
- 建议内容：节日场景截图、烟花表演、商店界面等
- 可以用游戏内截图或制作宣传图

**如何截图：**
1. 在游戏中按 `F12` (Steam) 或使用截图工具
2. 截取节日场景、烟花表演等精彩画面

### 第八步：上传文件

1. 点击 **Files** 标签
2. 点击 **Add File**
3. 上传你创建的 `Chinese Spring Festival v1.0.1.zip`
4. 填写版本信息：
   - **File name**: `Chinese Spring Festival v1.0.1`
   - **File version**: `1.0.1`
   - **File description**: `Bug fix: Fixed When condition issues for festival dates and shop to prevent triggering during other festivals. Added proper date/season restrictions.`

### 第九步：发布前检查

- [ ] Mod 名称和描述正确
- [ ] 压缩包包含两个必要文件夹
- [ ] 已添加主图
- [ ] 已选择正确的分类和标签

### 第十步：发布！

1. 检查所有信息无误
2. 点击 **Publish Mod** 或 **Submit**
3. 等待 Nexus Mods 审核（通常很快）
4. 发布成功！🎉

---

## 🔄 后续更新

当你需要更新 Mod 时：

1. 登录 Nexus Mods
2. 进入你的 Mod 页面
3. 点击 **Files** → **Add File**
4. 上传新版本压缩包
5. 更新版本号和更新说明

---

## 📋 Nexus Mods 页面示例信息

### 推荐页面标题
```
Chinese Spring Festival - 中国春节除夕烟花大会
```

### 推荐简短描述（英文）
```
Celebrate Chinese New Year's Eve on Winter 28! Features a spectacular fireworks show, costume booth, festive shop with traditional items (dumplings, tanghulu, red envelopes), and unique dialogues for 30+ NPCs. Fully bilingual (English/Chinese).
```

### 推荐简短描述（中文）
```
在冬季28日庆祝除夕！包含壮观的烟花表演、换装系统、节日商店（饺子、冰糖葫芦、红包）以及30+位村民的独特春节对话。完整中英文双语支持。
```

---

## ❓ 常见问题

**Q: 为什么要上传两个文件夹？**
A: `StardewValley-SpringFestival` 是 Content Patcher 内容包，包含节日数据和资源；`SpringFestivalHelper` 是 C# 模块，提供烟花表演、换装等交互功能。两者缺一不可。

**Q: 玩家只安装了一个会怎样？**
A: 如果只安装内容包，节日会存在但没有烟花表演和换装功能；如果只安装 C# 模块，会报错找不到依赖。

**Q: 可以在 GitHub 上也发布吗？**
A: 当然可以！GitHub 适合开源和版本管理，Nexus Mods 适合玩家下载。

---

## 🎊 恭喜！

你已经成功将 Mod 上传到 Nexus Mods！记得：
- 关注玩家评论和问题反馈
- 及时修复 Bug 并发布更新
- 在 Mod 描述中注明已知兼容性问题

**新年快乐！祝你的 Mod 大受欢迎！** 🧧🎆
