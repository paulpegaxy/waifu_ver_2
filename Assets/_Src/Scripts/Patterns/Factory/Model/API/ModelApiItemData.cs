using System;
using BreakInfinity;
using Newtonsoft.Json;
using Template.Defines;

namespace Game.Model
{
    [Serializable]
    public class ModelApiItemData
    {
        public string id;
        public string value;
        public string quantity;

        [JsonIgnore] public TypeResource IdResource => id.ToResourceType();
        
        [JsonIgnore]
        public BigDouble QuantityParse => string.IsNullOrEmpty(quantity) ? 0 : BigDouble.Parse(quantity);

        [JsonIgnore]
        public BigDouble ValueParse => string.IsNullOrEmpty(value) ? 0 : BigDouble.Parse(value);
    }
    
}