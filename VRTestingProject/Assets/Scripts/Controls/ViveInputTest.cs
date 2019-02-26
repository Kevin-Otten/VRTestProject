using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Valve.VR;

public class ViveInputTest : MonoBehaviour
{
    [Header("Actions")]
    public SteamVR_Action_Single squeezeAction;
    public SteamVR_Action_Vector2 touchPadAction;

    [Header("Input Values")]
    public bool touchPadTouch;
    public bool touchPadClick;
    public Vector2 touchpadValue;
    public bool squeezedTrigger;
    public float triggerValue;

    void Update()
    {
        CheckInputs();
    }

    void CheckInputs()
    {
        triggerValue = squeezeAction.GetAxis(SteamVR_Input_Sources.Any);

        touchpadValue = touchPadAction.GetAxis(SteamVR_Input_Sources.Any);

        touchPadTouch = SteamVR_Actions.MyControllSet.TouchPad.state;

        touchPadClick = SteamVR_Actions.MyControllSet.TouchPadClick.state;

        squeezedTrigger = SteamVR_Actions.MyControllSet.TriggerDown.state;
    }
}
