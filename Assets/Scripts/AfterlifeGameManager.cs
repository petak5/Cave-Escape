using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AfterlifeGameManager : MonoBehaviour
{
    [SerializeField] private GameObject buttonPowerUpEnable;
    [SerializeField] private GameObject buttonPowerUpDisable;

    private void Start()
    {
        buttonPowerUpEnable.SetActive(!GameManager.instance.hasPowerUp);
        buttonPowerUpDisable.SetActive(GameManager.instance.hasPowerUp);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(GameManager.instance.previousScene);
        }
    }

    public void EnablePowerUp()
    {
        GameManager.instance.EnablePowerUp();
        buttonPowerUpEnable.SetActive(false);
        buttonPowerUpDisable.SetActive(true);
    }

    public void DisablePowerUp()
    {
        GameManager.instance.DisablePowerUp();
        buttonPowerUpEnable.SetActive(true);
        buttonPowerUpDisable.SetActive(false);
    }
}
