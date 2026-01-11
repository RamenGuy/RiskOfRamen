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
using UnityEngine.Networking;
using BepInEx.Configuration;
using IL.RoR2.ContentManagement;

[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

[assembly: HG.Reflection.SearchableAttribute.OptIn]

namespace RiskOfRamen
{
    public class RiskOfRamenConfig
    {

        private static ConfigFile Config;
        public Dictionary<string, ItemTier> tierListStandard = new Dictionary<string, ItemTier>() { 
            { "White", ItemTier.Tier1 },
            { "Green", ItemTier.Tier2 },
            { "Red", ItemTier.Tier3 },
        };
        public Dictionary<string, ItemTier> tierListVoid = new Dictionary<string, ItemTier>() {
            { "Void White", ItemTier.VoidTier1 },
            { "Void Green", ItemTier.VoidTier2 },
            { "Void Red", ItemTier.VoidTier3 },
        };

        public static ConfigEntry<bool> enableVoidLunars;
        public static ConfigEntry<bool> spawnWaxWisp;
        public static ConfigEntry<bool> enableItem(ItemDef itemDef)
        {
            return Config.Bind<bool>("Items", $"Enable {Language.GetString(itemDef.nameToken)}", true, $"Whether or not {Language.GetString(itemDef.nameToken)} should appear in-game.");
        }

        public static void CreateConfig(ConfigFile config)
        {
            Config = config;
            enableVoidLunars = Config.Bind<bool>("Void Lunars", "Enable Void Lunars", true, "If enabled, Risk of Ramen's Void Lunar items will be obtainable and the Contamination Font will spawn in the Void Locus.");
            spawnWaxWisp = Config.Bind<bool>("Specific Item Configuration", "Wax Idol Spawns Wax Wisp", true, "If enabled, the Wax Idol will spawn a Wax Wisp minion. It currently has no AI, and is just kind of there, so if it becomes annoying you can turn it off.");

        }

        public static void CreateItemConfigs(RoR2.ContentManagement.NamedAssetCollection<RoR2.ItemDef> itemDefs)
        {
            foreach (var itemDef in itemDefs)
            {
                enableItem(itemDef);
            }
        }

        public static ConfigFile getConfig() { return Config; }
          
    }

}