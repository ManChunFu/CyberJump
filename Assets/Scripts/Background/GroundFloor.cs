using UnityEngine;
using UnityEngine.Assertions;

//Written by Mandy
public class GroundFloor : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Player player;

    private Vector2 origianlPosition;
    private AudioSource hitGroundClip;
    [SerializeField]private Animator playerAnim;

    private float distance;

    private void Awake()
    {
        Assert.IsNotNull(gameManager, "No reference to GameManager script.");

        Assert.IsNotNull(player, "No refernce to GameManager script.");

        hitGroundClip = GetComponent<AudioSource>();
        Assert.IsNotNull(hitGroundClip, "Failed to find AudioSource component.");

        Assert.IsNotNull(playerAnim, "No reference to Player's Animator component.");
    }

    private void Start()
    {
        origianlPosition = transform.position;
    }

    private void Update()
    {
        distance = Vector2.Distance(origianlPosition, transform.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (gameManager.NotFirstFloor)
        {
            gameManager.NotFirstFloor = false;
            hitGroundClip.Play();
            playerAnim.SetTrigger("HitGroundDeath");
            player.Damage(true);
        }
    }

    public void Reset()
    {
        transform.position = origianlPosition - new Vector2(0f, distance);
    }
}
