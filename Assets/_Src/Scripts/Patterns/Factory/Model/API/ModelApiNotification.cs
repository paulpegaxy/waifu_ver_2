using System;

namespace Game.Model
{
    public abstract class ModelApiNotification<T>
    {
        public static Action<T> OnChanged;
        
        public abstract void Notification();
    }
}