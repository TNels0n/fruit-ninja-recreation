using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StandardModeFruitScript : MonoBehaviour
{
    public float rotateSpeed;
    public Vector3 rotationDirection = new Vector3();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotateSpeed * rotationDirection * Time.deltaTime);
    }

    private void OnMouseExit()
    {
        destroyFruit();
    }

    public void destroyFruit()
    {
        StartMenuLogicScript.StartMenulogicInstance.fruitDeathSound();
        StartMenuLogicScript.StartMenulogicInstance.spawnDeathEffect(transform.position.x, transform.position.y);
        ScenesManager.sceneInstance.LoadNewGame();
        Destroy(gameObject);
    }
}
