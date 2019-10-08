using UnityEngine;
using UnityEngine.Assertions;

public class EnemyPatrolDies : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;

    private int score = 0;

    private Animator explosionAnim;
    
    public bool isDead = false;

    private void Awake()
    {
        explosionAnim = GetComponent<Animator>();
        Assert.IsNotNull(explosionAnim, "Failed to find Animator componenet.");

        Assert.IsNotNull(uiManager, "No reference to UIManager script.");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Laser")
        {
            explosionAnim.SetTrigger("Death");
            Destroy(collision.gameObject);

            float clipLength = explosionAnim.runtimeAnimatorController.animationClips.Length;
            foreach  (Transform t in gameObject.transform)
                Destroy(t.gameObject, 0.3f);

            Destroy(gameObject, clipLength);

            score += 100;
            uiManager.UpdateScore(score);
            isDead = true;
        }
    }
   
}
