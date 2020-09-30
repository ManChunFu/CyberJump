using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

//Written by Kevin

public class Moon : MonoBehaviour
{

    [SerializeField] private PlayerValues playerValues;
    private UIManager uiManager;


    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
        Assert.IsNotNull(uiManager, "No reference to UIManager script.");

        Assert.IsNotNull(playerValues, "No reference to PlayerValues scriptable object.");
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerValues.Scores += 5000;
            uiManager.UpdateScore(playerValues.Scores);
            Destroy(gameObject);
        }

    }
}
