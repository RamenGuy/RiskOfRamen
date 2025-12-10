using BepInEx;
using System.IO;
using UnityEngine;
using RoR2;
using R2API;
using RoR2.Items;

namespace RiskOfRamen
{

    public class WaxIdolBodyBehavior : BaseItemBodyBehavior
    {
        private static readonly float timeBetweenWispResummons = 30f;

        private static readonly float timeBetweenWispRetryResummons = 1f;

        private float wispResummonCooldown;

        [ItemDefAssociation(useOnServer = true, useOnClient = false)]
        private static ItemDef GetItemDef()
        {
            return RiskOfRamenContent._WaxIdol;
        }

        private void Start()
        {
            wispResummonCooldown = timeBetweenWispRetryResummons;
        }

        private void FixedUpdate()
        {
            int num = stack;
            CharacterMaster bodyMaster = base.body.master;
            if (!bodyMaster)
            {
                return;
            }
            int deployableCount = bodyMaster.GetDeployableCount(DeployableSlot.EngiTurret);
            //int deployableCount = 2;
            if (deployableCount >= num)
            {
                return;
            }
            wispResummonCooldown -= Time.fixedDeltaTime;
            if (wispResummonCooldown <= 0f)
            {
                DirectorSpawnRequest directorSpawnRequest = new DirectorSpawnRequest(RiskOfRamenContent._cscWaxWisp, new DirectorPlacementRule
                {
                    placementMode = DirectorPlacementRule.PlacementMode.Approximate,
                    minDistance = 3f,
                    maxDistance = 40f,
                    spawnOnTarget = base.transform
                }, RoR2Application.rng);
                directorSpawnRequest.summonerBodyObject = base.gameObject;
                directorSpawnRequest.onSpawnedServer = OnWispSpawned;
                DirectorCore.instance.TrySpawnObject(directorSpawnRequest);
                if (deployableCount < num)
                {
                    wispResummonCooldown = timeBetweenWispRetryResummons;
                }
                else
                {
                    wispResummonCooldown = timeBetweenWispResummons;
                }
            }
            void OnWispSpawned(SpawnCard.SpawnResult spawnResult)
            {
                GameObject spawnedInstance = spawnResult.spawnedInstance;
                if ((bool)spawnedInstance)
                {
                    CharacterMaster component = spawnedInstance.GetComponent<CharacterMaster>();
                    if ((bool)component)
                    {
                        component.inventory.GiveItemPermanent(RoR2Content.Items.BoostDamage, 30);
                        component.inventory.GiveItemPermanent(RoR2Content.Items.BoostHp, 10);
                        Deployable component2 = component.GetComponent<Deployable>();
                        if ((bool)component2)
                        {
                            bodyMaster.AddDeployable(component2, DeployableSlot.EngiTurret);
                        }
                    }
                }
            }
        }
    }
}