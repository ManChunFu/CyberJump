using UnityEngine;
using UnityEngine.Assertions;

//Written by Mandy

public class EnemyDies : MonoBehaviour
{
    [SerializeField] private PlayerValues playerValues;
    private UIManager uiManager;
    private Animator invaderExplosionAnim;
    private EnemyInvader invaderScript;
    private SpriteRenderer spriteRenderer;
    private UnityEngine.Experimental.Rendering.LWRP.Light2D lt;
    private AudioSource explosionClip;
    

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
        Assert.IsNotNull(uiManager, "No reference to UIManager script.");

        Assert.IsNotNull(playerValues, "No reference to PlayerValues scriptable object.");

        invaderExplosionAnim = GetComponentInParent<Animator>();
        Assert.IsNotNull(invaderExplosionAnim, "Failed to access to EnemyInvader's Animator.");

        invaderScript = GetComponentInParent<EnemyInvader>();
        Assert.IsNotNull(invaderScript, "Failed to access to EnemyInvader's script.");

        spriteRenderer = GetComponent<SpriteRenderer>();
        Assert.IsNotNull(spriteRenderer, "Failed to find Sprite Renderer component.");

        lt = GetComponentInChildren<UnityEngine.Experimental.Rendering.LWRP.Light2D>();
        Assert.IsNotNull(spriteRenderer, "Failed to find child light.");

        explosionClip = GetComponentInParent<AudioSource>();
        Assert.IsNotNull(explosionClip, "Failed to access to parent's Audio Source component.");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Laser")
        {
            this.gameObject.GetComponent<Collider2D>().enabled = false;
            Destroy(collision.gameObject);
            explosionClip.Play();
            invaderExplosionAnim.SetTrigger("Death");
            spriteRenderer.enabled = false;
            lt.intensity = 0f; //Kevin's code
            Destroy(transform.parent.gameObject, 0.7f);
            invaderScript.isAlive = false; //Kevin's code
            playerValues.Scores += 50;
            uiManager.UpdateScore(playerValues.Scores);
        }
    }
}
