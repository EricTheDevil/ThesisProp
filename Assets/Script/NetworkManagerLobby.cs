using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManagerLobby : NetworkManager
{

    [SerializeField] private int minPlayers = 2;
    [Scene] [SerializeField] private string menuScene = string.Empty;

    [Header("Room")]
    [SerializeField] private NetworkRoomPlayerLobby roomPlayerPrefab = null;

    [Header("Game")]
    [SerializeField] private NetworkGamePlayerLobby gamePlayerPrefab = null;
    [SerializeField] private GameObject playerSpawnSystem = null;


    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;
    public static event Action<NetworkConnection> OnServerReadied;

    // Not in original
    public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();

    public List<NetworkRoomPlayerLobby> RoomPlayers { get; } = new List<NetworkRoomPlayerLobby>();
    public List<NetworkGamePlayerLobby> GamePlayers { get; } = new List<NetworkGamePlayerLobby>();

    public override void OnStartClient()
    {
        Debug.Log("START");
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

        foreach(var prefab in spawnablePrefabs)
        {
            NetworkClient.RegisterPrefab(prefab);
        }
    }
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        OnClientConnected?.Invoke();
    }
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        OnClientDisconnected?.Invoke();
    }
    public override void OnServerConnect(NetworkConnection conn)
    {
        // disconnect if we have too many players
        /*
        if (numPlayers >= maxConnections)
        {
            conn.Disconnect();
            return;
        }
        */
        Debug.Log("Server connected");


        if (SceneManager.GetActiveScene().name != menuScene)
        {
            //conn.Disconnect();
            return;
        }
        
    }
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if (conn.identity != null)
        {
            var player = conn.identity.GetComponent<NetworkRoomPlayerLobby>();

            RoomPlayers.Remove(player);        
        }

        base.OnServerDisconnect(conn);
    }
    public override void OnServerAddPlayer(NetworkConnection conn)
    {

        Debug.Log("New player");
        // if no one else exist then we assign ourself as leader
        bool isLeader = RoomPlayers.Count == 0;
        NetworkRoomPlayerLobby roomPlayerInstance = Instantiate(roomPlayerPrefab);
        roomPlayerInstance.IsLeader = isLeader;

        NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);

    }


    public override void OnStopServer()
    {
        RoomPlayers.Clear();
    }
    public void StartGame()
    {
        ServerChangeScene("Game");
    }
    public override void ServerChangeScene(string newSceneName)
    {
        Debug.Log("E");
        if (SceneManager.GetActiveScene().name == menuScene )
        {
            for (int i = RoomPlayers.Count - 1; i >= 0; i--)
            {
                var conn = RoomPlayers[i].connectionToClient;
                var gameplayerInstance = Instantiate(gamePlayerPrefab);
                gameplayerInstance.SetDisplayName(RoomPlayers[i].DisplayName);

                //NetworkServer.Destroy(conn.identity.gameObject);

                //NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject);
            }
        }

        base.ServerChangeScene(newSceneName);
    }
    public override void OnServerSceneChanged(string sceneName)
    {      
        if (sceneName.StartsWith("Game"))
        {
            GameObject playerSpawnSystemInstance = Instantiate(playerSpawnSystem);
            NetworkServer.Spawn(playerSpawnSystemInstance);
        }
        base.OnServerSceneChanged(sceneName);
    }
    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);

        OnServerReadied?.Invoke(conn);
    }
}
