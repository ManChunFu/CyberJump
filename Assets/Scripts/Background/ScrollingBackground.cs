using UnityEngine;
using UnityEngine.Assertions;

//Written by Mandy

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] private float speed = 0.02f;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Transform ground;
    [SerializeField] private Player player;
    Vector2 offset;
    private bool reseting;
    private float originalDistanceToGround;
    private float acceration = 8.0f;
    private void Awake()
    {
        Assert.IsNotNull(gameManager, "No reference to GameManager.");

        Assert.IsNotNull(ground, "No reference to Ground.");

        Assert.IsNotNull(player, "No reference to Player.");
    }
    private void Start()
    {
        originalDistanceToGround = Vector2.Distance(transform.position, ground.position);
    }
    private void Update()
    {
        if (gameManager.BackgroundChanged == true && !reseting)
        {
            if (this.transform.position.y <= -11.0f)
            {
                offset = new Vector2(0f, 11.0f);
            }
            else
            {
                offset = new Vector2(0f, this.transform.position.y - Time.deltaTime * speed);
            }
            this.transform.position = offset;
        }
        
        if (reseting)
        {
            if (Vector2.Distance(transform.position, ground.position) <= originalDistanceToGround)
            {
                transform.position -= new Vector3(0, Vector2.Distance(transform.position, ground.position) - originalDistanceToGround, 0);
                reseting = false;
                return;
            }
            transform.Translate(Vector2.down * speed  * acceration * Time.deltaTime);
        }
    }

    public void EnableReset(bool enable)
    {
        reseting = enable;
    }

    public void SetResetSpeed(float backgroundDistance, float playerDistance)
    {
        speed = (backgroundDistance - originalDistanceToGround) / Mathf.Sqrt(playerDistance / Mathf.Abs(player.rb.velocity.y));
    }

}
