
using Template.Defines;

public static class ExtensionInt
{
	public static TypeResourceCategory ToResourceCategory(this int id)
	{
		var index = int.Parse(id.ToString().Substring(0, 1));
		return (TypeResourceCategory)index;
	}

	public static TypeEntity ToEntityType(this int id)
	{
		var index = int.Parse(id.ToString().Substring(0, 1));
		return (TypeEntity)(index - 2);
	}
}