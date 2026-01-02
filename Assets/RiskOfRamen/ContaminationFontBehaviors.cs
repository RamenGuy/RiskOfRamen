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
using UnityEngine.Networking;
//using MSU;

namespace RiskOfRamen
{
    public class ContaminationFontBehaviors : NetworkBehaviour
    {
        public PurchaseInteraction purchaseInteraction;
        private GameObject shrineUseEffect;


        public void Start()
        {
            shrineUseEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Common/VFX/ShrineUseEffect.prefab").WaitForCompletion();
            if (NetworkServer.active && Run.instance)
            {
                purchaseInteraction.SetAvailable(true);
            }
            purchaseInteraction.costType = RiskOfRamenMain.corruptibleLunarIndex;
            purchaseInteraction.onDetailedPurchaseServer.AddListener(OnDetailedPurchaseServer);
        }

        [Server]
        public void OnDetailedPurchaseServer(CostTypeDef.PayCostContext context, CostTypeDef.PayCostResults results)
        {
            if (!NetworkServer.active)
            {
                RiskOfRamenMain.LogInfo("[Server] function 'ContaminationFontBehaivors::OnPurchase(RoR2.Interactor)' called on client");
                return;
            }


            EffectManager.SpawnEffect(shrineUseEffect, new EffectData()
            {
                origin = gameObject.transform.position,
                rotation = Quaternion.identity,
                scale = 3f,
                color = Color.magenta
            }, true);
            ItemDef inItem = ItemCatalog.GetItemDef(results.itemStacksTaken[0].itemIndex);
            ItemDef outItem = RiskOfRamenContent.TryGetPairForLunar(inItem);
            ItemTierDef ItemTierDef1 = ItemTierCatalog.GetItemTierDef(inItem.tier);
            ItemTierDef ItemTierDef2 = ItemTierCatalog.GetItemTierDef(outItem.tier);
            string TierColor1 = ColorCatalog.GetColorHexString(ItemTierDef1.colorIndex);
            string TierColor2 = ColorCatalog.GetColorHexString(ItemTierDef2.colorIndex);
            Chat.SendBroadcastChat(new Chat.SubjectFormatChatMessage()
            {
                paramTokens = new string[] { 
                    Util.GetBestMasterName(context.activatorMaster),
                    TierColor1,
                    Language.GetString(inItem.nameToken),
                    TierColor2,
                    Language.GetString(outItem.nameToken),
                },
                baseToken = "{1} corrupted <color=#{2}>{3}</color> into <color=#{4}>{5}</color>"
            }, 0);
                
        }   

        public ItemDef tryCorruptInputLunar(ItemDef def)
        {
            return def;
        }
    }
}