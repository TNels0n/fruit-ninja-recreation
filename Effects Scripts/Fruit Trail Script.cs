using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

public class FruitTrailScript : MonoBehaviour
{
    public Transform transformVar;
    public static FruitTrailScript fruitTrailInstance;

    void Start(){
        fruitTrailInstance=this;
    }


    // Update is called once per frame
    void Update()
    {
        transform.localScale = transform.localScale - new Vector3(0.05f, 0.05f, 0f);
        if (transform.localScale.x < 0 && transform.localScale.y < 0)
        {
            Destroy(gameObject);
        }
    }
}
