using System.Collections.Generic;
using UnityEngine;

public class SpawnPlatform : MonoBehaviour
{
    [Tooltip("Chance to spawn extra platform.")]
    public int extraPlatformChance = 15;
    [Tooltip("Platform Spawn Frequency")]
    public float spawnFrequency = 0.7f;
    public GameManager GameManager;
    public GameObject[] prefabs = new GameObject[10];
    public Transform platformParent;

    GameObject[] platforms; //Array of all platforms that is checked to see which one is highest up
    GameObject highest_platform; //highest platform to check against for new platform spawning
    float highest_platform_position = -5f; //bottom of screen

    //todo: either rename SpawnManager or make it handle the spawns of enemies
    private void Start()
    {
        Invoke("PlatformSpawning", 0.05f);
    }
    private void Update()
    {
        PlatformSpawning();
    }


     
    void PlatformSpawning()
    {
        FindPlatforms(); //Run through all platforms to see if there is one on Y-level 5 to 6.5 (aka just out of view for player)

        float maximumDeviationLeft = -5f;
        float maximumDeviationRight = 5f;

        /*if (GameManager.ground.transform.position.y < -100)
        {
            maximumDeviationLeft -= 2.5f;
            maximumDeviationRight += 2.5f;
        }*/

        if (highest_platform_position < 10f)
        {
            float acceptableDeviationLeft = highest_platform.transform.position.x;
            float acceptableDeviationRight = highest_platform.transform.position.x;

            int random_spawn_pos = Random.Range(1, 100); //Randomized spawn either left or right of the highest platform.
            if (random_spawn_pos < 50)
            {
                acceptableDeviationLeft -= 1.3f;
                acceptableDeviationRight -= 1.3f;
            }
            else
            {
                acceptableDeviationLeft += 1.3f;
                acceptableDeviationRight += 1.3f;
            }

            if (acceptableDeviationLeft <= maximumDeviationLeft) //If a platform is about to spawn too far left, move it to the right
            {
                acceptableDeviationLeft += 5f;
                acceptableDeviationRight += 5f;
            }

            if (acceptableDeviationRight >= maximumDeviationRight) //Vice versa if it is about to spawn too far to the right.
            {
                acceptableDeviationLeft -= 5f;
                acceptableDeviationRight -= 5f;
            }
            Vector2 position = new Vector2(Random.Range(acceptableDeviationLeft, acceptableDeviationRight), highest_platform_position + Random.Range(1.3f, 1.5f));
            GameObject x = Instantiate(prefabs[Random.Range(0, prefabs.Length)], position, Quaternion.identity); //Finally, spawn a platform within the given parameters
            x.transform.SetParent(platformParent);
            // Extra platform spawn code
            int spawn_extra_platform = Random.Range(1, 101); //Randomized spawn either left or right of the highest platform.
            if (spawn_extra_platform < extraPlatformChance)
            {
                int random_extra_spawn_pos = Random.Range(1, 100); //Randomized spawn either left or right of the highest platform.
                if (random_extra_spawn_pos < 50) //either spawn left of the other platform...
                {
                    position.x -= Random.Range(2.5f, 4.5f);
                }
                else //or spawn right of it.
                {
                    position.x += Random.Range(2.5f, 4.5f);
                }

                if (position.x <= maximumDeviationLeft) //If a platform is about to spawn too far left, move it to the (far) right
                {
                    position.x += Random.Range(2f, 5f);
                }

                if (position.x >= maximumDeviationRight) //Vice versa if it is about to spawn too far to the right.
                {
                    position.x -= Random.Range(2f, 5f);
                }
                GameObject y = Instantiate(prefabs[Random.Range(0, prefabs.Length)], position, Quaternion.identity);
                y.transform.SetParent(platformParent);
            }
        }
    }

    void FindPlatforms()
    {
        highest_platform_position = -5f;
        platforms = GameObject.FindGameObjectsWithTag("Platform");
        foreach (GameObject platform in platforms)
        {
            if (platform.transform.position.y > highest_platform_position)
            {
                highest_platform_position = platform.transform.position.y;
                highest_platform = platform;
            }
        }
    }

}
