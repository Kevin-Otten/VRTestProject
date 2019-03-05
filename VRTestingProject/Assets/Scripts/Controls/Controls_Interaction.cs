using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Controls_Interaction : MonoBehaviour
{
    public SteamVR_Action_Single squeezeAction;
    public SteamVR_Action_Boolean squeezed;

    public float triggerValue;

    FixedJoint myJoint;
    private SteamVR_Behaviour_Pose pose;
    Interactables currentInteractable;
    public List<Interactables> allTouchedOjects = new List<Interactables>();

    void Awake()
    {
        myJoint = GetComponent<FixedJoint>();
        pose = GetComponent<SteamVR_Behaviour_Pose>();
    }

    void Update()
    {
        //Down
        if (squeezed.GetStateDown(pose.inputSource))
        {
            PickUp();
        }

        //Up
        if (squeezed.GetStateUp(pose.inputSource))
        {
            Drop();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Interactable"))
        {
            return;
        }
        allTouchedOjects.Add(other.gameObject.GetComponent<Interactables>());
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Interactable"))
        {
            return;
        }
        allTouchedOjects.Remove(other.gameObject.GetComponent<Interactables>());
    }

    Interactables NearestInteractable()
    {
        Interactables closest = null;

        return closest;
    }

    void PickUp()
    {

    }

    void Drop()
    {

    }
}
