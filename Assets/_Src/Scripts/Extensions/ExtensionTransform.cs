using UnityEngine;

namespace Game.Runtime
{
	public static class ExtensionTransform
	{
		public static void AttachTo(this Transform transform, Transform parent, Vector3 scale, Vector3 position = default, Quaternion rotation = default)
		{
			transform.SetParent(parent);
			transform.SetLocalPositionAndRotation(position, rotation);
			transform.localScale = scale;
		}
	}
}