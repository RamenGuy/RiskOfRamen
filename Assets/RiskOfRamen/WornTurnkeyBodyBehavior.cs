using BepInEx;
using System.IO;
using UnityEngine;
using RoR2;
using R2API;
using RoR2.Items;
using RoR2BepInExPack.GameAssetPaths;

namespace RiskOfRamen
{

    public class WornTurnkeyBodyBehavior : BaseItemBodyBehavior
    {
        private static readonly int secondsOfStillness = 1;



        [ItemDefAssociation(useOnServer = true, useOnClient = false)]
        private static ItemDef GetItemDef()
        {
            return RiskOfRamenContent._WornTurnkey;
        }

        private void Start()
        {
            //wispResummonCooldown = timeBetweenWispRetryResummons;
        }

        private void FixedUpdate()
        {
            int num = stack;
            if (body.HasBuff(RiskOfRamenContent._stillnessBuff)) { return; }
            if (body.GetNotMoving())
            {
                TeamComponent[] array = FindObjectsOfType<TeamComponent>(); 
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i].teamIndex == body.teamComponent.teamIndex)
                    {
                        //array[i].GetComponent<CharacterBody>().SetBuffCount(RiskOfRamenContent._stillnessBuff.buffIndex, num);
                        array[i].GetComponent<CharacterBody>().AddTimedBuff(RiskOfRamenContent._stillnessBuff, 3f, num);
                    }
                }
            }
        }
    }
}