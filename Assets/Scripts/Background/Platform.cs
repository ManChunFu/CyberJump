using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

//Written by Mandy

public class Platform : MonoBehaviour
{
    [SerializeField] private float limitToDestroy;
    [SerializeField] private PlayerValues playerValues;
    [SerializeField] private UIManager uiManager;

    private GroundCheck groundCheck;
    private GameManager gameManager;

    private void Start()
    {
        groundCheck = FindObjectOfType<GroundCheck>();
        Assert.IsNotNull(groundCheck, "Failed to access GroundCheck script.");

        gameManager = FindObjectOfType<GameManager>();
        Assert.IsNotNull(gameManager, "Failed to access to GameManger.");

        Assert.IsNotNull(playerValues, "Failed to access player values.");

        uiManager = FindObjectOfType<UIManager>();
        Assert.IsNotNull(uiManager, "Failed to access UIManager script.");
    }

    private void Update()
    {
        if (transform.position.y < limitToDestroy)
        {
            playerValues.Scores += 100;
            uiManager.UpdateScore(playerValues.Scores);
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.SetParent(transform);
            if (groundCheck.IsGrounded && gameManager.TimeToDestroyPlatform)
                StartCoroutine(PlatformBlinckAndDestroyRoutine());
        }
    }


    private void OnCollisionExit2D(Collision2D collision) => collision.transform.parent = null;

    private IEnumerator PlatformBlinckAndDestroyRoutine()
    {
        SpriteRenderer spriteRenderer = transform.GetComponent<SpriteRenderer>();

        yield return new WaitForSeconds(4f);

        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }
        playerValues.Scores += 100;
        uiManager.UpdateScore(playerValues.Scores);
        Destroy(gameObject);

    }
}
