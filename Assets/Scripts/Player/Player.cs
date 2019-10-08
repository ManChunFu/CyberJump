using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 3.5f;
    [SerializeField] private float jumpForce = 580f;
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerLives playerLives;
    [SerializeField] private GameObject followCamera;

    private Animator playerAnim;
    private GroundCheck groundCheck;
    private Rigidbody2D rb;
    private float groundedPositionY = -4.289589f;
    private bool isFallingToDie = false;
    private int lives = 3;
    private float fireRate = 0.3f;
    private float canFire = -1f;
    private bool isJumping = false;
    private bool isRunning = false;
    private bool isFalling = false;

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
    }
    private void Start()
    {
        transform.position = new Vector2(0, groundedPositionY);
        lives = playerLives.Lives;
    }

    private void Update()
    {
        if (transform.position.y < -5f && !isFallingToDie)
        {
            isFallingToDie = true;

            playerAnim.SetTrigger("Death");
            
            followCamera.SetActive(true);
            gameManager.AdjustParallexPosition();
            
            Vector3 cameraPos = followCamera.transform.position;
            float xPos = Mathf.Clamp(0f, -4f, 4f);
            followCamera.transform.position = new Vector3(xPos, cameraPos.y, cameraPos.z);
        }
        

        if (transform.position.x < leftEdgeX)
            transform.position = new Vector3(leftEdgeX, transform.position.y, transform.position.z);
        else if (transform.position.x > rightEdgeX)
            transform.position = new Vector3(rightEdgeX, transform.position.y, transform.position.z);

        if (!isFallingToDie)
        {
            Movement();
            ShootLaser();
        }
    }


    private void Movement()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            isRunning = true;
            playerAnim.SetBool("IsRunning", true);
            Vector3 localScale = transform.localScale;
            localScale.x = Mathf.Abs(localScale.x) * -1f;
            transform.localScale = localScale;
            transform.Translate(Vector2.left * speed * Time.smoothDeltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            isRunning = true;
            playerAnim.SetBool("IsRunning", true);
            Vector3 localScale = transform.localScale;
            localScale.x = Mathf.Abs(localScale.x);
            transform.localScale = localScale;
            transform.Translate(Vector2.right * speed * Time.smoothDeltaTime);
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            isRunning = false;
            playerAnim.SetBool("IsRunning", false);
            playerAnim.SetBool("IsFalling", false);
        }


        if (groundCheck.IsGrounded)
        {
            isJumping = false;
            isFalling = false;
            playerAnim.SetBool("IsJumping", false);
            playerAnim.SetBool("IsFalling", false);

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                isJumping = true;
                playerAnim.SetBool("IsRunning", false);
                playerAnim.SetBool("IsFalling", false);
                playerAnim.SetBool("IsJumping", true);
                rb.AddForce(Vector2.up * jumpForce);
                groundCheck.IsGrounded = false;
            }
        }

     
        if (rb.velocity.y < (gameManager.BackgroundChanged ? -3.8f : -0.2f))
        {
            isFalling = true;
            playerAnim.SetBool("IsFalling", true);
            rb.velocity += Vector2.up * Physics2D.gravity.y * 1.5f * Time.deltaTime;
        }

    }

    private void ShootLaser()
    {
        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.time > canFire)
            {
                canFire = Time.time + fireRate;
                if (isJumping)
                    playerAnim.SetTrigger("JumpShooting");
                else if (isFalling)
                    playerAnim.SetTrigger("FallShooting");
                else
                    playerAnim.SetTrigger("StandShooting");
                Instantiate(laserPrefab, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
            }
        }
    }

    public async void Damage(bool restaring)
    {
        lives--;
        playerLives.Lives = lives;
        uiManager.UpdateLivesImage(playerLives);

        if (lives <= 0)
        {
            uiManager.EnableGameOverPanel();
            await Task.Delay(1000);
            Time.timeScale = 0f;
        }

        if (restaring)
            if (lives > 0)
                StartCoroutine(gameManager.RestartGame());

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "EnemyLaser")
        {
            Destroy(other.gameObject);
            Damage(false);

        }

    }

}
