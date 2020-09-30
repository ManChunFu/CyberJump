using System.Collections.Generic;
using UnityEngine;

//Written by Kevin

public class SpawnPlatform : MonoBehaviour
{
    [Tooltip("Chance to spawn extra platform.")]
    public int extraPlatformChance = 15;
    [Tooltip("Platform Spawn Frequency")]
    public float spawnFrequency = 0.7f;
    [Tooltip("Platform Rare Spawn Chance")]
    public int rare_spawn_chance;
    public GameManager gameManager;
    public GameObject[] prefabs = new GameObject[11];
    public GameObject[] prefabs_flipped = new GameObject[11];
    public Transform platformParent;

    public GameObject heart;
    [Tooltip("Chance for a heart to spawn above platform")]
    public int heartSpawnChance;

    public GameObject guardian;
    [Tooltip("Chance for a Guardian to spawn")]
    public int guardianSpawnChance;

    GameObject[] platforms; //Array of all platforms that is checked to see which one is highest up
    GameObject highest_platform; //highest platform to check against for new platform spawning
    float highest_platform_position = -5f; //bottom of screen

    private void Update()
    {
        PlatformSpawning();
    }



    void PlatformSpawning()
    {
        FindPlatforms(); //Run through all platforms to see if there is one on Y-level 5 to 6.5 (aka just out of view for player)


        if (highest_platform_position < 10f)
        {
            float acceptableDeviationLeft = highest_platform.transform.position.x;
            float acceptableDeviationRight = highest_platform.transform.position.x;

            int random_spawn_pos = Random.Range(1, 100); //Randomized spawn either left or right of the highest platform.
            if (random_spawn_pos < 50)
            {
                acceptableDeviationLeft -= 1.4f;
                acceptableDeviationRight -= 1.4f;
            }
            else
            {
                acceptableDeviationLeft += 1.4f;
                acceptableDeviationRight += 1.4f;
            }

            if (acceptableDeviationLeft <= gameManager.maximumDeviationLeft) //If a platform is about to spawn too far left, move it to the right
            {
                acceptableDeviationLeft += 5f;
                acceptableDeviationRight += 5f;
            }

            if (acceptableDeviationRight >= gameManager.maximumDeviationRight) //Vice versa if it is about to spawn too far to the right.
            {
                acceptableDeviationLeft -= 5f;
                acceptableDeviationRight -= 5f;
            }
            Vector2 position = new Vector2(Random.Range(acceptableDeviationLeft, acceptableDeviationRight), highest_platform_position + Random.Range(1.7f, 1.9f));
            int chosen_platform = ChoosePlatform();

            int random__flip_chance = Random.Range(1, 101);
            if (random__flip_chance > 50)
            {
                GameObject x = Instantiate(prefabs_flipped[chosen_platform], position, Quaternion.identity); //Finally, spawn a platform within the given parameters
                x.transform.SetParent(platformParent);
            }
            else
            {
                GameObject x = Instantiate(prefabs[chosen_platform], position, Quaternion.identity); //Finally, spawn a platform within the given parameters
                x.transform.SetParent(platformParent);
            }

            int heart_spawn = Random.Range(1, 101);
            if(heart_spawn < heartSpawnChance)
            {
                position.y += 0.5f;
                Instantiate(heart, position, Quaternion.identity);
            }

            int guardian_spawn = Random.Range(1, 101);
            if (guardian_spawn < guardianSpawnChance)
            {
                position.y += 1.5f;
                Instantiate(guardian, position, Quaternion.identity);
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

    int ChoosePlatform()
    {
        int x = Random.Range(1, 101);
        if (x < rare_spawn_chance) //If a rare platform is chosen to spawn
        {
            int random_rare_platform = Random.Range(1, 6); //Randomizes between them. Note: Returns their position in the prefab array! Not their actual number (ex: platform_2 has position 1)
            if (random_rare_platform == 1)
            {
                return 1;
            }
            else if (random_rare_platform == 2)
            {
                return 2;
            }
            else if (random_rare_platform == 3)
            {
                return 3;
            }
            else if (random_rare_platform == 4)
            {
                return 4;
            }
            else
            {
                return 5;
            }
        }
        else
        {
            int random_normal_platform = Random.Range(1, 6);
            if (random_normal_platform == 1)
            {
                return 0;
            }
            else if (random_normal_platform == 2)
            {
                return 6;
            }
            else if (random_normal_platform == 3)
            {
                return 7;
            }
            else if (random_normal_platform == 4)
            {
                return 8;
            }
            else if (random_normal_platform == 5)
            {
                return 9;
            }
            else
            {
                return 10;
            }
        }
    }
}
