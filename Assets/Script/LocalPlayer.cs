using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class LocalPlayer : NetworkBehaviour
{
    [SerializeField] public GameObject root;
    public GameObject ovrCamRig;
    public Transform leftHand;
    public Transform rightHand;

    public Camera leftEye;
    public Camera rightEye;

    Vector3 pos;

    public bool hasAuth = false;
    public bool isLocal = false;
    private XRIActions controls;
    private XRIActions Controls
    {
        get
        {
            if (controls != null) { return controls; }
            return controls = new XRIActions();
        }
    }
    private void Start()
    {
        //enabled = false;
        root.SetActive(false);
        pos = transform.position;
    }
    private void Update()
    {
        if (hasAuthority)
        {
            enabled = true;
            root.SetActive(true);
        }
    }
    public override void OnStartAuthority()
    {
        hasAuth = this.hasAuthority;
        isLocal = this.isLocalPlayer;
        Debug.Log(this.hasAuthority);
        Debug.Log(this.isLocalPlayer);
  

        Controls.XRIHMD.Rotation.performed += ctx => LookHead(ctx.ReadValue<Quaternion>());
        Controls.XRIHMD.Position.performed += ctx => MoveHead(ctx.ReadValue<Vector3>());

        Controls.XRILeftHand.Position.performed += ctx => Hands(ctx.ReadValue<Vector3>());
        Controls.XRILeftHand.Rotation.performed += ctx => Hands(ctx.ReadValue<Quaternion>());

        Controls.XRIRightHand.Position.performed += ctx => rHands(ctx.ReadValue<Vector3>());
    }
    [ClientCallback]
    private void OnEnable() => Controls.Enable();
    [ClientCallback]
    private void OnDisable() => Controls.Disable();

   
    public void LHands()
    {
    }

    private void Hands(Vector3 lookAxis)
    {
        Debug.Log("Hands Moving");

        float deltaTime = Time.deltaTime;
        lookAxis = lookAxis + new Vector3(0, 1, 0);
        leftHand.transform.position = lookAxis;
    }

    private void Hands(Quaternion lookAxis)
    {
        float deltaTime = Time.deltaTime;

        leftHand.transform.rotation = lookAxis;
    }
    private void rHands(Vector3 lookAxis)
    {
        float deltaTime = Time.deltaTime;
        lookAxis = lookAxis + new Vector3(0, 1, 0);

        rightHand.transform.position = lookAxis;
        rightHand.Rotate(0f, lookAxis.x * deltaTime, 0f);

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

        ovrCamRig.transform.position = lookAxis;
        ovrCamRig.transform.Rotate(0f, lookAxis.x * deltaTime, 0f);
    }

}
