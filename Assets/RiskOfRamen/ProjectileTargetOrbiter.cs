using BepInEx;
using System.IO;
using UnityEngine;
using RoR2;
using R2API;
using RoR2.Items;
using RoR2BepInExPack.GameAssetPaths;
using UnityEngine.Networking;
using RoR2.Projectile;
using System.Runtime.InteropServices;

public class ProjectileTargetOrbiter : NetworkBehaviour
{
    [SyncVar]
    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    [SyncVar]
    private float initialDegreesFromOwnerForward;

    [SerializeField]
    [SyncVar]
    private float degreesPerSecond;

    [SerializeField]
    [SyncVar]
    public float radius;

    [SerializeField]
    [SyncVar]
    private Vector3 planeNormal = Vector3.up;


    private Rigidbody rigidBody;

    private Transform Target;

    private bool resetOnAcquireOwner = true;

    [SyncVar]
    private Vector3 initialRadialDirection;

    [SyncVar]
    private float initialRunTime;

    public Vector3 Networkoffset
    {
        get
        {
            return this.offset;
        }
        [param: In]
        set
        {
            base.SetSyncVar(value, ref this.offset, 1u);
        }
    }

    public float NetworkinitialDegreesFromOwnerForward
    {
        get
        {
            return this.initialDegreesFromOwnerForward;
        }
        [param: In]
        set
        {
            base.SetSyncVar(value, ref this.initialDegreesFromOwnerForward, 2u);
        }
    }

    public float NetworkdegreesPerSecond
    {
        get
        {
            return this.degreesPerSecond;
        }
        [param: In]
        set
        {
            base.SetSyncVar(value, ref this.degreesPerSecond, 4u);
        }
    }

    public float Networkradius
    {
        get
        {
            return this.radius;
        }
        [param: In]
        set
        {
            base.SetSyncVar(value, ref this.radius, 8u);
        }
    }

    public Vector3 NetworkplaneNormal
    {
        get
        {
            return this.planeNormal;
        }
        [param: In]
        set
        {
            base.SetSyncVar(value, ref this.planeNormal, 16u);
        }
    }

    public Vector3 NetworkinitialRadialDirection
    {
        get
        {
            return this.initialRadialDirection;
        }
        [param: In]
        set
        {
            base.SetSyncVar(value, ref this.initialRadialDirection, 32u);
        }
    }

    public float NetworkinitialRunTime
    {
        get
        {
            return this.initialRunTime;
        }
        [param: In]
        set
        {
            base.SetSyncVar(value, ref this.initialRunTime, 64u);
        }
    }

    public Transform getTarget()
    {
        return Target;
    }
    
    public void Initialize(Transform target, Vector3 planeNormal, float radius, float degreesPerSecond, float initialDegreesFromOwnerForward)
    {
        this.Target = target;
        this.NetworkplaneNormal = planeNormal;
        this.Networkradius = radius;
        this.NetworkdegreesPerSecond = degreesPerSecond;
        this.NetworkinitialDegreesFromOwnerForward = initialDegreesFromOwnerForward;
        this.ResetState();
    }

    private void OnEnable()
    {
        this.rigidBody = base.GetComponent<Rigidbody>();
        //this.AcquireTarget(Target);
    }

    public void FixedUpdate()
    {
        this.UpdatePosition(doSnap: false);
    }

    private void ResetState()
    {
        this.NetworkinitialRunTime = Time.fixedTime;
        this.planeNormal.Normalize();
        if ((bool)this.Target)
        {
            this.NetworkinitialRadialDirection = Quaternion.AngleAxis(this.initialDegreesFromOwnerForward, this.planeNormal) * this.Target.forward;
            this.resetOnAcquireOwner = false;
        }
        this.UpdatePosition(doSnap: true);
    }

    private void UpdatePosition(bool doSnap)
    {
        if ((bool)this.Target)
        {
            float angle = (Time.fixedTime - this.initialRunTime) * this.degreesPerSecond;
            Vector3 position = this.Target.position + this.offset + Quaternion.AngleAxis(angle, this.planeNormal) * this.initialRadialDirection * this.radius;
            if (!this.rigidBody || doSnap)
            {
                base.transform.position = position;
            }
            else if ((bool)this.rigidBody)
            {
                this.rigidBody.MovePosition(position);
            }
        }
    }

