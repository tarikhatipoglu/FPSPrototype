using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMoving : MonoBehaviour
{
    
    public float speed;
    public float sprintSpeed;
    public float crouchSpeed;
    public float gravity;
    public float jump;

    Vector3 velocity;
    public CharacterController controller;
    public Transform groundCheck;
    public float groundDistance;
    public LayerMask groundMask;
    public bool isGrounded;

    public Camera cameraFOV;
    public float FOVstart = 50f;
    public float FOVmax = 70f;
    public float FOVzoom = 15f;
    public float cameraZoomTime;

    public bool paused = false;
    public GameObject pauseText;

    public int missionCount;
    public float missionTime = 5;

    public GameObject mission1;
    public GameObject mission2;
    public GameObject mission3;
    public GameObject mission4;
    public GameObject mission5;
    public GameObject mission6;
    public TextMeshProUGUI missionText;

    public GameObject inventory;
    public bool inventoryBool = false;

    //public bool gunBool;
    //public GameObject gunObject;

    public float leanAngle = 45;
    public float leanSpeed = 5;
    public float leanBackSpeed = 6;

    CharacterController characterCollider;
    public float heightStand = 2.5f;
    public float heightCrouch = 4.3f;

    void Start()
    {
        characterCollider = gameObject.GetComponent<CharacterController>();
        MissionSystem();
        missionCount = 0;
        paused = false;
        pauseText.SetActive(false);
        inventoryBool = false;
        inventory.SetActive(false);
    }

    
    void Update()
    {
        //Crouch system
        if(Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftShift))
        {
            characterCollider.height = 2.5f;
            speed = 5f;
        }
        else if(!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftShift))
        {
            characterCollider.height = 4.3f;
            speed = 10f;
        }

        //To lean left or right
        if (Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.LeftShift))
        {
            LeanLeft();
        }
        if(Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.LeftShift))
        {
            LeanRight();
        }
        if(!Input.GetKey(KeyCode.Q) || !Input.GetKey(KeyCode.E))
        {
            LeanNormal();
        }

        //To fire
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            FireBullet();
        }

        //To check if character is grounded or not
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //To make character move left and right
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        //To make smooth moves

        //To control character's speed and moving
        controller.Move(move * speed * Time.deltaTime);

        //Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jump * -2f * gravity);
        }

        //Sprint
        if(Input.GetKey(KeyCode.LeftShift) && isGrounded && Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.Mouse1) && !Input.GetKey(KeyCode.LeftControl))
        {
            speed = Mathf.SmoothStep(speed, sprintSpeed, 30 * Time.deltaTime);
            cameraFOV.fieldOfView = Mathf.Lerp(cameraFOV.fieldOfView, FOVmax, cameraZoomTime);
        }
        if(speed > 10 && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.W))
        {
            speed -= 40 * Time.deltaTime;
            cameraFOV.fieldOfView = Mathf.Lerp(cameraFOV.fieldOfView, FOVstart, cameraZoomTime);
        }

        //For gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //For zooming while not sprinting
        if(Input.GetKey(KeyCode.Mouse1) && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKeyDown(KeyCode.Escape))
        {
            cameraFOV.fieldOfView = Mathf.Lerp(cameraFOV.fieldOfView, FOVzoom, cameraZoomTime);
        }
        if (!Input.GetKey(KeyCode.Mouse1))
        {
            cameraFOV.fieldOfView = Mathf.Lerp(cameraFOV.fieldOfView, FOVstart, cameraZoomTime);
        }

        //For pause button
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
            if (paused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }

        if(missionTime > 0)
        {
            missionText.enabled = true;
            missionTime -= Time.deltaTime;
        }
        if(missionTime <= 0)
        {
            missionText.enabled = false;
        }

        if(Input.GetKeyDown(KeyCode.Tab) && !paused)
        {

            inventoryBool = !inventoryBool;
            if (inventoryBool)
            {
                InventoryOpen();
            }
            else
            {
                InventoryClose();
            }
        }
    }

    //Leaning system
    void LeanLeft()
    {
        float currentAngle = transform.rotation.eulerAngles.z;
        float targetAngle = leanAngle;

        if(currentAngle > 180.0)
        {
            currentAngle = 360 - currentAngle;
        }
        float angle = Mathf.Lerp(currentAngle, targetAngle, leanSpeed * Time.deltaTime);

        Quaternion rotationAngle = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, angle);
        transform.rotation = rotationAngle;
    }
    void LeanRight()
    {
        float currentAngle = transform.rotation.eulerAngles.z;
        float targetAngle = leanAngle - 360;

        if (currentAngle > 180.0)
        {
            targetAngle = 360 - leanAngle;
        }
        float angle = Mathf.Lerp(currentAngle, targetAngle, leanSpeed * Time.deltaTime);

        Quaternion rotationAngle = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, angle);
        transform.rotation = rotationAngle;
    }
    void LeanNormal()
    {
        float currentAngle = transform.rotation.eulerAngles.z;
        float targetAngle = 0;

        if(currentAngle > 180.0)
        {
            targetAngle = 360;
        }
        float angle = Mathf.Lerp(currentAngle, targetAngle, leanBackSpeed * Time.deltaTime);

        Quaternion rotationAngle = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, angle);
        transform.rotation = rotationAngle;
    }

    //To make pause system basically
    void Resume()
    {
        Debug.Log("Pause menu is disabled");
        pauseText.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
    }
    void Pause()
    {
        Debug.Log("Pause menu is enabled");
        pauseText.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
    }

    //Inventory menu
    void InventoryOpen()
    {
        inventory.SetActive(true);
        inventoryBool = true;
    }
    void InventoryClose()
    {
        inventory.SetActive(false);
        inventoryBool = false;
    }

    //Mission system
    public void OnTriggerEnter(Collider vol)
    {
        if(vol.gameObject.tag == "Mission")
        {
            missionTime = 5;
            missionCount++;
            missionTime -= Time.deltaTime;
            MissionSystem();
        }
    }

    void MissionSystem()
    {
        if(missionCount == 0)
        {
            Debug.Log("Complete the First Mission");
            missionText.text = "Complete the First Mission";
        }
        else if (missionCount == 1)
        {
            Debug.Log("First Mission ended, get to the Second Mission");
            missionText.text = "First Mission ended, get to the Second Mission";
            mission1.SetActive(false);
            mission2.SetActive(true);
        }
        else if (missionCount == 2)
        {
            Debug.Log("Second Mission ended, get to the Third Mission");
            missionText.text = "Second Mission ended, get to the Third Mission";
            mission2.SetActive(false);
            mission3.SetActive(true);
        }
        else if (missionCount == 3)
        {
            Debug.Log("Third Mission ended, get to the Fourth Mission");
            missionText.text = "Third Mission ended, get to the Fourth Mission";
            mission3.SetActive(false);
            mission4.SetActive(true);
        }
        else if (missionCount == 4)
        {
            Debug.Log("Fourth Mission ended, get to the Fifth Mission");
            missionText.text = "Fourth Mission ended, get to the Fifth Mission";
            mission4.SetActive(false);
            mission5.SetActive(true);
        }
        else if(missionCount == 5)
        {
            Debug.Log("Fifth Mission ended, get to the Final Mission");
            missionText.text = "Fifth Mission ended, get to the Final Mission";
            mission5.SetActive(false);
            mission6.SetActive(true);
        }
        else if(missionCount == 6)
        {
            Debug.Log("Final Mission ended, congratulations the game is finished");
            missionText.text = "Final Mission ended, congratulations the game is finished";
            mission6.SetActive(false);
        }
        else if (missionCount > 6)
        {
            missionCount = 6;
        }
        else
        {
            missionCount = 1;
        }
    }

    void FireBullet()
    {
        
    }
}
