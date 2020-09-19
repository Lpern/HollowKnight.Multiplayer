﻿using System;
using HutongGames.PlayMaker.Actions;
using ModCommon.Util;
using UnityEngine;

namespace MultiplayerServer
{
    public class ServerSend
    {
        private static void SendTCPData(byte toClient, Packet packet)
        {
            packet.WriteLength();
            Server.clients[toClient].tcp.SendData(packet);
        }

        private static void SendUDPData(byte toClient, Packet packet)
        {
            packet.WriteLength();
            Server.clients[toClient].udp.SendData(packet);
        }
        
        private static void SendTCPDataToAll(Packet packet)
        {
            packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (Server.clients[i].player != null)
                {
                    Server.clients[i].tcp.SendData(packet);   
                }
            }
        }

        private static void SendTCPDataToAll(byte exceptClient, Packet packet)
        {
            packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != exceptClient)
                {
                    if (Server.clients[i].player != null)
                    {
                        Server.clients[i].tcp.SendData(packet);   
                    }
                }
            }
        }
        
        private static void SendUDPDataToAll(Packet packet)
        {
            packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (Server.clients[i].player != null)
                {
                    Server.clients[i].udp.SendData(packet);
                }
            }
        }

        private static void SendUDPDataToAll(byte exceptClient, Packet packet)
        {
            packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != exceptClient)
                {
                    if (Server.clients[i].player != null)
                    {
                        Server.clients[i].udp.SendData(packet);   
                    }
                }
            }
        }

        public static void Welcome(byte toClient, string msg)
        {
            using (Packet packet = new Packet((int) ServerPackets.Welcome))
            {
                packet.Write(toClient);
                packet.Write(msg);
                
                SendTCPData(toClient, packet);
            }
        }

        public static void RequestTexture(byte toClient, byte[] hash)
        {
            if (!ServerSettings.CustomKnightIntegration) return;

            using(Packet packet = new Packet((int) ServerPackets.TextureRequest))
            {
                packet.Write(hash);
                SendTCPData(toClient, packet);
            }
        }

        public static void SpawnPlayer(byte toClient, Player player)
        {
            using (Packet packet = new Packet((int) ServerPackets.SpawnPlayer))
            {
                packet.Write(player.id);
                packet.Write(player.username);
                packet.Write(player.position);
                packet.Write(player.scale);
                packet.Write(player.animation);
                for (int charmNum = 1; charmNum <= 40; charmNum++)
                {
                    packet.Write(player.GetAttr<Player, bool>("equippedCharm_" + charmNum));
                }
                packet.Write(player.team);
                packet.Write(ServerSettings.PvPEnabled);
                packet.Write(ServerSettings.TeamsEnabled);

                Log("Player texture hashes length: " + player.textureHashes.Count);
                foreach(var hash in player.textureHashes)
                {
                    packet.Write(hash);
                }

                Log($"Spawning Player {player.id} on Client {toClient} with Charms");
                SendTCPData(toClient, packet);
            }
        }

        #region CustomKnight Integration

        public static void SendTexture(byte fromClient, byte[] hash, byte[] texture)
        {
            using (Packet packet = new Packet((int) ServerPackets.TextureFragment))
            {
                // Since the ordering of TCP packets is guaranteed, we don't have
                // to put it in the packet - the client will handle it just fine.
                packet.Write(hash);
                packet.Write(texture.Length);
                packet.Write(texture);
                SendTCPData(fromClient, packet);
            }
        }
        
        #endregion CustomKnight Integration
        
        public static void DestroyPlayer(byte toClient, int clientToDestroy, bool newhost)
        {
            using (Packet packet = new Packet((int) ServerPackets.DestroyPlayer))
            {
                packet.Write(clientToDestroy);
                packet.Write(newhost);

                SendTCPData(toClient, packet);
            }
        }

        public static void PvPEnabled()
        {
            using (Packet packet = new Packet((int) ServerPackets.PvPEnabled))
            {
                packet.Write(ServerSettings.PvPEnabled);
                
                SendTCPDataToAll(packet);
            }
        }

        public static void TeamsEnabled()
        {
            using (Packet packet = new Packet((int)ServerPackets.TeamsEnabled))
            {
                packet.Write(ServerSettings.TeamsEnabled);

                SendTCPDataToAll(packet);
            }
        }

        public static void Team(byte id, int team)
        {
            using (Packet packet = new Packet((int)ServerPackets.Team))
            {
                packet.Write(id);
                packet.Write(team);

                SendTCPDataToAll(packet);
            }
        }

        public static void Chat(byte id, string message)
        {
            using (Packet packet = new Packet((int)ServerPackets.Chat))
            {
                packet.Write(id);
                packet.Write(message);

                SendTCPDataToAll(packet);
            }
        }

        public static void PlayerPosition(Player player)
        {
            using (Packet packet = new Packet((int) ServerPackets.PlayerPosition))
            {
                packet.Write(player.id);
                packet.Write(player.position);

                SendUDPDataToAll(player.id, packet);
            }
        }

        public static void PlayerScale(Player player)
        {
            using (Packet packet = new Packet((int) ServerPackets.PlayerScale))
            {
                packet.Write(player.id);
                packet.Write(player.scale);

                SendUDPDataToAll(player.id, packet);
            }
        }

        public static void PlayerAnimation(Player player)
        {
            using (Packet packet = new Packet((int) ServerPackets.PlayerAnimation))
            {
                packet.Write(player.id);
                packet.Write(player.animation);
             
                SendUDPDataToAll(player.id, packet);
            }
        }

        public static void HealthUpdated(byte fromClient, int health, int maxHealth, int healthBlue)
        {
            using (Packet packet = new Packet((int) ServerPackets.HealthUpdated))
            {
                packet.Write(fromClient);
                packet.Write(health);
                packet.Write(maxHealth);
                packet.Write(healthBlue);

                Log("Sending Health Data to all clients except " + fromClient);
                SendTCPDataToAll(fromClient, packet);
            }
        }
        
        public static void CharmsUpdated(byte fromClient, Player player)
        {
            using (Packet packet = new Packet((int) ServerPackets.CharmsUpdated))
            {
                packet.Write(fromClient);
                for (int charmNum = 1; charmNum <= 40; charmNum++)
                {
                    packet.Write(player.GetAttr<Player, bool>("equippedCharm_" + charmNum));
                }
                
                SendTCPDataToAll(fromClient, packet);
            }
        }

        public static void PlayerDisconnected(byte playerId, string scene)
        {
            bool first = true;
            foreach (Client c in Server.clients.Values)
            {
                if (c.player.id != playerId && c.player.activeScene == scene && first)
                {
                    first = false;
                    using (Packet packet = new Packet((int)ServerPackets.PlayerDisconnected))
                    {
                        packet.Write(playerId);
                        packet.Write(true);

                        Log("Sending Disconnect Packet to all clients but " + playerId);
                        //SendTCPDataToAll(playerId, packet); 
                    }
                }
                else
                {
                    using (Packet packet = new Packet((int)ServerPackets.PlayerDisconnected))
                    {
                        packet.Write(playerId);
                        packet.Write(false);

                        Log("Sending Disconnect Packet to all clients but " + playerId);
                        //SendTCPDataToAll(playerId, packet); 
                    }
                }
            }
        }

        public static void DisconnectPlayer(byte playerId)
        {
            Log("Sending Disconnect Packet to everyone");
            using (Packet packet = new Packet((int) ServerPackets.DisconnectPlayer))
            { 
                SendTCPData(playerId, packet);
            }
        }
        public static void SyncEnemy(byte toClient, string goName, int id)
        {
            using (Packet packet = new Packet((int)ServerPackets.SyncEnemy))
            {
                packet.Write(goName);
                packet.Write(id);

                SendTCPData(toClient, packet);
            }
        }

        public static void EnemyPosition(byte toClient, Vector3 position, int id)
        {
            using (Packet packet = new Packet((int)ServerPackets.EnemyPosition))
            {
                packet.Write(position);
                packet.Write(id);

                SendTCPData(toClient, packet);
            }
        }

        public static void EnemyScale(byte toClient, Vector3 scale, int id)
        {
            using (Packet packet = new Packet((int)ServerPackets.EnemyScale))
            {
                packet.Write(scale);
                packet.Write(id);

                SendTCPData(toClient, packet);
            }
        }

        public static void EnemyAnimation(byte toClient, string clipName, int id)
        {
            using (Packet packet = new Packet((int)ServerPackets.EnemyAnimation))
            {
                packet.Write(clipName);
                packet.Write(id);

                SendTCPData(toClient, packet);
            }
        }

        private static void Log(object message) => Modding.Logger.Log("[Server Send] " + message);
    }
}
