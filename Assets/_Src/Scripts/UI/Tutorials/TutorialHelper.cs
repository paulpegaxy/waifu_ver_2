using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Game.Runtime;

namespace Game.UI
{
	public static class TutorialHelper
	{
		public static void SetHighlight(TutorialData data, bool interactable)
		{
			if (!data.gameObject.TryGetComponent<Canvas>(out var canvas))
			{
				canvas = data.gameObject.AddComponent<Canvas>();
				if (interactable)
				{
					data.gameObject.AddComponent<GraphicRaycaster>();
				}
				data.gameObject.SetActive(true);
			}

			canvas.overrideSorting = true;
			canvas.sortingLayerName = "Tutorial";
			canvas.sortingOrder = 1;
			canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1 | AdditionalCanvasShaderChannels.Normal | AdditionalCanvasShaderChannels.Tangent;
		}

		public static void RemoveHighlight(TutorialData data)
		{
			if (data.gameObject.TryGetComponent<Canvas>(out var canvas))
			{
				if (data.gameObject.TryGetComponent<GraphicRaycaster>(out var raycaster))
				{
					GameObject.DestroyImmediate(raycaster);
				}

				GameObject.DestroyImmediate(canvas);
			}
		}

		public static TutorialData GetObjectByName(Enum type)
		{
			var objects = GameObject.FindObjectsOfType<TutorialData>(true);
			return objects.FirstOrDefault(x => x.TypeData.ToString() == type.ToString());
		}
	}
}