    public void SetInitialDegreesFromOwnerForward(float degrees)
    {
        this.NetworkinitialDegreesFromOwnerForward = degrees;
        if ((bool)this.Target)
        {
            this.NetworkinitialRadialDirection = Quaternion.AngleAxis(this.initialDegreesFromOwnerForward, this.planeNormal) * this.Target.forward;
        }
    }

    public float GetInitialRunTime()
    {
        return this.initialRunTime;
    }

    public void SetInitialRunTime(float _time)
    {
        this.NetworkinitialRunTime = Mathf.Max(_time, 0f);
    }

    private void AcquireTarget(Transform target)
    {
        if (!target.transform) { return; }
        this.Target = target.transform;
        if (this.resetOnAcquireOwner)
        {
            this.resetOnAcquireOwner = false;
            this.ResetState();
        }
    }

    private void UNetVersion()
    {
    }

    public override bool OnSerialize(NetworkWriter writer, bool forceAll)
    {
        if (forceAll)
        {
            writer.Write(this.offset);
            writer.Write(this.initialDegreesFromOwnerForward);
            writer.Write(this.degreesPerSecond);
            writer.Write(this.radius);
            writer.Write(this.planeNormal);
            writer.Write(this.initialRadialDirection);
            writer.Write(this.initialRunTime);
            return true;
        }
        bool flag = false;
        if ((base.syncVarDirtyBits & 1) != 0)
        {
            if (!flag)
            {
                writer.WritePackedUInt32(base.syncVarDirtyBits);
                flag = true;
            }
            writer.Write(this.offset);
        }
        if ((base.syncVarDirtyBits & 2) != 0)
        {
            if (!flag)
            {
                writer.WritePackedUInt32(base.syncVarDirtyBits);
                flag = true;
            }
            writer.Write(this.initialDegreesFromOwnerForward);
        }
        if ((base.syncVarDirtyBits & 4) != 0)
        {
            if (!flag)
            {
                writer.WritePackedUInt32(base.syncVarDirtyBits);
                flag = true;
            }
            writer.Write(this.degreesPerSecond);
        }
        if ((base.syncVarDirtyBits & 8) != 0)
        {
            if (!flag)
            {
                writer.WritePackedUInt32(base.syncVarDirtyBits);
                flag = true;
            }
            writer.Write(this.radius);
        }
        if ((base.syncVarDirtyBits & 0x10) != 0)
        {
            if (!flag)
            {
                writer.WritePackedUInt32(base.syncVarDirtyBits);
                flag = true;
            }
            writer.Write(this.planeNormal);
        }
        if ((base.syncVarDirtyBits & 0x20) != 0)
        {
            if (!flag)
            {
                writer.WritePackedUInt32(base.syncVarDirtyBits);
                flag = true;
            }
            writer.Write(this.initialRadialDirection);
        }
        if ((base.syncVarDirtyBits & 0x40) != 0)
        {
            if (!flag)
            {
                writer.WritePackedUInt32(base.syncVarDirtyBits);
                flag = true;
            }
            writer.Write(this.initialRunTime);
        }
        if (!flag)
        {
            writer.WritePackedUInt32(base.syncVarDirtyBits);
        }
        return flag;
    }

    public override void OnDeserialize(NetworkReader reader, bool initialState)
    {
        if (initialState)
        {
            this.offset = reader.ReadVector3();
            this.initialDegreesFromOwnerForward = reader.ReadSingle();
            this.degreesPerSecond = reader.ReadSingle();
            this.radius = reader.ReadSingle();
            this.planeNormal = reader.ReadVector3();
            this.initialRadialDirection = reader.ReadVector3();
            this.initialRunTime = reader.ReadSingle();
            return;
        }
        int num = (int)reader.ReadPackedUInt32();
        if ((num & 1) != 0)
        {
            this.offset = reader.ReadVector3();
        }
        if ((num & 2) != 0)
        {
            this.initialDegreesFromOwnerForward = reader.ReadSingle();
        }
        if ((num & 4) != 0)
        {
            this.degreesPerSecond = reader.ReadSingle();
        }
        if ((num & 8) != 0)
        {
            this.radius = reader.ReadSingle();
        }
        if ((num & 0x10) != 0)
        {
            this.planeNormal = reader.ReadVector3();
        }
        if ((num & 0x20) != 0)
        {
            this.initialRadialDirection = reader.ReadVector3();
        }
        if ((num & 0x40) != 0)
        {
            this.initialRunTime = reader.ReadSingle();
        }
    }

    public override void PreStartClient()
    {
    }
}
