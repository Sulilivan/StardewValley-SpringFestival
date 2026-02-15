using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Objects;

namespace SpringFestivalHelper;

/// <summary>
/// SMAPI mod that adds interactive features to the Spring Festival:
/// - Costume change booth (walk into tent area, like Desert Festival)
/// - Fireworks triggered by talking to Lewis (second interaction)
/// - Pierre shop integration
/// </summary>
internal sealed class ModEntry : Mod
{
    // === STATE TRACKING ===
    private bool IsChanging;
    private int CooldownTicks;
    private Point LastPlayerTile;
    private bool HasTalkedToLewis;
    private bool FireworksTriggered;

    // === FESTIVAL COORDINATES ===
    // Costume change point (single tile)
    private const int CostumeTileX = 35;
    private const int CostumeTileY = 72;
    
    // Lewis position (from JSON: Lewis 33 66 2)
    private const int LewisX = 33;
    private const int LewisY = 66;
    
    // Pierre position (from JSON: Pierre 25 72 2)
    private const int PierreX = 25;
    private const int PierreY = 72;

    public override void Entry(IModHelper helper)
    {
        // Use High priority to intercept before game handles it
        helper.Events.Input.ButtonPressed += this.OnButtonPressed;
        helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
        helper.Events.GameLoop.DayStarted += this.OnDayStarted;
        
        this.Monitor.Log("SpringFestivalHelper loaded.", LogLevel.Info);
        this.Monitor.Log($"  Costume tile: ({CostumeTileX}, {CostumeTileY})", LogLevel.Info);
        this.Monitor.Log($"  Lewis position: ({LewisX}, {LewisY})", LogLevel.Info);
        this.Monitor.Log($"  Pierre position: ({PierreX}, {PierreY})", LogLevel.Info);
    }

    private void OnDayStarted(object? sender, DayStartedEventArgs e)
    {
        this.HasTalkedToLewis = false;
        this.FireworksTriggered = false;
        this.IsChanging = false;
        this.CooldownTicks = 0;
        this.LastPlayerTile = Point.Zero;
        this.Monitor.Log("Spring Festival states reset.", LogLevel.Debug);
    }

    private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
    {
        // Only handle in festivals
        if (!Context.IsWorldReady || !Game1.isFestival())
            return;

        // Only handle action buttons
        if (!e.Button.IsActionButton())
            return;

        // Get player position
        var playerTile = Game1.player.Tile;
        int px = (int)playerTile.X;
        int py = (int)playerTile.Y;

        this.Monitor.Log($"Button pressed at player position ({px}, {py})", LogLevel.Trace);

        // Check proximity to Lewis (use hardcoded position + check NPC)
        bool nearLewis = this.IsNearLewis(px, py);
        
        if (nearLewis)
        {
            this.Monitor.Log("Player is near Lewis - handling interaction", LogLevel.Debug);
            this.Helper.Input.Suppress(e.Button);
            this.HandleLewisInteraction();
            return;
        }

        // Check proximity to Pierre for shop
        bool nearPierre = this.IsNearPierre(px, py);
        if (nearPierre)
        {
            this.Monitor.Log("Player is near Pierre - opening shop", LogLevel.Debug);
            this.Helper.Input.Suppress(e.Button);
            this.OpenFestivalShop();
            return;
        }
    }

    private bool IsNearLewis(int playerX, int playerY)
    {
        // Check hardcoded position first
        float distToHardcoded = MathF.Sqrt(MathF.Pow(playerX - LewisX, 2) + MathF.Pow(playerY - LewisY, 2));
        if (distToHardcoded <= 2.5f)
        {
            this.Monitor.Log($"Near Lewis (hardcoded): distance = {distToHardcoded:F2}", LogLevel.Trace);
            return true;
        }

        // Also check actual NPC position
        var lewis = Game1.currentLocation?.getCharacterFromName("Lewis");
        if (lewis != null)
        {
            float distToNpc = Vector2.Distance(new Vector2(playerX, playerY), lewis.Tile);
            if (distToNpc <= 2.5f)
            {
                this.Monitor.Log($"Near Lewis (NPC): distance = {distToNpc:F2}", LogLevel.Trace);
                return true;
            }
        }

        return false;
    }

    private bool IsNearPierre(int playerX, int playerY)
    {
        // Use hardcoded position first
        float distToHardcoded = MathF.Sqrt(MathF.Pow(playerX - PierreX, 2) + MathF.Pow(playerY - PierreY, 2));
        if (distToHardcoded <= 2.5f)
        {
            this.Monitor.Log($"Near Pierre (hardcoded): distance = {distToHardcoded:F2}", LogLevel.Trace);
            return true;
        }

        // Also check actual NPC position
        var pierre = Game1.currentLocation?.getCharacterFromName("Pierre");
        if (pierre != null)
        {
            float distToNpc = Vector2.Distance(new Vector2(playerX, playerY), pierre.Tile);
            if (distToNpc <= 2.5f)
            {
                this.Monitor.Log($"Near Pierre (NPC): distance = {distToNpc:F2}", LogLevel.Trace);
                return true;
            }
        }

        return false;
    }

