using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JackpotItemTicket : MonoBehaviour
{
    [SerializeField] private TMP_Text txtTicket;
    [SerializeField] private Image imgActive;

    private void OnDisable()
    {
        imgActive.gameObject.SetActive(false);
    }

    public void SetData(int ticket)
    {
        txtTicket.text = ticket.ToString("00000");
        imgActive.gameObject.SetActive(true);
        imgActive.LoadSpriteAutoParseAsync("holder_jackpot_ticket");
    }

    public void SetTicketWin(int ticket)
    {
        txtTicket.text = ticket.ToString("00000");
        imgActive.gameObject.SetActive(true);
        imgActive.LoadSpriteAutoParseAsync("holder_jackpot_ticket_win");
    }

    public void Clear()
    {
        txtTicket.text = "";
        imgActive.gameObject.SetActive(false);
    }
}
