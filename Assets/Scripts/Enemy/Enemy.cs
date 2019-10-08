using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int enemyID; // 0 = left to right, 1 = right to left 
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private Vector3 startOffset;
    [SerializeField] private Vector3 endOffset;
    [SerializeField] private GameObject enemyLaserPrefab;

    private bool shootingPlayer = false;
    private bool isMovingToRight = false;
    private float fireRate = 0.3f;
    private float canFire = -1f;

    private void Start()
    {
        shootingPlayer = false;
    }
    private void Update()
    {
        if (enemyID == 0)
            EnemySurveyLeftToRight();

        if (enemyID == 1)
            EnemySurveyRightToLeft();
    }

    private void EnemySurveyLeftToRight()
    {
        if (shootingPlayer == false)
        {
            if (transform.position.x <= startOffset.x)
                isMovingToRight = true;
            else if (transform.position.x >= endOffset.x)
                isMovingToRight = false;

            if (isMovingToRight)
                transform.Translate(Vector3.right * Time.deltaTime * speed);
            else
                transform.Translate(Vector3.left * Time.deltaTime * speed);
        }
    }

    private void EnemySurveyRightToLeft()
    {
        if (shootingPlayer == false)
        {
            if (transform.position.x >= startOffset.x)
                isMovingToRight = false;
            else if (transform.position.x <= endOffset.x)
                isMovingToRight = true;

            if (!isMovingToRight)
                transform.Translate(Vector3.left * Time.deltaTime * speed);
            else
                transform.Translate(Vector3.right * Time.deltaTime * speed);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (Time.time > canFire)
            {
                canFire = Time.time + fireRate;
                Instantiate(enemyLaserPrefab, transform.position + new Vector3(0f, -0.5f, 0f), Quaternion.identity);
                shootingPlayer = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        shootingPlayer = false;
    }


}
