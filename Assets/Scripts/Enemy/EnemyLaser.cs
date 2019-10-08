using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField] private float speed = 1f;

    private void Update()
    {
        transform.Translate(Vector2.down * Time.deltaTime * speed);

        if (transform.position.y < -6f)
            Destroy(gameObject);
    }
}
