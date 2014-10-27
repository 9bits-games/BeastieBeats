﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/**
 * GUIManager Handles the graphical interface and its inputs.
 */
public class GUIManager : MonoBehaviour9Bits {

    //The size of the board in screen percentage.
    public float bgSize = 0.4f;
    //The padding of the buttons to the border of the board in screen percentage.
    public float paddingH = 0.1f;
    //The margin of the buttons to the top of the board in screen percentage.
    public float marginTop = 0.1f;
    //The size the buttons in screen percentage.
    public float buttonSize = 0.2f;
    //Texture of a button
    public Texture2D buttonImage;
    //Texture of the note ahead effect
    public Texture2D noteAheadImage;

    //The list of buttons to be presented on the board.
    ButtonNote[] buttonNotes = {
        new ButtonNote(Note.DO, "Note0"),
        new ButtonNote(Note.RE, "Note1"),
        new ButtonNote(Note.MI, "Note2"),
        new ButtonNote(Note.SOL, "Note3"),
        new ButtonNote(Note.LA, "Note4"),
    };

    ScoreManager scoreManager;
    Commander commander;

    /**
     * Adds an effect that indicates that a note is near in the track.
     * note: The note that is aproaching.
     * timeaAhead: The time for the note to reach the header of the track.
     * trackTime: The position of the note in the track.
     **/
    public void NoteAhead(Note note, float timeAhead, float trackTime) {
//        Debug.Log(String.Format("Ahead Note: {0} at {1}", note.Name, trackTime));

        ButtonNote btn = getButtonNote(note);

        NoteAheadEffect effect = new NoteAheadEffect{
            buttonNote = btn,
            timeAhead = timeAhead,
            currentEffectTime = 0
        };

        btn.noteAheadEffects.Add(effect);
    }

    //Injects Commander and ScoreManager instances.
    public void Set(Commander commander, ScoreManager scoreManager) {
        this.commander = commander;
        this.scoreManager = scoreManager;

        Array.ForEach(buttonNotes, btn => btn.commander = commander);
    }

	// Use this for initialization
//	void Start () {}

	// Update is called once per frame
    void Update () {
        Array.ForEach(buttonNotes, btn => btn.Update());
    }

	void OnGUI () {
        //Thr rectangle of the the board
        Rect GUIRect = new Rect(0f, Screen.height * (1f - bgSize), Screen.width, Screen.height * bgSize);

        //Draw the board
//        GUI.Box(GUIRect, "");

        //Drawing the Buttons
        //Buttons size
        float buttonPixelSize = buttonSize * Screen.width;
        //Horizontal Maring 
        float marginH = (Screen.width - buttonNotes.Length * buttonPixelSize - (buttonNotes.Length - 1) * paddingH * Screen.width) / 2f;
        float count = 0f;
        foreach (ButtonNote buttonNote in buttonNotes) {
            Rect btn_r = new Rect(
                left: marginH + count * paddingH * Screen.width + count * buttonPixelSize,
                top: (marginTop) * Screen.height + GUIRect.yMin,
                width: buttonPixelSize,
                height: buttonPixelSize 
            );

            //Here we handle the input and effects:
            buttonNote.OnGUI(btn_r, buttonImage, noteAheadImage);

            count++;
        }

        //Only for debug:
        GUI.Box(new Rect((Screen.width - 170)/2, 10, 170, 25), String.Format("T: {0}, E: {1}, C: {2}",
            scoreManager.TotalScore, scoreManager.EmotionMeter, scoreManager.ComboCount));
		
	}

    private ButtonNote getButtonNote(Note note) {
        return Array.Find(buttonNotes, btn => btn.note == note);
    }

    /**
     * Graphical effect that indicates that a note is comming in the track.
     * Is must be placed over a ButtonNote to indicate the user that the button
     * must be pressed.
     **/
    private class NoteAheadEffect {
        // The button over wich the effect is showed.
        public ButtonNote buttonNote;
        /* The total time of the animation of the effect sincronized with
         * the arrival of the note.
         */
        public float timeAhead;
        // The current reproduction time of the note.
        public float currentEffectTime;

        //The extra size of the effect over the button, relative to the size of the button.
        const float relativeSize = 4f;

        //Plays the effect animation.
        public void OnGUI(Rect area, Texture2D noteAheadImage) {
            if (currentEffectTime <= timeAhead) {
                Rect effectRect;

                float sizeFactor = (1f - currentEffectTime / timeAhead) * relativeSize + 1f;
                sizeFactor *= 0.4f; // The texture needs to be 0.4x smaller
                effectRect = Util.InflateRect(area, sizeFactor * area.width, sizeFactor * area.height);
//                GUI.Box(effectRect, "");

                GUI.DrawTexture(effectRect, noteAheadImage);
            }
        }

        public void Update () {
            currentEffectTime += Time.deltaTime;
//            Debug.Log(Time.deltaTime);
        }
    }

    /**
     * A button in the board.
     * On pressed plays a note.
     **/
    private class ButtonNote {
        //The note to play when pressed
        public Note note;
        //The list of NoteAheadEffect to render over this button.
        public List<NoteAheadEffect> noteAheadEffects;
        public Commander commander;

        //The Unity input name.
        private string buttonName;

        /**
         * note: The note to play when pressed.
         * buttonName: The Unity input name.
         **/
        public ButtonNote(Note note, string buttonName) {
            this.note = note;
            noteAheadEffects = new List<NoteAheadEffect>();
            this.buttonName = buttonName;
        }

        //This will animate the effects and check for user input.
        public void OnGUI(Rect area, Texture2D buttonImage, Texture2D noteAheadImage) {
            Color prevColor = GUI.color;
            GUI.color = note.Color;
            GUI.DrawTexture(Util.InflateRectByFactor(area, 2f, 2f), buttonImage);
            noteAheadEffects.ForEach(effect => effect.OnGUI(area, noteAheadImage));
            GUI.color = prevColor;

            //if(GUI.Button(area, note.Name)) playNote();
            if (Event.current.type == EventType.MouseUp && area.Contains(Event.current.mousePosition)) {
                playNote();
            }
        }

        //This will check for user input
        public void Update () {
            noteAheadEffects.ForEach(effect => effect.Update());
            if (Input.GetButtonDown(buttonName)) playNote();
        }

        //Sends the PlayNoteCommand to be excecuted by the commander.
        private void playNote() {
            commander.AddCommand(new PlayNoteCommand(note));
        }
    }
}
