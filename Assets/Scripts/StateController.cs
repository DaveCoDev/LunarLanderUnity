using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StateController : MonoBehaviour
{
    public ScoreController ScoreController;
    public GameObject Lander;
    public Rigidbody2D LanderRB;
    public GameObject Platform;

    public float landerX;
    public float landerY;
    public float landerVelX;
    public float landerVelY;
    public float landerAngle;
    public float landerAngularVel;
    public bool landingPadContact;

    public float platformPos;

    public float angleMax;
    public float anglularVelMax;
    public float velXMax;
    public float velYMax;
    public int framesInZoneThresh;

    private int framesInZone = 0;
    private bool isGameOver = false;
    
    void Start()
    {
        Lander = GameObject.Find("Lander");
        LanderRB = Lander.GetComponent<Rigidbody2D>();
        ScoreController = GameObject.Find("GameManager").GetComponent<ScoreController>();
        Platform = GameObject.Find("Platform");

        UpdateLanderState();
        //Randomly places the landing platform on the horizontal plane
        Platform.transform.position = new Vector3(Random.Range(-8f, 5.5f), -3f, 0f);
    }

    void Update()
    {
        UpdateLanderState();
        CheckGameOver();
    }

    private void UpdateLanderState()
    {
        landerX = Lander.transform.position.x;
        landerY = Lander.transform.position.y;
        landerVelX = LanderRB.velocity.x;
        landerVelY = LanderRB.velocity.y;
        landerAngle = Lander.transform.rotation.z;
        landerAngularVel = LanderRB.angularVelocity;
    }

    private void CheckGameOver()
    {
        // If the score (fuel) is less than 0, it means we ran out of fuel and the game ends
        if (ScoreController.score <= 0f && !isGameOver)
        {
            isGameOver = true;
            ScoreController.GameOverUpdateScore(true);
            ResetGame();
        }
    }

    private void ResetGame()
    {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }

    public void LanderCollision(Collision2D collision)
    {   
        //Ends the game if any of the non-platform colliders are hit by the lander
        string colliderName = collision.collider.name;
        if (colliderName == "Ground" || colliderName ==  "Left" || colliderName == "Right" || colliderName == "Top")
        {
            isGameOver = true;
            ScoreController.GameOverUpdateScore(true);
            ResetGame();
        }
    }

    public void LanderTrigger(Collider2D collider)
    {
        string colliderName = collider.name;
        if (colliderName == "LandingPadCheckZone")
        {
            landingPadContact = true;
            //If we have been in the zone for long enough without exceeding landing thresholds, we win.
            if (framesInZone > framesInZoneThresh)
            {
                isGameOver = true;
                ScoreController.GameOverUpdateScore(false);
                ResetGame();
            }
            else
            {
                //Check parameters needed for successful landing. If any are violated, we had a failed landing and lose.
                if (Mathf.Abs(landerAngle) > angleMax || Mathf.Abs(landerAngularVel) > anglularVelMax || Mathf.Abs(landerVelX) > velXMax || Mathf.Abs(landerVelY) > velYMax)
                {
                    isGameOver = true;
                    ScoreController.GameOverUpdateScore(true);
                    ResetGame();
                }
                else
                {
                    framesInZone++;
                }
            }
        }
    }

    public void LanderTriggerExit(Collider2D collider)
    {
        string colliderName = collider.name;
        if (colliderName == "LandingPadCheckZone")
        {
            landingPadContact = false;
            framesInZone = 0;
        }
    }

}
