using UnityEngine;
using System;
using System.Collections;

public class ScoreManager : MonoBehaviour9Bits
{
    public delegate void EmotionBelowLimit();
    public event EmotionBelowLimit OnEmotionBelowLimit;

    public int TotalScore { get; private set; }
    public int EmotionMeter { get; private set; }

    public int MaxEmotionMeter = 1000;
    public int InitialEmotionMeter = 500;
    public int ScorePerGoodNote = 100;
    public int ScorePerBadNote = 70;
    public int ScorePerNotPlayNote = 120;

    void Start() {
        EmotionMeter = InitialEmotionMeter;
    }
//    void Update() {}

    public void OnNoteWellPlayed(Note note, float trackTime, float playTime) {
        TotalScore += ScorePerGoodNote;
        EmotionMeter = Mathf.Min(EmotionMeter + ScorePerGoodNote, MaxEmotionMeter);
        Debug.Log(String.Format("Note Well Played {0}", note.Name));
    }

    public void OnNoteBadPlayed(Note note, float playTime) {
//        TotalScore -= ScorePerBadNote;
        EmotionMeter -= ScorePerBadNote;
        checkEmotionMeter();
        Debug.Log(String.Format("Note Bad Played {0}", note.Name));
    }

    public void OnNoteNotPlayed(Note note) {
//        TotalScore -= ScorePerNotPlayNote;
        EmotionMeter -= ScorePerNotPlayNote;
        checkEmotionMeter();
        Debug.Log(String.Format("Note Not Played {0}", note.Name));
    }

    private void checkEmotionMeter() {
        if (EmotionMeter <= 0) {
            if (OnEmotionBelowLimit != null) OnEmotionBelowLimit();
        }
    }
}
