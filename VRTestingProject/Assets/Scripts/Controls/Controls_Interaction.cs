using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
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
        if (!other.gameObject.CompareTag("Interactable") || !other.gameObject.CompareTag("Climbable"))
        {
            return;
        }
        allTouchedOjects.Add(other.gameObject.GetComponent<Interactables>());

        if (other.gameObject.CompareTag("Climbable"))
        {
            
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Interactable") || !other.gameObject.CompareTag("Climbable"))
        {
            return;
        }
        allTouchedOjects.Remove(other.gameObject.GetComponent<Interactables>());
    }

    Interactables NearestInteractable()
    {
        Interactables closest = null;

        float distance;
        float minDistance = float.MaxValue;

        foreach (Interactables interactables in allTouchedOjects)
        {
            distance = Vector3.Distance(interactables.transform.position, transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                closest = interactables;
            }
        }

        return closest;
    }

    void PickUp()
    {
        //Check for closest object
        if (allTouchedOjects.Count != 0)
        {
            currentInteractable = NearestInteractable();
        }
        else
        {
            //return if list of objects is empty
            return;
        }

        //check if object has already been grabbed ifso call drop function in that hand
        if (currentInteractable.activeHand)
        {
            currentInteractable.activeHand.Drop();
        }

        //Attach object to hand using the fixed joint
        Rigidbody targetBody = currentInteractable.GetComponent<Rigidbody>();
        myJoint.connectedBody = targetBody;

        //Set active hand value of object to this hand
        currentInteractable.activeHand = this;
    }

    void Drop()
    {
        //Check if hand has a object attatched
        if (!currentInteractable)
        {
            return;
        }

        //Set velocity of object to keep movemnt of hand
        Rigidbody targetBody = currentInteractable.GetComponent<Rigidbody>();
        targetBody.velocity = pose.GetVelocity();
        targetBody.angularVelocity = pose.GetAngularVelocity();

        //Detach the object from hand and wipe its refrences from the hand
        myJoint.connectedBody = null;
        currentInteractable.activeHand = null;
        currentInteractable = null;
    }

    void Climb()
    {
        Vector3 origHandPos = transform.position;

        //Check for closest object
        if (allTouchedOjects.Count != 0)
        {
            currentInteractable = NearestInteractable();
        }
        else
        {
            //return if list of objects is empty
            return;
        }

        //decide new height position by using the origHandPos compared to the hands current position
        Vector3 newPos;

        //newPos = origHandPos

        //Set position of the Camera rig to new height
        //ControlsManager.instance.VR_CameraRig.transform.position
    }
}