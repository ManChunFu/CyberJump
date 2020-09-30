using UnityEngine;
using UnityEngine.Assertions;

public class PushArm : MonoBehaviour
{
    private Player player;
    [SerializeField] private int pushArmID; // 0 = left, 1 = right

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        Assert.IsNotNull(player, "No reference to Player script.");
    }

 //Wait for Sprite/Animation 
    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        if (pushArmID == 0)
    //            player.BeingPushedFromLeftSide();
    //        else if (pushArmID == 1)
    //            player.BeingPushedFromRightSide();
    //    }
    //}
}
