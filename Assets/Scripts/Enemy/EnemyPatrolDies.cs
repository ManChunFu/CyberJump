using UnityEngine;
using UnityEngine.Assertions;

//Written by Mandy

public class EnemyPatrolDies : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private PlayerValues playerValues;


    private Animator explosionAnim;
    private BoxCollider2D[] boxCollider2D;
    private AudioSource explosionClip;
    
    public bool isDead = false;

    private void Start()
    {
        explosionAnim = GetComponent<Animator>();
        Assert.IsNotNull(explosionAnim, "Failed to find Animator componenet.");

        uiManager = FindObjectOfType<UIManager>();
        Assert.IsNotNull(uiManager, "No reference to UIManager script.");

        Assert.IsNotNull(playerValues, "No reference to PlayerValues scriptable object.");

        boxCollider2D = GetComponents<BoxCollider2D>();
        Assert.IsNotNull(boxCollider2D, "Failed to get Box Coliier 2D component.");

        explosionClip = GetComponent<AudioSource>();
        Assert.IsNotNull(explosionClip, "Failed to get Audio Source component.");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Laser")
        {
            explosionClip.Play();
            explosionAnim.SetTrigger("Death");
            this.gameObject.GetComponent<Collider2D>().enabled = false;
            Destroy(collision.gameObject);

            foreach (var item in boxCollider2D)
                item.enabled = false;
            
            float clipLength = explosionAnim.runtimeAnimatorController.animationClips.Length;
            foreach  (Transform t in gameObject.transform)
                Destroy(t.gameObject, 0.1f);

            
            Destroy(gameObject, clipLength);

            playerValues.Scores += 100;
            uiManager.UpdateScore(playerValues.Scores);
            isDead = true;
        }
    }
   
}
