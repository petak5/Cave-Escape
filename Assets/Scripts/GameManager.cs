using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public bool hasDiamond { get; private set; } = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void GotDiamond()
    {
        hasDiamond = true;
    }

    public void RestartDiamond()
    {
        hasDiamond = false;
    }
}
