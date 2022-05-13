using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public NetworkIdentity currentPlayer;
    public bool isLeader;
    public static GameManager Instance { get { return _instance; } }
}
