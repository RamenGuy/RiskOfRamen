using BepInEx;
using System.IO;
using UnityEngine;
using RoR2;
using R2API;
using UnityEngine.AddressableAssets;
using RoR2.UI;
using LoadingScreenFix;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;
using HG;
using System;
//using MSU;
using System.Security;
using System.Security.Permissions;


namespace RiskOfRamen
{
    public class RamenUtils
    {

        public static float GetBarrierPercentage(CharacterBody self)
        {
            return self.healthComponent.barrier / self.healthComponent.fullBarrier;
        }

        public static bool IsAnyVoidTier(ItemDef item)
        {
            ItemTier[] voidTiers = { ItemTier.VoidTier1, ItemTier.VoidTier2, ItemTier.VoidTier3, ItemTier.VoidBoss, RiskOfRamenContent._VoidLunarTier.tier };
            return voidTiers.Contains<ItemTier>(item.tier);
        }

        internal static Color GetItemColor(ItemDef item)
        {
            return ColorCatalog.GetColor(item._itemTierDef.colorIndex);
        }

    }
}
