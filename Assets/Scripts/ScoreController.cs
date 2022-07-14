using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public static float startScore = 200f;

    public float scoreThrustUpLoss = 0.3f;
    public float scoreThrustSideLoss = 0.03f;

    public float score;
    public float lastScore;

    private void Start()
    {
        score = startScore;
        lastScore = PlayerPrefs.GetFloat("lastScore", -101f);
    }

    public void UpdateScore(float thrustUp, float thrustLeft, float thrustRight)
    {
        if (thrustUp > 0f) {
            score -= scoreThrustUpLoss;
        }
        
        if (Mathf.Abs(thrustLeft) > 0f) {
            score -= scoreThrustSideLoss;
        }

        if (Mathf.Abs(thrustRight) > 0f)
        {
            score -= scoreThrustSideLoss;
        }

        score = (float)System.Math.Round(score, 2);
    }

    public void GameOverUpdateScore(bool wasNotSuccess)
    {
        if (wasNotSuccess)
        {
            score -= 100f;
        }
        else
        {
            score += 100f;
        }
        score = (float)System.Math.Round(score, 2);
        PlayerPrefs.SetFloat("lastScore", score);
    }


    public void ResetScore()
    {
        score = startScore;
    }

}
