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
using RoR2.Items;

[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
        
[assembly: HG.Reflection.SearchableAttribute.OptIn]

namespace RiskOfRamen
{
    #region Dependencies
    [BepInDependency("___riskofthunder.RoR2BepInExPack")]
    [BepInDependency(RecalculateStatsAPI.PluginGUID)]
    [BepInDependency("Nebby1999.LoadingScreenFix", BepInDependency.DependencyFlags.HardDependency)]
    //[BepInDependency("com.TeamMoonstorm.MSU", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.bepis.r2api.content_management", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.bepis.r2api.items", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.bepis.r2api.language", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.bepis.r2api.prefab", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.bepis.r2api.recalculatestats", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.bepis.r2api.director", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.bepis.r2api.orb", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.bepis.r2api.networking", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.bepis.r2api.deployable", BepInDependency.DependencyFlags.HardDependency)]

    #endregion
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class RiskOfRamenMain : BaseUnityPlugin
    {
        public const string GUID = "com.Ramen.RiskOfRamen";
        public const string MODNAME = "Risk Of Ramen";
        public const string VERSION = "1.1.2";  

        public static PluginInfo pluginInfo { get; private set; }
        public static RiskOfRamenMain instance { get; private set; }
        internal static AssetBundle assetBundle { get; private set; }
        internal static string assetBundleDir => System.IO.Path.Combine(System.IO.Path.GetDirectoryName(pluginInfo.Location), "riskoframenassets");

        internal static string loadingScreenBundleDir => System.IO.Path.Combine(System.IO.Path.GetDirectoryName(pluginInfo.Location), "riskoframenssa");

        public static CostTypeDef corruptibleLunar;
        public static CostTypeIndex corruptibleLunarIndex;

        [System.Obsolete]
        private void Awake()
        {
            instance = this;
            pluginInfo = Info;
            RiskOfRamenConfig.CreateConfig(Config);
            new RiskOfRamenContent();
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            SceneDirector.onPostPopulateSceneServer += SceneDirector_onPostPopulatesceneServer;
            PurchaseInteraction.onPurchaseGlobalServer += PurchaseInteraction_onPurchaseGlobalServer;

            LoadingScreenFix.LoadingScreenFix.AddSpriteAnimations(GetLoadingScreenBundle());
            CostTypeCatalog.modHelper.getAdditionalEntries += ModHelper_getAdditionalEntries;

            On.RoR2.CharacterMaster.OnServerStageBegin += CharacterMaster_OnServerStageBegin;
            SpawnCard.onSpawnedServerGlobal += SpawnCard_onSpawnedServerGlobal;
            On.RoR2.CharacterBody.GetVisibilityLevel_CharacterBody += CharacterBody_GetVisibilityLevel_CharacterBody;
            On.RoR2.CharacterBody.OnBuffFinalStackLost += CharacterBody_OnBuffFinalStackLost;
            
        }


        [SystemInitializer(typeof(CostTypeCatalog))]
        private static void Init()
        {
            corruptibleLunarIndex = (CostTypeIndex)Array.IndexOf(CostTypeCatalog.costTypeDefs, corruptibleLunar);
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
            var stillnessBuffDef = RiskOfRamenContent._stillnessBuff;
            var hermitDebuffDef = RiskOfRamenContent._hermitDebuff;
            var hermitBuffDef = RiskOfRamenContent._hermitBuff;
            var clamBuffDef = RiskOfRamenContent._parasiticClamBuff;

            if (!self.inventory)
            {
                return;
            }

            int denkuRopeCount = self.inventory.GetItemCountEffective(RiskOfRamenContent._DenkuRope);
            int obsidianCardCount = self.inventory.GetItemCountEffective(RiskOfRamenContent._ObsidianCard);
            int waxIdolCount = self.inventory.GetItemCountEffective(RiskOfRamenContent._WaxIdol);
            int dentedBuckleCount = self.inventory.GetItemCountEffective(RiskOfRamenContent._StainedBelt);
            int wornTurnkeyCount = self.inventory.GetItemCountEffective(RiskOfRamenContent._WornTurnkey);
            int parasiticClamCount = self.inventory.GetItemCountEffective(RiskOfRamenContent._ParasiticClam);
            int glassTiaraCount = self.inventory.GetItemCountEffective(RiskOfRamenContent._GlassTiara);
            int chitinousChiselCount = self.inventory.GetItemCountEffective(RiskOfRamenContent._ChitinousChisel);

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
                args.critAdd += RamenUtils.GetBarrierPercentage(self) * (0.25f * dentedBuckleCount);
            }
            if (parasiticClamCount >= 1)
            {
                args.baseRegenAdd -= self.baseRegen;
            }


            if (self.HasBuff(stillnessBuffDef))
            {
                var stillnessBoost = .1f + (0.1f * self.GetBuffCount(stillnessBuffDef));
                args.healthMultAdd += stillnessBoost;
                args.regenMultAdd += stillnessBoost;
                args.moveSpeedMultAdd += stillnessBoost;
                args.damageMultAdd += stillnessBoost;
                args.attackSpeedMultAdd += stillnessBoost;
                args.critAdd += stillnessBoost;
                args.armorTotalMult *= 1f + stillnessBoost;
            }
            if (self.HasBuff(hermitBuffDef))
            {
                args.damageTotalMult *= 2;
            }
            if (self.HasBuff(clamBuffDef))
            {
                args.attackSpeedMultAdd += .10f + (.05f * self.GetBuffCount(clamBuffDef));
                args.moveSpeedMultAdd += .10f + (.05f * self.GetBuffCount(clamBuffDef));
            }

            if (glassTiaraCount >= 1)
            {
                GlassTiaraBodyBehavior tiaraBehavior = self.GetComponent<GlassTiaraBodyBehavior>();
                args.baseCurseAdd += tiaraBehavior.curseAdd / self.baseMaxHealth;
                //args.baseCurseAdd += tiaraBehavior.curseAdd * self.maxHealth * 0.01f;
            }
            if (chitinousChiselCount >= 1)
            {
                args.luckAdd += chitinousChiselCount;
            }
        }

        private void PurchaseInteraction_onPurchaseGlobalServer(CostTypeDef.PayCostContext context, CostTypeDef.PayCostResults results)
        {
            if (context.purchaseInteraction.isShrine)
            {
                if (context.activatorInventory.GetItemCountEffective(RiskOfRamenContent._ChitinousChisel) > 0)
                {
                    List<ItemIndex> corruptibleItems = RamenUtils.GetCorruptibleItemsInInventory(context.activatorInventory);
                    LogDebug(corruptibleItems);
                    if (corruptibleItems.Count() > 0)
                    {
                        ItemIndex toCorrupt = Run.instance.treasureRng.NextElementUniform<ItemIndex>(corruptibleItems);
                        ContagiousItemManager.TryForceReplacement(context.activatorInventory, toCorrupt);
                    }
                }
            }
        }


        private void CharacterBody_OnBuffFinalStackLost(On.RoR2.CharacterBody.orig_OnBuffFinalStackLost orig, CharacterBody self, BuffDef buffDef)
        {
            if (buffDef == RiskOfRamenContent._hermitDebuff)
            {
                self.AddBuff(RiskOfRamenContent._hermitBuff);
            }
        }


        private VisibilityLevel CharacterBody_GetVisibilityLevel_CharacterBody(On.RoR2.CharacterBody.orig_GetVisibilityLevel_CharacterBody orig, CharacterBody self, CharacterBody observer)
        {
            if (observer.HasBuff(RiskOfRamenContent._hermitDebuff))
            {
                return VisibilityLevel.Untargetable;
            }
            return VisibilityLevel.Revealed;
        }

        private static void SpawnCard_onSpawnedServerGlobal(SpawnCard.SpawnResult spawnResult)
        {
            if (RiskOfRamenConfig.enableItem(RiskOfRamenContent._EsotericEremite).Value)
            {
                int eremiteCount = 0;
                List<CharacterMaster> playersWithEremite = new List<CharacterMaster>();
                foreach (PlayerCharacterMasterController playerCharacterMasterController in PlayerCharacterMasterController.instances)
                {
                    if (playerCharacterMasterController.master.inventory.GetItemCountEffective(RiskOfRamenContent._EsotericEremite) >= 1)
                    {
                        playersWithEremite.Add(playerCharacterMasterController.master);
                    }
                    eremiteCount += playerCharacterMasterController.master.inventory.GetItemCountEffective(RiskOfRamenContent._EsotericEremite);
                    
                }
                if (eremiteCount > 0)
                {

                    if (Util.CheckRoll(20f + (5f * eremiteCount)))
                    {
                        if (spawnResult.spawnedInstance.GetComponent<CharacterMaster>() != null)
                        {
                            spawnResult.spawnedInstance.GetComponent<CharacterMaster>().GetBody().AddTimedBuff(RiskOfRamenContent._hermitDebuff, 5 + (5 * eremiteCount));
                        }
                    }
                }
            }
        }
        private static void SceneDirector_onPostPopulatesceneServer(SceneDirector sceneDirector) 
        {
            if (SceneCatalog.currentSceneDef.baseSceneName == "voidstage" && RiskOfRamenConfig.enableVoidLunars.Value)
            {
                DirectorCore.instance.TrySpawnObject(
                new DirectorSpawnRequest
                (RiskOfRamenContent._iscContaminationFont, new DirectorPlacementRule
                {
                    placementMode = DirectorPlacementRule.PlacementMode.DirectWithoutRandomRotation,
                    position = new Vector3(-38.9945f, 16.818f, -212.6613f),
                    IgnoreSwarmsArtifact = true
                }, RoR2Application.rng));
            }
            
        }

        private void CharacterMaster_OnServerStageBegin(On.RoR2.CharacterMaster.orig_OnServerStageBegin orig, CharacterMaster self, Stage stage)
        {
            uint LargeChestCost = (uint)Run.instance.GetDifficultyScaledCost(50, Stage.instance.entryDifficultyCoefficient);
            uint GlassTiaraCount = (uint)self.inventory.GetItemCountEffective(RiskOfRamenContent._GlassTiara);
            if (GlassTiaraCount >= 1)
            {
                self.GiveMoney(LargeChestCost * (GlassTiaraCount + 1));
            }
        }

        internal static AssetBundle GetLoadingScreenBundle()
        {
            return AssetBundle.LoadFromFile(loadingScreenBundleDir);
        }


        public static Inventory.ItemTransformation GetTransformationForSpecificItemPairCost(ItemIndex itemIndex, ItemDef.Pair[] pairs, int cost)
        {
            ItemIndex pairedIndex = ItemIndex.None;
            foreach (ItemDef.Pair pair in pairs)
            {
                if (pair.itemDef1.itemIndex.Equals(itemIndex))
                {
                    pairedIndex = pair.itemDef2.itemIndex;
                }
            }
            Inventory.ItemTransformation result = default(Inventory.ItemTransformation);
            result.allowWhenDisabled = false;
            result.minToTransform = cost;
            result.maxToTransform = 1;
            result.originalItemIndex = itemIndex;
            result.newItemIndex = pairedIndex;
            result.forbidPermanentItems = false;
            result.forbidTempItems = true;
            
            result.transformationType = ItemTransformationTypeIndex.None;
            return result;
        }
       

        public static CostTypeDef.PayCostDelegate GetSpecificItemPayCost(ItemDef.Pair[] pairs)
        {
            return delegate (CostTypeDef.PayCostContext context, CostTypeDef.PayCostResults results)
            {
                Inventory inventory = context.activator.GetComponent<CharacterBody>().AsValidOrNull()?.inventory;
                List<ItemIndex> itemIndices = new List<ItemIndex>();
                foreach (ItemDef.Pair pair in pairs)
                {
                    if (inventory.GetItemCountPermanent(pair.itemDef1) >= 1)
                    {
                        itemIndices.Add(pair.itemDef1.itemIndex);
                    }
                }   
                foreach (ItemIndex index in itemIndices)
                {
                    if (GetTransformationForSpecificItemPairCost(index, pairs, context.cost).TryTransform(inventory, out var result2))
                    {
                        results.AddTakenItemsFromTransformation(result2);
                        break;
                    }
                }
                
            };
        }

        public static CostTypeDef.IsAffordableDelegate GetSpecificItemIsAffordable(ItemDef.Pair[] pairs)
        {
            return delegate (CostTypeDef costTypeDef, CostTypeDef.IsAffordableContext context)
            {
                Inventory inventory2 = context.activator.GetComponent<CharacterBody>().AsValidOrNull()?.inventory;
               
                return (bool)inventory2 && GetTransformationForSpecificItemPairCost(RamenUtils.chooseRandomItemIndex(inventory2, pairs), pairs, context.cost).CanTake(inventory2).HasValue;
            };
        }


        private void ModHelper_getAdditionalEntries(List<CostTypeDef> list)
        {

            corruptibleLunar = new CostTypeDef()
            {
                buildCostString = delegate (CostTypeDef costTypeDef, CostTypeDef.BuildCostStringContext context)
                {
                    context.stringBuilder.Append(context.cost);
                    context.stringBuilder.Append(" Lunar");
                },
                
                payCost = GetSpecificItemPayCost(RiskOfRamenContent._corruptibleLunars),
                isAffordable = GetSpecificItemIsAffordable(RiskOfRamenContent._corruptibleLunars),
                colorIndex = ColorCatalog.ColorIndex.VoidItem,
                
            };
            list.Add(corruptibleLunar);
        }

    }
}
