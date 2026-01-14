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
using RoR2.Items;

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

        public static ItemIndex chooseRandomItemIndex(Inventory inventory, ItemDef.Pair[] pairs)
        {
            ItemIndex randomIndexChosen = 0;
            List<ItemIndex> itemIndices = new List<ItemIndex>();
            foreach (ItemDef.Pair pair in pairs)
            {
                if (inventory.GetItemCountPermanent(pair.itemDef1) >= 1)
                {
                    itemIndices.Add(pair.itemDef1.itemIndex);
                }
            }
            if (itemIndices.Count > 0)
            {
                randomIndexChosen = Run.instance.treasureRng.NextElementUniform<ItemIndex>(itemIndices);
                return randomIndexChosen;
            }
            else { return ItemIndex.None; }
        }

        public static List<ItemIndex> GetCorruptibleItemsInInventory(Inventory inventory)
        {
            List<ItemIndex> corruptible = new List<ItemIndex>();
            foreach (ItemIndex itemIndex in inventory.itemAcquisitionOrder) 
            {
                if (ContagiousItemManager.GetTransformedItemIndex(itemIndex) != ItemIndex.None)
                {
                    corruptible.Add(itemIndex);
                }
            }
            return corruptible; 
        }

        public static int SafeGetEffectiveItemCount(Inventory inventory, ItemDef item)
        {
            if (RiskOfRamenConfig.enableItem(item).Value)
            {
                return inventory.GetItemCountEffective(item);
            }
            return 0;
        }
        public static int SafeGetTempItemCount(Inventory inventory, ItemDef item)
        {
            if (RiskOfRamenConfig.enableItem(item).Value)
            {
                return inventory.GetItemCountTemp(item);
            }
            return 0;
        }

        public static int SafeGetPermaItemCount(Inventory inventory, ItemDef item)
        {
            if (RiskOfRamenConfig.enableItem(item).Value)
            {
                return inventory.GetItemCountPermanent(item);
            }
            return 0;
        }
    }
}
