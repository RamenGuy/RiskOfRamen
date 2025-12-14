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

            SwapAllShaders();
                 
        
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
