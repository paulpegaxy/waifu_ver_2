// Author: ad   -
// Created: 14/11/2024  : : 00:11
// DateUpdate: 14/11/2024

using System;
using System.Collections.Generic;
using Game.Model;
using Game.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemJackpotMyHistory : MonoBehaviour
    {
        [SerializeField] private GameObject[] arrObjStatus;
        [SerializeField] private TMP_Text txtDes;
        [SerializeField] private JackpotBarHistoryTicket barTicket;

        public void LoadData(ModelApiEventJackpotMyHistory data)
        {
            LoadDes(data.date);
            barTicket.SetData(data.tickets);
            arrObjStatus[0].SetActive(data.is_win);
            arrObjStatus[1].SetActive(!data.is_win);
        }

        private void LoadDes(string date)
        {
            txtDes.text = Localization.Get(TextId.Event_JackpotDesPurchased) + " ";
            txtDes.text += date.SetHighlightStringGreen4_40FF1A();
        }
    }
}