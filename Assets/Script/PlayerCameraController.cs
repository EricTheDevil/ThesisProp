using Cinemachine;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerCameraController : NetworkBehaviour
{
    [Header("Camera")]
    [SerializeField] private Vector2 maxFollowOffset = new Vector2(-1f, 6f);
    [SerializeField] private Vector2 cameraVelocity = new Vector2(4f, 0.25f);
    [SerializeField] private Transform playerTransform = null;
    [SerializeField] private GameObject virtualCamera = null;
    [SerializeField] private GameObject playerLeftHand = null;
    [SerializeField] private GameObject playerRightHand = null;



    private XRIActions actions;
    private XRIActions Actions
    {
        get
        {
            if (actions != null) { return actions; }
            return actions = new XRIActions();
        }
    }
    private Controls controls;
    private Controls Controls
    {
        get
        {
            if (controls != null) { return controls; }
            return controls = new Controls();
        }
    }

    // QUESTIONS!!!
    private GameObject transposer;

    public override void OnStartAuthority()
    {
        GUIConsole.print("HAS AUTHORITY");
        transposer = virtualCamera;

        virtualCamera.gameObject.SetActive(true);

        enabled = true;


        //Controls.Player.Look.performed += ctx => Look(ctx.ReadValue<Vector2>());
        //Actions.XRIHMD.Move.performed += ctx => Look(ctx.ReadValue<Vector2>());
        Actions.XRIHMD.Rotation.performed += ctx => LookHead(ctx.ReadValue<Quaternion>());
        Actions.XRIHMD.Position.performed += ctx => MoveHead(ctx.ReadValue<Vector3>());

        Actions.XRILeftHand.Position.performed += ctx => Hands(ctx.ReadValue<Vector3>());
        Actions.XRILeftHand.Rotation.performed += ctx => Hands(ctx.ReadValue<Quaternion>());

        Actions.XRIRightHand.Position.performed += ctx => rHands(ctx.ReadValue<Vector3>());
      //  Actions.XRIRightHand.Position.performed += ctx => rHands(ctx.ReadValue<Quaternion>());

    }
    [ClientCallback]
    private void OnEnable() => Actions.Enable();
    [ClientCallback]
    private void OnDisable() => Actions.Disable();

    private void Hands(Vector3 lookAxis)
    {
        float deltaTime = Time.deltaTime;
        lookAxis = lookAxis + new Vector3(0, 1, 0);
        playerLeftHand.transform.position = lookAxis;
    }
    private void Hands(Quaternion lookAxis)
    {
        float deltaTime = Time.deltaTime;

        playerLeftHand.transform.rotation = lookAxis;
    }
    private void rHands(Vector3 lookAxis)
    {
        float deltaTime = Time.deltaTime;
        lookAxis = lookAxis + new Vector3(0, 1, 0);

        playerRightHand.transform.position = lookAxis;
        playerTransform.Rotate(0f, lookAxis.x* deltaTime, 0f);

    }

    private void Look(Quaternion lookAxis)
    {
        float deltaTime = Time.deltaTime;

        GUIConsole.print(lookAxis);
    }
    private void LookHead(Quaternion lookAxis)
    {
        float deltaTime = Time.deltaTime;

        //playerTransform.rotation = lookAxis;


        //  UnityEngine.XR.InputDevice handR = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
       // InputDevice hed = InputDevices.GetDeviceAtXRNode(XRNode.Head);
        //hed.TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceRotation, out Quaternion rotR);
        //playerTransform.rotation = rotR;
        //hed = InputDevices.GetDeviceAtXRNode(XRNode.Head);
        //playerTransform.rotation = (0f,lookAxis.normalized.x * cameraVelocity.x * deltaTime, 0f,0f);
    }
    private void MoveHead(Vector3 lookAxis)
    {
        float deltaTime = Time.deltaTime;

        playerTransform.position = lookAxis;
        playerTransform.Rotate(0f, lookAxis.x * cameraVelocity.x * deltaTime, 0f);
    }
}
