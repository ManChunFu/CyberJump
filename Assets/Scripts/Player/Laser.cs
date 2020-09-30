using UnityEngine;
using UnityEngine.Assertions;

//Written by Mandy

public class Laser : MonoBehaviour
{
    [SerializeField] private float speed = 2f;


    private void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
        if (transform.position.y > 6f)
            Destroy(gameObject);
    }

}
