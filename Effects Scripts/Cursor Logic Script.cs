using System.Collections;

public class CursorLogicScript : MonoBehaviour
{
    private Vector3 prevMousePos;
    private Vector3 currentMousePos;
    private Boolean shouldPlaySwipeSound;
    public AudioSource[] audioSources; // Array to hold all AudioSource components
    public float swipeSpeedThreshold; // Adjust this value to your preference
    public Boolean paused;

    private Coroutine swipeSoundCoroutine; // Reference to the coroutine

    public static CursorLogicScript cursorLogicInstance;

    void Start()
    {
        cursorLogicInstance = this;
        prevMousePos = Input.mousePosition;
    }

    void Update()
    {
        if (!paused)
        {
            currentMousePos = Input.mousePosition;
            Vector3 movementDirection = currentMousePos - prevMousePos;
            float movementSpeed = movementDirection.magnitude / Time.deltaTime;

            if (movementSpeed > swipeSpeedThreshold)
            {
                shouldPlaySwipeSound = true;
            }
            else
            {
                shouldPlaySwipeSound = false; // Reset the flag if movement speed is below threshold
            }

            prevMousePos = currentMousePos;

            if (shouldPlaySwipeSound && swipeSoundCoroutine == null)
            {
                swipeSoundCoroutine = StartCoroutine(PlaySwipeSoundDelay());
            }
        }
    }

    private IEnumerator PlaySwipeSoundDelay()
    {
        // Randomly select an AudioSource from the array
        int randomIndex = UnityEngine.Random.Range(0, audioSources.Length);
        audioSources[randomIndex].PlayOneShot(audioSources[randomIndex].clip);

        yield return new WaitForSeconds(0.1f); // Delay to ensure the sound is played after the swipe motion

        swipeSoundCoroutine = null; // Reset the coroutine reference
    }
}
