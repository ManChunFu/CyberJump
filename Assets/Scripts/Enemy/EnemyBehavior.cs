using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private float startPoint, endPoint;
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float interval = 2f;
    [SerializeField] private float lastIntervalX = 0;
    [SerializeField] private float laserSpeed = 5f;
    [SerializeField] private GameObject enemyLaserPrefab;

    private bool animaiting;
    private bool isMovingToRight = false;
    private bool isShooting = false;
    private float fireRate = 2f;
    private float canFire = -1f;
    private Animator surveyAnim;
    private EnemyPatrolDies enemyPatrolDies;

    public bool hasFiredLaser = false;


    private void Awake()
    {
        surveyAnim = GetComponent<Animator>();
        Assert.IsNotNull(surveyAnim, "Failed to find Animator component.");

        enemyPatrolDies = GetComponentInParent<EnemyPatrolDies>();
        Assert.IsTrue(enemyPatrolDies, "Failed to get access to EnemyBehavior script.");

    }
    private void Update()
    {
        StartCoroutine(SurveyCoroutine());

        if (enemyPatrolDies.isDead)
            StopCoroutine(SurveyCoroutine());

    }

    private IEnumerator SurveyCoroutine()
    {
        if (!isShooting && !animaiting)
        {
            if (transform.position.x <= startPoint)
                isMovingToRight = true;
            else if (transform.position.x >= endPoint)
                isMovingToRight = false;


            if ((isMovingToRight && transform.position.x > lastIntervalX + interval) ||
                !isMovingToRight && transform.position.x < lastIntervalX - interval)
            {
                animaiting = true;
                surveyAnim.SetBool("IsSurveying", true);
                yield return new WaitForSeconds(5.6f);
                surveyAnim.SetBool("IsSurveying", false);
                lastIntervalX = transform.position.x;
                animaiting = false;
            }
            transform.Translate((isMovingToRight ? Vector2.right : Vector2.left) * moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            float playerXPos = other.transform.position.x;
            Vector2 targetPos = new Vector2(playerXPos, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, targetPos, 0.05f);
            if (!hasFiredLaser)
            {
                isShooting = true;
                StartCoroutine(FireRoutine());
            }
        }
    }

    private IEnumerator FireRoutine()
    {
        if (Time.time > canFire)
        {
            hasFiredLaser = true;
            canFire = Time.time + fireRate;
            Instantiate(enemyLaserPrefab, transform.position - new Vector3(0f, 1f, 0f), Quaternion.identity);
            yield return new WaitForSeconds(1f);
            hasFiredLaser = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other) => isShooting = false;
    

}
