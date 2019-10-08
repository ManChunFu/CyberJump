using UnityEngine;
using UnityEngine.Assertions;

public class ScrollingEntities : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private GameManager GameManager;
    private GameObject[] platforms;
    private GameObject[] enemies;

    private void Awake()
    {
        if (GameManager == null)
            Debug.LogError("GameManager is not assigned.");
    }
    private void Update()
    {
        if (GameManager.BackgroundChanged == true)
        {
            platforms = GameObject.FindGameObjectsWithTag("Platform");
            foreach (GameObject platform in platforms)
            {
                Vector2 platformoffset = new Vector2(platform.transform.position.x, platform.transform.position.y - Time.deltaTime * speed);
                platform.transform.position = platformoffset;
            }

            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                Vector2 enemyoffset = new Vector2(enemy.transform.position.x, enemy.transform.position.y - Time.deltaTime * speed);
                enemy.transform.position = enemyoffset;
            }
        }

    }
}