﻿using System.Collections.Generic;
using HutongGames.PlayMaker.Actions;
using IL.TMPro;
using ModCommon;
using ModCommon.Util;
using UnityEngine;
using TMPro;
using System.Collections;

namespace MultiplayerClient
{
    public enum TextureType
    {
        Baldur,
        Fluke,
        Grimm,
        Hatchling,
        Knight,
        Shield,
        Sprint,
        Unn,
        Void,
        VS,
        Weaver,
        Wraiths,
    }

    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance;
        
        public byte id;
        public string username;
        public int team = 0;

        private TMPro.TextMeshPro chattext;
        private Coroutine routine;

        public string activeScene;
        public bool CurrentRoomSyncHost;
        
        public bool equippedCharm_1;
        public bool equippedCharm_2;
        public bool equippedCharm_3;
        public bool equippedCharm_4;
        public bool equippedCharm_5;
        public bool equippedCharm_6;
        public bool equippedCharm_7;
        public bool equippedCharm_8;
        public bool equippedCharm_9;
        public bool equippedCharm_10;
        public bool equippedCharm_11;
        public bool equippedCharm_12;
        public bool equippedCharm_13;
        public bool equippedCharm_14;
        public bool equippedCharm_15;
        public bool equippedCharm_16;
        public bool equippedCharm_17;
        public bool equippedCharm_18;
        public bool equippedCharm_19;
        public bool equippedCharm_20;
        public bool equippedCharm_21;
        public bool equippedCharm_22;
        public bool equippedCharm_23;
        public bool equippedCharm_24;
        public bool equippedCharm_25;
        public bool equippedCharm_26;
        public bool equippedCharm_27;
        public bool equippedCharm_28;
        public bool equippedCharm_29;
        public bool equippedCharm_30;
        public bool equippedCharm_31;
        public bool equippedCharm_32;
        public bool equippedCharm_33;
        public bool equippedCharm_34;
        public bool equippedCharm_35;
        public bool equippedCharm_36;
        public bool equippedCharm_37;
        public bool equippedCharm_38;
        public bool equippedCharm_39;
        public bool equippedCharm_40;

        public int health;
        public int maxHealth;
        public int healthBlue;

        public Dictionary<byte[], TextureType> texHashes = new Dictionary<byte[], TextureType>(new ByteArrayComparer());
        public Dictionary<TextureType, Texture2D> textures = new Dictionary<TextureType, Texture2D>();

        private void Awake()
        {
            Instance = this;
        }

        public void SetChat(string text, TMPro.TextMeshPro textMesh = null)
        {
            if (textMesh != null)
                chattext = textMesh;
            if (chattext != null)
            {
                chattext.text = text;
                if (routine != null)
                {
                    StopCoroutine(routine);
                }
                routine = StartCoroutine(ChatRoutine());
            }
        }

        public string GetChat()
        {
            if (chattext != null)
                return chattext.text;
            else
                return "";
        }

        private IEnumerator ChatRoutine()
        {
            chattext.color = Color.white;
            yield return new WaitForSeconds(1.5f);
            for (int i = 9; i >= 0; i--)
            {
                chattext.color = new Color(1, 1, 1, i / 10f);
                yield return new WaitForSeconds(0.1f);
            }
        }
        
        private void Log(object message) => Modding.Logger.Log("[Player Manager] " + message);
    }
}