using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class MovementProvider : LocomotionProvider
{
    public List<XRController> controllers = null;
    private CharacterController characterController = null;
    private Camera head = null;

    protected override void Awake()
    {
        characterController = GetComponent<CharacterController>();
        head = GetComponent<XROrigin>().Camera;
    }
    // Start is called before the first frame update
    void Start()
    {
        PositionController();
    }
    private void PositionController()
    {
        // Get the head in local playspace ground.
        float headHeight = Mathf.Clamp(head.transform.localPosition.y, 1, 2);

        // Cut in half, add skin
        /*
        Vector3 newCenter = Vector3.zero;
        newCenter.y = characterController.height / 2;
        newCenter.y = characterController.skinWidth;

        // move the capsule in local space as well;

        newCenter.x = head.transform.localPosition.x;
        newCenter.z = head.transform.localPosition.z;

        // apply
        characterController.center = newCenter;
        */
    }
    // Update is called once per frame
    void Update()
    {
        PositionController();
        CheckForInput();
        ApplyGravity();
    }
    void CheckForInput()
    {
        foreach(XRController controller in controllers)
        {
            if (controller.enableInputActions)
                CheckForMovement(controller.inputDevice);
        }
    }

    void CheckForMovement(InputDevice device)
    {
        if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 position))
            StartMove(position);
    }
    void StartMove(Vector2 position)
    {
        // Apply the touch position to the head's forward Vector

        // Rotate the input direction by the horizontal head rotation

        // Apply speed and move
    }
    void ApplyGravity()
    {

    }
}
