using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class MyNetworkManager : NetworkManager
{
    public List<GameObject> Players;
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        //Transform start = numPlayers == 0 ? leftSpawn : rightSpawn;
        GameObject player = Instantiate(playerPrefab, transform);
        Players.Add(player);      
        NetworkServer.AddPlayerForConnection(conn, player);
    }
    public override void OnServerDisconnect(NetworkConnection conn)
    {
       // NetworkServer.Destroy();
    }
    public override void OnStartServer()
    {
        Debug.Log("Server Started");
    }
    public override void OnStopServer()
    {
        Debug.Log("Server Stopped");
    }
    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("Connect");
    }
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        Debug.Log("Disconnected");
    }
}
