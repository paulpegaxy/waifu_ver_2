using System;
using Game.UI;

namespace Game.Model
{
    [Serializable]
    public abstract class AModelTransactionCellView : IESModel<TypeTransactionCellView>
    {
        public TypeTransactionCellView Type { get; set; }
    }

    public enum TypeTransactionCellView
    {
        Info,
        History
    }
}