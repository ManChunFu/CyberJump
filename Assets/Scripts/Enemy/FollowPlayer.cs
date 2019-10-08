using UnityEngine;
using UnityEngine.Assertions;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform player;

    private void Awake()
    {
        Assert.IsNotNull(player, "Do you forget to assign the Player?");
    }
    private void Update()
    {
        float cameraY = transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, player.position, 1f);
    }
}
