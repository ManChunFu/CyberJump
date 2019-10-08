using UnityEngine;
using UnityEngine.Assertions;

public class EnemyDies : MonoBehaviour
{
  
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Laser")
        {
            Destroy(collision.gameObject, 1f);
            Destroy(transform.parent.gameObject);
        }
            
    }
}
