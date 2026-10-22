using BepInEx;
using System.IO;
using UnityEngine;
using RoR2;
using R2API;
using RoR2.Items;

namespace RiskOfRamen
{

    public class WarpedOpticalBodyBehavior : BaseItemBodyBehavior, IOnDamageDealtServerReceiver
    {

        [ItemDefAssociation(useOnServer = true, useOnClient = false)]
        private static ItemDef GetItemDef()
        {
            if (RiskOfRamenConfig.enableItem(RiskOfRamenContent._WarpedOptical).Value == false) { return null; }
            return RiskOfRamenContent._WarpedOptical;
        }

        public void OnDamageDealtServer(DamageReport damageReport)
        {
            
        }
    }
}