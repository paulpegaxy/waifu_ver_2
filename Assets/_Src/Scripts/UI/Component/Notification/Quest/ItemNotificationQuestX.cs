	namespace Game.UI
{
	public class ItemNotificationQuestX : ItemNotificationQuestAll
	{
		protected override bool IsValid()
		{
			if (_data == null) return false;
			return _data.Find(x => x.IsQuestX() && x.can_claim) != null;
		}
	}
}