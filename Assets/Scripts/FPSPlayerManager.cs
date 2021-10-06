using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class FPSPlayerManager : MonoBehaviour
{
    public GameObject playerObj;
    public Rigidbody rb;
    public Transform playerCam;
    public float vertSensitivity = 100f;
    public float horizSensitivity = 100f;
    public float moveSpeed = 5.0f;
    public float playerGrav = -9.81f;
    public float minClamp = -89.9f;
    public float maxClamp = 89.9f;
    public float groundCheckDist = 1.0f;
    public float groundCheckDeadZone = 0.05f;
    public float stepHeight = 0.5f;

    public GameObject pauseMenu;
    public TMP_Text nameText;

    public static FPSPlayerManager instance;

    bool groundCheck;
    float cameraPitch = 0f;

    bool forwards, backwards, left, right = false;
    Vector2 leftStick, rightStick = Vector2.zero;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.GetPhotonView().IsMine)
        {
            rb = playerObj.GetComponent<Rigidbody>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        rb.useGravity = false;
        leftStick = Vector2.zero;
        rightStick = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            pauseMenu.SetActive(true);
        }

        if (!pauseMenu.activeSelf)
        {
            GetInput();
            MoveStrafe(leftStick);
            RotateRight(rightStick.x);
            CameraPitch(rightStick.y);
        }

        CheckForGround();

        if (!groundCheck)
        {
            rb.useGravity = true;
            Physics.gravity = new Vector3(0f, playerGrav, 0f);
        }
    }

    void CameraPitch(float value)
    {
        if(gameObject.GetPhotonView().IsMine)
        {
            cameraPitch -= (value * vertSensitivity);
            cameraPitch = Mathf.Clamp(cameraPitch, minClamp, maxClamp);
            playerCam.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
        }
    }

    void RotateRight(float value)
    {
        if(gameObject.GetPhotonView().IsMine)
        {
            gameObject.transform.Rotate(Vector3.up * value * Time.deltaTime * horizSensitivity);
        }   
    }

    void GetInput()
    {
        if (gameObject.GetPhotonView().IsMine)
        {
            leftStick = Vector2.zero;
            rightStick = Vector2.zero;
            forwards = Input.GetKey(KeyCode.W);
            backwards = Input.GetKey(KeyCode.S);
            right = Input.GetKey(KeyCode.D);
            left = Input.GetKey(KeyCode.A);

            rightStick.x = Input.GetAxis("Mouse X");
            rightStick.y = Input.GetAxis("Mouse Y");
            KeyToAxis();
        }
    }

    void KeyToAxis()
    {
        if (forwards)
        {
            leftStick.y = 1;
        }
        if (backwards)
        {
            leftStick.y = -1;
        }
        if (right)
        {
            leftStick.x = 1;
        }
        if (left)
        {
            leftStick.x = -1;
        }

    }

    void MoveStrafe(Vector2 value)
    {
        if (gameObject.GetPhotonView().IsMine)
        {
            MoveStrafe(value.x, value.y);
        }
    }

    void MoveStrafe(float horizontal, float vertical)
    {
        rb.velocity = Vector3.zero;
        rb.velocity += (playerObj.transform.forward * vertical * moveSpeed);
        rb.velocity += (playerObj.transform.right * horizontal * moveSpeed);
    }

    void CheckForGround()
    {
        RaycastHit hit;
        bool result = Physics.Raycast(gameObject.transform.position, Vector3.down, out hit, groundCheckDist);
        Debug.DrawRay(gameObject.transform.position, Vector3.down, new Color(165, 23, 57), groundCheckDist);
        groundCheck = result;

        if (result)
        {
            if (hit.distance < groundCheckDeadZone)
            {
                return;
            }
            if (hit.distance < stepHeight)
            {
                gameObject.transform.position = hit.point;
            }
        }
    }
}
