using Doozy.Runtime.UIManager.Components;


namespace Game.UI
{
	public class ClubFilterTime : AItemFilter<FilterTimeType>
	{
		public void ShowAllTime(bool value)
		{
			if (!value)
			{
				if (_toggleGroup.toggles[0].isOn)
				{
					_toggleGroup.toggles[1].SetIsOn(true);
				}
			}
			_toggleGroup.toggles[0].gameObject.SetActive(value);
		}
	}
}
