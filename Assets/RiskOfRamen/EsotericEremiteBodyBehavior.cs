using BepInEx;
using System.IO;
using UnityEngine;
using RoR2;
using R2API;
using RoR2.Items;
using RoR2BepInExPack.GameAssetPaths;
using System;

namespace RiskOfRamen
{

    public class EsotericEremiteBodyBehavior : BaseItemBodyBehavior
    {
        uint prevGold = 0;
        public float curseAdd = 0;

        [ItemDefAssociation(useOnServer = true, useOnClient = false)]
        private static ItemDef GetItemDef()
        {
            if (RiskOfRamenContent._EsotericEremite == null) { return null; }
            return RiskOfRamenContent._EsotericEremite;
        }

        private void Start()
        {
            prevGold = body.master.money;
        }

        private void FixedUpdate()
        {
            int num = stack;
        }
    }
}