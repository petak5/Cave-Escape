using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public bool hasDiamond { get; private set; } = false;
    public bool hasPowerUp { get; private set; } = false;
    public string previousScene;

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

    public bool GetDiamond()
    {
        return hasDiamond;
    }
    
    public void RestartDiamond()
    {
        hasDiamond = false;
    }

    public void EnablePowerUp()
    {
        hasPowerUp = true;
    }

    public void DisablePowerUp()
    {
        hasPowerUp = false;
    }
}
