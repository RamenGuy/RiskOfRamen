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

    public class GlassTiaraBodyBehavior : BaseItemBodyBehavior
    {
        uint prevGold = 0;
        public float curseAdd = 0;

        [ItemDefAssociation(useOnServer = true, useOnClient = false)]
        private static ItemDef GetItemDef()
        {
            if (RiskOfRamenContent._GlassTiara == null) { return null; }
            return RiskOfRamenContent._GlassTiara;
        }

        private void Start()
        {
            prevGold = body.master.money;
        }

        private void FixedUpdate()
        {
            int num = stack;
            
            if (curseAdd < 0) { curseAdd = 0; }

            if (body.master.money < prevGold)
            {
                OnGoldSpent(prevGold, body.master.money);
            }
            if (body.master.money > prevGold) 
            {
                OnGoldEarned(prevGold, body.master.money);   
            }


            prevGold = body.master.money;
            

        }

        private void OnGoldSpent(uint oldMoney, uint newMoney)
        {


            curseAdd += (oldMoney - newMoney) * (0.15f + .05f * stack);
            //curseAdd += (oldMoney - newMoney);
            //RiskOfRamenMain.LogDebug($"OLD: {oldMoney} | NEW: {newMoney} | CURSEADD: {curseAdd}");
        }
        private void OnGoldEarned(uint oldMoney, uint newMoney)
        {


            curseAdd -= (newMoney - oldMoney) * (.1f * stack);
            //curseAdd -= (newMoney - oldMoney);
            //RiskOfRamenMain.LogDebug($"OLD: {oldMoney} | NEW: {newMoney} | CURSEADD: {curseAdd}");
        }
    }
}