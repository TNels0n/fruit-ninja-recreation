using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuLogicScript : MonoBehaviour
{
    public float globalTimer;
    private Boolean canIncrementTimer = true;

    public ParticleSystem deathEffectPrefab;
    public AudioSource deathSoundEffect;

    public static StartMenuLogicScript StartMenulogicInstance;

    void Start()
    {
        StartMenulogicInstance = this;
    }


    void Update()
    {
        if (canIncrementTimer)
        {
            StartCoroutine(incrememntGlobalTimer());
        }
    }

    public void fruitDeathSound()
    {
        deathSoundEffect.Play();
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
        ParticleSystem deathEffectInstance = Instantiate(deathEffectPrefab, new UnityEngine.Vector2(x, y), transform.rotation);
        deathEffectInstance.Play();
    }

}
