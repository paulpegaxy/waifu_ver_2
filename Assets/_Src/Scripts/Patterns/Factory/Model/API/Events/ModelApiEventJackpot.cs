using System;
using System.Collections.Generic;
using System.IO;
using Doozy.Runtime.Common.Extensions;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Model
{
    [Serializable]
    public class ModelApiEventJackpot
    {
        public float today_reward;
        public long open_date;
        public long reset_at;
        public List<ModelApiEventJackpotData> win_history;
        public List<int> my_today_tickets;

        // public ModelApiEventJackpot MockUp()
        // {
        //     var file = File.ReadAllText(Path.Combine(Application.dataPath, "../Mockup/mockupJackpot.json"));
        //     return JsonConvert.DeserializeObject<ModelApiEventJackpot>(file);
        // }

        public void MockUpHistory()
        {
            win_history = new List<ModelApiEventJackpotData>();
            for (int i = 0; i < 5; i++)
            {
                var ele = new ModelApiEventJackpotData();
                ele.MockUp(i);
                win_history.Add(ele);
            }
        }
    }

    [Serializable]
    public class ModelApiEventJackpotData
    {
        public ModelApiUserData user;
        public float win_amount;
        public DateTime created_at;
        public List<int> bought_tickets;     //Danh sách ticket user sở hữu/đã mua (Max là 3)
        public int win_ticket;               //Ticket trúng thưởng

        public void MockUp(int i)
        {
            user = new ModelApiUserData()
            {
                id = Random.Range(0, 1000),
                name = "User Test " + i,
            };
            win_amount = Random.Range(0.1f, 10f);
            created_at = DateTime.Now;
            bought_tickets = new List<int>();
            win_ticket = Random.Range(0, 100);
            bought_tickets.Add(win_ticket);

            bool isHaveTicket = Random.Range(0, 2) == 1;
            if (isHaveTicket)
                bought_tickets.Add(Random.Range(0, 10000));

            isHaveTicket = Random.Range(0, 2) == 1;
            if (isHaveTicket)
                bought_tickets.Add(Random.Range(0, 10000));

            bought_tickets.Shuffle();
        }
    }
}