using UnityEngine;

public class BackgroundMoving : MonoBehaviour
{
    [Range (0, 1)]
    public float MovingSpeed;

    public Vector3 DifferenceToTheFirst { get; internal set; }

    private void Start()
    {
        DifferenceToTheFirst = new Vector3(0, 0.06f, 0) - transform.position;
    }
    public void MoveDown()
    {
        transform.Translate(Vector2.down * (Time.deltaTime * MovingSpeed));        
    }
    

}
