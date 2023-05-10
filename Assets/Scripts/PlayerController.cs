using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    public float MouseJumpForce = 75f, MouseFrontMoveDistance = 0.1f;
    private Rigidbody2D MouseRigidbody;
    public Animator MouseAnimator;
    [SerializeField] ParticleSystem Jetpack;
    bool JetPackActive;
    public float score;
    [SerializeField] Audio audioManager;
    [SerializeField] GameUi UIManager;
    public float JetpackPower;
    [SerializeField] ParralexCamera parralexCamera;
    int Distance;
    float IntialDistance;
    [SerializeField] RewardedAdsButton Ad;
    [SerializeField] Button _showAdButton, closeAdButton;
    void Start()
    {
        JetPackActive = true;
        MouseRigidbody = gameObject.GetComponent<Rigidbody2D>();
        MouseAnimator = gameObject.GetComponent<Animator>();
        score = 0;
        UIManager.UpdateScore();
        JetpackPower = PlayerPrefs.GetFloat("JetpackPower");
        IntialDistance = transform.position.x;
    }
    void FixedUpdate()
    {
        if (Input.GetButton("Fire1") && !MouseAnimator.GetBool("isDead") && JetpackPower > 1)
        {
            MouseRigidbody.AddForce(new Vector3(0, MouseJumpForce, 0));
            JetPackActive = true;
            JetpackPower -= 20 * Time.fixedDeltaTime;
        }
        else
        {
            JetPackActive = false;
        }
        UIManager.SetJetpackSliderAndText(JetpackPower);
        if (!MouseAnimator.GetBool("isDead")) gameObject.transform.position = new Vector3(gameObject.transform.position.x + MouseFrontMoveDistance, gameObject.transform.position.y, gameObject.transform.position.z);
        var JetpackEmission = Jetpack.emission;
        if (JetPackActive)
        {
            JetpackEmission.rateOverTime = 300;
            audioManager.SetJetpackSoundMax();
        }
        else
        {
            JetpackEmission.rateOverTime = 150;
            audioManager.SetJetpackSoundMid();
        }
        parralexCamera.offset = transform.position.x;
        Distance = (int)(transform.position.x - IntialDistance);
        UIManager.SetDistance(Distance);
        if ((int)JetpackPower <= 0 && !_showAdButton.gameObject.activeSelf && !Ad.closedByPlayer)
        {
            Ad.showAdButtons();
        }
        else if (((int)JetpackPower > 20 && _showAdButton.gameObject.activeSelf) || MouseAnimator.GetBool("isDead"))
        {
            Ad.hideAdButtons();
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            MouseAnimator.SetBool("isGrounded", true);
            var JetpackEmission = Jetpack.emission;
            JetpackEmission.enabled = false;
            audioManager.SetJetpackSoundMin();
            if (!MouseAnimator.GetBool("isDead"))
            {
                audioManager.SetFootSoundMax();
            }
            else
            {
                audioManager.SetFootSoundMin();
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            MouseAnimator.SetBool("isGrounded", false);
            var JetpackEmission = Jetpack.emission;
            JetpackEmission.enabled = true;
            audioManager.SetFootSoundMin();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!MouseAnimator.GetBool("isDead"))
        {
            CollidableInterface Obj = collision.gameObject.GetComponent<CollidableInterface>();
            if (Obj != null)
            {
                Obj.hit(gameObject, audioManager, UIManager);
            }
        }
    }
}
