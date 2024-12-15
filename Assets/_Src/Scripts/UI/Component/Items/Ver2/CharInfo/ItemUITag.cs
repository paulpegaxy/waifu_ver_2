using System.Collections;
using System.Collections.Generic;
using Game.Defines;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUITag : MonoBehaviour
{
    [SerializeField] private Image imgIcon;
    [SerializeField] private TMP_Text txtTagName;

    public void SetData(TypeStoryGenres type)
    {
        imgIcon.sprite = ControllerSprite.Instance.GetStoryGenresIcon(type);
        txtTagName.text = type.ToString();
    }
}
