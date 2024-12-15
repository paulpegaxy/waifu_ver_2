
using UnityEngine;

namespace Template.VfxManager
{
    public class DefaultVfxHandler : MonoBehaviour, IVfxHandler
    {

        [SerializeField] private Transform graphicHolder;
        private int remainDuration;
        private bool isActivated;
        private GameObject graphicObject;

        private void Update()
        {
            if (!isActivated) { return; }
            remainDuration = (int)Mathf.Max(remainDuration - Time.deltaTime * 1000, 0);
            if (remainDuration == 0)
            {
                Remove();
            }
        }

        public void AssignGraphic(GameObject graphic)
        {
            graphicObject = Instantiate(graphic);
            graphicObject.transform.SetParent(graphicHolder);
            graphicObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        public void AttachTarget(Transform target, string attachChildName = null)
        {
            if (target == null) { return; }
            Transform attachTransform = target;
            if (!string.IsNullOrEmpty(attachChildName))
            {
                Transform childTransform = target.Find(attachChildName);
                attachTransform = childTransform ?? target;
            }
            graphicHolder.SetParent(attachTransform);
            graphicHolder.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        public void Init(Vector3 initPosition, Quaternion initRotation)
        {
            transform.position = initPosition;
            transform.rotation = initRotation;
        }

        public void Remove()
        {
            isActivated = false;
            Destroy(gameObject);
        }

        public void Show(int duration)
        {
            remainDuration = duration;
            isActivated = true;
        }
    }
}