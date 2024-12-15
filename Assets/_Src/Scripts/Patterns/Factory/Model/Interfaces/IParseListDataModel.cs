using System.Collections.Generic;

namespace Game.Model.Utils
{
    public interface IParseListDataModel
    {
        public void ParseRowData(List<Dictionary<string, string>> listItemValue);
    }
}