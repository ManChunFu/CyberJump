using System.Collections.Generic;
using UnityEngine;

//Written by Kevin

public class SpawnForeground : MonoBehaviour
{
    public GameObject foreground_sign_1;
    public GameObject foreground_sign_2;
    public GameObject foreground_sign_3;
    public GameObject foreground_sign_4;
    public GameObject tree_1;
    public GameObject tree_2;
    public GameObject tree_3;
    public GameManager GameManager; //need to access GameManager to check height of the ground object
    private GameObject chosen_foreground; //the foreground that will be spawned; semirandomly determined
    bool continual_spawning = true; //keep spawning foreground assets while true
    bool tree_spawning = true; //trees will only spawn on lower y-coords
    public Transform foregroundParent;

    GameObject[] foreground_objects;
    float highest_fg_object_pos = -5f; //bottom of screen

    private void Update()
    {
        FGSpawning();
    }



    void FGSpawning()
    {
        if (GameManager.Ground.transform.position.y < -50) //disables tree spawning above -50f
        {
            tree_spawning = false;
        }
        if (GameManager.Ground.transform.position.y < -100)
        {
            continual_spawning = false;
        }
        CheckHeightOfForeground();
        if (highest_fg_object_pos < 7f)
        {
            Vector2 position = new Vector2(Random.Range(7.4f, 8.2f), 12f);
            int random_fg = Random.Range(1, 101);

            if (continual_spawning) //spawning is disabled if player is in clouds
            {
                if (tree_spawning)
                {
                    int tree_or_sign = Random.Range(1, 101);
                    if (tree_or_sign < 50) // Random has chosen tree!
                    {
                        if (random_fg <= 33)
                        {
                            chosen_foreground = tree_1;
                        }
                        else if (random_fg > 33 && random_fg <= 66)
                        {
                            chosen_foreground = tree_2;
                        }
                        else
                        {
                            chosen_foreground = tree_3;
                        }
                    }
                    else
                    {
                        if (random_fg <= 25) // 50/50 to replace it with another variation
                        {
                            chosen_foreground = foreground_sign_1;
                        }
                        else if (random_fg > 25 && random_fg <= 50)
                        {
                            chosen_foreground = foreground_sign_2;
                        }
                        else if (random_fg > 50 && random_fg <= 75)
                        {
                            chosen_foreground = foreground_sign_3;
                        }
                        else
                        {
                            chosen_foreground = foreground_sign_4;
                        }
                    }
                }
                else
                {
                    if (random_fg <= 25) // 50/50 to replace it with another variation
                    {
                        chosen_foreground = foreground_sign_1;
                    }
                    else if (random_fg > 25 && random_fg <= 50)
                    {
                        chosen_foreground = foreground_sign_2;
                    }
                    else if (random_fg > 50 && random_fg <= 75)
                    {
                        chosen_foreground = foreground_sign_3;
                    }
                    else
                    {
                        chosen_foreground = foreground_sign_4;
                    }
                }
            }
            if (continual_spawning)
            {
                int random_side = Random.Range(1, 101);
                SpriteRenderer render = chosen_foreground.GetComponent<SpriteRenderer>(); //gets the renderer so image can be flipped depending on spawn side
                if (random_side < 50)
                {
                    position.x = Random.Range(-7.3f, -9f);
                    render.flipX = true;
                }
                else
                {
                    render.flipX = false;
                }
                GameObject y = Instantiate(chosen_foreground, position, Quaternion.identity);
                y.transform.SetParent(foregroundParent);
                render.flipX = false;
            }
        }
    }

    void CheckHeightOfForeground()
    {

        foreground_objects = GameObject.FindGameObjectsWithTag("Foreground");
        foreach (GameObject mg in foreground_objects)
        {
            highest_fg_object_pos = mg.transform.position.y;
        }
    }
}
