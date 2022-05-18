using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

   

[RequireComponent(typeof(NetworkMatch))]
public class MatchController : NetworkBehaviour
{
    internal readonly SyncDictionary<NetworkIdentity, MatchPlayerData> matchPlayerData = new SyncDictionary<NetworkIdentity, MatchPlayerData>();

    bool playAgain = false;

    [Header("Canvas References")]
    public CanvasGroup optionPanel;
    public CanvasGroup playerPanel;
    public GameObject createButton;

    [Header("GUI References")]
    public CanvasGroup canvasGroup;
    public Text gameText;
    public Button exitButton;

    [Header("Diagnostics - Do Not Modify")]
    public CanvasController canvasController;
    public List<NetworkIdentity>  players;
    public NetworkIdentity player1;
    public NetworkIdentity startingPlayer;

    public NetworkIdentity currentPlayer;
    public NetworkIdentity localPlayer;

    UnityAction firstButtonEvent;
    
    void Awake()
    {
        canvasController = FindObjectOfType<CanvasController>();
    }

    public override void OnStartServer()
    {
        StartCoroutine(AddPlayersToMatchController());
    }

    // For the SyncDictionary to properly fire the update callback, we must
    // wait a frame before adding the players to the already spawned MatchController
    IEnumerator AddPlayersToMatchController()
    {
        yield return null;

        for (int i = 0; i < CanvasController.playerInfos.Count; i++)
        {
            // Something went wrong here?
            if(players[i].connectionToClient != null)
                matchPlayerData.Add(players[i], new MatchPlayerData { playerIndex = CanvasController.playerInfos[players[i].connectionToClient].playerIndex });
        }
    }


    public override void OnStartClient()
    {
        StartCoroutine(AddPlayersToMatchController());

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        exitButton.gameObject.SetActive(false);
    }



    // Assigned in inspector to BackButton::OnClick
    [Client]
    public void RequestExitGame()
    {
        exitButton.gameObject.SetActive(false);
        CmdRequestExitGame();
    }

    [Command(requiresAuthority = false)]
    public void CmdRequestExitGame(NetworkConnectionToClient sender = null)
    {
        StartCoroutine(ServerEndMatch(sender, false));
    }

    public void OnPlayerDisconnected(NetworkConnection conn)
    {
        // Check that the disconnecting client is a player in this match
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] == conn.identity)
            {
                StartCoroutine(ServerEndMatch(conn, true));
            }
        }
  
    }

    public IEnumerator ServerEndMatch(NetworkConnection conn, bool disconnected)
    {
        canvasController.OnPlayerDisconnected -= OnPlayerDisconnected;

        RpcExitGame();

        // Skip a frame so the message goes out ahead of object destruction
        yield return null;

        // Mirror will clean up the disconnecting client so we only need to clean up the other remaining client.
        // If both players are just returning to the Lobby, we need to remove both connection Players

        if (!disconnected)
        {
            NetworkServer.RemovePlayerForConnection(player1.connectionToClient, true);
            CanvasController.waitingConnections.Add(player1.connectionToClient);

           // NetworkServer.RemovePlayerForConnection(player2.connectionToClient, true);
            //CanvasController.waitingConnections.Add(player2.connectionToClient);
        }
        else if (conn == player1.connectionToClient)
        {
            // player1 has disconnected - send player2 back to Lobby
           // NetworkServer.RemovePlayerForConnection(player2.connectionToClient, true);
           // CanvasController.waitingConnections.Add(player2.connectionToClient);
        }
        /*
        else if (conn == player2.connectionToClient)
        {
            // player2 has disconnected - send player1 back to Lobby
            NetworkServer.RemovePlayerForConnection(player1.connectionToClient, true);
            CanvasController.waitingConnections.Add(player1.connectionToClient);
        }
        */

        // Skip a frame to allow the Removal(s) to complete
        yield return null;

        // Send latest match list
        canvasController.SendMatchList();

        NetworkServer.Destroy(gameObject);
    }
    
    [ClientRpc]
    public void RpcExitGame()
    {
        canvasController.OnMatchEnded();
    }

    [Command(requiresAuthority = false)]
    public void CmdGetPlayers()
    {
        List<NetworkIdentity> playerList = players;

        Debug.Log(players[1].connectionToClient);
        Debug.Log(players[0]);

        RpcGetPlayer(playerList);
    }
    [ClientRpc]
    public void RpcGetPlayer( List<NetworkIdentity> playersList)
    {
        for(int i=0; i < playersList.Count; i++)
        {
            Debug.Log(playersList[i]);
        }

        //loser.gameObject.GetComponent<LocalPlayer>().root.SetActive(false);
        //winner.gameObject.GetComponent<LocalPlayer>().root.SetActive(true);
        //winner.gameObject.SetActive(false);
        //loser.gameObject.SetActive(false);
    }
    [ClientRpc]
    public void RpcSpawnItem(NetworkIdentity play)
    {
       
        //play(false);
    }
    [Command(requiresAuthority = false)]
    public void CmdSpawnItem()
    {
        List<NetworkIdentity> playerList = players;

        for (int i = 0; i < playerList.Count; i++)
        {
            FindMyself(players,i);
        }
    }
    [Command(requiresAuthority = false)]

    void FirstAbilitySlot(NetworkIdentity loser, NetworkIdentity winner)
    {
        StealHeadset(loser, winner);
    }
    [ClientRpc]
    public void StealHeadset(NetworkIdentity loser, NetworkIdentity winner)
    {   
        winner.gameObject.GetComponent<LocalPlayer>().root.SetActive(false);
        winner.gameObject.SetActive(false);
        loser.gameObject.GetComponent<LocalPlayer>().root.SetActive(true);
        loser.gameObject.SetActive(true);       
    }
    [ClientRpc]
    public void Unsteal(NetworkIdentity loser, NetworkIdentity winner)
    {
        loser.gameObject.GetComponent<LocalPlayer>().root.SetActive(false);
        winner.gameObject.GetComponent<LocalPlayer>().root.SetActive(true);
    }
    [ClientRpc]
    public void FindMyself(List<NetworkIdentity> players, int i)
    {   
        GameObject button = Instantiate(createButton, Vector3.zero, Quaternion.identity);

        button.GetComponentInChildren<TextMeshProUGUI>().text = players[i].ToString();
        RectTransform rectTransform = button.GetComponent<RectTransform>();
        rectTransform.SetParent(playerPanel.transform);

        button.GetComponent<Button>().onClick.AddListener(() => firstButtonEvent());
        Debug.Log(players[0]);
        firstButtonEvent = () => FirstAbilitySlot(players[i], players[0]);
    }
    [ClientRpc]
    public void getCurrentPlayer(NetworkIdentity help)
    {
        NetworkIdentity current = help;
        localPlayer = current;
        Debug.Log(help.GetComponent<NetworkIdentity>());
     
    }
    public void Update()
    {
     
    }
}

