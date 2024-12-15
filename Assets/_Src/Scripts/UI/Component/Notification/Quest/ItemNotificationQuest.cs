namespace Game.UI
{
	public class ItemNotificationQuest : ItemNotificationQuestAll
	{
		protected override bool IsValid()
		{
			if (_data == null) return false;
			return _data.Find(x => x.IsQuest() && x.can_claim) != null;
		}
	}
}