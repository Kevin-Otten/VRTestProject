using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Controls_Movement : MonoBehaviour
{
    public enum MovementTypes
    {
        Smooth,
        Teleport
    }

    private SteamVR_Behaviour_Pose pose;

    //Smooth\Teleportation movement values
    public MovementTypes movementTypes;
    public SteamVR_Action_Vector2 touchPadAction;
    public bool touchPadTouch;
    public Vector2 touchpadValue;
    public bool touchPadClick;

    //Teleport Specific
    private bool recentlyTeleported;


    public static Controls_Movement activeHand;

    

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
            if (touchpadValue != Vector2.zero)
            {
                if (activeHand == this || activeHand == null)
                {
                    if (activeHand == null)
                    {
                        activeHand = this; 
                    }
                    switch (movementTypes)
                    {
                        case MovementTypes.Smooth:
                            SmoothMove();
                            break;
                        case MovementTypes.Teleport:
                            TeleportMove();
                            break;
                    }
                }
                else if(activeHand.touchpadValue == Vector2.zero)
                {
                    activeHand = null;
                }
            }
        }
        else
        {
            activeHand = null;
            if (ControlsManager.instance.teleportParticle != null && movementTypes == MovementTypes.Teleport)
            {
                if (ControlsManager.instance.teleportParticle.activeSelf)
                {
                    ControlsManager.instance.teleportParticle.SetActive(false);
                }
            }
        }
    }

    void SmoothMove()
    {
        //Define next position
        Vector3 side = ControlsManager.instance.VR_Camera.right * touchpadValue.x * ControlsManager.instance.movementSpeed * Time.deltaTime;
        Vector3 forward = ControlsManager.instance.VR_Camera.forward * touchpadValue.y * ControlsManager.instance.movementSpeed * Time.deltaTime;
        Vector3 nextPosition = forward + side;
        nextPosition.y = 0;

        //Set position to next position
        if (touchPadClick)
        {
            Debug.Log(nextPosition);
            ControlsManager.instance.VR_CameraRig.localPosition += nextPosition;
        }
    }

    void TeleportMove()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward,out hit, float.MaxValue))
        {
            ControlsManager.instance.teleportParticle.SetActive(true);
            Debug.DrawRay(transform.position, transform.forward, Color.red);
            DrawTeleportLine(hit);

            ControlsManager.instance.teleportParticle.transform.position = hit.point;
            ControlsManager.instance.teleportParticle.transform.rotation = Quaternion.LookRotation(hit.normal);

            if (touchPadClick && !recentlyTeleported)
            {
                //Move player to hit position
                recentlyTeleported = true;
                Invoke("ResetTeleportTimer",1);
                ControlsManager.instance.VR_CameraRig.localPosition = hit.point;
            }
        }
        else
        {
            if (ControlsManager.instance.teleportParticle.activeSelf)
            {
                ControlsManager.instance.teleportParticle.SetActive(false);
            }
        }
    }

    void DrawTeleportLine(RaycastHit hit)
    {

    }

    void ResetTeleportTimer()
    {
        recentlyTeleported = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position,0.1f);
    }
}