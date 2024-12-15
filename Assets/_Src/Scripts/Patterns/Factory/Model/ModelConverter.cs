using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Game.Model
{
	public class ModelConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			var types = new List<Type>
			{
				typeof(ModelGameConfigParam),
				typeof(ModelTrigger)
			};

			return types.Contains(objectType);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType != JsonToken.Null)
			{
				JObject jObject = JObject.Load(reader);
				object target = null;

				switch (objectType.Name)
				{
					case "ModelGameConfigParam":
						var typeGameConfig = jObject["Type"].ToObject<TypeGameConfig>();
						target = FactoryModelGameConfigParam.Get<ModelGameConfigParam>(typeGameConfig);
						break;
					case "ModelTrigger":
						var triggerType = jObject["Type"].ToObject<ModelTriggerType>();
						target = FactoryModelTrigger.Get<ModelTrigger>(triggerType);
						break;
				}

				if (target == null)
				{
					UnityEngine.Debug.Log("loi null target");
				}
				else
					serializer.Populate(jObject.CreateReader(), target);

				return target;
			}

			return null;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}
	}
}