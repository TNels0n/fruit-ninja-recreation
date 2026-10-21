using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawnerScript : MonoBehaviour
{
    public Boolean canSpawnFruit;
    public Boolean canIncrementTimer;
    public float globalTimer;
    public float timerDelay;
    public Boolean paused;
    public Boolean gameOver;
    public Boolean hardmode;

    public GameObject fruit;
    public GameObject bomb;

    public static FruitSpawnerScript fruitSpawnerInstance;

    void Start()
    {
        fruitSpawnerInstance = this;
    }
    void Update()
    {
        if (!paused && !gameOver)
        {
            if (canIncrementTimer)
            {
                StartCoroutine(incrementGlobalTimer());
            }

            if (canSpawnFruit && globalTimer == 3)
            {
                StartCoroutine(threeRandom());
            }
            else if (canSpawnFruit && globalTimer == 6)
            {
                StartCoroutine(redPepperRightSide());
            }
            else if (canSpawnFruit && globalTimer == 7)
            {
                StartCoroutine(threeLeftSide());
            }
            else if (canSpawnFruit && globalTimer == 10)
            {
                StartCoroutine(threeBottomRight());
            }
            else if (canSpawnFruit && globalTimer == 14)
            {
                StartCoroutine(frenzy(5));
            }
            else if (canSpawnFruit && globalTimer == 17)
            {
                StartCoroutine(specialFruitRightSide());
            }
             if (canSpawnFruit && globalTimer == 20)
            {
                StartCoroutine(threeRandom());
            }
            else if (canSpawnFruit && globalTimer == 23)
            {
                StartCoroutine(threeRightSide());
            }
            else if (canSpawnFruit && globalTimer == 26)
            {
                StartCoroutine(threeLeftSide());
            }
            else if (canSpawnFruit && globalTimer == 28)
            {
                StartCoroutine(threeBottomRight());
            }
            else if (canSpawnFruit && globalTimer == 30)
            {
                StartCoroutine(frenzy(5));
            }
            else if (canSpawnFruit && globalTimer == 32)
            {
                StartCoroutine(specialFruitRightSide());
            }
        }
    }

    //main fruit functions  

    public void boost(float num)
    {
        timerDelay=num;
    }

    IEnumerator threeRandom()
    {
        canSpawnFruit = false;

        for (int i = 0; i < 3; i++)
        {
            spawnFruit();
        }

        yield return new WaitForSeconds(1);
        canSpawnFruit = true;
    }

    IEnumerator threeBottomRight()
    {
        canSpawnFruit = false;

        for (int i = 0; i < 3; i++)
        {
            spawnFruit(-0.5f, -1 - (i * 2), 4, 10, 6);
        }

        yield return new WaitForSeconds(1);
        canSpawnFruit = true;
    }

    IEnumerator threeRightSide()
    {
        canSpawnFruit = false;

        for (int i = 0; i < 3; i++)
        {
            spawnFruit(-0.03f, -8.4f, -5 + (i * 3) + 3, 10, 12);
        }

        yield return new WaitForSeconds(1);
        canSpawnFruit = true;
    }

    IEnumerator threeLeftSide()
    {
        canSpawnFruit = false;

        for (int i = 0; i < 3; i++)
        {
            spawnFruit(-0.03f, 8.4f, -5 + (i * 3) + 3, -10, 12);
        }

        yield return new WaitForSeconds(1);
        canSpawnFruit = true;
    }

    IEnumerator specialFruitRightSide()
    {
        canSpawnFruit = false;

        spawnFruit(-0.05f, 0, 1.5f, 12, 5, 89);

        yield return new WaitForSeconds(1);
        canSpawnFruit = true;
    }

    IEnumerator redPepperRightSide()
    {
        canSpawnFruit = false;

        spawnFruit(-0.05f, 0, 1.5f, 12, 5, 89);

        yield return new WaitForSeconds(1);
        canSpawnFruit = true;
    }

    IEnumerator yellowPepperRightSide()
    {
        canSpawnFruit = false;

        spawnFruit(-0.05f, 0, 1.5f, 12, 5, 67);

        yield return new WaitForSeconds(1);
        canSpawnFruit = true;
    }

    IEnumerator frenzy(int numOfFruits)
    {
        canSpawnFruit = false;

        for (int i = 0; i < numOfFruits; i++)
        {
            spawnFruit();
        }

        yield return new WaitForSeconds(1);
        canSpawnFruit = true;
    }

    //Spawn Functions

    IEnumerator incrementGlobalTimer()
    {
        canIncrementTimer = false;
        globalTimer += 1;
        yield return new WaitForSeconds(timerDelay);
        canIncrementTimer = true;
    }

    public Boolean willSpawnBomb()
    {
        float temp = UnityEngine.Random.Range(0, 100);
        if (hardmode)
        {
            if (temp < 20)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (temp < 10)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public void spawnFruit()
    {
        if (willSpawnBomb())
        {
            spawnBomb();
        }
        else
        {
            Instantiate(fruit, transform.position, transform.rotation);
        }
    }

    public void spawnFruit(float a, float h, float k, float dev)
    {
        if (willSpawnBomb())
        {
            spawnBomb(a, h, k, dev);
        }
        else
        {
            GameObject test = Instantiate(fruit, transform.position, transform.rotation);
            test.GetComponent<FruitScript>().assignValues(a, h, k, dev);
        }
    }

    public void spawnFruit(float a, float h, float k, float dev, float arch)
    {
        if (willSpawnBomb())
        {
            spawnBomb(a, h, k, dev, arch);
        }
        else
        {
            GameObject test = Instantiate(fruit, transform.position, transform.rotation);
            test.GetComponent<FruitScript>().assignValues(a, h, k, dev, arch);
        }

    }

    public void spawnFruit(float a, float h, float k, float dev, float arch, int spr)
    {
        if (willSpawnBomb())
        {
            spawnBomb(a, h, k, dev, arch, spr);
        }
        else
        {
            GameObject test = Instantiate(fruit, transform.position, transform.rotation);
            test.GetComponent<FruitScript>().assignValues(a, h, k, dev, arch, spr);
        }

    }

    public void spawnBomb()
    {
        GameObject test = Instantiate(bomb, transform.position, transform.rotation);
        test.GetComponent<FruitScript>().assignValues(true);
    }

    public void spawnBomb(float a, float h, float k, float dev)
    {
        GameObject test = Instantiate(bomb, transform.position, transform.rotation);
        test.GetComponent<FruitScript>().assignValues(a, h, k, dev, true);
    }

    public void spawnBomb(float a, float h, float k, float dev, float arch)
    {
        GameObject test = Instantiate(bomb, transform.position, transform.rotation);
        test.GetComponent<FruitScript>().assignValues(a, h, k, dev, arch, true);
    }

    public void spawnBomb(float a, float h, float k, float dev, float arch, int spr)
    {
        GameObject test = Instantiate(bomb, transform.position, transform.rotation);
        test.GetComponent<FruitScript>().assignValues(a, h, k, dev, arch, spr, true);
    }
}
