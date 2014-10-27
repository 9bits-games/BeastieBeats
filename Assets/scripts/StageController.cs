using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using StageElements;

public class StageController : MonoBehaviour9Bits
{
    public ScoreManager scoreManager;
    public Track track;
    //The speed at wich the state of the stage will aproach to the score. Meassured in points per second
    public float scoreSpeed = 100;
    //The elements in the stage that will change with the score.
    public StageElement[] elements;

    private int maxScore;
    private float currentStateScore;

    public float PercentageToTarget { get { return currentStateScore / maxScore; } }

    void Start() {
        maxScore = scoreManager.CalculateMaximunScoreOfTrack(track);
        currentStateScore = scoreManager.TotalScore;
    }

    void Update () {
        if (currentStateScore < scoreManager.TotalScore) {
            currentStateScore = Math.Min(currentStateScore + scoreSpeed, scoreManager.TotalScore);
        } else  if (currentStateScore > scoreManager.TotalScore) {
            currentStateScore = Math.Max(currentStateScore - scoreSpeed, scoreManager.TotalScore);
        }

        float ptt = PercentageToTarget;
        Array.ForEach(elements, e => e.updateStatus(ptt));
    }
}
