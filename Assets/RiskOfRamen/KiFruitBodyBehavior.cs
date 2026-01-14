using BepInEx;
using System.IO;
using UnityEngine;
using RoR2;
using R2API;
using RoR2.Items;
using RoR2BepInExPack.GameAssetPaths;
using System;
using UnityEngine.Networking;

namespace RiskOfRamen
{

    public class KiFruitBodyBehavior : BaseItemBodyBehavior, IOnIncomingDamageServerReceiver
    {

        public int armorReduction = 0;

        [ItemDefAssociation(useOnServer = true, useOnClient = false)]
        private static ItemDef GetItemDef()
        {
            if (RiskOfRamenConfig.enableItem(RiskOfRamenContent._KiFruit).Value == false) { return null; }
            return RiskOfRamenContent._KiFruit;
        }

        private void FixedUpdate()
        {
            if (!NetworkServer.active) { return; }

            if (base.body.HasBuff(RoR2Content.Buffs.Cripple))
            {
                base.body.RemoveBuff(RoR2Content.Buffs.Cripple);
            }
        }


        public void OnIncomingDamageServer(DamageInfo damageInfo)
        {
            if (damageInfo.damage > (base.body.healthComponent.health * .1f))
            {
                armorReduction += 1;
            }
        }
    }
}