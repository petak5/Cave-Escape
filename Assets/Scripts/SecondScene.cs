using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondScene : MonoBehaviour
{

    public static SecondScene instance { get; private set; }

    private GameObject enemy;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.FindWithTag("Enemy2");
        if (enemy != null)
        {
            enemy.GetComponent<Renderer>().enabled = GameManager.instance.GetDiamond();
            enemy.GetComponent<Collider2D>().enabled = GameManager.instance.GetDiamond();
        }
    }

}
