using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    public ScoreController ScoreController;
    public StateController StateController;

    private PlayerInputActions playerInputActions;

    public float maxVelocity = 5;
    public float rotationSpeed = 0.25f;
    public float thrustForce = 9;
    public float initRotation = 35f;

    private Rigidbody2D rb;

    private const string RebindsPrefsName = "rebinds";

    private ParticleSystem thrustParticlesUp;
    private float defaultParticleUpStartSize;
    private ParticleSystem thrustParticlesLeft;
    private float defaultParticleLeftStartSize;
    private ParticleSystem thrustParticlesRight;
    private float defaultParticleRightStartSize;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ScoreController = GameObject.Find("GameManager").GetComponent<ScoreController>();
        StateController = GameObject.Find("GameManager").GetComponent<StateController>();

        thrustParticlesUp = GameObject.Find("MainParticles").GetComponent<ParticleSystem>();
        defaultParticleUpStartSize = thrustParticlesUp.main.startSize.constant;

        thrustParticlesLeft = GameObject.Find("LeftParticles").GetComponent<ParticleSystem>();
        defaultParticleLeftStartSize = thrustParticlesLeft.main.startSize.constant;

        thrustParticlesRight = GameObject.Find("RightParticles").GetComponent<ParticleSystem>();
        defaultParticleRightStartSize = thrustParticlesRight.main.startSize.constant;

        playerInputActions = new PlayerInputActions();
        LoadBindings();
        playerInputActions.Lander.Enable();
        playerInputActions.Lander.BackToMenu.performed += ReturnToMenu;
        
        //Set the Lander in a random trajectory at the start of the episode.
        rb.velocity = new Vector2(Random.Range(-maxVelocity / 2, maxVelocity / 2), Random.Range(-maxVelocity / 2, maxVelocity / 2));
        rb.rotation = Random.Range(-initRotation, initRotation);
        rb.angularVelocity = Random.Range(-initRotation, initRotation);
    }

    void FixedUpdate()
    {
        float thrustUpAmount = playerInputActions.Lander.ThrustUp.ReadValue<float>();
        ThrustUp(thrustUpAmount);
        PlayThrustParticle(thrustParticlesUp, defaultParticleUpStartSize, thrustUpAmount);

        float thrustLeftAmount = playerInputActions.Lander.ThrustLeft.ReadValue<float>();
        float thrustRightAmount = playerInputActions.Lander.ThrustRight.ReadValue<float>();
        ThrustSide(thrustLeftAmount, thrustRightAmount);
        PlayThrustParticle(thrustParticlesLeft, defaultParticleLeftStartSize, thrustLeftAmount);
        PlayThrustParticle(thrustParticlesRight, defaultParticleRightStartSize, thrustRightAmount);

        ClampVelocity();
        ScoreController.UpdateScore(thrustUpAmount, thrustLeftAmount, thrustRightAmount);
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        StateController.LanderCollision(collider);
    }
    
    void OnTriggerStay2D(Collider2D collision)
    {
        StateController.LanderTrigger(collision);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        StateController.LanderTriggerExit(collision);
    }

    private void ClampVelocity()
    {
        float x = Mathf.Clamp(rb.velocity.x, -maxVelocity, maxVelocity);
        float y = Mathf.Clamp(rb.velocity.y, -maxVelocity, maxVelocity);

        rb.velocity = new Vector2(x, y);
    }

    private void ThrustUp(float amount)
    {
        Vector2 force = transform.up * amount * thrustForce;
        rb.AddForce(force);
    }

    private void ThrustSide(float leftAmount, float rightAmount)
    {
        float amount = leftAmount + rightAmount;
        rb.AddTorque(-rotationSpeed * amount);
    }

    private void LoadBindings()
    {
        string rebinds = PlayerPrefs.GetString(RebindsPrefsName, string.Empty);

        if (string.IsNullOrEmpty(rebinds))
        {
            return;
        }
        else
        {
            playerInputActions.LoadBindingOverridesFromJson(rebinds);
        }
    }

    private void ReturnToMenu(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(0);
    }

    private void PlayThrustParticle(ParticleSystem ps, float defaultSize, float amount)
    {
        amount = Mathf.Abs(amount);
        if (amount > 0)
        {
            //Normalize size by upAmount
            ParticleSystem.MainModule main = ps.main;
            ParticleSystem.MinMaxCurve minMaxCurve = main.startSize;
            minMaxCurve.constant = amount * defaultSize;
            main.startSize = minMaxCurve;
            
            ps.Play();
        }
        else
        {
            ps.Stop();
        }
    }
}
