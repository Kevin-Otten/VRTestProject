using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class CharacterControls : MonoBehaviour
{

    public float speed = 10.0f;
    public float gravity = 10.0f;
    public bool canJump = true;
    public float jumpHeight = 2.0f;
    private bool grounded = false;
    private Rigidbody rigidbody;

    [Space(10)]
    [SerializeField]
    private float myCamRotateSpeed = 10;

    private float currentAngle;
    [SerializeField]
    private float angleLimit;
    [SerializeField]
    private bool lockMouse;

    private float xMove;
    private float yMove;
    [SerializeField]
    private float maxVelocity;
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.freezeRotation = true;
        rigidbody.useGravity = false;

    }

    void Start()
    {
        if (lockMouse)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }


    void FixedUpdate()
    {
        if (grounded)
        {
            Movement();
            // Jump
            if (canJump && Input.GetButton("Jump"))
            {
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, CalculateJumpVerticalSpeed(), rigidbody.velocity.z);
            }
        }

        // We apply gravity manually for more tuning control
        rigidbody.AddForce(new Vector3(0, -gravity * rigidbody.mass, 0));

        grounded = false;

        CameraRotation();
    }

    void OnCollisionStay()
    {
        grounded = true;
    }

    float CalculateJumpVerticalSpeed()
    {
        // From the jump height and gravity we deduce the upwards speed 
        // for the character to reach at the apex.
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }

    private void CameraRotation()
    {
        float x = Input.GetAxis("Mouse X") * myCamRotateSpeed * Time.deltaTime;
        float y = Input.GetAxis("Mouse Y") * myCamRotateSpeed * Time.deltaTime;

       

            if (y >= angleLimit)
            {
                y = angleLimit;
            }
            else if (y <= -angleLimit)
            {
                y = -angleLimit;
            }
            Camera.main.transform.eulerAngles += new Vector3(-y, 0, 0);
            transform.eulerAngles += new Vector3(0, x, 0);
    }

    private void Movement()
    {
        xMove = Input.GetAxis("Horizontal");
        yMove = Input.GetAxis("Vertical");

        Vector3 forwardMove = transform.forward * yMove * speed * Time.deltaTime;
        Vector3 sideMove = transform.right * xMove * speed * Time.deltaTime;
        Vector3 movement = forwardMove + sideMove;

           

        if (xMove == 0 && yMove == 0 && grounded)
        {
            if (rigidbody.velocity != Vector3.zero)
            {
                rigidbody.velocity = Vector3.zero;
            }
        }

        if (rigidbody.velocity.magnitude < maxVelocity)
        {
            rigidbody.velocity = movement;
        }
        else if(rigidbody.velocity.magnitude >= maxVelocity)
        {
            Vector3 velocity = rigidbody.velocity.normalized;
            velocity *= maxVelocity;
            rigidbody.velocity = velocity;

        }


        
    }
}