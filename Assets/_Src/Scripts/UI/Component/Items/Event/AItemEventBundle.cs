using System.Collections;
using System.Collections.Generic;
using Game.Model;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public abstract class AItemEventBundle : MonoBehaviour
    {
        [SerializeField] protected TMP_Text txtLimit;
        [SerializeField] protected GameObject objSoldOut;

        public abstract void SetData(ModelApiShopData shopData);

        public abstract void OnSuccessBuy();
    }
}