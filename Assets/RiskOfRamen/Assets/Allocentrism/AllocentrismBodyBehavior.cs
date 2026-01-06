using BepInEx;
using System.IO;
using UnityEngine;
using RoR2;
using R2API;
using RoR2.Items;
using RoR2BepInExPack.GameAssetPaths;
using UnityEngine.Networking;
using RoR2.Projectile;
using System;
using System.Linq;

namespace RiskOfRamen.Assets.Allocentrism
{
    public class AllocentrismBodyBehavior : BaseItemBodyBehavior, IOnDamageDealtServerReceiver
    {
        private const float secondsPerTransform = 60f;

        private const float secondsPerProjectile = 3f;

        private const int baseMaxProjectiles = 2;

        private const int maxProjectilesPerStack = 1;

        private const float baseOrbitDegreesPerSecond = 180f;

        private const float orbitDegreesPerSecondFalloff = 0.9f;

        private const float baseOrbitRadius = 2f;

        private const float orbitRadiusPerStack = 0.25f;

        private const float maxInclinationDegrees = 0f;

        private const float baseDamageCoefficient = 3.6f;


        private float projectileTimer;

        private float transformTimer;

        private GameObject projectilePrefab;

        private Xoroshiro128Plus transformRng;

        private static readonly Inventory.TryTransformRandomItemArgs.FilterDelegate transformationFilter = TransformationFilter;

        public event Action<AllocentrismBodyBehavior> onDisabled;

        [ItemDefAssociation(useOnServer = true, useOnClient = false)]
        private static ItemDef GetItemDef()
        {
            return RiskOfRamenContent._Allocentrism;
        }

        public static int GetMaxProjectiles(Inventory inventory)
        {
            return 4 + (1 * inventory.GetItemCountEffective(RiskOfRamenContent._Allocentrism));
        }


        public void InitializeOrbiter(ProjectileTargetOrbiter orbiter, AllocentrismBombController controller, Transform target)
        {
            float num = base.body.radius + 2f + UnityEngine.Random.Range(0.25f, 0.25f * stack);
            float num2 = num / 2f;
            num2 *= num2;
            float degreesPerSecond = 180f * Mathf.Pow(0.9f, num2);
            Quaternion quaternion = Quaternion.AngleAxis(UnityEngine.Random.Range(0f, 360f), Vector3.up);
            Quaternion quaternion2 = Quaternion.AngleAxis(UnityEngine.Random.Range(0f, 0f), Vector3.forward);
            Vector3 planeNormal = quaternion * quaternion2 * Vector3.up;
            float initialDegreesFromOwnerForward = UnityEngine.Random.Range(0f, 360f);
            orbiter.Initialize(target, planeNormal, num, degreesPerSecond, initialDegreesFromOwnerForward);
            onDisabled += DestroyOrbiter;
            void DestroyOrbiter(AllocentrismBodyBehavior allocentrismBodyBehavior)
            {
                if (controller)
                {
                    controller.Detonate();
                }
            }
        }

        private void Start()
        {
            enabled = true;
            projectilePrefab = RiskOfRamenContent._AllocentrismBomb;

            ulong seed = Run.instance.seed ^ (ulong)Run.instance.stageClearCount;
            transformRng = new Xoroshiro128Plus(seed);
        }
        private void OnDisable() 
        {
            onDisabled?.Invoke(this);
            onDisabled = null;
        }

        private void FixedUpdate()
        {
            //projectileTimer += Time.fixedDeltaTime;
            CharacterMaster bodyMaster = base.body.master;
            if (!bodyMaster)
            {
                return;
            }
            if (!bodyMaster.IsDeployableLimited(RiskOfRamenContent.AllocentrismBomb))
            {
                /*if (projectileTimer > 3f / stack)
                {
                    projectileTimer = 0f;
                    FireProjectileInfo fireProjectileInfo = new FireProjectileInfo
                    {
                        projectilePrefab = projectilePrefab,
                        crit = base.body.RollCrit(),
                        damage = base.body.damage * 3.6f,
                        damageColorIndex = DamageColorIndex.Item,
                        force = 0f,
                        owner = gameObject,
                        position = base.transform.position,
                        rotation = Quaternion.identity,

                    };
                    ProjectileManager.instance.FireProjectile(fireProjectileInfo);
                    RiskOfRamenMain.LogDebug("Projectile fired!");
                }*/
            }
            transformTimer += Time.fixedDeltaTime;
            if (transformTimer > 60f)
            {
                transformTimer = 0f;
                TransformItem();
            }
        }

        private void TransformItem()
        {
            if (base.body.inventory && base.body.master && base.body.inventory.TryTransformRandomItem(new Inventory.TryTransformRandomItemArgs
            {
                filter = AllocentrismBodyBehavior.transformationFilter,
                rng = transformRng
            }, out var result))
            {
                CharacterMasterNotificationQueue.SendTransformNotification(base.body.master, result.originalItemIndex, result.newItemIndex, CharacterMasterNotificationQueue.TransformationType.LunarSun);
            }
        }

        private static ItemIndex TransformationFilter(Inventory.TryTransformRandomItemArgs.FilterArgs args)
        {
            if (args.itemIndex != RiskOfRamenContent._Allocentrism.itemIndex)
            {
                ItemDef itemDef = ItemCatalog.GetItemDef(args.itemIndex);
                if (itemDef.tier != ItemTier.NoTier && itemDef.canRemove && !RamenUtils.IsAnyVoidTier(itemDef))
                {
                    return RiskOfRamenContent._Allocentrism.itemIndex;
                }
            }
            return ItemIndex.None;
        }

       

        void IOnDamageDealtServerReceiver.OnDamageDealtServer(DamageReport damageReport)
        {
            if (!damageReport.victimBody) { return; }
            if (!damageReport.attackerMaster.IsDeployableLimited(RiskOfRamenContent.AllocentrismBomb))
            {
                //projectilePrefab.GetComponent<AllocentrismBombController>().target = damageReport.victim;
                FireProjectileInfo fireProjectileInfo = new FireProjectileInfo
                {
                    projectilePrefab = projectilePrefab,
                    crit = base.body.RollCrit(),
                    damage = base.body.damage * 3.6f,
                    damageColorIndex = DamageColorIndex.Item,
                    force = 0f,
                    owner = gameObject,
                    position = damageReport.victimBody.corePosition + (damageReport.victimBody.corePosition - damageReport.victimBody.footPosition),
                    rotation = Quaternion.identity,
                    target = damageReport.victim.gameObject,

                };
                GameObject projectile = ProjectileManager.instance.FireProjectileImmediateServer(fireProjectileInfo);
                ProjectileTargetOrbiter orbiter = projectile.GetComponent<ProjectileTargetOrbiter>();
                AllocentrismBombController controller = projectile.GetComponent<AllocentrismBombController>();
                Transform target = projectile.GetComponent<ProjectileTargetComponent>().target;
                InitializeOrbiter(orbiter, controller, target);
                RiskOfRamenMain.LogDebug($"Allocentrism bomb fired at {damageReport.victim.name}");
            }
        }
    }

}