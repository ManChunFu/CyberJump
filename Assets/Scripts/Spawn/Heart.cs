using UnityEngine;
using UnityEngine.Assertions;

//Written by Kevin

public class Heart : MonoBehaviour
{

    [SerializeField] private PlayerValues playerValues;
    private UIManager uiManager;
    private GameObject spawnManager;

    [Tooltip("How large the heart will grow")]
    public float scale = 0.1f;

    float XScale;
    float YScale;

    [Tooltip("Duration for rescaling")]
    public float DurationOfRescale = 2f;
    float original_duration_rescale;
    bool expand = true;

    public bool AddLive = false;

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        Assert.IsNotNull(uiManager, "No reference to UIManager script.");

        spawnManager = GameObject.Find("SpawnManager");
        Assert.IsNotNull(uiManager, "No reference to UIManager script.");

        Assert.IsNotNull(playerValues, "No reference to PlayerValues scriptable object.");

        XScale = transform.localScale.x;
        YScale = transform.localScale.x;
        original_duration_rescale = DurationOfRescale;

    }

    // Update is called once per frame
    void Update()
    {

        if (expand)
        {
            if (DurationOfRescale > 0)
            {
                XScale += scale/2;
                YScale += scale/2;
                transform.localScale = new Vector3(XScale, YScale, transform.localScale.z);
                DurationOfRescale -= Time.deltaTime;
            }
            else
            {
                DurationOfRescale = original_duration_rescale;
                expand = false;
            }
        }
        if (!expand)
        {
            if (DurationOfRescale > 0)
            {
                XScale -= scale/2;
                YScale -= scale/2;
                transform.localScale = new Vector3(XScale, YScale, transform.localScale.z);
                DurationOfRescale -= Time.deltaTime;
            }
            else
            {
                DurationOfRescale = original_duration_rescale;
                expand = true;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (playerValues.Lives < 3)
            {
                AddLive = true;
                playerValues.Lives += 1;
                uiManager.UpdateLivesImage(playerValues.Lives);
            }
            AddLive = false;
            Destroy(gameObject);

        }

    }
}
