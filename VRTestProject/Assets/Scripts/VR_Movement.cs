using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VR_Movement : MonoBehaviour
{
    private SteamVR_TrackedObject trackedObject;

    void Start()
    {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
    }

    void Update()
    {
        
    }
}
