using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    
    // Fireworks state
    private bool FireworksShowActive;
    private int FireworksTimer;
    private int FireworksPhase;
    private readonly Random FireworksRandom = new();

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
        this.FireworksShowActive = false;
        this.FireworksTimer = 0;
        this.FireworksPhase = 0;
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
                ? "欢迎来到除夕庆典！新年快乐！^先去到处逛逛吧，和乡亲们聊聊天。^你还可以去彩色帐篷里换一身新衣服！^准备好之后再来找我，我们一起点燃烟花！"
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
        this.StartFireworksShow();
    }

    private void StartFireworksShow()
    {
        // Show Lewis's opening speech
        string msg = Game1.content.GetCurrentLanguage() == LocalizedContentManager.LanguageCode.zh
            ? "各位乡亲们，让我们一起迎接新年的到来！现在……点燃烟花！"
            : "Everyone, let's welcome the new year together! Now... light the fireworks!";
        
        Game1.drawObjectDialogue(msg);
        
        // Start the fireworks show
        this.FireworksShowActive = true;
        this.FireworksTimer = 0;
        this.FireworksPhase = 0;
        
        this.Monitor.Log("Fireworks show started!", LogLevel.Info);
    }

    private void UpdateFireworksShow()
    {
        if (!this.FireworksShowActive)
            return;
            
        this.FireworksTimer++;
        
        var location = Game1.currentLocation;
        if (location == null)
            return;

        // Launch fireworks at intervals
        if (this.FireworksTimer % 30 == 0 && this.FireworksPhase < 20) // Every 0.5 seconds, 20 fireworks total
        {
            this.LaunchFirework(location);
            this.FireworksPhase++;
        }

        // End the show after all fireworks
        if (this.FireworksPhase >= 20 && this.FireworksTimer > 700) // About 12 seconds total
        {
            this.FireworksShowActive = false;
            
            // Show final message
            string finalMsg = Game1.content.GetCurrentLanguage() == LocalizedContentManager.LanguageCode.zh
                ? "新年快乐！祝大家在新的一年里万事如意、幸福安康！恭喜发财！"
                : "Happy New Year! May the coming year bring prosperity and happiness to us all!";
            Game1.drawObjectDialogue(finalMsg);
            
            this.Monitor.Log("Fireworks show ended!", LogLevel.Info);
        }
    }

    private void LaunchFirework(GameLocation location)
    {
        try
        {
            // Random position in the sky above the festival area
            float baseX = (25 + this.FireworksRandom.Next(25)) * 64f; // X: tiles 25-50
            float baseY = (45 + this.FireworksRandom.Next(15)) * 64f; // Y: tiles 45-60 (in the sky)
            
            // Play firework sound
            Game1.playSound("firework");
            
            // Create the firework explosion effect using TemporaryAnimatedSprite
            // Use the game's built-in animations texture
            Color[] fireworkColors = {
                Color.Red, Color.Gold, Color.Orange, Color.White, 
                Color.Cyan, Color.Magenta, Color.Lime, Color.Yellow
            };
            Color color = fireworkColors[this.FireworksRandom.Next(fireworkColors.Length)];
            
            // Create multiple particles for the explosion effect
            int particleCount = 8 + this.FireworksRandom.Next(8);
            for (int i = 0; i < particleCount; i++)
            {
                float angle = (float)(i * Math.PI * 2 / particleCount);
                float speed = 2f + (float)this.FireworksRandom.NextDouble() * 3f;
                float motionX = (float)Math.Cos(angle) * speed;
                float motionY = (float)Math.Sin(angle) * speed;
                
                var particle = new TemporaryAnimatedSprite(
                    textureName: "LooseSprites\\Cursors",
                    sourceRect: new Rectangle(352, 1200, 16, 16), // Star/sparkle sprite
                    animationInterval: 100f,
                    animationLength: 6,
                    numberOfLoops: 0,
                    position: new Vector2(baseX, baseY),
                    flicker: true,
                    flipped: false,
                    layerDepth: 1f,
                    alphaFade: 0.02f,
                    color: color,
                    scale: 3f + (float)this.FireworksRandom.NextDouble() * 2f,
                    scaleChange: -0.05f,
                    rotation: 0f,
                    rotationChange: 0f
                )
                {
                    motion = new Vector2(motionX, motionY),
                    acceleration = new Vector2(0f, 0.1f) // Gravity
                };
                
                location.temporarySprites.Add(particle);
            }
            
            // Add a central flash
            var flash = new TemporaryAnimatedSprite(
                textureName: "LooseSprites\\Cursors",
                sourceRect: new Rectangle(368, 1200, 16, 16), // Bright flash sprite
                animationInterval: 50f,
                animationLength: 4,
                numberOfLoops: 0,
                position: new Vector2(baseX - 32, baseY - 32),
                flicker: false,
                flipped: false,
                layerDepth: 1f,
                alphaFade: 0.1f,
                color: Color.White,
                scale: 6f,
                scaleChange: -0.3f,
                rotation: 0f,
                rotationChange: 0f
            );
            location.temporarySprites.Add(flash);
            
            // Screen flash effect
            Game1.screenGlowOnce(color, false, 0.3f, 0.2f);
            
            this.Monitor.Log($"Launched firework at ({baseX/64}, {baseY/64})", LogLevel.Trace);
        }
        catch (Exception ex)
        {
            this.Monitor.Log($"Error launching firework: {ex.Message}", LogLevel.Error);
        }
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

        // Update fireworks show if active
        this.UpdateFireworksShow();

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
