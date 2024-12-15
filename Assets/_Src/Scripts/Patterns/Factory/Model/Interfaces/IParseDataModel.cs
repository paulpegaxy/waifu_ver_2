using System.Collections.Generic;

namespace Game.Model.Utils
{
    public interface IParseDataModel
    {
        void ParseRowData(Dictionary<string, string> itemValue);
    }
}