    private void HandleLewisInteraction()
    {
        // Already triggered fireworks
        if (this.FireworksTriggered)
        {
            string msg = Game1.content.GetCurrentLanguage() == LocalizedContentManager.LanguageCode.zh
                ? "今晚的烟花表演真是太精彩了！新年快乐！"
                : "What a spectacular fireworks show! Happy New Year!";
            Game1.drawObjectDialogue(msg);
            return;
        }

        // First interaction
        if (!this.HasTalkedToLewis)
        {
            this.HasTalkedToLewis = true;
            string msg = Game1.content.GetCurrentLanguage() == LocalizedContentManager.LanguageCode.zh
                ? "欢迎来到春节庆典！新年快乐！^先去到处逛逛吧，和乡亲们聊聊天。^你还可以去彩色帐篷里换一身新衣服！^准备好之后再来找我，我们一起点燃烟花！"
                : "Welcome to the Spring Festival! Happy New Year!^Feel free to explore and chat with everyone.^You can visit the colorful tent to change outfits!^Come back when you're ready to light the fireworks!";
            Game1.drawObjectDialogue(msg);
            this.Monitor.Log("First Lewis interaction - showed welcome message", LogLevel.Debug);
            return;
        }

        // Second interaction - show choice
        string question = Game1.content.GetCurrentLanguage() == LocalizedContentManager.LanguageCode.zh
            ? "准备好开始烟花表演了吗？"
            : "Ready to start the fireworks show?";

        var responses = new List<Response>
        {
            new Response("yes", Game1.content.GetCurrentLanguage() == LocalizedContentManager.LanguageCode.zh 
                ? "开始吧！" 
                : "Let's do it!"),
            new Response("no", Game1.content.GetCurrentLanguage() == LocalizedContentManager.LanguageCode.zh 
                ? "再等等" 
                : "Not yet")
        };

        Game1.currentLocation?.createQuestionDialogue(question, responses.ToArray(), this.OnFireworksChoice);
        this.Monitor.Log("Second Lewis interaction - showed choice dialogue", LogLevel.Debug);
    }

    private void OnFireworksChoice(Farmer who, string answer)
    {
        this.Monitor.Log($"Fireworks choice: {answer}", LogLevel.Debug);
        
        if (answer != "yes")
            return;

        this.FireworksTriggered = true;
        this.TriggerMainEvent();
    }

