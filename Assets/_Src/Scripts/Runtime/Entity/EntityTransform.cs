using UnityEngine;

namespace Game.Runtime
{
    public class EntityTransform
    {
        private Transform _pivot;
        private Entity _owner;

        public EntityTransform(Entity owner)
        {
            foreach (Transform child in owner.transform)
            {
                if (child.name == "Pivot")
                {
                    _pivot = child;
                    break;
                }
            }

            _owner = owner;
        }

        public void Attach(GameObject mesh)
        {
            mesh.transform.SetParent(_pivot);
            mesh.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            mesh.transform.localScale = Vector3.one;

            _pivot.localRotation = Quaternion.identity;
        }

        public void DetachAll()
        {
            foreach (Transform child in _pivot)
            {
                // ControllerSpawner.Instance.Return(child.gameObject);
            }
        }

        public void LookAt(Vector2Int from, Vector2Int to)
        {
            var direction = to - from;

            _pivot.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.y));
            _pivot.Rotate(Vector3.up, 45);
        }
    }
}