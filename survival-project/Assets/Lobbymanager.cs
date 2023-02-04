//using System;
//using System.Collections;
//using System.Collections.Generic;
//using Unity.Services.Authentication;
//using Unity.Services.Core;
//using Unity.Services.Lobbies;
//using Unity.Services.Lobbies.Models;
//using UnityEngine;

//public class Lobbymanager : MonoBehaviour
//{
//    private float heartbeatTimer;
//    private float lobbyPollTimer;
//    private Lobby joinedLobby;
//    private string playerName;

//    private async void Start()
//    {
//        await UnityServices.InitializeAsync();

//        AuthenticationService.Instance.SignedIn += () => {
//            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
//        };

//        await AuthenticationService.Instance.SignInAnonymouslyAsync();
//        playerName = "Testplayer" + UnityEngine.Random.Range(10, 99);
//        Debug.Log(playerName);
//    }

//    private void Update()
//    {
//        HandleLobbyHeartbeat();
//        HandleLobbyPolling();
//    }

//    private async void HandleLobbyHeartbeat()
//    {
//        if (IsLobbyHost())
//        {
//            heartbeatTimer -= Time.deltaTime;
//            if (heartbeatTimer < 0f)
//            {
//                float heartbeatTimerMax = 15f;
//                heartbeatTimer = heartbeatTimerMax;

//                Debug.Log("Heartbeat");
//                await LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
//            }
//        }
//    }

//    private async void HandleLobbyPolling()
//    {
//        if (joinedLobby != null)
//        {
//            lobbyPollTimer -= Time.deltaTime;
//            if (lobbyPollTimer < 0f)
//            {
//                float lobbyPollTimerMax = 1.1f;
//                lobbyPollTimer = lobbyPollTimerMax;

//                joinedLobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);

//                if (!IsPlayerInLobby())
//                {
//                    // Player was kicked out of this lobby
//                    Debug.Log("Kicked from Lobby!");
//                    joinedLobby = null;
//                }
//            }
//        }
//    }
//    public Lobby GetJoinedLobby()
//    {
//        return joinedLobby;
//    }
//    public bool IsLobbyHost()
//    {
//        return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
//    }

//    private bool IsPlayerInLobby()
//    {
//        if (joinedLobby != null && joinedLobby.Players != null)
//        {
//            foreach (Player player in joinedLobby.Players)
//            {
//                if (player.Id == AuthenticationService.Instance.PlayerId)
//                {
//                    //This player is in this lobby
//                    return true;
//                }
//            }
//        }
//        return false;
//    }

//    public async void CreateLobby()
//    {
//        try
//        {
//            string lobbyName = "MyLobby";
//            int maxPlayers = 4;
//            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
//            {
//                IsPrivate = false,
//                Player = GetPlayer(),
                
//            };

//            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);
//            hostLobby = lobby;

//            Debug.Log("Created Lobby! " + lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Id + " " + lobby.LobbyCode);
//        }
//        catch (LobbyServiceException e)
//        {
//            Debug.Log(e);
//        }
//    }
//}
