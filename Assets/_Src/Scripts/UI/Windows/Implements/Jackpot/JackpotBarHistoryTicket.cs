// Author: ad   -
// Created: 14/11/2024  : : 00:11
// DateUpdate: 14/11/2024

using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

namespace Game.UI
{
    public class JackpotBarHistoryTicket : MonoBehaviour
    {
        [SerializeField] private JackpotItemTicket[] tickets;
        
        public void SetData(List<int> listTicket)
        {
            tickets.ForEach(x => x.Clear());
            for (int i = 0; i < listTicket.Count; i++)
            {
                tickets[i].SetData(listTicket[i]);
            }
        }
    }
}