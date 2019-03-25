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
    private LineRenderer lineRenderer;

    

    void Start()
    {
        pose = GetComponent<SteamVR_Behaviour_Pose>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        touchPadTouch = SteamVR_Actions.MyControllSet.TouchPad.state;
        touchPadClick = SteamVR_Actions.MyControllSet.TouchPadClick.state;

        if (touchPadTouch)
        {
            switch (movementTypes)
            {
                case MovementTypes.Smooth:
                    touchpadValue = touchPadAction.GetAxis(pose.inputSource);
                    SmoothMove();
                    break;
                case MovementTypes.Teleport:
                    ControlsManager.instance.teleportParticle.SetActive(true);
                    TeleportMove();
                    break;
            }
        }
        else if (ControlsManager.instance.teleportParticle != null && movementTypes == MovementTypes.Teleport)
        {
            if (ControlsManager.instance.teleportParticle.activeSelf)
            {
                
                ControlsManager.instance.teleportParticle.SetActive(false);
            }

            if (lineRenderer.enabled)
            {
                lineRenderer.enabled = false;
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
        if (Physics.Raycast(transform.localPosition, transform.forward,out hit, float.MaxValue))
        {
            Debug.DrawRay(transform.position, transform.forward, Color.red);
            DrawTeleportLine(hit);
            //place teleport particle on hit position
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
            lineRenderer.enabled = false;
        }
    }

    void DrawTeleportLine(RaycastHit hit)
    {
        if (lineRenderer.enabled == false)
        {
            lineRenderer.enabled = true;
            
            lineRenderer.SetPosition(0,transform.position);
            lineRenderer.SetPosition(1,hit.point);
        }
    }

    void ResetTeleportTimer()
    {
        recentlyTeleported = false;
    }
}