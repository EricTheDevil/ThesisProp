using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayerController : NetworkBehaviour
{

    [SerializeField] private Vector3 movement = new Vector3();

    
    [Client]
    void Update()

    {
        if (!hasAuthority) { return; }
        if (!Input.GetKeyDown(KeyCode.Space)) { }

        transform.Translate(movement);
    }
}
