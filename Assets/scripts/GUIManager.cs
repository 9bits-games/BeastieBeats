using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GUIManager : MonoBehaviour9Bits {

    public float bgSize = 0.4f;
    public float paddingH = 0.1f;
    public float marginTop = 0.1f;
    public float buttonSize = 0.2f;

    ButtonNote[] buttonNotes = {
        new ButtonNote(Note.DO, "Note0"),
        new ButtonNote(Note.RE, "Note1"),
        new ButtonNote(Note.MI, "Note2"),
        new ButtonNote(Note.SOL, "Note3"),
        new ButtonNote(Note.LA, "Note4"),
    };

    ScoreManager scoreManager;
    Commander commander;

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
        Rect GUIRect = new Rect(0f, Screen.height * (1f - bgSize), Screen.width, Screen.height * bgSize);

        //Background
        GUI.Box(GUIRect, "");

        //Buttons
        float buttonPixelSize = buttonSize * Screen.width;
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
            buttonNote.OnGUI(btn_r);

            count++;
        }

        GUI.Box(new Rect(10, 10, 100, 40), String.Format("T: {0}", scoreManager.TotalScore));
        GUI.Box(new Rect(10, 50, 100, 40), String.Format("E: {0}", scoreManager.EmotionMeter));

		
	}

    private ButtonNote getButtonNote(Note note) {
        return Array.Find(buttonNotes, btn => btn.note == note);
    }

    private class NoteAheadEffect {
        public ButtonNote buttonNote;
        public float timeAhead;
        public float currentEffectTime;

        const float relativeSize = 0.5f;

        public void OnGUI(Rect area) {
            if (currentEffectTime <= timeAhead) {
                Rect effectRect = new Rect();

                float percentage = 1f - currentEffectTime / timeAhead;
                effectRect.width = area.width + percentage * relativeSize * area.width;
                effectRect.height = area.height + percentage * relativeSize * area.height;
                effectRect.center = area.center;
                GUI.Box(effectRect, "");
            }
        }

        public void Update () {
            currentEffectTime += Time.deltaTime;
//            Debug.Log(Time.deltaTime);
        }
    }

    private class ButtonNote {
        public Note note;
        public List<NoteAheadEffect> noteAheadEffects;
        public Commander commander;

        private string buttonName;

        public ButtonNote(Note note, string buttonName) {
            this.note = note;
            noteAheadEffects = new List<NoteAheadEffect>();
            this.buttonName = buttonName;
        }

        public void OnGUI(Rect area) {
            if(GUI.Button(area, note.Name)) playNote();

            noteAheadEffects.ForEach(effect => effect.OnGUI(area));
        }

        public void Update () {
            noteAheadEffects.ForEach(effect => effect.Update());
            if (Input.GetButtonDown(buttonName)) playNote();
        }

        private void playNote() {
            commander.AddCommand(new PlayNoteCommand(note));
        }
    }
}
