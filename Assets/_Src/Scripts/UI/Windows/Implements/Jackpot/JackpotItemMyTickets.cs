using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using Game.Runtime;
using Game.UI;
using Sirenix.Utilities;
using UnityEngine;

public class JackpotItemMyTickets : MonoBehaviour
{
   [SerializeField] private JackpotBarHistoryTicket barTicket;
   
   public void SetData(List<int> listTicket)
   {
      barTicket.SetData(listTicket);
   }
}
