using UnityEngine;

namespace Template.VfxManager
{
    public interface IVfxHandler
    {
        void Init(Vector3 initPosition, Quaternion initRotation);

        void AssignGraphic(GameObject graphic);
        void AttachTarget(Transform target, string attachChildName = null);
        void Show(int duration);
        void Remove();
    }
}