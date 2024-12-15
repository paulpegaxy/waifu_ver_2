using System.Collections;
using System.Collections.Generic;
using _Src.Scripts.Data.DBM.Configs;
using Doozy.Runtime.UIManager.Components;
using Game.Defines;
using Game.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomProfileItemStoryGenres : MonoBehaviour
{
    [SerializeField] private Image imgIcon;
    [SerializeField] private TMP_Text txtDes;

    private string _itemName;

    public string ItemName => _itemName;

    private UIToggle _toggle;

    public UIToggle Toggle => _toggle ??= GetComponent<UIToggle>();
    
    public void SetData(DataItemStoryGenres data)
    {
        // Data = data;
        imgIcon.sprite = data.sprIcon;
        txtDes.text = data.name;
        GetComponent<UIToggle>().isOn = false;
    }

    public void SetData(TypeStoryGenres type,bool status)
    {
        switch (type)
        {
            case TypeStoryGenres.LGBTQ:
                txtDes.text = "LGBTQ+";
                break;
            case TypeStoryGenres.Scifi:
                txtDes.text = "Sci-fi";
                break;
            case TypeStoryGenres.Onenight:
                txtDes.text = "One-night";
                break;
            default:
                txtDes.text= type.ToString();
                break;
        }

        _itemName = type + "_" + (int)type;
        imgIcon.sprite = ControllerSprite.Instance.GetStoryGenresIcon(type);
        imgIcon.SetNativeSize();
        Toggle.isOn = status;
    }
}
