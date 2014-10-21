using UnityEngine;
using System;
using System.Collections;

/**
 * Manages the Score of the game, the score is measured by the emotionometer.
 * The emotionometer meassures the exitement of the people for the performance
 * of the player, if the player hits notes the emotionometer raises, otherwise
 * it falls.
 * 
 * The emotionometer has a maximun that can't be surpased, but if the emotionometer
 * reaches 0 the player lost the game.
 **/
public class ScoreManager : MonoBehaviour9Bits
{
    // Triggered when the emotionometer reaches zero.
    public delegate void EmotionBelowLimit();
    public event EmotionBelowLimit OnEmotionBelowLimit;

    // The total score of all notes played.
    public int TotalScore { get; private set; }
    // The meassurement of the emotionometer.
    public int EmotionMeter { get; private set; }

    // Upper limit of the emotionometer.
    public int MaxEmotionMeter = 1000;
    // Initial points of the emotionometer.
    public int InitialEmotionMeter = 500;
    // The amount of points for correctly play a note.
    public int ScorePerGoodNote = 100;
    // The amount of points lost for missplay a note.
    public int ScorePerBadNote = 70;
    // The amount of points for let a note pass in the track without play it.
    public int ScorePerNotPlayNote = 120;

    void Start() {
        EmotionMeter = InitialEmotionMeter;
    }
//    void Update() {}

    //Listener of OnNoteWellPlayed
    public void OnNoteWellPlayed(Note note, float trackTime, float playTime) {
        TotalScore += ScorePerGoodNote;
        EmotionMeter = Mathf.Min(EmotionMeter + ScorePerGoodNote, MaxEmotionMeter);
        Debug.Log(String.Format("Note Well Played {0}", note.Name));
    }

    //Listener of OnNoteBadPlayed
    public void OnNoteBadPlayed(Note note, float playTime) {
//        TotalScore -= ScorePerBadNote;
        EmotionMeter -= ScorePerBadNote;
        checkEmotionMeter();
        Debug.Log(String.Format("Note Bad Played {0}", note.Name));
    }

    //Listener of OnNoteNotPlayed
    public void OnNoteNotPlayed(Note note) {
//        TotalScore -= ScorePerNotPlayNote;
        EmotionMeter -= ScorePerNotPlayNote;
        checkEmotionMeter();
        Debug.Log(String.Format("Note Not Played {0}", note.Name));
    }

    //Checks if the emotionometer is below 0 to trigger the EmotionBelowLimit event.
    private void checkEmotionMeter() {
        if (EmotionMeter <= 0) {
            if (OnEmotionBelowLimit != null) OnEmotionBelowLimit();
        }
    }
}
