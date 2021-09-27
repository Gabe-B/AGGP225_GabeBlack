using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class FPSPlayerManager : MonoBehaviour
{
    //public float speed = 5f;
    public float vertSensitivity = 1;
    public float horizSensitivity = 1;
    public float turnAngle = 90.0f;
    float xRot;
    float yRot;

    public Rigidbody rb;
    public Camera playerCam;

    public float moveSpeed = 2f;


    Vector3 movement;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetPhotonView().IsMine)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                MoveForwards(1);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                MoveForwards(-1);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                MoveRight(1);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                MoveRight(-1);
            }

            CameraRotate();
            PlayerRotate();

            /*if (cController.isGrounded)
            {
                movement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
                movement *= speed;
            }
			else
			{
                movement = new Vector3(Input.GetAxis("Horizontal"), -9.81f, Input.GetAxis("Vertical"));
                movement *= (speed * 0.8f);
                //movement.y -= 9.81f * Time.deltaTime;
            }

            cController.Move(movement * Time.deltaTime);*/
        }
    }

    public void CameraRotate()
	{
        xRot += Input.GetAxis("Mouse Y") * vertSensitivity;

        xRot = Mathf.Clamp(xRot, -turnAngle, turnAngle);

        playerCam.transform.localRotation = Quaternion.Euler(-xRot, 0, 0);
	}

    public void PlayerRotate()
	{
        yRot = Input.GetAxis("Mouse X");

        gameObject.transform.localRotation *= Quaternion.Euler(0, yRot, 0);
	}

	public void MoveForwards(int direction)
	{
        rb.MovePosition(gameObject.transform.position * moveSpeed * Time.deltaTime * direction);
	}

    public void MoveRight(int direction)
    {
        rb.MovePosition(gameObject.transform.right * moveSpeed * Time.deltaTime * direction);
    }
}
