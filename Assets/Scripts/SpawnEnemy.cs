using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{

    [Tooltip("What type of enemy to spawn. ")]
    public GameObject enemy;
    [Tooltip("The number of enemies to spawn in each batch.")]
    public int enemiesToSpawn = 0;
    [Tooltip("Time before first enemy is spawned.")]
    public float enemySpawnWait = 5f;
    [Tooltip("Frequency with which it spawns enemies.")]
    public float enemySpawnFrequency = 5f;
    
    float enemy_horiz_spawn_pos = -4.5f;
    float enemy_verti_spawn_pos = 0f;


    public static SpawnManager instance;
    // Start is called before the first frame update
    void Start()
    {
        enemy_verti_spawn_pos = this.transform.position.y;
        InvokeRepeating("SpawnEnemyInvader", enemySpawnWait, enemySpawnFrequency);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void SpawnEnemyInvader()
    {
        List<GameObject> enemy_spawn_list = new List<GameObject>();
        Vector2 enemy_spawn_position = new Vector2(enemy_horiz_spawn_pos, enemy_verti_spawn_pos);
        while (enemy_spawn_list.Count < enemiesToSpawn)
        {
            Instantiate(enemy, enemy_spawn_position, Quaternion.identity);
            enemy_spawn_position.x += 2f;
            enemy_spawn_list.Add(enemy);
        }

    }
}
