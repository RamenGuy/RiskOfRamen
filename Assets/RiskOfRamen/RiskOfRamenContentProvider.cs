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
        public static ItemTierDef _VoidLunarTier;

        public static GameObject _ContaminationFont;
        public static InteractableSpawnCard _iscContaminationFont;

        public static GameObject _WaxWispBody;
        public static GameObject _WaxWispMaster;
        public static CharacterSpawnCard _cscWaxWisp;

        public static BuffDef _stillnessBuff;

        public static GameObject _AllocentrismBomb;
        public static GameObject _AllocentrismGhost;
        public static EffectDef _AllocentrismExplosion;

        public static DeployableSlot AllocentrismBomb;

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

            _WaxIdol = _assetBundle.LoadAsset<ItemDef>("WaxIdol");
            _ObsidianCard = _assetBundle.LoadAsset<ItemDef>("ObsidianCard");
            _DenkuRope = _assetBundle.LoadAsset<ItemDef>("DenkuRope");
            _StainedBelt = _assetBundle.LoadAsset<ItemDef>("StainedBelt");
            _WornTurnkey = _assetBundle.LoadAsset<ItemDef>("WornTurnkey");
            _GlassTiara = _assetBundle.LoadAsset<ItemDef>("GlassTiara");
            _Allocentrism = _assetBundle.LoadAsset<ItemDef>("Allocentrism");

            _VoidLunarTier = _assetBundle.LoadAsset<ItemTierDef>("VoidLunarTierDef");

            _WaxWispBody = _assetBundle.LoadAsset<GameObject>("WaxWispBody");
            _WaxWispMaster = _assetBundle.LoadAsset<GameObject>("WaxWispMaster");
            _cscWaxWisp = _assetBundle.LoadAsset<CharacterSpawnCard>("cscWaxWisp");

            _ContaminationFont = _assetBundle.LoadAsset<GameObject>("ContaminationFontInteractable");
            _iscContaminationFont = _assetBundle.LoadAsset<InteractableSpawnCard>("iscContaminationFont");

            _stillnessBuff = _assetBundle.LoadAsset<BuffDef>("StillnessBuff");

            _AllocentrismBomb = _assetBundle.LoadAsset<GameObject>("AllocentrismBombProjectile");
            _AllocentrismGhost = _assetBundle.LoadAsset<GameObject>("AllocentrismBombProjectileGhost");
            _AllocentrismExplosion = new EffectDef(_assetBundle.LoadAsset<GameObject>("ExplosionAllocentrism"));

            var expansionDef = _assetBundle.LoadAsset<ExpansionDef>("RiskOfRamenExpansion");


            RiskOfRamenContentPack.itemDefs.Add(new ItemDef[] { _WaxIdol });
            RiskOfRamenContentPack.itemDefs.Add(new ItemDef[] { _ObsidianCard }); 
            RiskOfRamenContentPack.itemDefs.Add(new ItemDef[] { _DenkuRope });
            RiskOfRamenContentPack.itemDefs.Add(new ItemDef[] { _StainedBelt });
            RiskOfRamenContentPack.itemDefs.Add(new ItemDef[] { _WornTurnkey });

            RiskOfRamenContentPack.itemDefs.Add(new ItemDef[] { _GlassTiara });

            RiskOfRamenContentPack.itemDefs.Add(new ItemDef[] { _Allocentrism });
            RiskOfRamenContentPack.itemTierDefs.Add(new ItemTierDef[] { _VoidLunarTier }); 

            RiskOfRamenContentPack.bodyPrefabs.Add(new GameObject[] { _WaxWispBody });
            RiskOfRamenContentPack.masterPrefabs.Add(new GameObject[] { _WaxWispMaster });

            RiskOfRamenContentPack.buffDefs.Add(new BuffDef[] { _stillnessBuff });
            RiskOfRamenContentPack.networkedObjectPrefabs.Add(new GameObject[] { _ContaminationFont });
            
            RiskOfRamenContentPack.expansionDefs.Add(new ExpansionDef[] { expansionDef });
            RiskOfRamenContentPack.effectDefs.Add(new EffectDef[] { _AllocentrismExplosion });

            SwapAllShaders();

            var AllocentrismVoid = CreateVoidPair(_Allocentrism, Addressables.LoadAssetAsync<ItemDef>("RoR2/DLC1/LunarSun/LunarSun.asset").WaitForCompletion());
            
            RiskOfRamenContentPack.itemRelationshipProviders.Add(new ItemRelationshipProvider[] { AllocentrismVoid });

            var GlassTiaraVoid = CreateVoidPair(_GlassTiara, Addressables.LoadAssetAsync<ItemDef>("RoR2/Base/GoldOnHit/GoldOnHit.asset").WaitForCompletion());

            RiskOfRamenContentPack.itemRelationshipProviders.Add(new ItemRelationshipProvider[] { GlassTiaraVoid });

            AllocentrismBomb = DeployableAPI.RegisterDeployableSlot((self, deployableCountMultiplier) =>
            {
                if (self)
                {
                    return AllocentrismBodyBehavior.GetMaxProjectiles(self.inventory);
                }
                return 1;
            });

            _corruptibleLunars = PopulateCorruptibleLunars();
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
                }
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
                    RiskOfRamenMain.LogInfo("Swapped shader " + material.shader.name + " with " + shaderName);
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
