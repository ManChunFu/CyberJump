using UnityEngine;
using UnityEngine.Assertions;


public class BackgroundMoving : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float movingSpeed; // Let 2D artists to desgin each background's scrolling speed
    [SerializeField] private Transform groundFloor;
    [SerializeField] private Player player;

    private float originalDistanceToGround;
    private float playerDistanceToGround;
    private float resetSpeed;
    private bool reseting;
    private const float MinHeightFall = 30.0f;
    private float acceration = 1.0f;
    private float accerationIncrement = 0.02f;

    private void Awake()
    {
        Assert.IsNotNull(groundFloor, "No reference to GroundFloor.");

        Assert.IsNotNull(player, "No referece to Player.");
    }
    private void Start()
    {
        originalDistanceToGround = Vector2.Distance(transform.position, groundFloor.position);
    }

    private void Update()
    {
        //Debug.Log(Vector2.Distance(player.transform.position, groundFloor.position));
        // Adjust the background moving down speed to match up the player's falling speed
        if (reseting)
        {
            if (Vector2.Distance(transform.position, groundFloor.position) <= originalDistanceToGround)
            {
                transform.position -= new Vector3(0, Vector2.Distance(transform.position, groundFloor.position) - originalDistanceToGround, 0);
                reseting = false;
                return;
            }
            transform.Translate(Vector2.down * ((playerDistanceToGround < MinHeightFall) ?  resetSpeed : resetSpeed * acceration* Time.deltaTime));
            acceration += accerationIncrement;
        }
    }
    public void MoveDown()
    {
        if (!reseting)
            transform.Translate(Vector2.down * (Time.deltaTime * movingSpeed));
    }

    public void EnableReset(bool enable)
    {
        reseting = enable;
    }

    public void SetResetSpeed(float backgroundDistance, float playerDistance)
    {
        playerDistanceToGround = playerDistance;
                                                                         // how much time it takes for the player to touch the ground 
        resetSpeed = (backgroundDistance - originalDistanceToGround) / Mathf.Sqrt(playerDistanceToGround / Mathf.Abs(player.rb.velocity.y));
    }
}
