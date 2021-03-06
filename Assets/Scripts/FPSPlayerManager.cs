using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class FPSPlayerManager : MonoBehaviour
{
    #region Player vars
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
    public int maxHealth = 100;
    public int currHealth;
    public List<GameObject> spawnPoints = new List<GameObject>();
    public int kills = 0;

    bool groundCheck;
    float cameraPitch = 0f;

    bool forwards, backwards, left, right = false;
    Vector2 leftStick, rightStick = Vector2.zero;
    #endregion

    #region Weapon vars
    public Transform projectileSpawn;
    public GameObject projectilePrefab;
    #endregion

    #region UI
    public GameObject pauseMenu;
    public TMP_Text nameText;
    public FPSHealthBar healthBar;
    public TMP_Text healthText;
    //public TMP_InputField input;
    public TMP_Text playerNameField;
    public GameObject panel, textPrefab;
    public int maxMessages = 4;

    [SerializeField]
    public List<Message> messageList = new List<Message>();
    #endregion

    #region MISC
    public Animator weaponAnim;
    public Animator playerAnim;
    public GameObject WeaponSoundPrefab;
    public Renderer playerRenderer;
    public bool canMove = true;
    public float R, G, B;
    #endregion

    public GameObject p;

    [SerializeField]
    bool isChatting = false;

    void Awake()
    {
        R = PhotonManager.instance.RED;
        G = PhotonManager.instance.GREEN;
        B = PhotonManager.instance.BLUE;
        currHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthText.text = maxHealth.ToString();
        gameObject.GetPhotonView().RPC("FPSUsernameRPC", RpcTarget.AllBuffered);
        gameObject.GetPhotonView().RPC("ChangeColor", RpcTarget.AllBuffered, R, G, B);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.GetPhotonView().IsMine)
        {
            rb = playerObj.GetComponent<Rigidbody>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            playerNameField.text = PhotonNetwork.NickName;
            FPSGameManager.instance.input.interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        rb.useGravity = false;
        leftStick = Vector2.zero;
        rightStick = Vector2.zero;

        if (gameObject.GetPhotonView().IsMine)
        {

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                pauseMenu.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                FPSGameManager.instance.input.interactable = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                canMove = false;
                isChatting = true;
            }

            if (Input.GetKeyDown(KeyCode.Return) && isChatting)
            {
                gameObject.GetPhotonView().RPC("ChatRPC", RpcTarget.AllBuffered, playerNameField.text, FPSGameManager.instance.input.text);
                Debug.Log(FPSGameManager.instance.input.text);
                FPSGameManager.instance.input.text = "";
                FPSGameManager.instance.input.interactable = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                canMove = true;
                isChatting = false;
            }

            if (currHealth <= 0)
            {
                gameObject.GetPhotonView().RPC("isDead", RpcTarget.AllBuffered);
            }

            if (!pauseMenu.activeSelf && canMove)
            {
                GetInput();
                MoveStrafe(leftStick);
                RotateRight(rightStick.x);
                CameraPitch(rightStick.y);

                if (forwards)
                {
                    playerAnim.SetBool("IsMoving", true);
                }
                else if (backwards)
                {
                    playerAnim.SetBool("IsMoving", true);
                }
                else if (left)
                {
                    playerAnim.SetBool("IsMoving", true);
                }
                else if (right)
                {
                    playerAnim.SetBool("IsMoving", true);
                }
                else
                {

                    playerAnim.SetBool("IsMoving", false);
                }

                if (Input.GetMouseButtonDown(0))
                {
                    weaponAnim.SetBool("Fire", true);
                    FireWeapon();
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    weaponAnim.SetBool("Fire", false);
                }
            }
        }

        CheckForGround();

        if (!groundCheck)
        {
            rb.useGravity = true;
            Physics.gravity = new Vector3(0f, playerGrav, 0f);
        }
    }

    void FireWeapon()
    {
        if (gameObject.GetPhotonView().IsMine)
        {
            GameObject s = PhotonNetwork.Instantiate(WeaponSoundPrefab.name, projectileSpawn.position, projectileSpawn.rotation);
            p = PhotonNetwork.Instantiate(projectilePrefab.name, projectileSpawn.position, projectileSpawn.rotation);

            p.GetComponent<Projectile>().owner = this;

            if(!s.GetComponent<AudioSource>().isPlaying)
            {
                Destroy(s);
            }
        }
    }

    #region RPC's
    [PunRPC]
    public void TakeDamage(int damage)
    {
        currHealth -= damage;
        healthText.text = currHealth.ToString();
        healthBar.SetHealth(currHealth);
    }

    [PunRPC]
    public void isDead()
    {
        currHealth += maxHealth;
        healthText.text = currHealth.ToString();
        healthBar.SetHealth(currHealth);
        GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count - 1)];
        gameObject.transform.position = spawnPoint.transform.position;
    }

    [PunRPC]
    void FPSUsernameRPC()
    {
        nameText.text = gameObject.GetPhotonView().Owner.NickName;
    }

    [PunRPC]
    void ChangeColor(float r, float g, float b)
    {
        Color32 newCol = new Color32((byte)r, (byte)g, (byte)b, (byte)255);
        playerRenderer.material.color = newCol;
    }

    [PunRPC]
    void ChatRPC(string _username, string _chat)
    {
        string message = _username + ": " + _chat + "\n";

        Debug.Log(message);

        FPSChat.instance.SendMessageToChat(message);
    }
    #endregion

    #region Movement Methods
    void CameraPitch(float value)
    {
        if (gameObject.GetPhotonView().IsMine)
        {
            cameraPitch -= (value * vertSensitivity);
            cameraPitch = Mathf.Clamp(cameraPitch, minClamp, maxClamp);
            playerCam.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
        }
    }

    void RotateRight(float value)
    {
        if (gameObject.GetPhotonView().IsMine)
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
    #endregion
}