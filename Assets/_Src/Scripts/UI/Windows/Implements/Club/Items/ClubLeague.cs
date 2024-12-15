using System;
using UnityEngine;
using UnityEngine.UI;
using Doozy.Runtime.UIManager.Components;
using Game.Runtime;
using TMPro;

namespace Game.UI
{
	public class ClubLeague : MonoBehaviour
	{
		[SerializeField] private Image imageTrohpy;
		[SerializeField] private TMP_Text txtRankName;
		[SerializeField] private UIButton buttonNext;
		[SerializeField] private UIButton buttonPrev;

		public static Action<TypeLeague> OnChanged;

		private TypeLeague _type = TypeLeague.Bronze;

		private void OnEnable()
		{
			buttonNext.onClickEvent.AddListener(OnNext);
			buttonPrev.onClickEvent.AddListener(OnPrev);
		}

		private void OnDisable()
		{
			buttonNext.onClickEvent.RemoveListener(OnNext);
			buttonPrev.onClickEvent.RemoveListener(OnPrev);
		}

		private void OnNext()
		{
			_type++;
			if (_type > TypeLeague.Royal)
			{
				_type = TypeLeague.Bronze;
			}

			SetData(_type);
			OnChanged?.Invoke(_type);
		}

		private void OnPrev()
		{
			_type--;
			if (_type < TypeLeague.Bronze)
			{
				_type = TypeLeague.Royal;
			}

			SetData(_type);
			OnChanged?.Invoke(_type);
		}

		public void SetData(TypeLeague type)
		{
			// imageTrohpy.sprite = ControllerSprite.Instance.GetLeagueIconBig(type);
			imageTrohpy.LoadSpriteAutoParseAsync("league_big_" + (int)type);
			imageTrohpy.SetNativeSize();
			txtRankName.text = type.ToString();
			txtRankName.color = DBM.Config.rankingConfig.GetClubRankData(type).rankColor;
			_type = type;
		}
	}

}