    private void TriggerMainEvent()
    {
        try
        {
            if (Game1.CurrentEvent != null && Game1.CurrentEvent.isFestival)
            {
                // Load festival data and trigger mainEvent
                var festivalData = Game1.temporaryContent.Load<Dictionary<string, string>>("Data\\Festivals\\winter28");
                if (festivalData.TryGetValue("mainEvent", out string? script) && !string.IsNullOrEmpty(script))
                {
                    Game1.CurrentEvent.eventCommands = script.Split('/');
                    Game1.CurrentEvent.currentCommand = 0;
                    this.Monitor.Log("Triggered mainEvent successfully!", LogLevel.Info);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            this.Monitor.Log($"Error triggering mainEvent: {ex.Message}", LogLevel.Error);
        }

        // Fallback message
        string msg = Game1.content.GetCurrentLanguage() == LocalizedContentManager.LanguageCode.zh
            ? "让我们一起迎接新年！新年快乐！"
            : "Let's welcome the new year! Happy New Year!";
        Game1.drawObjectDialogue(msg);
    }

    private void OpenFestivalShop()
    {
        try
        {
            Utility.TryOpenShopMenu("SpringFestival_Shop", Game1.currentLocation);
            this.Monitor.Log("Opened SpringFestival_Shop", LogLevel.Debug);
        }
        catch (Exception ex)
        {
            this.Monitor.Log($"Error opening shop: {ex.Message}", LogLevel.Error);
            
            string msg = Game1.content.GetCurrentLanguage() == LocalizedContentManager.LanguageCode.zh
                ? "欢迎光临！请看看我们的节日商品！"
                : "Welcome! Check out our festival goods!";
            Game1.drawObjectDialogue(msg);
        }
    }

    private void OnUpdateTicked(object? sender, UpdateTickedEventArgs e)
    {
        if (!Context.IsWorldReady)
            return;

        if (this.IsChanging)
            return;

        if (this.CooldownTicks > 0)
        {
            this.CooldownTicks--;
            return;
        }

        if (!Game1.isFestival())
            return;

        // Get current player tile
        Point playerTile = new((int)Game1.player.Tile.X, (int)Game1.player.Tile.Y);

        // Only check on tile change
        if (playerTile == this.LastPlayerTile)
            return;
        
        this.LastPlayerTile = playerTile;

        // Log position periodically for debugging
        if (e.IsMultipleOf(60))
        {
            this.Monitor.Log($"Player at ({playerTile.X}, {playerTile.Y})", LogLevel.Trace);
        }

        // Check costume change tile
        if (playerTile.X == CostumeTileX && playerTile.Y == CostumeTileY)
        {
            this.Monitor.Log($"Player entered costume tile at ({playerTile.X}, {playerTile.Y})", LogLevel.Info);
            this.IsChanging = true;
            this.DoCostumeChange();
        }
    }

    private void DoCostumeChange()
    {
        Game1.globalFadeToBlack(() =>
        {
            try
            {
                this.RandomizeOutfit();
            }
            catch (Exception ex)
            {
                this.Monitor.Log($"Error changing outfit: {ex}", LogLevel.Error);
            }

            Game1.globalFadeToClear(() =>
            {
                string msg = Game1.content.GetCurrentLanguage() == LocalizedContentManager.LanguageCode.zh
                    ? "你换上了一身崭新的节日盛装！新年新气象！"
                    : "You've changed into a festive new outfit! New year, new look!";
                Game1.drawObjectDialogue(msg);

                this.IsChanging = false;
                this.CooldownTicks = 300; // 5 second cooldown
            });
        }, 0.02f);
    }

    private void RandomizeOutfit()
    {
        var farmer = Game1.player;
        var random = Game1.random;

        // Festive shirt IDs (valid vanilla clothing item IDs)
        // These are actual Clothing item IDs that can be equipped
        string[] festiveShirtIds = { 
            "1000", "1001", "1002", "1003", "1004", "1005", "1006", "1007",
            "1008", "1009", "1010", "1011", "1012", "1013", "1014", "1015"
        };
        
        // Festive pants IDs
        string[] festivePantsIds = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14" };

        // Handle shirt swap
        try
        {
            // Save old shirt to inventory if exists
            if (farmer.shirtItem.Value != null)
            {
                var oldShirt = farmer.shirtItem.Value;
                farmer.addItemToInventory(oldShirt);
                this.Monitor.Log($"Old shirt added to inventory: {oldShirt.ItemId}", LogLevel.Debug);
            }

            // Create and equip new shirt
            string newShirtId = festiveShirtIds[random.Next(festiveShirtIds.Length)];
            var newShirt = new Clothing(newShirtId);
            farmer.shirtItem.Value = newShirt;
            this.Monitor.Log($"Equipped new shirt: {newShirtId}", LogLevel.Debug);
        }
        catch (Exception ex)
        {
            this.Monitor.Log($"Shirt swap error: {ex.Message}", LogLevel.Error);
        }

        // Handle pants swap
        try
        {
            // Save old pants to inventory if exists
            if (farmer.pantsItem.Value != null)
            {
                var oldPants = farmer.pantsItem.Value;
                farmer.addItemToInventory(oldPants);
                this.Monitor.Log($"Old pants added to inventory: {oldPants.ItemId}", LogLevel.Debug);
            }

            // Create and equip new pants
            string newPantsId = festivePantsIds[random.Next(festivePantsIds.Length)];
            var newPants = new Clothing(newPantsId);
            newPants.clothesType.Value = Clothing.ClothesType.PANTS;
            farmer.pantsItem.Value = newPants;
            this.Monitor.Log($"Equipped new pants: {newPantsId}", LogLevel.Debug);
        }
        catch (Exception ex)
        {
            this.Monitor.Log($"Pants swap error: {ex.Message}", LogLevel.Error);
        }

        // Apply festive color to pants
        Color[] festiveColors = 
        {
            new Color(220, 40, 40),   // Festive Red
            new Color(200, 0, 0),     // Deep Red
            new Color(255, 200, 0),   // Gold
            new Color(255, 170, 0),   // Orange Gold
            new Color(180, 50, 50),   // Maroon
            new Color(255, 100, 50),  // Coral
        };
        
        try
        {
            Color pantsColor = festiveColors[random.Next(festiveColors.Length)];
            farmer.changePantsColor(pantsColor);
            this.Monitor.Log($"Changed pants color", LogLevel.Debug);
        }
        catch (Exception ex)
        {
            this.Monitor.Log($"Color change error: {ex.Message}", LogLevel.Debug);
        }

        this.Monitor.Log("Outfit swapped successfully - old clothes in inventory", LogLevel.Info);
    }
}
