using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

//Written by Mandy

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 3.5f;
    [SerializeField] private float jumpForce = 580f;
    [SerializeField] private float accerationSpeed = 1.5f;
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerValues playerValues;
    [SerializeField] private GameObject followCamera;
    [SerializeField] private AudioClip injureSoundClip;
    [SerializeField] private GameObject muzzleFlashGreen;

    private Animator muzzleFlashAnim;
    private Camera mainCamera;
    private Animator playerAnim;
    private GroundCheck groundCheck;
    public Rigidbody2D rb;
    private AudioSource audioSource;
    private bool muzzleFlashPosChanged = false;
    private float groundedPositionY = -3.05f;
    private bool isFallingToDie = false;
    private bool isDamageToDie = false;
    private bool isJumping = false;
    private bool isRunning = false;
    private bool isFalling = false;
    private int lives = 3;
    private float fireRate = 0.2f;
    private float canFire = -1f;
    private float startPositionX = -0.8f;
    private float fallingHeight = -5.0f;
    private float backgroundMaxHeight = -0.2f;
    private float backgroundMinHeight = -3.8f;

    private int RunID = Animator.StringToHash("IsRunning");
    private int JumpID = Animator.StringToHash("IsJumping");
    private int FallID = Animator.StringToHash("IsFalling");
    private int RunShootID = Animator.StringToHash("RunShooting");
    private int JumpShootID = Animator.StringToHash("JumpShooting");
    private int StandShootID = Animator.StringToHash("StandShooting");
    private int FallShootID = Animator.StringToHash("FallShooting");
    private int DamageDeathID = Animator.StringToHash("Damage_Death");
    private int GotShotWhileJumpingID = Animator.StringToHash("GotShotJumping");
    private int GotShotWhileFallingID = Animator.StringToHash("GotShotFalling");
    private int GotshotWhileStandingID = Animator.StringToHash("GotShotStanding");
    private int FallingDeathID = Animator.StringToHash("FallingDeath");

    private const float leftEdgeX = -8.5f, rightEdgeX = 8.5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Assert.IsNotNull(rb, "Failed to find Rigibody component.");

        Assert.IsNotNull(laserPrefab, "The laser prefab is not assigned.");

        Assert.IsNotNull(uiManager, "The UIManager is not assigned.");

        Assert.IsNotNull(gameManager, "The gameManger is not assigned.");

        groundCheck = transform.GetComponentInChildren<GroundCheck>();
        Assert.IsNotNull(groundCheck, "Failed to access the child script GroundCheck.");

        playerAnim = GetComponent<Animator>();
        Assert.IsNotNull(playerAnim, "Failed to find Animator component.");

        Assert.IsNotNull(followCamera, "No reference to Camera game object.");
        followCamera.SetActive(false);

        mainCamera = Camera.main;
        Assert.IsNotNull(mainCamera, "Failed to access Main Camera.");

        audioSource = GetComponent<AudioSource>();
        Assert.IsNotNull(audioSource, "Failed to find Audio Source component.");

        Assert.IsNotNull(injureSoundClip, "No reference to Injure Sound Clip.");

        Assert.IsNotNull(muzzleFlashGreen, "No reference to MuzzleFlashGreen game object.");
        muzzleFlashAnim = muzzleFlashGreen.transform.GetComponent<Animator>();
        Assert.IsNotNull(muzzleFlashGreen, "Failed to access to MuzzleFlashGreen's Animator");
    }
    private void Start()
    {
        transform.position = new Vector2(startPositionX, groundedPositionY);
        lives = playerValues.Lives;

        isFallingToDie = false;
        isDamageToDie = false;
    }

    private void Update()
    {
        if (transform.position.y < fallingHeight && !isFallingToDie)
        {
            isFallingToDie = true;
            audioSource.Play();
            playerAnim.SetTrigger(FallingDeathID);

            followCamera.SetActive(true);
            gameManager.AdjustParallexPosition();

            float minXPos = -4.0f;
            float maxXPos = 4.0f; 
            Vector3 cameraPos = followCamera.transform.position;
            float xPos = Mathf.Clamp(0f, minXPos, maxXPos);
            followCamera.transform.position = new Vector3(xPos, cameraPos.y, cameraPos.z);
        }


        if (transform.position.x < leftEdgeX)
            transform.position = new Vector3(leftEdgeX, transform.position.y, transform.position.z);
        else if (transform.position.x > rightEdgeX)
            transform.position = new Vector3(rightEdgeX, transform.position.y, transform.position.z);

        if (!isFallingToDie && !isDamageToDie)
        {
            Movement();
            ShootLaser();
        }
    }

    private void Movement()
    {
        if (isFallingToDie && isDamageToDie || uiManager.PauseActive)
            return;

        Time.timeScale = 1;

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            isRunning = true;
            isFalling = false;
            playerAnim.SetBool(RunID, true);
            Vector3 localScale = transform.localScale;
            localScale.x = Mathf.Abs(localScale.x) * -1f;
            transform.localScale = localScale;
            transform.Translate(Vector2.left * speed * Time.smoothDeltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            isRunning = true;
            isFalling = false;
            playerAnim.SetBool(RunID, true);
            Vector3 localScale = transform.localScale;
            localScale.x = Mathf.Abs(localScale.x);
            transform.localScale = localScale;
            transform.Translate(Vector2.right * speed * Time.smoothDeltaTime);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            isRunning = false;
            playerAnim.SetBool(RunID, false);
        }

        #region JoystickMode: Move left and right
        if (gameManager.JoystickMode)
        {
            if (Input.GetAxis("Horizontal") < 0)
            {
                isRunning = true;
                isFalling = false;
                playerAnim.SetBool(RunID, true);
                Vector3 localScale = transform.localScale;
                localScale.x = Mathf.Abs(localScale.x) * -1f;
                transform.localScale = localScale;
                transform.Translate(Vector2.left * speed * Time.smoothDeltaTime);
            }
            else if (Input.GetAxis("Horizontal") > 0)
            {
                isRunning = true;
                isFalling = false;
                playerAnim.SetBool(RunID, true);
                Vector3 localScale = transform.localScale;
                localScale.x = Mathf.Abs(localScale.x);
                transform.localScale = localScale;
                transform.Translate(Vector2.right * speed * Time.smoothDeltaTime);
            }

            if (Input.GetAxis("Horizontal") == 0)
            {
                isRunning = false;
                playerAnim.SetBool(RunID, false);
            }
        }
        #endregion JoystickMode: Move left and right


        if (groundCheck.IsGrounded)
        {
            isJumping = false;
            isFalling = false;
            playerAnim.SetBool(JumpID, false);
            playerAnim.SetBool(FallID, false);
            if (followCamera.activeInHierarchy && mainCamera.transform.position.y == followCamera.transform.position.y)
                followCamera.SetActive(false);

            if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
            {
                isJumping = true;
                playerAnim.SetBool(RunID, false);
                playerAnim.SetBool(FallID, false);
                playerAnim.SetBool(JumpID, true);
                rb.AddForce(Vector2.up * jumpForce);
                if (rb.velocity.y > 3f)
                {
                    followCamera.SetActive(true);
                    Vector3 cameraPos = followCamera.transform.position;
                    float xPos = Mathf.Clamp(0f, -4f, 4f);
                    followCamera.transform.position = new Vector3(xPos, cameraPos.y, cameraPos.z);
                }
                groundCheck.IsGrounded = false;
            }
            #region JoystickMode: Jump
            else if (gameManager.JoystickMode)
            {
                if (Input.GetKeyDown(KeyCode.Joystick1Button3))
                {
                    isJumping = true;
                    playerAnim.SetBool(RunID, false);
                    playerAnim.SetBool(FallID, false);
                    playerAnim.SetBool(JumpID, true);
                    rb.AddForce(Vector2.up * jumpForce);
                    groundCheck.IsGrounded = false;
                }
            }
            #endregion Joystick: Jump
        }


        if (rb.velocity.y < (gameManager.BackgroundChanged ? backgroundMinHeight : backgroundMaxHeight))
        {
            isFalling = true;
            isRunning = false;
            playerAnim.SetBool(FallID, true);
            rb.velocity += Vector2.up * Physics2D.gravity.y * accerationSpeed * Time.deltaTime;
        }
    }

    private void ShootLaser()
    {
        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            if (Time.time > canFire)
            {
                canFire = Time.time + fireRate;
                if (isJumping)
                    playerAnim.SetTrigger(JumpShootID);
                else if (isRunning)
                    playerAnim.SetTrigger(RunShootID);
                else if (isFalling)
                    playerAnim.SetTrigger(FallShootID);
                else
                    playerAnim.SetTrigger(StandShootID);

                if (!isRunning)
                {
                    if (!muzzleFlashPosChanged)
                        muzzleFlashAnim.SetTrigger("Shoot");
                    else
                    {
                        muzzleFlashGreen.transform.position = transform.position - new Vector3(transform.localScale.x * 0.03f, -1f, 0f);
                        muzzleFlashAnim.SetTrigger("Shoot");
                        muzzleFlashPosChanged = false;
                    }
                }
                else
                {
                    muzzleFlashGreen.transform.position = transform.position + new Vector3(transform.localScale.x * 0.05f, 0.7f, 0f);
                    muzzleFlashAnim.SetTrigger("Shoot");
                    muzzleFlashPosChanged = true;
                }

                Instantiate(laserPrefab, transform.position + new Vector3(
                    (!isRunning ? 0f : transform.localScale.x * 0.2f),
                    (!isRunning ? 1f : 0.55f),
                    0f), Quaternion.identity);
            }

        }
    }

    /// <summary>
    /// EnemyPush feature : wait for sprites/animation
    /// </summary>
    /// <param name="restaring"></param>
    //public void BeingPushedFromLeftSide()
    //{
    //    rb.AddForce(new Vector2(-2000f, 0f));
    //}

    //public void BeingPushedFromRightSide()
    //{
    //    rb.AddForce(new Vector2(2000f, 0f));
    //}

    public async void Damage(bool restaring)
    {
        if (isFalling)
        {
            AudioSource.PlayClipAtPoint(injureSoundClip, Camera.main.transform.position, 1f);
            playerAnim.SetTrigger(GotShotWhileFallingID);
        }
        else if (isJumping)
        {
            AudioSource.PlayClipAtPoint(injureSoundClip, Camera.main.transform.position, 1f);
            playerAnim.SetTrigger(GotShotWhileJumpingID);
        }
        else
        {
            AudioSource.PlayClipAtPoint(injureSoundClip, Camera.main.transform.position, 1f);
            playerAnim.SetTrigger(GotshotWhileStandingID);
        }

        lives--;
        playerValues.Lives = lives;
        uiManager.UpdateLivesImage(playerValues.Lives);


        if (lives == 0 && !isFallingToDie)
        {
            isDamageToDie = true;
            playerAnim.SetBool(DamageDeathID, true);
            await Task.Delay(1000);
            uiManager.EnableGameOverPanel();
        }

        else if (restaring)
        {
            playerValues.Lives = 0;
            uiManager.UpdateLivesImage(playerValues.Lives);
            await Task.Delay(550);
            uiManager.EnableGameOverPanel();
        }
    }
}

