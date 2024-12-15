using System;
using UnityEngine;
using DG.Tweening;
using Game.Core;
using Game.Runtime;
using Game.UI;
using Template.Defines;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Template.Runtime
{
	public class ControllerUI : Singleton<ControllerUI>
	{
		[SerializeField] private Canvas CanvasUI;
		[SerializeField] private Transform DefaultExp;
		[SerializeField] private Transform DefaultChatPoint;
		[SerializeField] private Transform MainPal;
		[SerializeField] private Transform MainGold;

		private void Spawn(TypeResource type, Vector3 from, Vector3 to, float offsetX, float offsetY,float scale=1f,Action callBack=null)
		{
			var item = ControllerSpawner.Instance.Spawn(AnR.GetKey(AnR.CommonKey.ResourceIcon), true, parent: transform);
			item.SetActive(true);
			item.transform.position = from;
			item.transform.localScale = Vector3.one * scale;
			item.transform.localEulerAngles = Random.Range(0, 360) * Vector3.forward;
			item.GetComponent<ItemResourceIcon>().SetData(type);

			var sequence = DOTween.Sequence();
			sequence.Append(item.transform.DOMove(new Vector3(from.x + offsetX, from.y + offsetY, from.z), Random.Range(0.25f, 0.5f)).SetEase(Ease.OutQuad));
			sequence.Append(item.transform.DORotate(Vector3.zero, Random.Range(0.25f, 0.5f)).SetEase(Ease.OutQuad));
			sequence.Append(item.transform.DOMove(to, Random.Range(0.5f, 1f)).SetEase(Ease.InQuad).OnComplete(() =>
			{
				item.SetActive(false);
				callBack?.Invoke();
			}));
		}

		private void SpawnDynamicItem(Sprite sprite, Vector3 from, Vector3 to, float offsetX, float offsetY,float scale=1f,Action callBack=null)
		{
			var item = ControllerSpawner.Instance.Spawn(AnR.GetKey(AnR.CommonKey.ResourceIcon), true, parent: transform);
			item.SetActive(true);
			item.transform.position = from;
			item.transform.localScale = Vector3.one * scale;
			item.GetComponent<ItemResourceIcon>().SetData(sprite);

			var sequence = DOTween.Sequence();
			sequence.Append(item.transform.DOMove(new Vector3(from.x + offsetX, from.y + offsetY, from.z), Random.Range(0.25f, 0.5f)).SetEase(Ease.OutQuad));
			sequence.Append(item.transform.DOMove(to, Random.Range(0.5f, 1f)).SetEase(Ease.InQuad).OnComplete(() =>
			{
				item.SetActive(false);
				callBack?.Invoke();
			}));
		}
		
		private void SpawnDynamicItemSpecial(Sprite sprite, Vector3 from, Vector3 to, float offsetX, float offsetY,float scale=1f,Action callBack=null)
		{
			var item = ControllerSpawner.Instance.Spawn(AnR.GetKey(AnR.CommonKey.ResourceEffectIcon), true, parent: transform);
			item.SetActive(true);
			item.transform.position = from;
			item.transform.localScale = Vector3.one * scale;
			item.GetComponent<ItemResourceEffectIcon>().SetData(sprite);

			var sequence = DOTween.Sequence();
			sequence.Append(item.transform.DOMove(new Vector3(from.x + offsetX, from.y + offsetY, from.z), Random.Range(0.25f, 0.75f)).SetEase(Ease.OutQuad));
			sequence.Append(item.transform.DOMove(to, Random.Range(0.5f, 1f)).SetEase(Ease.InQuad).OnComplete(() =>
			{
				item.SetActive(false);
				callBack?.Invoke();
			}));
		}

		private void SpawnFloatText(Vector3 from,Vector3 to, float offsetX,float offsetY)
		{
			
		}

		public Vector3 WorldPointToUIWorldPoint(Vector3 position)
		{
			var cameraMain = CameraManager.Instance.GetCamera(CameraManager.CameraType.MainCamera);
			var cameraUI = CameraManager.Instance.GetCamera(CameraManager.CameraType.UICamera);
			var viewpoint = cameraMain.WorldToViewportPoint(position);

			return cameraUI.ViewportToWorldPoint(viewpoint);
		}

		public float GetWidth()
		{
			return Screen.width / CanvasUI.scaleFactor;
		}

		public float GetHeight()
		{
			return Screen.height / CanvasUI.scaleFactor;
		}

		public void Spawn(TypeResource type, Vector3 from, int count,float scale=1f,Action callBack=null)
		{
			var to = GetPosition(type);
			for (var i = 0; i < count; i++)
			{
				var offsetX = Random.Range(-1f, 1f);
				var offsetY = Random.Range(-1f, 1f);
				if (count > 1)
				{
					scale = Random.Range(0.4f, 1f) * scale;
				}

				Spawn(type, from, to, offsetX, offsetY,scale,callBack);
			};
		}

		public void SpawnScDynamic(Sprite spr,bool isSpecial,Vector3 from, int count,float scale=1f,Action callBack=null)
		{
			var to = GetPosition(TypeResource.HeartPoint);
			for (var i = 0; i < count; i++)
			{
				var offsetX = Random.Range(-1f, 1f);
				var offsetY = Random.Range(-1f, 1f);

				if (isSpecial)
					SpawnDynamicItemSpecial(spr, from, to, offsetX, offsetY, scale, callBack);
				else
					SpawnDynamicItem(spr, from, to, offsetX, offsetY, scale, callBack);
			};
		}
		

		public void SpawnCustomPos(TypeResource type, Vector3 from, Vector3 to, int count,float scale=1f, Action callBack = null)
		{
			for (var i = 0; i < count; i++)
			{
				var offsetX = Random.Range(-1f, 1f);
				var offsetY = Random.Range(-1f, 1f);

				Spawn(type, from, to, offsetX, offsetY,scale,callBack);
			};
		}

		private Vector3 GetPosition(TypeResource type)
		{
			var isMain = SpecialExtensionUI.GetCurrentNode() == UIId.UIViewName.Main.ToString();
			return type switch
			{
				TypeResource.ExpWaifu => DefaultExp.position,
				TypeResource.HeartPoint =>isMain? MainGold.position : DefaultChatPoint.position,
				TypeResource.Berry => isMain ? MainPal.position : DefaultExp.position,
				_ => Vector3.zero,
			};
		}
	}
}