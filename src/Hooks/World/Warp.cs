﻿using System.Collections.Generic;

namespace Pearlcat;

public partial class Hooks
{
    // Fix up warp compatibility
    public static void ApplyWarpHooks()
    {
        On.Menu.PauseMenu.Singal += PauseMenu_Singal;

        //On.AbstractPhysicalObject.GetAllConnectedObjects += AbstractPhysicalObject_GetAllConnectedObjects;
    }

    private static void PauseMenu_Singal(On.Menu.PauseMenu.orig_Singal orig, Menu.PauseMenu self, Menu.MenuObject sender, string message)
    {
        orig(self, sender, message);

        if (WarpEnabled(self.game) && message.EndsWith("warp"))
        {
            Plugin.Logger.LogWarning("PEARLCAT WARP");

            foreach (var playerModule in self.game.GetAllPlayerData())
            {
                if (!playerModule.PlayerRef.TryGetTarget(out var player)) continue;

                player.UpdateInventorySaveData(playerModule);

                for (int i = playerModule.Inventory.Count - 1; i >= 0; i--)
                {
                    var item = playerModule.Inventory[i];

                    player.RemoveFromInventory(item);

                    item.destroyOnAbstraction = true;
                    item.Abstractize(item.pos);
                }

                playerModule.JustWarped = true;
            }
        }
    }

    public static bool WarpEnabled(RainWorldGame game) => game.IsStorySession && (!ModManager.MSC || !game.rainWorld.safariMode);


    // Deprecated
    private static List<AbstractPhysicalObject> AbstractPhysicalObject_GetAllConnectedObjects(On.AbstractPhysicalObject.orig_GetAllConnectedObjects orig, AbstractPhysicalObject self)
    {
        var result = orig(self);

        if (self.realizedObject is not Player player) return result;

        if (!player.TryGetPearlcatModule(out var playerModule)) return result;

        result.AddRange(playerModule.Inventory);
        return result;
    }
}
