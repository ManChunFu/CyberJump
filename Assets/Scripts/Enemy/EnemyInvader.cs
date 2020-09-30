using System.Collections;
using UnityEngine;

public class EnemyInvader : MonoBehaviour
{
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private GameManager GameManager;
    private bool isMovingToRight = true;
    public bool mustDescend = false;
    public float durationOfDescent = 2f;
    public float target_horizontal_pos = 5.5f;
    public float target_vertical_pos = 0f;
    public bool isAlive = true;
    private void Start()
    {
        GameManager = FindObjectOfType<GameManager>();
        if (GameManager == null)
            Debug.LogError("GameManager is not assigned.");
        target_vertical_pos = this.transform.position.y;
    }
    private void Update()
    {
        if (this.transform.position.y < -5.3f)
            Destroy(gameObject);
        if(isAlive)
        {
            if (GameManager.BackgroundChanged == true) //If scrolling is enabled
            {
                if (target_horizontal_pos <= this.transform.position.x && isMovingToRight == true) //Check if you are on edge of screen. If yes....
                {
                    mustDescend = true; //Enemy must descend.
                    if (mustDescend)
                    {
                        durationOfDescent -= Time.smoothDeltaTime; //Ticks down the time the enemy must descend.
                        if (durationOfDescent >= 0)
                        {
                            transform.Translate(Vector3.down * Time.deltaTime * (speed * 1.5f)); //Move enemy down, 50% faster than platforms/middleground/patrol enemies
                        }
                        else //If descent is completed, update target horiz+verti position, reset descent duration and flag
                        {
                            mustDescend = false;
                            target_horizontal_pos = -5f;
                            target_vertical_pos = this.transform.position.y;
                            isMovingToRight = false;
                            durationOfDescent = 2f;
                        }
                    }
                }
                else if (target_horizontal_pos >= this.transform.position.x && isMovingToRight == false) //As above, but for left side of the screen
                {
                    mustDescend = true;
                    if (mustDescend)
                    {
                        durationOfDescent -= Time.smoothDeltaTime;
                        if (durationOfDescent >= 0)
                        {
                            transform.Translate(Vector3.down * Time.deltaTime * (speed * 1.5f));
                        }
                        else
                        {
                            mustDescend = false;
                            target_horizontal_pos = 5.5f;
                            target_vertical_pos = this.transform.position.y;
                            isMovingToRight = true;
                            durationOfDescent = 2f;
                        }
                    }
                }
                else //While not at horizontal target pos, scroll down with everything else and move left or right.
                {
                    Vector2 offset = new Vector2(this.transform.position.x, this.transform.position.y - Time.deltaTime * 1);
                    this.transform.position = offset;
                    if (target_horizontal_pos > this.transform.position.x) //if your X pos is LESSER than target pos (ai, you are to the left) then go right
                        EnemyInvaderLeftToRight();
                    else if (target_horizontal_pos < this.transform.position.x) //if your X pos is GREATER than target pos (ai, you are to the right) then go left
                        EnemyInvaderRightToLeft();
                    target_vertical_pos = this.transform.position.y;
                }

            }
            else //If scrolling is disabled (ai, player is on street level or idle)
            {
                if (target_vertical_pos <= this.transform.position.y)
                {
                    mustDescend = true;
                    if (mustDescend)
                    {
                        durationOfDescent -= Time.smoothDeltaTime;
                        if (durationOfDescent >= 0)
                        {
                            transform.Translate(Vector3.down * Time.deltaTime * (speed * 1.5f));
                        }
                        else
                        {
                            mustDescend = false;
                            target_horizontal_pos = 5.5f;
                            target_vertical_pos = this.transform.position.y;
                            isMovingToRight = true;
                            durationOfDescent = 2f;
                        }
                    }
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
            }
        }
    }

    private void EnemyInvaderGoDown()//
    {
        transform.Translate(Vector3.down * Time.deltaTime * (speed * 1.5f));
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
}
