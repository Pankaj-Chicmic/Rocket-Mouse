using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour,CollidableInterface
{
    public void hit(GameObject Mouse,Audio audioManager,GameUi UIManager)
    {
        audioManager.CoinPlay();
        PlayerController MousePlayerController = Mouse.GetComponent<PlayerController>();
        MousePlayerController.score += 1;
        gameObject.SetActive(false);
        UIManager.UpdateScore();
        if (MousePlayerController.JetpackPower < 20)
        {
            MousePlayerController.JetpackPower += 2;
            UIManager.SetJetpackSliderAndText(MousePlayerController.JetpackPower);
        }
    }
}
