using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSyncPosition : NetworkBehaviour
{
    [SyncVar]
    private Vector3 syncPos;

    [SyncVar]
    private Quaternion syncRot;

    [SerializeField] Transform myTransform;

    [SyncVar]
    private Vector3 syncPos1;

    [SyncVar]
    private Quaternion syncRot1;

    [SerializeField] Transform myTransform1;

    [SyncVar]
    private Vector3 syncPos2;

    [SyncVar]
    private Quaternion syncRot2;

    [SerializeField] Transform myTransform2;

    void FixedUpdate()
    {
        TransmitPosition();
        LerpPosition();
    }

    void LerpPosition()
    {
        if (!isLocalPlayer)
        {
            myTransform.position = syncPos;
            myTransform.rotation = syncRot;

            myTransform1.position = syncPos1;
            myTransform1.rotation = syncRot1;

            myTransform2.position = syncPos2;
            myTransform2.rotation = syncRot2;
        }
    }

    [Command]
    void CmdProvidePositionToServer(Vector3 pos, Quaternion rot, Vector3 pos1, Quaternion rot1, Vector3 pos2, Quaternion rot2)
    {
        syncPos = pos;
        syncRot = rot;

        syncPos1 = pos1;
        syncRot1 = rot1;

        syncPos2 = pos2;
        syncRot2 = rot2;
    }

    [ClientCallback]
    void TransmitPosition()
    {
        if (hasAuthority)
        {
            CmdProvidePositionToServer(myTransform.position, myTransform.rotation,
                myTransform1.position, myTransform1.rotation,
                myTransform2.position, myTransform2.rotation);
        }
    }
}
