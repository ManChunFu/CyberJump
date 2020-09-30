using UnityEngine;

//Written by Mandy

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] private float speed = 0.02f;
    [SerializeField] private GameManager GameManager;

    Vector2 offset;

    private void Update()
    {
        if (GameManager.BackgroundChanged == true)
        {
            if (this.transform.position.y <= -11)
            {
                offset = new Vector2(0f, 11f);
            }
            else
            {
                offset = new Vector2(0f, this.transform.position.y - Time.deltaTime * speed);
            }
            this.transform.position = offset;
        }
    }
}
