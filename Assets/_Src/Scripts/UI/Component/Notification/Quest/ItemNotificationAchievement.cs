namespace Game.UI
{
	public class ItemNotificationAchievement : ItemNotificationQuestAll
	{
		protected override bool IsValid()
		{
			if (_data == null) return false;
			return _data.Find(x => x.IsAchievement() && x.can_claim) != null;
		}
	}
}