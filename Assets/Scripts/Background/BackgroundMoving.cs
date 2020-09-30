using UnityEngine;


//Written by Mandy

public class BackgroundMoving : MonoBehaviour
{
    [Range(0, 1)]
    public float MovingSpeed;

    public Vector2 OriginalPosition { get; private set; }
    public Vector2 DifferenceToTheFirst { get; internal set; }

    private float distance;
    public float scale1Y { get; set; }

    private void Start()
    {
        OriginalPosition = transform.position;
        DifferenceToTheFirst = transform.position;
    }

    private void Update()
    {
        distance = Vector2.Distance(OriginalPosition, transform.position);
    }
    public void MoveDown()
    {
        transform.Translate(Vector2.down * (Time.deltaTime * MovingSpeed));

        DifferenceToTheFirst += Vector2.down * Time.deltaTime;
    }

    public void PareToFirstOne()
    {
        transform.Translate(Vector2.up * (OriginalPosition + DifferenceToTheFirst));
    }

    public void Reset()
    {
        transform.position = OriginalPosition - new Vector2(0f, distance);
        
        PareToFirstOne();
    }


}
