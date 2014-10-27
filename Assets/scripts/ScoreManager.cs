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
    // The meassurement of the emotionometer.
    public int ComboCount {
        get { return comboCount; }
        private set {
            comboCount = value;
            CheckCombo();
        }
    }

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

    public int GoodComboStreak = 3;
    public int CoolComboStreak = 7;
    public int GreatComboStreak = 15;
    public int AwesomeComboStreak = 20;

    public AudioClip GoodComboSound;
    public AudioClip CoolComboSound;
    public AudioClip GreatComboSound;
    public AudioClip AwesomeComboSound;

    private int comboCount;
    private AudioSource SoundEffect;

    private int comboLevel;

    void Start() {
        EmotionMeter = InitialEmotionMeter;
        comboCount = 0;
        comboLevel = 0;

        SoundEffect = GetComponent<AudioSource>();
        SoundEffect.priority = 0;
        SoundEffect.minDistance = float.MaxValue;

    }
//    void Update() {}

    public int CalculateMaximunScoreOfTrack(Track track) {
        return track.NotesCount * ScorePerGoodNote;
    }

    //Listener of OnNoteWellPlayed
    public void OnNoteWellPlayed(Note note, float trackTime, float playTime) {
        TotalScore += ScorePerGoodNote;
        EmotionMeter = Mathf.Min(EmotionMeter + ScorePerGoodNote, MaxEmotionMeter);
        Debug.Log(String.Format("Note Well Played {0}", note.Name));

        ComboCount++;
    }

    //Listener of OnNoteBadPlayed
    public void OnNoteBadPlayed(Note note, float playTime) {
        TotalScore -= ScorePerBadNote;
        EmotionMeter -= ScorePerBadNote;
        checkEmotionMeter();
        Debug.Log(String.Format("Note Bad Played {0}", note.Name));
        ComboCount = 0;
    }

    //Listener of OnNoteNotPlayed
    public void OnNoteNotPlayed(Note note) {
        TotalScore -= ScorePerNotPlayNote;
        EmotionMeter -= ScorePerNotPlayNote;
        checkEmotionMeter();
        Debug.Log(String.Format("Note Not Played {0}", note.Name));
        ComboCount = 0;
    }

    public void CheckCombo() {
        int toComboLVL = -1;

        if(ComboCount >= AwesomeComboStreak) {
            if(comboLevel < 4) {
                SoundEffect.clip = AwesomeComboSound;
                toComboLVL = 4;
            };
        } else if(ComboCount >= GreatComboStreak) {
            if(comboLevel < 3) {
                SoundEffect.clip = GreatComboSound;
                toComboLVL = 3;
            };
        } if(ComboCount >= CoolComboStreak) {
            if(comboLevel < 2) {
                SoundEffect.clip = CoolComboSound;
                toComboLVL = 2;
            };
        } else if(ComboCount >= GoodComboStreak) {
            if(comboLevel < 1) {
                SoundEffect.clip = GoodComboSound;
                toComboLVL = 1;
            };
        } else {
            comboLevel = 0;
        }

        if(toComboLVL > 0) {
            SoundEffect.Play();
            comboLevel = toComboLVL;
        }
        Debug.Log("Combo Lvl " + comboLevel);

    }

    //Checks if the emotionometer is below 0 to trigger the EmotionBelowLimit event.
    private void checkEmotionMeter() {
        if (EmotionMeter <= 0) {
            if (OnEmotionBelowLimit != null) OnEmotionBelowLimit();
        }
    }
}
