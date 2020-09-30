using UnityEngine;
using UnityEngine.Assertions;


//Written by Kevin

public class ScrollingEntities : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private GameManager GameManager;
    private GameObject[] platforms;
    private GameObject[] foreground;
    private GameObject[] middleground;
    private GameObject[] enemies;
    private GameObject[] moon_array;
    private GameObject[] hearts;

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

            foreground = GameObject.FindGameObjectsWithTag("Foreground");
            foreach (GameObject fg in foreground)
            {
                Vector2 fgoffset = new Vector2(fg.transform.position.x, fg.transform.position.y - Time.deltaTime * (speed * 1.2f));
                fg.transform.position = fgoffset;
            }

            middleground = GameObject.FindGameObjectsWithTag("Middleground");
            foreach (GameObject mg in middleground)
            {
                Vector2 mgoffset = new Vector2(mg.transform.position.x, mg.transform.position.y - Time.deltaTime * speed);
                mg.transform.position = mgoffset;
            }

            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                Vector2 enemyoffset = new Vector2(enemy.transform.position.x, enemy.transform.position.y - Time.deltaTime * speed);
                enemy.transform.position = enemyoffset;
            }

            moon_array = GameObject.FindGameObjectsWithTag("Moon");
            foreach (GameObject moon in moon_array)
            {
                if(moon.transform.position.y > 4f)
                {
                    Vector2 enemyoffset = new Vector2(moon.transform.position.x, moon.transform.position.y - Time.deltaTime * speed);
                    moon.transform.position = enemyoffset;
                }
            }

            hearts = GameObject.FindGameObjectsWithTag("Heart");
            foreach (GameObject heart in hearts)
            {
                Vector2 heartoffset = new Vector2(heart.transform.position.x, heart.transform.position.y - Time.deltaTime * speed);
                heart.transform.position = heartoffset;
            }
        }
    }
}