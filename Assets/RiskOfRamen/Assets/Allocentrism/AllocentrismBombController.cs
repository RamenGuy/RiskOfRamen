using BepInEx;
using System.IO;
using UnityEngine;
using RoR2;
using R2API;
using RoR2.Items;
using RoR2BepInExPack.GameAssetPaths;
using UnityEngine.Networking;
using RoR2.Projectile;

namespace RiskOfRamen.Assets.Allocentrism
{
    public class AllocentrismBombController : MonoBehaviour
    {
        public ProjectileImpactExplosion explosion;

        public ProjectileControllerTrigger controllerTrigger;

        private DeployableSlot deployableSlot;

        internal Transform target;

        private void Start()
        {
            if (NetworkServer.active)
            {
                this.DeployToOwner();
            }
        }

        public void OnEnable()
        {
            deployableSlot = RiskOfRamenContent.AllocentrismBomb;
            if (NetworkServer.active)
            {
                if (controllerTrigger.owner)
                {
                    //AcquireOwner(controllerTrigger);
                }
            }
        }   

        private void AcquireOwner(ProjectileControllerTrigger controller)
        {
            CharacterBody component = controller.owner.GetComponent<CharacterBody>();
            if ((bool)component)
            {
                ProjectileTargetOrbiter component2 = GetComponent<ProjectileTargetOrbiter>();
                component.GetComponent<AllocentrismBodyBehavior>().InitializeOrbiter(component2, this, target);
            }
        }

        public void Detonate()
        {
            if (explosion)
            {
                explosion.Detonate();
            }
        }
        private void DeployToOwner()
        {
            GameObject owner = base.GetComponent<ProjectileControllerTrigger>().owner;
            if (!owner)
            {
                return;
            }
            CharacterBody component = owner.GetComponent<CharacterBody>();
            if ((bool)component)
            {
                CharacterMaster master = component.master;
                if ((bool)master)
                {
                    master.AddDeployable(base.GetComponent<Deployable>(), this.deployableSlot);
                }
            }
        }
    }
}