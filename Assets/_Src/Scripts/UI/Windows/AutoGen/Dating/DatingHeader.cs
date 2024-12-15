using System.Collections;
using System.Collections.Generic;
using Game.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DatingHeader : MonoBehaviour
{
    [SerializeField] private Image imgAvaChar;
    [SerializeField] private TMP_Text txtCharName;
    [SerializeField] private TMP_Text txtStatus;
    
    public Sprite GetSpriteImage()
    {
        return imgAvaChar.sprite;
    }

    public void SetData(ModelApiEntityConfig data)
    {
        if (data==null)
        {
            Debug.Log("Null data in DatingHeader");
            return;
        }
        
        imgAvaChar.LoadSpriteAsync(data.AvaCharKey);
        txtCharName.text = data.name;
        txtStatus.text = "Active";
    }
}
