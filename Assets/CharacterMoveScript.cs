using System.Numerics;
using UnityEngine;
using UnityEngine.AI;


public class CharacterMoveScript : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;
    public Transform head;
    public Camera mainCamera;

    [Header("Configurations")]
    public float walkSpeed;
    public float runSpeed;

    [Header("Character Steps")]
    [SerializeField] GameObject StepLowerRaycaster;
    [SerializeField] GameObject StepUpperRaycaster;

    [SerializeField] float stepHeight = 0.6f;
    [SerializeField] float stepSmooth = 0.1f;

    public NavMeshAgent agent;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        StepUpperRaycaster.transform.position = new UnityEngine.Vector3(StepUpperRaycaster.transform.position.x, stepHeight, StepUpperRaycaster.transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {
        // horizontal rotation
        transform.Rotate(UnityEngine.Vector3.up * Input.GetAxis("Mouse X") * 10f);

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }
    }

    void FixedUpdate()
    {
        UnityEngine.Vector3 newVelocity = UnityEngine.Vector3.up * rb.linearVelocity.y;
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        newVelocity.x = Input.GetAxis("Horizontal") * speed;
        newVelocity.z = Input.GetAxis("Vertical") * speed;
        rb.linearVelocity = transform.TransformDirection(newVelocity);
        handleStepClimb();
    }

    void LateUpdate()
    {
        // vertical rotation
        UnityEngine.Vector3 e = head.eulerAngles;
        e.x -= Input.GetAxis("Mouse Y") * 10f;
        e.x = RestrictHeadAngle(e.x, -85, 85);
        head.eulerAngles = e;

    }

    void handleStepClimb()
    {
        RaycastHit hitLower;
        // This will: Init a raycast, check the lower game object, extend forwards
        // (outwards), and check for something to hit according to the set distance.
        if (Physics.Raycast(StepLowerRaycaster.transform.position, transform.TransformDirection(UnityEngine.Vector3.forward), out hitLower, 0.2f))
        {
            RaycastHit hitUpper;
            // make sure the thing is not too tall
            if (!Physics.Raycast(StepUpperRaycaster.transform.position, transform.TransformDirection(UnityEngine.Vector3.forward), out hitUpper, 0.3f))
            {
                rb.position -= new UnityEngine.Vector3(0f, -stepSmooth, 0f);
            }
        }
    }

    public static float RestrictHeadAngle(float angle, float angleMin, float angleMax)
    {
        if (angle > 180)
        {
            angle -= 360;
        }
        else if (angle < -180)
        {
            angle += 360;
        }

        if (angle > angleMax)
        {
            angle = angleMax;
        }
        else if (angle < angleMin)
        {
            angle = angleMin;
        }

        return angle;
    }
}
