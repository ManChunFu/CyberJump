using UnityEngine;
using UnityEngine.Assertions;

//Written by Mandy
public class HeartDamage : MonoBehaviour
{
    private Animator heartDamageAnim;

    private void Awake()
    {
        heartDamageAnim = GetComponent<Animator>();
        Assert.IsNotNull(heartDamageAnim, "Failed to find Animator conpomnent.");
    }

    public void ShowLiveDamage() => heartDamageAnim.SetTrigger("Damage");
}
