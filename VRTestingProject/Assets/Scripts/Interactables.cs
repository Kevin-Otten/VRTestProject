using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Interactables : MonoBehaviour
{
    public Controls_Interaction activeHand;

    void Start()
    {
        if (gameObject.tag != "Interactable")
        {
           gameObject.tag = "Interactable"; 
        }
    }
}
