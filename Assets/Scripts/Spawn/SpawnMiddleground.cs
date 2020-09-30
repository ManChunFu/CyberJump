using System.Collections.Generic;
using UnityEngine;

//Written by Kevin

public class SpawnMiddleground : MonoBehaviour
{
    public GameObject middleground; //first prefab of middleground
    public GameObject middleground2; //2nd prefab of middleground
    public GameObject middlegroundtop; //the top prefab of middleground
    public GameManager GameManager; //need to access GameManager to check height of the ground object
    private GameObject chosen_middleground; //the middleground that will be spawned; semirandomly determined
    bool continual_spawning = true; //keep spawning middleground buildings

    GameObject[] middleground_objects;
    float highest_mg_object_pos = -5f; //bottom of screen

    private void Start()
    {
        Vector2 pos = new Vector2(0f, 12.75f);
        Instantiate(middleground, pos, Quaternion.identity);
    }
    private void Update()
    {
        MGSpawning();
    }



    void MGSpawning()
    {

        CheckHeightOfMiddleground();
        if (highest_mg_object_pos < 8f)
        {
            if(continual_spawning)
            {
                if (GameManager.Ground.transform.position.y < -100)
                {
                    continual_spawning = false;
                }
                Vector2 position = new Vector2(0f, 19.4f);
                int random_bg = Random.Range(1, 101);
                if (random_bg > 50) // 50/50 to replace it with another variation
                {
                    chosen_middleground = middleground;
                }
                else
                {
                    chosen_middleground = middleground2;
                }
                if(continual_spawning)
                {
                    Instantiate(chosen_middleground, position, Quaternion.identity);
                }
                else
                {
                    Instantiate(middlegroundtop, position, Quaternion.identity);
                }
            }
        }
    }

    void CheckHeightOfMiddleground()
    {

        middleground_objects = GameObject.FindGameObjectsWithTag("Middleground");
        foreach (GameObject mg in middleground_objects)
        {
            highest_mg_object_pos = mg.transform.position.y;
        }
    }
}
