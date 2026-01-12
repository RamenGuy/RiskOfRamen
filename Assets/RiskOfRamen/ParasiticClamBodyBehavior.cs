using BepInEx;
using System.IO;
using UnityEngine;
using RoR2;
using R2API;
using RoR2.Items;

namespace RiskOfRamen
{

    public class ParasiticClamBodyBehavior : BaseItemBodyBehavior, IOnTakeDamageServerReceiver
    {

        [ItemDefAssociation(useOnServer = true, useOnClient = false)]
        private static ItemDef GetItemDef()
        {   
            if (RiskOfRamenContent._ParasiticClam == null) { return null; }
            return RiskOfRamenContent._ParasiticClam;
        }

        public void OnTakeDamageServer(DamageReport damageReport)
        {
            if (damageReport.attackerBody != null)
            {
                damageReport.victimBody.AddTimedBuff(RiskOfRamenContent._parasiticClamBuff, 10, 10);
            }
        }
    }
}