using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drink : MonoBehaviour,CollidableInterface
{
    public void hit(GameObject Mouse, Audio audioManager, GameUi UIManager)
    {
        PlayerController mousePlayerController = Mouse.GetComponent<PlayerController>();
        mousePlayerController.JetpackPower += 10;
        if (mousePlayerController.JetpackPower > 100)
        {
            mousePlayerController.JetpackPower = 100;
        }
        audioManager.CoinPlay();
        UIManager.SetJetpackSliderAndText(mousePlayerController.JetpackPower);
        gameObject.SetActive(false);
    }
}
