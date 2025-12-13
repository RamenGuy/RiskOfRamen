using BepInEx;
using System.IO;
using UnityEngine;
using RoR2;
using R2API;
using UnityEngine.AddressableAssets;
using RoR2.UI;
using LoadingScreenFix;
//using MSU;

[assembly: HG.Reflection.SearchableAttribute.OptIn]

namespace RiskOfRamen
{
    #region Dependencies
    [BepInDependency("___riskofthunder.RoR2BepInExPack")]
    [BepInDependency(RecalculateStatsAPI.PluginGUID)]
    [BepInDependency("Nebby1999.LoadingScreenFix", BepInDependency.DependencyFlags.HardDependency)]
    //[BepInDependency("com.TeamMoonstorm.MSU", BepInDependency.DependencyFlags.HardDependency)]

    #endregion
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class RiskOfRamenMain : BaseUnityPlugin
    {
        public const string GUID = "com.Ramen.RiskOfRamen";
        public const string MODNAME = "Risk Of Ramen";
        public const string VERSION = "1.0.3";  

        public static PluginInfo pluginInfo { get; private set; }
        public static RiskOfRamenMain instance { get; private set; }
        internal static AssetBundle assetBundle { get; private set; }
        internal static string assetBundleDir => System.IO.Path.Combine(System.IO.Path.GetDirectoryName(pluginInfo.Location), "riskoframenassets");

        internal static string loadingScreenBundleDir => System.IO.Path.Combine(System.IO.Path.GetDirectoryName(pluginInfo.Location), "riskoframenssa");


        [System.Obsolete]
        private void Awake()
        {
            instance = this;
            pluginInfo = Info;
            new RiskOfRamenContent();
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            LoadingScreenFix.LoadingScreenFix.AddSpriteAnimations(GetLoadingScreenBundle());            
        }

        internal static void LogFatal(object data)
        {   
            instance.Logger.LogFatal(data);
        }
        internal static void LogError(object data)
        {
            instance.Logger.LogError(data);
        }
        internal static void LogWarning(object data)
        {
            instance.Logger.LogWarning(data);
        }
        internal static void LogMessage(object data)
        {
            instance.Logger.LogMessage(data);
        }
        internal static void LogInfo(object data)
        {
            instance.Logger.LogInfo(data);
        }
        internal static void LogDebug(object data)
        {
            instance.Logger.LogDebug(data);
        }
        private static void RecalculateStatsAPI_GetStatCoefficients(CharacterBody self, RecalculateStatsAPI.StatHookEventArgs args)
        {
            var onFireBuffDef = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Common/bdOnFire.asset").WaitForCompletion();
            var strongerBurnBuffDef = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Common/bdStrongerBurn.asset").WaitForCompletion();

            if (!self.inventory)
            {
                return;
            }

            int denkuRopeCount = self.inventory.GetItemCountEffective(RiskOfRamenContent._DenkuRope);
            int obsidianCardCount = self.inventory.GetItemCountEffective(RiskOfRamenContent._ObsidianCard);
            int waxIdolCount = self.inventory.GetItemCountEffective(RiskOfRamenContent._WaxIdol);
            int dentedBuckleCount = self.inventory.GetItemCountEffective(RiskOfRamenContent._StainedBelt);
            if (denkuRopeCount >= 1)
            {
                args.critDamageMultAdd += 0.1f * denkuRopeCount;                
            }
            if (obsidianCardCount >= 1)
            {
                args.barrierDecayMult -= 0.25f + (0.1f * obsidianCardCount);
            }
            if (waxIdolCount >= 1)
            {
                if (self.HasBuff(onFireBuffDef) || self.HasBuff(strongerBurnBuffDef))
                {
                    args.armorAdd += 40 + (10 * waxIdolCount);
                }
            }
            if (dentedBuckleCount >= 1)
            {
                // Add (barrier/max health) * 0.25 per stack to crit chance
                args.critAdd += GetBarrierPercentage(self) * (0.25f * dentedBuckleCount);
            }
        }


        internal static AssetBundle GetLoadingScreenBundle()
        {
            return AssetBundle.LoadFromFile(loadingScreenBundleDir);
        }

        public static float GetBarrierPercentage(CharacterBody self)
        {
            return self.healthComponent.barrier / self.healthComponent.fullBarrier;
        }



    }
}
