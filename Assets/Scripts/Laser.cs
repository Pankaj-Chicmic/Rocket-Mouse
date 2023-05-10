using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour, CollidableInterface
{
    SpriteRenderer LaserRenderer;
    [SerializeField] Sprite LaserOn;
    [SerializeField] Sprite LaserOff;
    [SerializeField] float minRotation;
    [SerializeField] float maxRotation;
    [SerializeField] float minToggle;
    [SerializeField] float maxToggle;
    float Rotation;
    float Toggle;
    bool isLaserOn;
    Collider2D laserCollider;

    void Start()
    {
        LaserRenderer = gameObject.GetComponent<SpriteRenderer>();
        Rotation = Random.Range(minRotation, maxRotation);
        Toggle = Random.Range(minToggle, maxToggle);
        isLaserOn = false;
        laserCollider = gameObject.GetComponent<Collider2D>();
        InvokeRepeating("laserToggle", 0f, Toggle);
    }
    void FixedUpdate()
    {
        gameObject.transform.Rotate(Vector3.forward, Rotation * Time.fixedDeltaTime);
    }
    void laserToggle()
    {
        isLaserOn = !isLaserOn;
        if (isLaserOn)
        {
            LaserRenderer.sprite = LaserOn;
            laserCollider.enabled = true;
        }
        else
        {
            LaserRenderer.sprite = LaserOff;
            laserCollider.enabled = false;
        }
    }
    public void hit(GameObject Mouse, Audio audioManager, GameUi UIManager)
    {
        audioManager.laserPlay();
        Mouse.GetComponent<Animator>().SetBool("isDead", true);
        UIManager.slideInRestartPanel();
    }
}
