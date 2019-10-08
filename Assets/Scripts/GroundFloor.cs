using UnityEngine;
using UnityEngine.Assertions;

public class GroundFloor : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Player player;

    private void Awake()
    {
        Assert.IsNotNull(gameManager, "No reference to GameManager script.");

        Assert.IsNotNull(player, "No refernce to GameManager script.");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameManager.NotFirstFloor)
            player.Damage(true);
    }
}
