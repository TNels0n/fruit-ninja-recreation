using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FruitScript : MonoBehaviour
{
    private float a;
    private float h;
    public float k;
    public Boolean paused;
    public float archSpeed;
    private float overideA;
    private float overideH;
    private float overideK;
    private float overideXDeviation;
    private float overideArchSpeed;
    private int overideSpr;
    private bool fruitValsOveridden;
    public Sprite[] spriteArray;
    public Sprite[] spriteDeathArray;
    public float rotationSpeed;
    private Boolean canPlaySizzleSfx = true;
    private Boolean canPlayFruitPopUpSFX = true;
    private Boolean gameOver;
    private Boolean isBigFruit;
    private Boolean leftSideFruit;
    private Boolean isBomb;
    private Boolean isYellowPepper;
    private Boolean isRedPepper;
    private float sliceCounter;
    private SpriteRenderer spriteRenderer;
    private float xDeviation;
    private float yVal;
    private float xVal;
    private float vertexX;
    private float vertexY;
    private float rot;

    public GameObject fruitTrail;
    public AudioSource sizzleSFX;
    public AudioSource fruitPopUpSFX;
    public GameObject fruitGameObject;

    public new Rigidbody2D rigidbody;

    public static FruitScript fruitInstance;

    void Start()
    {
        //Setting basic stuff needed for the game
        Physics2D.IgnoreLayerCollision(6, 6, true);
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        fruitInstance = this;
        // Store a reference to the Apple GameObject
        fruitGameObject = gameObject;

        // Access the LogicScript instance
        LogicScript logicScript = LogicScript.logicInstance;

        // Add the Apple GameObject to the objectsList
        logicScript.prefabInstances.Add(fruitGameObject);
        if (!isBomb)
        {
            ChangeSprite();
        }
        Application.targetFrameRate = 120;

        //randomizing the fruits trajectory and location unless its overidden
        if (fruitValsOveridden)
        {
            a = overideA;
            h = overideH;
            k = overideK;
            xDeviation = overideXDeviation;
            if (overideArchSpeed != 0)
            {
                archSpeed = overideArchSpeed;
            }
            transform.position = new Vector2(xDeviation, -6.85f);
            if (xDeviation > 0)
            {
                leftSideFruit = false;
            }
            else
            {
                leftSideFruit = true;
            }
        }
        else
        {
            h = UnityEngine.Random.Range(-5, 5);
            a = UnityEngine.Random.Range(-1.5f, -0.5f);
            xDeviation = UnityEngine.Random.Range(1f, 0f);
            if (xDeviation < 0.5)
            {
                xDeviation = -10;
                leftSideFruit = true;
            }
            else
            {
                xDeviation = 10;
                leftSideFruit = false;
            }
            transform.position = new Vector2(xDeviation, -6.85f);
        }
        if (overideSpr != 0)
        {
            ChangeSprite(overideSpr);
        }
        yVal = transform.position.y;
        vertexX = -h;
        vertexY = k;
    }

    void Update()
    {
        //checking if it is a bomb, if it is on screen and if the sfx is already playing
        if (isBomb && canPlaySizzleSfx && transform.position.y > -6 && transform.position.x < 10 && transform.position.x > -10)
        {
            canPlaySizzleSfx = false;
            sizzleSFX.Play();
        }
        if (paused || isBomb && (transform.position.y < -6 || transform.position.x > 10 || transform.position.x < -10))
        {
            canPlaySizzleSfx = true;
            sizzleSFX.Stop();
        }

        if (canPlayFruitPopUpSFX && transform.position.y > -6 && transform.position.x < 10 && transform.position.x > -10)
        {
            canPlayFruitPopUpSFX = false;
            fruitPopUpSFX.Play();
        }

        if (!paused && !gameOver)
        {
            rot += rotationSpeed;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, rot);
            /*
            guide on controlling the parabola:
            a value - lengthens and shortens the parabola. if it is negative, it faces downwards, if it is positive, it faces upwards
            h value - moves the parabola horizontally
            k value - moves the parabola vertically
            */
            // if the object goes out of camera view, its destroyed
            if (transform.position.y < -200)
            {
                removeGameObjectFromLogicList();
                Destroy(gameObject);
            }
            // make the object go from left to right
            if (leftSideFruit)
            {
                xVal = transform.position.x + archSpeed * Time.deltaTime;
                yVal = a * ((xVal + h) * (xVal + h)) + k;
                transform.position = new Vector2(xVal, yVal);
            }
            //make the object go from right to left 
            else
            {
                xVal = transform.position.x - archSpeed * Time.deltaTime;
                yVal = -a * ((xVal + h) * (xVal + h)) - k;
                yVal = -yVal;
                transform.position = new Vector2(xVal, yVal);
            }
            createTrail();
        }
    }

    void ChangeSprite()
    {
        spriteRenderer.sprite = spriteArray[UnityEngine.Random.Range(0, 99)];
    }

    void ChangeSprite(int spriteIndex)
    {
        spriteRenderer.sprite = spriteArray[spriteIndex];
    }

    public void removeGameObjectFromLogicList()
    {
        // Access the LogicScript instance
        LogicScript logicScript = LogicScript.logicInstance;

        // Remove the Apple GameObject from the objectsList
        logicScript.prefabInstances.Remove(fruitGameObject);
    }

    private void OnMouseExit()
    {
        //checks to see if the game is paused
        if (!paused && !gameOver)
        {
            if (isBomb)
            {
                bombDeathEffect();
            }
            else
            {
                if (isYellowPepper)
                {
                    FruitSpawnerScript.fruitSpawnerInstance.boost(0.75f);
                    destroyFruit();
                }
                else if (isRedPepper)
                {
                    FruitSpawnerScript.fruitSpawnerInstance.boost(0.5f);
                    destroyFruit();
                }
                else if (isBigFruit)
                {
                    if (sliceCounter == 20)
                    {
                        LogicScript.logicInstance.addScore(70);
                        destroyFruit();
                    }
                    else
                    {
                        LogicScript.logicInstance.fruitDeathSound();
                        LogicScript.logicInstance.spawnDeathEffect(transform.position.x, transform.position.y);
                        LogicScript.logicInstance.addScore(10);
                        sliceCounter++;
                    }
                }
                else
                {
                    destroyFruit();
                }
            }
            //if the fruit is a special fruit that needs to be sliced multiple times 

        }
    }

    public void createTrail()
    {
        Instantiate(fruitTrail, new Vector2(transform.position.x, transform.position.y), transform.rotation);
    }

    public void bombDeathEffect()
    {
        removeGameObjectFromLogicList();
        LogicScript.logicInstance.gameOver = true;
        FruitSpawnerScript.fruitSpawnerInstance.gameOver = true;
        LogicScript.logicInstance.bombDeathEffect(transform.position.x, transform.position.y);
        gameOver = true;

        StartCoroutine(delayedDeath(1));

    }

    public IEnumerator delayedDeath(int num)
    {
        yield return new WaitForSeconds(num);
        Destroy(gameObject);
    }

    public void destroyFruit()
    {
        LogicScript.logicInstance.fruitDeathSound();
        LogicScript.logicInstance.spawnDeathEffect(transform.position.x, transform.position.y);
        LogicScript.logicInstance.addScore(50);

        removeGameObjectFromLogicList();

        Destroy(gameObject);
    }

    //assign values for fruit objects

    public void assignValues(float a, float h, float k, float dev)
    {
        fruitValsOveridden = true;
        overideA = a;
        overideH = h;
        overideK = k;
        overideXDeviation = dev;
    }

    public void assignValues(float a, float h, float k, float dev, float arch)
    {
        fruitValsOveridden = true;
        overideA = a;
        overideH = h;
        overideK = k;
        overideXDeviation = dev;
        overideArchSpeed = arch;
    }

    public void assignValues(float a, float h, float k, float dev, float arch, int spr)
    {
        fruitValsOveridden = true;
        overideA = a;
        overideH = h;
        overideK = k;
        overideXDeviation = dev;
        overideArchSpeed = arch;
        overideSpr = spr;
        //checks to see if the fruit that is being passed in is a special fruit
        if (spr == 89)
        {
            transform.localScale = new Vector2(9, 9);
            isBigFruit = true;
        }
        else if (spr == 67 || spr == 29)
        {
            transform.localScale = new Vector2(8, 8);
            isYellowPepper = true;
        }
        else if (spr == 68 || spr == 30)
        {
            transform.localScale = new Vector2(8, 8);
            isRedPepper = true;
        }
    }

    //assign values for bomb objects

    public void assignValues(bool isbomb)
    {
        isBomb = isbomb;
    }

    public void assignValues(float a, float h, float k, float dev, bool isbomb)
    {
        fruitValsOveridden = true;
        isBomb = isbomb;
        overideA = a;
        overideH = h;
        overideK = k;
        overideXDeviation = dev;
    }

    public void assignValues(float a, float h, float k, float dev, float arch, bool isbomb)
    {
        fruitValsOveridden = true;
        isBomb = isbomb;
        overideA = a;
        overideH = h;
        overideK = k;
        overideXDeviation = dev;
        overideArchSpeed = arch;
    }

    public void assignValues(float a, float h, float k, float dev, float arch, int spr, bool isbomb)
    {
        fruitValsOveridden = true;
        isBomb = isbomb;
        overideA = a;
        overideH = h;
        overideK = k;
        overideXDeviation = dev;
        overideArchSpeed = arch;
        overideSpr = spr;
    }
}
