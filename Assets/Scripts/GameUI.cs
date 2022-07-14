using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameUI : MonoBehaviour
{
    public ScoreController ScoreController;
    public StateController StateController;

    private Label scoreLabel;
    private Label lastScoreLabel;

    private Label XPosLabel;
    private Label YPosLabel;
    private Label XVelLabel;
    private Label YVelLabel;
    private Label AngleLabel;
    private Label AngleVelLabel;
    private Label LandingPadContactLabel;


    public float lastScoreFadeTime = 2f;
    private float remainingFadeTime;

    private const string debugPrefsName = "debugEnabled";
    private bool isDebugToggleEnabled;

    private void Start()
    {
        // get ScoreController component which is on the GameObject GameManager
        ScoreController = GameObject.Find("GameManager").GetComponent<ScoreController>();
        StateController = GameObject.Find("GameManager").GetComponent<StateController>();
        remainingFadeTime = lastScoreFadeTime;
        isDebugToggleEnabled = PlayerPrefs.GetInt(debugPrefsName) == 1;

        var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
        scoreLabel = rootVisualElement.Q<Label>("scoreLabel");
        lastScoreLabel = rootVisualElement.Q<Label>("lastScoreLabel");

        var debugContainer = rootVisualElement.Q<VisualElement>("DebugContainer");
        XPosLabel = debugContainer.Q<Label>("XPosLabel");
        YPosLabel = debugContainer.Q<Label>("YPosLabel");
        XVelLabel = debugContainer.Q<Label>("XVelLabel");
        YVelLabel = debugContainer.Q<Label>("YVelLabel");
        AngleLabel = debugContainer.Q<Label>("AngleLabel");
        AngleVelLabel = debugContainer.Q<Label>("AngleVelLabel");
        LandingPadContactLabel = debugContainer.Q<Label>("LandingPadContactLabel");

        if (isDebugToggleEnabled)
        {
            debugContainer.style.display = DisplayStyle.Flex;
        }
    }

    private void Update()
    {
        SetScore();
        SetLastScore();
        SetDebugInfo();
        remainingFadeTime -= Time.deltaTime;
    }

    private void SetScore()
    {
        scoreLabel.text = "Score: " + ScoreController.score;
    }

    private void SetLastScore()
    {
        float lastScore = ScoreController.lastScore;
        if (lastScore >= -101f)
        {
            lastScoreLabel.text = "Last Score: " + ScoreController.lastScore;
            lastScoreLabel.style.opacity = Mathf.Lerp(0f, 1f, remainingFadeTime/lastScoreFadeTime);
        }
        else
        {
            lastScoreLabel.text = "";
        }
    }

    private void SetDebugInfo()
    {
        if (isDebugToggleEnabled)
        {
            XPosLabel.text = "X Pos: " + System.Math.Round(StateController.landerX, 3).ToString();
            YPosLabel.text = "Y Pos: " + System.Math.Round(StateController.landerY, 3).ToString();
            XVelLabel.text = "X Vel: " + System.Math.Round(StateController.landerVelX, 3).ToString();
            YVelLabel.text = "Y Vel: " + System.Math.Round(StateController.landerVelY, 3).ToString();
            AngleLabel.text = "Angle: " + System.Math.Round(StateController.landerAngle, 3).ToString();
            AngleVelLabel.text = "Angle Vel: " + System.Math.Round(StateController.landerAngularVel, 3).ToString();
            LandingPadContactLabel.text = "Landing Pad Contact?: " + StateController.landingPadContact.ToString();
        }
    }
    
}
