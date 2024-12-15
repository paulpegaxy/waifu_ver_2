// Author:    -    ad
// Created: 18/07/2024  : : 10:51 PM
// DateUpdate: 18/07/2024

using System;
using System.Runtime.InteropServices;
using Game.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Template.Utils
{
    public class Bridge : Singleton<Bridge>
    {
        [DllImport("__Internal")]
        private static extern void postMessage(string data);

        [DllImport("__Internal")]
        private static extern void walletConnect();

        [DllImport("__Internal")]
        private static extern void walletDisconnect();

        [DllImport("__Internal")]
        private static extern void walletSendTransaction(string data);

        [DllImport("__Internal")]
        private static extern bool isWalletConnected();

        public void PostMessage(ModelBridge data)
        {
            var json = JsonConvert.SerializeObject(data);
#if !UNITY_EDITOR && UNITY_WEBGL
            postMessage(json);
#endif
        }

        public void Connect()
        {
            walletConnect();
        }

        public void Disconnect()
        {
            walletDisconnect();
        }

        public bool IsConnected()
        {
            return isWalletConnected();
        }

        public void Buy(ModelBridge data)
        {
            var stringData = JsonConvert.SerializeObject(data);
            walletSendTransaction(stringData);
        }

        public void OnMessage(string message)
        {
            Debug.Log("OnMessage: " + message);
            var model = JsonConvert.DeserializeObject<ModelBridge>(message);

            Debug.Log($"Type = {model.type}");
            var data = JObject.FromObject(model.data);

            Debug.Log("Data:" + data);
            foreach (var item in data)
            {
                Debug.Log($"{item.Key} = {item.Value}");
            }
        }
    }
    
    [Serializable]
    public class ModelBridge
    {
        public string type;
        public object data;
    }
}