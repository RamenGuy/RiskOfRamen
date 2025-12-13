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

        public static GameObject _StainedBeltDisplay;
        public static GameObject _WaxWispBody;
        public static GameObject _WaxWispMaster;
        public static CharacterSpawnCard _cscWaxWisp;

        public static ItemDisplayRuleDict _idrsObsidianCard;
        

        public static AssetBundle _assetBundle;


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
            _StainedBeltDisplay = _assetBundle.LoadAsset<GameObject>("StainedBeltDisplay");

            _WaxWispBody = _assetBundle.LoadAsset<GameObject>("WaxWispBody");
            _WaxWispMaster = _assetBundle.LoadAsset<GameObject>("WaxWispMaster");
            _cscWaxWisp = _assetBundle.LoadAsset<CharacterSpawnCard>("cscWaxWisp");

            var expansionDef = _assetBundle.LoadAsset<ExpansionDef>("RiskOfRamenExpansion");
            RiskOfRamenContentPack.itemDefs.Add(new ItemDef[] { _WaxIdol });
            RiskOfRamenContentPack.itemDefs.Add(new ItemDef[] { _ObsidianCard }); 
            RiskOfRamenContentPack.itemDefs.Add(new ItemDef[] { _DenkuRope });
            RiskOfRamenContentPack.itemDefs.Add(new ItemDef[] { _StainedBelt });
            RiskOfRamenContentPack.bodyPrefabs.Add(new GameObject[] { _WaxWispBody });
            RiskOfRamenContentPack.masterPrefabs.Add(new GameObject[] { _WaxWispMaster });
            RiskOfRamenContentPack.expansionDefs.Add(new ExpansionDef[] { expansionDef });

            //ShaderUtil.SwapStubbedShadersAsync(_assetBundle);
            SwapAllShaders();
                 
            #region ITEM DISPLAY RULES

            #region DENKU ROPE
            var DenkuRopeDisplay = new ItemDisplayRuleDict();

            DenkuRopeDisplay.Add("mdlCommandoDualies", new ItemDisplayRule[]
            {
                    new ItemDisplayRule {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = _DenkuRope.pickupModelPrefab,
                        followerPrefabAddress = new UnityEngine.AddressableAssets.AssetReferenceGameObject(""),
                        limbMask = LimbFlags.None,
                        childName = "Stomach",
                        localPos = new Vector3(-0.01069F, 0.20945F, 0.10322F),
                        localAngles = new Vector3(7.32446F, 93.89349F, 34.56269F),
                        localScale = new Vector3(0.7F, 0.7F, 0.7F)

                    },
                   
            });

            DenkuRopeDisplay.Add("mdlHuntress", new ItemDisplayRule[]
            {
                    new ItemDisplayRule {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = _DenkuRope.pickupModelPrefab,
                        followerPrefabAddress = new UnityEngine.AddressableAssets.AssetReferenceGameObject(""),
                        limbMask = LimbFlags.None,
                        childName = "Pelvis",
                        localPos = new Vector3(-0.00065F, 0.02525F, -0.17033F),
                        localAngles = new Vector3(357.2977F, 241.9827F, 353.2495F),
                        localScale = new Vector3(0.8F, 0.8F, 0.8F)


                    },

            });

            DenkuRopeDisplay.Add("mdlBandit2", new ItemDisplayRule[]
            {
                    new ItemDisplayRule {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = _DenkuRope.pickupModelPrefab,
                        followerPrefabAddress = new UnityEngine.AddressableAssets.AssetReferenceGameObject(""),
                        limbMask = LimbFlags.None,
                        childName = "Head",
                        localPos = new Vector3(0.04059F, -0.07579F, 0.04173F),
                        localAngles = new Vector3(5.20473F, 311.6991F, 148.7727F),
                        localScale = new Vector3(0.3F, 0.3F, 0.3F)

                    },

            });

            DenkuRopeDisplay.Add("mdlToolbot", new ItemDisplayRule[]
            {
                    new ItemDisplayRule {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = _DenkuRope.pickupModelPrefab,
                        followerPrefabAddress = new UnityEngine.AddressableAssets.AssetReferenceGameObject(""),
                        limbMask = LimbFlags.None,
                        childName = "Base",
                        localPos = new Vector3(-0.23566F, 1.7088F, 3.49431F),
                        localAngles = new Vector3(1.66824F, 260.5212F, 260.5211F),
                        localScale = new Vector3(4F, 4F, 4F)

                    },

            });

            DenkuRopeDisplay.Add("mdlEngi", new ItemDisplayRule[]
            {
                    new ItemDisplayRule {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = _DenkuRope.pickupModelPrefab,
                        followerPrefabAddress = new UnityEngine.AddressableAssets.AssetReferenceGameObject(""),
                        limbMask = LimbFlags.None,
                        childName = "LowerArmL",
                        localPos = new Vector3(0.00505F, 0.06586F, 0.08095F),
                        localAngles = new Vector3(354.1721F, 286.9717F, 167.6142F),
                        localScale = new Vector3(0.4F, 0.4F, 0.4F)

                    },

            });

            DenkuRopeDisplay.Add("mdlMerc", new ItemDisplayRule[]
            {
                    new ItemDisplayRule {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = _DenkuRope.pickupModelPrefab,
                        followerPrefabAddress = new UnityEngine.AddressableAssets.AssetReferenceGameObject(""),
                        limbMask = LimbFlags.None,
                        childName = "Head",
                        localPos = new Vector3(0.00989F, 0.07221F, -0.01828F),
                        localAngles = new Vector3(76.23507F, 324.0986F, 224.7837F),
                        localScale = new Vector3(0.5F, 0.5F, 0.5F)

                    },

            });

            DenkuRopeDisplay.Add("mdlMage", new ItemDisplayRule[]
            {
                    new ItemDisplayRule {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = _DenkuRope.pickupModelPrefab,
                        followerPrefabAddress = new UnityEngine.AddressableAssets.AssetReferenceGameObject(""),
                        limbMask = LimbFlags.None,
                        childName = "Head",
                        localPos = new Vector3(-0.0076F, -0.04877F, -0.00465F),
                        localAngles = new Vector3(0F, 74.59808F, 0F),
                        localScale = new Vector3(0.3F, 0.3F, 0.3F)


                    },

            });

            DenkuRopeDisplay.Add("mdlTreebot", new ItemDisplayRule[]
            {
                    new ItemDisplayRule {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = _DenkuRope.pickupModelPrefab,
                        followerPrefabAddress = new UnityEngine.AddressableAssets.AssetReferenceGameObject(""),
                        limbMask = LimbFlags.None,
                        childName = "Base",
                        localPos = new Vector3(-0.22865F, 0.38963F, -0.78915F),
                        localAngles = new Vector3(320.8967F, 95.01687F, 82.54913F),
                        localScale = new Vector3(1F, 1F, 1F)
                    },

            });

            DenkuRopeDisplay.Add("mdlLoader", new ItemDisplayRule[]
            {
                    new ItemDisplayRule {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = _DenkuRope.pickupModelPrefab,
                        followerPrefabAddress = new UnityEngine.AddressableAssets.AssetReferenceGameObject(""),
                        limbMask = LimbFlags.None,
                        childName = "Neck",
                        localPos = new Vector3(-0.02591F, 0.03909F, -0.14636F),
                        localAngles = new Vector3(0F, 90F, 166.7403F),
                        localScale = new Vector3(0.6F, 0.6F, 0.6F)
                    },

            });

            DenkuRopeDisplay.Add("mdlCroco", new ItemDisplayRule[]
            {
                    new ItemDisplayRule {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = _DenkuRope.pickupModelPrefab,
                        followerPrefabAddress = new UnityEngine.AddressableAssets.AssetReferenceGameObject(""),
                        limbMask = LimbFlags.None,
                        childName = "Head",
                        localPos = new Vector3(-0.30599F, 1.35453F, -1.89603F),
                        localAngles = new Vector3(6.1305F, 244.7297F, 327.0264F),
                        localScale = new Vector3(7F, 7F, 7F)

                    },

            });

            DenkuRopeDisplay.Add("mdlCaptain", new ItemDisplayRule[]
            {
                    new ItemDisplayRule {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = _DenkuRope.pickupModelPrefab,
                        followerPrefabAddress = new UnityEngine.AddressableAssets.AssetReferenceGameObject(""),
                        limbMask = LimbFlags.None,
                        childName = "Stomach",
                        localPos = new Vector3(0.06066F, 0.18265F, 0.00761F),
                        localAngles = new Vector3(7.79403F, 38.64711F, 162.7407F),
                        localScale = new Vector3(0.6F, 0.6F, 0.6F)

                    },

            });

            DenkuRopeDisplay.Add("mdlRailGunner", new ItemDisplayRule[]
            {
                    new ItemDisplayRule {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = _DenkuRope.pickupModelPrefab,
                        followerPrefabAddress = new UnityEngine.AddressableAssets.AssetReferenceGameObject(""),
                        limbMask = LimbFlags.None,
                        childName = "Base",
                        localPos = new Vector3(0.06066F, 0.04395F, 0.06448F),
                        localAngles = new Vector3(288.186F, 98.37619F, 79.05092F),
                        localScale = new Vector3(0.6F, 0.6F, 0.6F)

                    },

            });

            DenkuRopeDisplay.Add("mdlVoidSurvivor", new ItemDisplayRule[]
            {
                    new ItemDisplayRule {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = _DenkuRope.pickupModelPrefab,
                        followerPrefabAddress = new UnityEngine.AddressableAssets.AssetReferenceGameObject(""),
                        limbMask = LimbFlags.None,
                        childName = "MuzzleMegaBlaster",
                        localPos = new Vector3(-0.02028F, -0.00406F, -0.05083F),
                        localAngles = new Vector3(298.6656F, 34.01602F, 207.773F),
                        localScale = new Vector3(0.5F, 0.5F, 0.5F)


                    },

            });


            //ItemAPI.Add(new CustomItem(_DenkuRope, DenkuRopeDisplay));
            #endregion
            #region OBSIDIAN CARD
            var ObsidianCardDisplay = new ItemDisplayRuleDict();

            ObsidianCardDisplay.Add("mdlCommandoDualies", new ItemDisplayRule[]
            {
                    new ItemDisplayRule {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = _ObsidianCard.pickupModelPrefab,
                        followerPrefabAddress = new UnityEngine.AddressableAssets.AssetReferenceGameObject(""),
                        limbMask = LimbFlags.None,
                        childName = "CalfR",
                        localPos = new Vector3(-0.08452F, 0.2241F, 0.02499F),
                        localAngles = new Vector3(338.3474F, 98.51011F, 0.91329F),
                        localScale = new Vector3(0.3F, 0.3F, 0.3F)

                    },

            });

            ObsidianCardDisplay.Add("mdlHuntress", new ItemDisplayRule[]
            {
                    new ItemDisplayRule {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = _ObsidianCard.pickupModelPrefab,
                        followerPrefabAddress = new UnityEngine.AddressableAssets.AssetReferenceGameObject(""),
                        limbMask = LimbFlags.None,
                        childName = "UpperArmL",
                        localPos = new Vector3(-0.08723F, 0.09964F, -0.01873F),
                        localAngles = new Vector3(27.84832F, 281.4216F, 174.5701F),
                        localScale = new Vector3(0.3F, 0.3F, 0.3F)

                    },

            });

            ObsidianCardDisplay.Add("mdlBandit2", new ItemDisplayRule[]
            {
                    new ItemDisplayRule {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = _ObsidianCard.pickupModelPrefab,
                        followerPrefabAddress = new UnityEngine.AddressableAssets.AssetReferenceGameObject(""),
                        limbMask = LimbFlags.None,
                        childName = "Stomach",
                        localPos = new Vector3(-0.05635F, 0.04164F, 0.16436F),
                        localAngles = new Vector3(28.10276F, 156.4939F, 180.7979F),
                        localScale = new Vector3(0.3F, 0.3F, 0.3F)

                    },

            });

            ObsidianCardDisplay.Add("mdlToolbot", new ItemDisplayRule[]
            {
                    new ItemDisplayRule {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = _ObsidianCard.pickupModelPrefab,
                        followerPrefabAddress = new UnityEngine.AddressableAssets.AssetReferenceGameObject(""),
                        limbMask = LimbFlags.None,
                        childName = "UpperArmL",
                        localPos = new Vector3(-0.02233F, 1.97931F, -0.99985F),
                        localAngles = new Vector3(2.15264F, 5.1404F, 176.7258F),
                        localScale = new Vector3(3.5F, 3.5F, 3.5F)

                    },

            });

            ObsidianCardDisplay.Add("mdlEngi", new ItemDisplayRule[]
            {
                    new ItemDisplayRule {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = _ObsidianCard.pickupModelPrefab,
                        followerPrefabAddress = new UnityEngine.AddressableAssets.AssetReferenceGameObject(""),
                        limbMask = LimbFlags.None,
                        childName = "LowerArmL",
                        localPos = new Vector3(0.01394F, 0.13063F, -0.09528F),
                        localAngles = new Vector3(21.59121F, 341.9269F, 182.462F),
                        localScale = new Vector3(0.3F, 0.3F, 0.3F)

                    },

            });

            ObsidianCardDisplay.Add("mdlMerc", new ItemDisplayRule[]
            {
                    new ItemDisplayRule {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = _ObsidianCard.pickupModelPrefab,
                        followerPrefabAddress = new UnityEngine.AddressableAssets.AssetReferenceGameObject(""),
                        limbMask = LimbFlags.None,
                        childName = "ThighR",
                        localPos = new Vector3(-0.13783F, 0.10082F, 0.02668F),
                        localAngles = new Vector3(20.26766F, 267.0182F, 183.088F),
                        localScale = new Vector3(0.3F, 0.3F, 0.3F)

                    },

            });

            ObsidianCardDisplay.Add("mdlMage", new ItemDisplayRule[]
            {
                    new ItemDisplayRule {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = _ObsidianCard.pickupModelPrefab,
                        followerPrefabAddress = new UnityEngine.AddressableAssets.AssetReferenceGameObject(""),
                        limbMask = LimbFlags.None,
                        childName = "UpperArmL",
                        localPos = new Vector3(0.05757F, 0.15938F, -0.00618F),
                        localAngles = new Vector3(27.84833F, 281.4216F, 174.5701F),
                        localScale = new Vector3(0.3F, 0.3F, 0.3F)

                    },

            });

            ObsidianCardDisplay.Add("mdlTreebot", new ItemDisplayRule[]
            {
                    new ItemDisplayRule {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = _ObsidianCard.pickupModelPrefab,
                        followerPrefabAddress = new UnityEngine.AddressableAssets.AssetReferenceGameObject(""),
                        limbMask = LimbFlags.None,
                        childName = "FlowerBase",
                        localPos = new Vector3(-0.26653F, 0.74262F, 0.66245F),
                        localAngles = new Vector3(25.29461F, 341.2172F, 176.4987F),
                        localScale = new Vector3(1F, 1F, 1F)
                    },

            });

            ObsidianCardDisplay.Add("mdlLoader", new ItemDisplayRule[]
            {
                    new ItemDisplayRule {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = _ObsidianCard.pickupModelPrefab,
                        followerPrefabAddress = new UnityEngine.AddressableAssets.AssetReferenceGameObject(""),
                        limbMask = LimbFlags.None,
                        childName = "MechHandL",
                        localPos = new Vector3(-0.09678F, 0.0888F, 0.06277F),
                        localAngles = new Vector3(28.01476F, 302.2359F, 174.3781F),
                        localScale = new Vector3(0.3F, 0.3F, 0.3F)
                    },

            });

            ObsidianCardDisplay.Add("mdlCroco", new ItemDisplayRule[]
            {
                    new ItemDisplayRule {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = _ObsidianCard.pickupModelPrefab,
                        followerPrefabAddress = new UnityEngine.AddressableAssets.AssetReferenceGameObject(""),
                        limbMask = LimbFlags.None,
                        childName = "MuzzleHandL",
                        localPos = new Vector3(-0.79111F, 0.41593F, -1.39771F),
                        localAngles = new Vector3(45.29366F, 346.007F, 95.27855F),
                        localScale = new Vector3(2F, 2F, 2F)

                    },
            });

            ObsidianCardDisplay.Add("mdlCaptain", new ItemDisplayRule[]
            {
                    new ItemDisplayRule {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = _ObsidianCard.pickupModelPrefab,
                        followerPrefabAddress = new UnityEngine.AddressableAssets.AssetReferenceGameObject(""),
                        limbMask = LimbFlags.None,
                        childName = "CalfR",
                        localPos = new Vector3(0.00546F, -0.00302F, -0.06705F),
                        localAngles = new Vector3(12.07266F, 348.8349F, 189.9568F),
                        localScale = new Vector3(0.2F, 0.2F, 0.2F)

                    },

            });

            ObsidianCardDisplay.Add("mdlRailGunner", new ItemDisplayRule[]
            {
                    new ItemDisplayRule {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = _ObsidianCard.pickupModelPrefab,
                        followerPrefabAddress = new UnityEngine.AddressableAssets.AssetReferenceGameObject(""),
                        limbMask = LimbFlags.None,
                        childName = "TopRail",
                        localPos = new Vector3(0.00546F, 0.71316F, 0.04062F),
                        localAngles = new Vector3(345.0563F, 6.60493F, 355.4799F),
                        localScale = new Vector3(0.2F, 0.2F, 0.2F)


                    },

            });

            ObsidianCardDisplay.Add("mdlVoidSurvivor", new ItemDisplayRule[]
            {
                    new ItemDisplayRule {
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = _ObsidianCard.pickupModelPrefab,
                        followerPrefabAddress = new UnityEngine.AddressableAssets.AssetReferenceGameObject(""),
                        limbMask = LimbFlags.None,
                        childName = "Chest",
                        localPos = new Vector3(-0.14121F, 0.06264F, 0.18703F),
                        localAngles = new Vector3(21.73587F, 330.568F, 202.2781F),
                        localScale = new Vector3(0.2F, 0.2F, 0.2F)
                    },

            });


            ItemAPI.Add(new CustomItem(_ObsidianCard, ObsidianCardDisplay));
            #endregion

            #endregion
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
