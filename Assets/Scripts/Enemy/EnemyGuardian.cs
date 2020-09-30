using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

//Written by Mandy

public class EnemyGuardian : MonoBehaviour
{
    [SerializeField] private GameObject guardianLaserPrefab;
    [SerializeField] private GameObject muzzleFlashRed;

    private Animator enemyGuardianAnimator;
    private float fireRate = 1f;
    private float canFire = -1f;
    private EnemyPatrolDies enemyPatrolDies;
    private int SpotPlayerID = Animator.StringToHash("SpotPlayer");

    private void Start()
    {
        Assert.IsNotNull(guardianLaserPrefab, "No refernce to GardianLaserPrefab.");

        enemyPatrolDies = GetComponentInParent<EnemyPatrolDies>();
        Assert.IsTrue(enemyPatrolDies, "Failed to get access to EnemyBehavior script.");

        enemyGuardianAnimator = GetComponent<Animator>();
        Assert.IsNotNull(enemyGuardianAnimator, "Failed to access to Animator component.");

        Assert.IsNotNull(muzzleFlashRed, "No reference to MuzzleFlashRed game object.");
        if (muzzleFlashRed != null)
            muzzleFlashRed.SetActive(false);
    }

    private void Update()
    {
        if (enemyPatrolDies.isDead)
            StopCoroutine(FireRoutine());
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            enemyGuardianAnimator.SetBool(SpotPlayerID, true);
            StartCoroutine(FireRoutine());
        }
    }

    private IEnumerator FireRoutine()
    {
        if (Time.time > canFire)
        {
            canFire = Time.time + fireRate;
            muzzleFlashRed.SetActive(true);
            Instantiate(guardianLaserPrefab, transform.position - new Vector3(0f, 0.5f, 0f), Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
            if (enemyPatrolDies.isDead)
                yield break;
            else
                muzzleFlashRed.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other) => enemyGuardianAnimator.SetBool(SpotPlayerID, false);
  
}
