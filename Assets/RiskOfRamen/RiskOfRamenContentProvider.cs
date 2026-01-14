    using RoR2.ContentManagement;
using UnityEngine;
using RoR2;
using RoR2.ExpansionManagement;
using System.Collections;
using R2API;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using static RoR2.Console;
using RoR2.UI;
using UnityEngine.UIElements;
using RoR2.Projectile;
using RiskOfRamen.Assets.Allocentrism;
using BepInEx.Configuration;
using RoR2BepInExPack.GameAssetPaths;
//using MSU;

namespace RiskOfRamen
    {
    public class RiskOfRamenContent : IContentPackProvider
    {
        public string identifier => RiskOfRamenMain.GUID;

        public static ReadOnlyContentPack readOnlyContentPack => new ReadOnlyContentPack(RiskOfRamenContentPack);
        internal static ContentPack RiskOfRamenContentPack { get; } = new ContentPack();    

        public static ItemDef _WaxIdol;
        public static ItemDef _ObsidianCard;
        public static ItemDef _DenkuRope;
        public static ItemDef _StainedBelt;
        public static ItemDef _WornTurnkey;
        public static ItemDef _GlassTiara;
        public static ItemDef _Allocentrism;
        public static ItemDef _EsotericEremite;
        public static ItemDef _ParasiticClam;
        public static ItemDef _ChitinousChisel;
        public static ItemTierDef _VoidLunarTier;

        public static GameObject _ContaminationFont;
        public static InteractableSpawnCard _iscContaminationFont;

        public static GameObject _WaxWispBody;
        public static GameObject _WaxWispMaster;
        public static CharacterSpawnCard _cscWaxWisp;

        public static BuffDef _stillnessBuff;
        public static BuffDef _hermitDebuff;
        public static BuffDef _hermitBuff;
        public static BuffDef _parasiticClamBuff;

        public static GameObject _AllocentrismBomb;
        public static GameObject _AllocentrismGhost;
        public static EffectDef _AllocentrismExplosion;

        public static DeployableSlot AllocentrismBomb;
        public static DeployableSlot WaxWispSlot;

        public static ExpansionDef _expansionDef;

        public static AssetBundle _assetBundle;

        public static ItemDef.Pair[] _corruptibleLunars;


        [System.Obsolete]

        public IEnumerator LoadStaticContentAsync(LoadStaticContentAsyncArgs args)
        {
            var asyncOperation = AssetBundle.LoadFromFileAsync(RiskOfRamenMain.assetBundleDir);
            while(!asyncOperation.isDone)
            {
                args.ReportProgress(asyncOperation.progress);
                yield return null;
            }

            //Write code here to initialize your mod post assetbundle load
            _assetBundle = asyncOperation.assetBundle;

            RiskOfRamenMain.LogInfo("Loading assets...");
            LoadAssets();

            RiskOfRamenMain.LogInfo("Adding content...");
            AddContent();

            RiskOfRamenMain.LogInfo("Swapping stubbed shaders...");
            SwapAllShaders();

            RiskOfRamenMain.LogInfo("Registering deployable slots...");
            AllocentrismBomb = DeployableAPI.RegisterDeployableSlot((self, deployableCountMultiplier) =>
            {
                if (self)
                {
                    return AllocentrismBodyBehavior.GetMaxProjectiles(self.inventory);
                }
                return 1;
            });

            WaxWispSlot = DeployableAPI.RegisterDeployableSlot((self, deployableCountMultiplier) =>
            {
                if (self)
                {
                    return WaxIdolBodyBehavior.GetMaxProjectiles(self.inventory);
                }
                return 1;
            });

            RiskOfRamenMain.LogInfo("Populating Void Lunar lists...");
            _corruptibleLunars = PopulateCorruptibleLunars();

            RiskOfRamenMain.LogInfo("Creating item configs...");
            RiskOfRamenConfig.CreateItemConfigs(RiskOfRamenContentPack.itemDefs);


        }


        private static void LoadAssets()
        {
            _WaxIdol = _assetBundle.LoadAsset<ItemDef>("WaxIdol");
            _ObsidianCard = _assetBundle.LoadAsset<ItemDef>("ObsidianCard");
            _DenkuRope = _assetBundle.LoadAsset<ItemDef>("DenkuRope");
            _StainedBelt = _assetBundle.LoadAsset<ItemDef>("StainedBelt");
            _WornTurnkey = _assetBundle.LoadAsset<ItemDef>("WornTurnkey");
            _GlassTiara = _assetBundle.LoadAsset<ItemDef>("GlassTiara");
            _Allocentrism = _assetBundle.LoadAsset<ItemDef>("Allocentrism");
            _EsotericEremite = _assetBundle.LoadAsset<ItemDef>("EsotericEremite");
            _ParasiticClam = _assetBundle.LoadAsset<ItemDef>("ParasiticClam");
            _ChitinousChisel = _assetBundle.LoadAsset<ItemDef>("ChitinousChisel");
            _VoidLunarTier = _assetBundle.LoadAsset<ItemTierDef>("VoidLunarTierDefRamen");
            _WaxWispBody = _assetBundle.LoadAsset<GameObject>("WaxWispBody");
            _WaxWispMaster = _assetBundle.LoadAsset<GameObject>("WaxWispMaster");
            _cscWaxWisp = _assetBundle.LoadAsset<CharacterSpawnCard>("cscWaxWisp");

            _ContaminationFont = _assetBundle.LoadAsset<GameObject>("ContaminationFontInteractable");
            _iscContaminationFont = _assetBundle.LoadAsset<InteractableSpawnCard>("iscContaminationFont");

            _stillnessBuff = _assetBundle.LoadAsset<BuffDef>("StillnessBuff");
            _hermitDebuff = _assetBundle.LoadAsset<BuffDef>("HermitDebuff");
            _hermitBuff = _assetBundle.LoadAsset<BuffDef>("HermitBuff");
            _parasiticClamBuff = _assetBundle.LoadAsset<BuffDef>("ParasiticClamBuff");

            _AllocentrismBomb = _assetBundle.LoadAsset<GameObject>("AllocentrismBomb");
            _AllocentrismGhost = _assetBundle.LoadAsset<GameObject>("AllocentrismBombProjectileGhost");
            _AllocentrismExplosion = new EffectDef(_assetBundle.LoadAsset<GameObject>("ExplosionAllocentrism"));

            _expansionDef = _assetBundle.LoadAsset<ExpansionDef>("RiskOfRamenExpansion");
        }

        private static void AddContent()
        {
            /*RiskOfRamenContentPack.itemDefs.Add(new ItemDef[] { _WaxIdol });
            RiskOfRamenContentPack.itemDefs.Add(new ItemDef[] { _ObsidianCard });
            RiskOfRamenContentPack.itemDefs.Add(new ItemDef[] { _DenkuRope });
            RiskOfRamenContentPack.itemDefs.Add(new ItemDef[] { _StainedBelt });
            RiskOfRamenContentPack.itemDefs.Add(new ItemDef[] { _WornTurnkey });

            RiskOfRamenContentPack.itemDefs.Add(new ItemDef[] { _GlassTiara });
            RiskOfRamenContentPack.itemDefs.Add(new ItemDef[] { _EsotericEremite });
            RiskOfRamenContentPack.itemDefs.Add(new ItemDef[] { _Allocentrism });*/
            TryAddItem(_WaxIdol);
            TryAddItem(_ObsidianCard);
            TryAddItem(_DenkuRope);
            TryAddItem(_StainedBelt);
            TryAddItem(_WornTurnkey);
            TryAddItem(_GlassTiara);
            TryAddItem(_EsotericEremite);
            TryAddItem(_ParasiticClam);
            TryAddItem(_ChitinousChisel);
            TryAddItem(_Allocentrism);

            RiskOfRamenContentPack.itemTierDefs.Add(new ItemTierDef[] { _VoidLunarTier });

            RiskOfRamenContentPack.bodyPrefabs.Add(new GameObject[] { _WaxWispBody });
            RiskOfRamenContentPack.masterPrefabs.Add(new GameObject[] { _WaxWispMaster });

            RiskOfRamenContentPack.buffDefs.Add(new BuffDef[] { _stillnessBuff });
            RiskOfRamenContentPack.buffDefs.Add(new BuffDef[] { _hermitDebuff });
            RiskOfRamenContentPack.buffDefs.Add(new BuffDef[] { _hermitBuff });
            RiskOfRamenContentPack.buffDefs.Add(new BuffDef[] { _parasiticClamBuff });
            RiskOfRamenContentPack.networkedObjectPrefabs.Add(new GameObject[]  { _ContaminationFont });

            RiskOfRamenContentPack.expansionDefs.Add(new ExpansionDef[] { _expansionDef });
            RiskOfRamenContentPack.effectDefs.Add(new EffectDef[] { _AllocentrismExplosion });


            if (RiskOfRamenConfig.enableItem(_Allocentrism).Value)
            {
                var AllocentrismVoid = CreateVoidPair(_Allocentrism, Addressables.LoadAssetAsync<ItemDef>("RoR2/DLC1/LunarSun/LunarSun.asset").WaitForCompletion());
                RiskOfRamenContentPack.itemRelationshipProviders.Add(new ItemRelationshipProvider[] { AllocentrismVoid });
            }
            if (RiskOfRamenConfig.enableItem(_GlassTiara).Value)
            {
                var GlassTiaraVoid = CreateVoidPair(_GlassTiara, Addressables.LoadAssetAsync<ItemDef>("RoR2/Base/GoldOnHit/GoldOnHit.asset").WaitForCompletion());

                RiskOfRamenContentPack.itemRelationshipProviders.Add(new ItemRelationshipProvider[] { GlassTiaraVoid });
            }
            if (RiskOfRamenConfig.enableItem(_EsotericEremite).Value)
            {
                var EsotericEremiteVoid = CreateVoidPair(_EsotericEremite, Addressables.LoadAssetAsync<ItemDef>("RoR2/DLC1/HalfAttackSpeedHalfCooldowns/HalfAttackSpeedHalfCooldowns.asset").WaitForCompletion());

                RiskOfRamenContentPack.itemRelationshipProviders.Add(new ItemRelationshipProvider[] { EsotericEremiteVoid });
            }
            if (RiskOfRamenConfig.enableItem(_ParasiticClam).Value)
            {
                var ParasiticClamVoid = CreateVoidPair(_ParasiticClam, Addressables.LoadAssetAsync<ItemDef>("RoR2/DLC1/HalfSpeedDoubleHealth/HalfSpeedDoubleHealth.asset").WaitForCompletion());

                RiskOfRamenContentPack.itemRelationshipProviders.Add(new ItemRelationshipProvider[] { ParasiticClamVoid });
            }
            if (RiskOfRamenConfig.enableItem(_ChitinousChisel).Value)
            {
                var ChitinousChiselVoid = CreateVoidPair(_ChitinousChisel, Addressables.LoadAssetAsync<ItemDef>("RoR2/Base/MonstersOnShrineUse/MonstersOnShrineUse.asset").WaitForCompletion());

                RiskOfRamenContentPack.itemRelationshipProviders.Add(new ItemRelationshipProvider[] { ChitinousChiselVoid });
            }
        }

        private static void TryAddItem(ItemDef item)
        {
            if (RiskOfRamenConfig.getConfig().Bind<bool>("Items", $"Enable {Language.GetString(item.nameToken)}", true, $"Whether or not {Language.GetString(item.nameToken)} should appear in-game.").Value)
            {
                RiskOfRamenContentPack.itemDefs.Add(new ItemDef[] { item });
            }
        }

        private static ItemRelationshipProvider CreateVoidPair(ItemDef corrupted, ItemDef normal)
        {
            var provider = ScriptableObject.CreateInstance<ItemRelationshipProvider>();

            provider.name = $"{corrupted.name}{normal.name}Relationship";
            provider.relationshipType = Addressables.LoadAssetAsync<ItemRelationshipType>("RoR2/DLC1/Common/ContagiousItem.asset").WaitForCompletion();
            
            provider.relationships = new ItemDef.Pair[] { 
                new ItemDef.Pair
                {
                    itemDef1 = normal,
                    itemDef2 = corrupted
                }
            };

            return provider;
        }

        private static ItemDef.Pair[] PopulateCorruptibleLunars()
        {
            return new ItemDef.Pair[] {
                new ItemDef.Pair
                {
                    itemDef1 = Addressables.LoadAssetAsync<ItemDef>("RoR2/DLC1/LunarSun/LunarSun.asset").WaitForCompletion(),
                    itemDef2 = _Allocentrism,
                },
                new ItemDef.Pair
                {
                    itemDef1 = Addressables.LoadAssetAsync<ItemDef>("RoR2/Base/GoldOnHit/GoldOnHit.asset").WaitForCompletion(),
                    itemDef2 = _GlassTiara,
                },
                new ItemDef.Pair
                {
                    itemDef1 = Addressables.LoadAssetAsync<ItemDef>("RoR2/DLC1/HalfAttackSpeedHalfCooldowns/HalfAttackSpeedHalfCooldowns.asset").WaitForCompletion(),
                    itemDef2 = _EsotericEremite,
                },
                new ItemDef.Pair
                {
                    itemDef1 = Addressables.LoadAssetAsync<ItemDef>("RoR2/Base/MonstersOnShrineUse/MonstersOnShrineUse.asset").WaitForCompletion(),
                    itemDef2 = _ChitinousChisel,
                },
                new ItemDef.Pair
                {
                    itemDef1 = Addressables.LoadAssetAsync<ItemDef>("RoR2/DLC1/HalfSpeedDoubleHealth/HalfSpeedDoubleHealth.asset").WaitForCompletion(),
                    itemDef2 = _ParasiticClam,
                },
            };
        }
        public static ItemDef TryGetPairForLunar(ItemDef lunar)
        {
            foreach (var pair in _corruptibleLunars)
            {
                if (pair.itemDef1 == lunar)
                {
                    return pair.itemDef2;
                }
            }
            return null;
        }

        private static void SwapAllShaders()
        {
            foreach (var material in _assetBundle.LoadAllAssets<Material>())
            {
                TrySwapShader(material);
            }
        }

        internal static void TrySwapShader(Material material)
        {
            var shaderName = material.shader.name;
            if (shaderName.Contains("Stubbed"))
            {
                shaderName = shaderName.Replace("Stubbed", string.Empty) + ".shader";
                var replacementShader = Addressables.LoadAssetAsync<Shader>(shaderName).WaitForCompletion();

                if (replacementShader != null)
                {
                    material.shader = replacementShader;
                } 
                else
                {
                    RiskOfRamenMain.LogError("Failed to load shader " + shaderName);
                }
            }
            else if (shaderName == "Standard")
            {
                var normalMap = material.GetTexture("_BumpMap");
                var normalStrength = material.GetFloat("_BumpScale");
                var emissionMap = material.GetTexture("_EmissionMap");

                material.shader = Resources.Load<Shader>("Shaders/Deferred/HGStandard");

                material.SetTexture("_NormalMap", normalMap);
                material.SetFloat("_NormalStrength", normalStrength);
                material.SetTexture("_EmTex", emissionMap);

                material.SetColor("_EmColor", new Color(0.2f, 0.2f, 0.2f));
                material.SetFloat("_EmPower", 0.15f);

            }
        }
        
        public IEnumerator GenerateContentPackAsync(GetContentPackAsyncArgs args)
        {
            ContentPack.Copy(RiskOfRamenContentPack, args.output);
            args.ReportProgress(1f);
            yield break;
        }
        public IEnumerator FinalizeAsync(FinalizeAsyncArgs args)
        {
            args.ReportProgress(1f);
            yield break;
        }
        private void AddSelf(ContentManager.AddContentPackProviderDelegate addContentPackProvider)
        {
            addContentPackProvider(this);
        }

        internal RiskOfRamenContent()
        {
            ContentManager.collectContentPackProviders += AddSelf;
        }
    }
}
