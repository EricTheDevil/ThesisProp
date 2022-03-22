using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.XR;

public class LocalPlayer : NetworkBehaviour
{
    public GameObject ovrCamRig;
    public Transform leftHand;
    public Transform rightHand;

    public Camera head;

    Vector3 pos;

    private void Start()
    {
        pos = transform.position;
    }
    void Update()
    {
        // if local player 
        if (!isLocalPlayer)
        {
            ovrCamRig.SetActive(false);
        }
        else
        {
            leftHand.localRotation = InputTracking.GetLocalRotation(XRNode.LeftHand);

        }
    }
}
