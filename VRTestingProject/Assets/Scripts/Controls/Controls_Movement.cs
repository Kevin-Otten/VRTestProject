using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Controls_Movement : MonoBehaviour
{
    private SteamVR_Behaviour_Pose pose;

    //Smooth\Teleportation movement values
    public SteamVR_Action_Vector2 touchPadAction;
    public bool touchPadTouch;
    public Vector2 touchpadValue;
    public bool touchPadClick;

    void Start()
    {
        pose = GetComponent<SteamVR_Behaviour_Pose>();
    }

    void Update()
    {
        touchPadTouch = SteamVR_Actions.MyControllSet.TouchPad.state;
        touchPadClick = SteamVR_Actions.MyControllSet.TouchPadClick.state;

        if (touchPadTouch)
        {
            touchpadValue = touchPadAction.GetAxis(pose.inputSource);
            Move();
        }
    }

    void Move()
    {
        //Get next position
        Vector3 nextPosition = ControlsManager.instance.VR_Camera.right * (touchpadValue.x * ControlsManager.instance.movementSpeed) + ControlsManager.instance.VR_Camera.forward * (touchpadValue.y * ControlsManager.instance.movementSpeed) * Time.deltaTime;
        nextPosition.y = 0;
        nextPosition += ControlsManager.instance.VR_Camera.transform.position;

        //Set position to next position
        if (touchPadClick)
        {
            ControlsManager.instance.VR_CameraRig.position += nextPosition;
        }
    }
}
