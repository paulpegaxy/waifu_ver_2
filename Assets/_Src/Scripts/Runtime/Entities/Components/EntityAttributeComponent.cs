using System;
using System.Collections.Generic;

namespace Slime
{
    public enum Attribute
    {
        CurrentHP
    }
    public class EntityAttribute
    {
        public Action<float> onValueChanged;
        public float CurrentValue
        {
            get => currentValue;
            set
            {
                if (currentValue == value) { return; }
                currentValue = value;
                onValueChanged?.Invoke(currentValue);
            }
        }
        private float currentValue;

        public EntityAttribute(float initValue)
        {
            CurrentValue = initValue;
        }
    }
    public class EntityAttributeComponent
    {
        private Dictionary<Attribute, EntityAttribute> attributes = new();

        public void AddAttribute(Attribute attribute, float initValue)
        {
            if (attributes.ContainsKey(attribute))
            {
                attributes[attribute] = new EntityAttribute(initValue);
            }
            else
            {
                attributes.Add(attribute, new EntityAttribute(initValue));
            }
        }

        public EntityAttribute GetAttribute(Attribute attribute)
        {
            return attributes[attribute];
        }
    }
}