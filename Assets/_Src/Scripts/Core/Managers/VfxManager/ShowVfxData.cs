using System;
using UnityEngine;

namespace Template.VfxManager
{
    public struct ShowVfxData
    {
        public string vfxId;
        public Vector3 initPosition;
        public Quaternion initRotation;
        public Transform attachTarget;

        public ShowVfxData(string vfxId, Vector3 initPosition, Quaternion initRotation, Transform attachTarget = null)
        {
            this.vfxId = vfxId;
            this.initPosition = initPosition;
            this.initRotation = initRotation;
            this.attachTarget = attachTarget;
        }
    }
}