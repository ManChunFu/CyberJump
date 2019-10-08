using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameManager GameManager;
    public GameObject platform;
    private GameObject[] platforms;
    float platform_horiz_spawn_pos = 0f; //centered to begin with.
    //todo: either rename SpawnManager or make it handle the spawns of enemies in a good manner.
    private void Start()
    {
        InvokeRepeating("FindPlatforms", 0.01f, 3f);
        InvokeRepeating("SpawnPlatform", 0.05f, 2f);
    }
    private void Update()
    {

    }
     
    void SpawnPlatform() //todo: adjust and add functionalty to check Y-axis of platforms below you to guarantee spawn within jumping distance
    {
        int spawned_platforms = FindPlatforms(); //Run through all platforms to see if there is one on Y-level 5 to 6.5 (aka just out of view for player)
        if (spawned_platforms < 2) //If there are less than two platforms in that area, continue to spawning.
        {
            if (spawned_platforms == 1) //If there is ONE platform in that range, check it's X-axis (to avoid overlapping)
            {
                if (platform_horiz_spawn_pos >= 0)
                {
                    platform_horiz_spawn_pos = (Random.Range(-4f, -1f));
                }
                else
                {
                    platform_horiz_spawn_pos = (Random.Range(1f, 4f));
                }
            }
            else //If there are no platforms in that range, spawn anywhere.
            {
                platform_horiz_spawn_pos = (Random.Range(-4f, 4f));
            }
            Vector2 position = new Vector2(platform_horiz_spawn_pos, Random.Range(5.8f,6.2f)); //Finally, spawns a platform with the given X-coords and a slightly varied Y-axis
            Instantiate(platform, position, Quaternion.identity);
        }
        else
        {
            Debug.Log("I want to spawn a platform but there's too many already!");
            //todo: spawn stuff way higher?
        }
        
    }

    int FindPlatforms()
    {
        int platform_counter = 0;
        platforms = GameObject.FindGameObjectsWithTag("Platform");
        foreach (GameObject platform in platforms)
        {
            if (platform.transform.position.y >= 5f && platform.transform.position.y <= 6.5f) //looks through all platforms at top of screen or just above
            {
                platform_horiz_spawn_pos = platform.transform.position.x;
                platform_counter += 1;
            }
            else
            {
                //Do nothing. Maybe add some logic to search for platforms below you here?
            }
        }
        return platform_counter;
    }
}
