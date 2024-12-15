using UnityEngine;
using TMPro;

namespace Game.UI
{
    public class ItemPage : MonoBehaviour
    {
        [SerializeField] private TMP_Text textNumber;

        public void SetData(int number)
        {
            textNumber.text = number.ToString();
        }
    }
}