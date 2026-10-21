using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogicScript : MonoBehaviour
{
    public float globalTimer;
    public float playerScore;
    private float currentPitch = 1;
    public float pitchIncrement;
    public float targetPitch;
    public float whiteScreenRef;
    public Boolean paused;
    private Boolean isPlaying;
    private Boolean canIncrementTimer = true;
    private Boolean canFadeIn = true;
    private Boolean canGameOver = true;

    public TMP_Text scoreText;
    public TMP_Text finalScoreText;
    public SpriteRenderer whiteScreen;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public GameObject GameOverText;
    public ParticleSystem fruitDeathEffectPrefab;
    public ParticleSystem bombDeathEffectPrefab;
    public AudioSource fruitDeathSoundEffect;
    public AudioSource Ding;
    public AudioSource Explosion;
    public List<GameObject> prefabInstances = new List<GameObject>();

    public Boolean gameOver;

    public static LogicScript logicInstance;

    [SerializeField] Button _playAgain;
    [SerializeField] Button _resume;
    [SerializeField] Button _ReturntoMenu1;
    [SerializeField] Button _ReturntoMenu2;


    void Start()
    {
        _resume.onClick.AddListener(Resume);
        _ReturntoMenu1.onClick.AddListener(ReturnToMenu);
        _ReturntoMenu2.onClick.AddListener(ReturnToMenu);
        _playAgain.onClick.AddListener(playAgain);
        logicInstance = this;

    }


    void Update()
    {
        if (!paused && !gameOver)
        {
            if (canIncrementTimer)
            {
                StartCoroutine(incrememntGlobalTimer());
            }
            if (Input.GetKey(KeyCode.Escape))
            {
                pause();
            }
        }

        if (gameOver && canGameOver)
        {
            gameOverRoutine();
        }
    }

    public void gameOverRoutine()
    {
        canGameOver = false;
        for (int i = 0; i < prefabInstances.Count; i++)
        {
            Destroy(prefabInstances[i]);
        }
        finalScoreText.text = "Score: " + playerScore.ToString();
        scoreText.text = "";
        Invoke("fadeInWhiteScreen", 0.5f);

        Invoke("gameOverTextTrue", 1.5f);

        Invoke("gameOverMenuTrue", 5);
    }

    public void gameOverTextTrue()
    {
        GameOverText.SetActive(true);
    }

    public void gameOverMenuTrue()
    {
        gameOverMenu.SetActive(true);
        GameOverText.SetActive(false);
    }

    public void fadeInWhiteScreen()
    {
        whiteScreenRef += 0.05f;
        whiteScreen.color = new Color(1, 1, 1, whiteScreenRef);
        if (whiteScreenRef >= 1)
        {
            canFadeIn = false;
            Invoke("fadeOutWhiteScreen", 0.2f);
        }
        else if (canFadeIn)
        {
            Invoke("fadeInWhiteScreen", 0.05f);
        }
    }

    public void fadeOutWhiteScreen()
    {
        whiteScreenRef -= 0.05f;
        whiteScreen.color = new Color(1, 1, 1, whiteScreenRef);
        if (whiteScreenRef >= 0)
        {
            Invoke("fadeOutWhiteScreen", 0.05f);
        }
    }

    public void fruitDeathSound()
    {
        fruitDeathSoundEffect.Play();
    }

    public void bombDeathEffect(float x, float y)
    {

        // Check if the sound is not already playing
        if (!isPlaying)
        {
            isPlaying = true;
            increasePitchOfBomb(x, y);
        }
    }

    public void increasePitchOfBomb(float x, float y)
    {
        //play a death effect for the bomb
        ParticleSystem deathEffectInstance = Instantiate(bombDeathEffectPrefab, new UnityEngine.Vector2(x, y), transform.rotation);
        deathEffectInstance.Play();

        // Set the pitch of the AudioSource
        Ding.pitch = currentPitch;

        // Play the sound clip
        Ding.Play();

        // Increment the pitch for the next loop
        currentPitch += pitchIncrement;

        // Check if the target pitch is reached
        if (currentPitch < targetPitch)
        {
            // Call this function again after the clip has finished playing
            StartCoroutine(increasePitchLooper(x, y));
        }
        else
        {
            // Reset the pitch and isPlaying flag
            currentPitch = 1;
            isPlaying = false;
            Explosion.Play();
        }
    }

    public void destroyAllOnScreen()
    {
        for (int i = 0; i < prefabInstances.Count; i++)
        {
            GameObject temp = prefabInstances[i];
            temp.GetComponent<FruitScript>().destroyFruit();
        }
    }

    IEnumerator increasePitchLooper(float x, float y)
    {
        yield return new WaitForSeconds(Ding.clip.length - 0.15f);
        increasePitchOfBomb(x, y);
    }

    [ContextMenu("increase score")]
    public void addScore(int scoreToAdd)
    {
        playerScore += scoreToAdd;
        scoreText.text = playerScore.ToString();
    }

    IEnumerator incrememntGlobalTimer()
    {
        canIncrementTimer = false;
        globalTimer += 1;
        yield return new WaitForSeconds(1);
        canIncrementTimer = true;
    }

    public void spawnDeathEffect(float x, float y)
    {
        ParticleSystem deathEffectInstance = Instantiate(fruitDeathEffectPrefab, new UnityEngine.Vector2(x, y), transform.rotation);
        deathEffectInstance.Play();
    }

    public void pause()
    {
        // Find all game objects tagged "Fruit"
        GameObject[] fruitObjects = GameObject.FindGameObjectsWithTag("Fruit");

        // Loop through each fruit and pause it
        foreach (GameObject fruit in fruitObjects)
        {
            // Access and pause the fruit's script
            fruit.GetComponent<FruitScript>().paused = true;
        }
        paused = true;
        CursorLogicScript.cursorLogicInstance.paused = true;
        FruitSpawnerScript.fruitSpawnerInstance.paused = true;
        pauseMenu.SetActive(true);
    }

    private void Resume()
    {
        GameObject[] fruitObjects = GameObject.FindGameObjectsWithTag("Fruit");

        // Loop through each fruit and pause it
        foreach (GameObject fruit in fruitObjects)
        {
            // Access and pause the fruit's script
            fruit.GetComponent<FruitScript>().paused = false;
        }
        paused = false;
        CursorLogicScript.cursorLogicInstance.paused = false;
        FruitSpawnerScript.fruitSpawnerInstance.paused = false;
        pauseMenu.SetActive(false);
    }

    private void ReturnToMenu()
    {
        ScenesManager.sceneInstance.LoadMainMenu();
    }

    private void playAgain()
    {
        ScenesManager.sceneInstance.LoadNewGame();
    }

}
