using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsManager : MonoBehaviour
{
    public static ControlsManager instance;


    public Transform VR_Camera;
    public Transform VR_CameraRig;
    public float movementSpeed;
    public GameObject teleportParticle;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (teleportParticle != null) 
        {
            teleportParticle = Instantiate(teleportParticle, Vector3.zero, Quaternion.identity);
        }
        else
        {
            Debug.LogError("No teleport particle has been assigned in the ControlsManager");
        }
    }

    void Update()
    {
        
    }
}
