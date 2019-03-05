using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactables : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.tag != "Interactable")
        {
           gameObject.tag = "Interactable"; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
