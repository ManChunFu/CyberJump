using System.Collections;
using UnityEngine;

public class EnemyInvader : MonoBehaviour
{
    [SerializeField] private int enemyID; // 0 = left to right, 1 = right to left 
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private Vector3 startOffset;
    [SerializeField] private Vector3 endOffset;
    [SerializeField] private GameObject enemyLaserPrefab;
    private bool isMovingToRight = true;
    private bool shootingPlayer = false;

    float target_horizontal_pos = 5.5f;
    public float target_vertical_pos = 0f;
    private void Start()
    {
        target_vertical_pos = this.transform.position.y;
        shootingPlayer = false;
    }
    private void Update()
    {
        if (target_vertical_pos <= this.transform.position.y)
        {
            EnemyInvaderGoDown();
        }
        else if (target_horizontal_pos <= this.transform.position.x && isMovingToRight == true)
        {
            target_horizontal_pos = -5f;
            target_vertical_pos -= 1f;
            isMovingToRight = false;
        }
        else if (target_horizontal_pos >= this.transform.position.x && isMovingToRight == false)
        {
            target_horizontal_pos = 5.5f;
            target_vertical_pos -= 1f;
            isMovingToRight = true;
        }
        else if (target_horizontal_pos > this.transform.position.x) //if your X pos is LESSER than target pos (ai, you are to the left) then go right
            EnemyInvaderLeftToRight();
        else if (target_horizontal_pos < this.transform.position.x) //if your X pos is GREATER than target pos (ai, you are to the right) then go left
            EnemyInvaderRightToLeft();
        //else
          //  target_vertical_pos = this.transform.position.y - 1f;
    }

    private void EnemyInvaderGoDown()
    {
        transform.Translate(Vector3.down * Time.deltaTime * speed);
    }

    private void EnemyInvaderLeftToRight()
    {
        isMovingToRight = true;
        transform.Translate(Vector3.right * Time.deltaTime * speed);
    }

    private void EnemyInvaderRightToLeft()
    {
        isMovingToRight = false;
        transform.Translate(Vector3.left * Time.deltaTime * speed);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Instantiate(enemyLaserPrefab, transform.position + new Vector3(0f, -0.5f, 0f), Quaternion.identity, transform);
            shootingPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        shootingPlayer = false;
    }

}
