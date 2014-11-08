using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/**
 * GUIManager Handles the graphical interface and its inputs.
 */
public class GUIManager : MonoBehaviour9Bits {
    public enum GUIState {Playing, Paused, Loose, Win};
    public GUIState state;

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
    public Texture2D resetBtn;
    public Texture2D pauseBtn;
    //Texture of the note ahead effect
    public Texture2D noteAheadImage;

    public GUIStyle notePlayedFeedbackStyle;

    //The list of buttons to be presented on the board.
    ButtonNote[] buttonNotes = {
        new ButtonNote(Note.DO, "Note0"),
        new ButtonNote(Note.RE, "Note1"),
        // new ButtonNote(Note.MI, "Note2"),
        new ButtonNote(Note.FA, "Note2"),
        new ButtonNote(Note.SOL, "Note3"),
        new ButtonNote(Note.LA, "Note4"),
    };

    ScoreManager scoreManager;
    Commander commander;
    MainSC mainSC; // TODO: Quitar dependencia y reemplazar por comunicación con Commander o Eventos.

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

    public void NoteWellPlayed(Note note, float trackTime, float playTime) {
        ButtonNote btn = getButtonNote(note);

        PlayFeedbackEffect effect = PlayFeedbackEffect.WellPlayedEffect(
            btn, notePlayedFeedbackStyle
        );

        btn.playFeedbackEffects.Add(effect);
    }

    public void NoteBadPlayed(Note note, float playTime) {
        ButtonNote btn = getButtonNote(note);

        PlayFeedbackEffect effect = PlayFeedbackEffect.BadPlayedEffect(
            btn, notePlayedFeedbackStyle
        );

        btn.playFeedbackEffects.Add(effect);
    }

    public void NoteNotPlayed(Note note) {
        ButtonNote btn = getButtonNote(note);

        PlayFeedbackEffect effect = PlayFeedbackEffect.MissedEffect(
            btn, notePlayedFeedbackStyle
        );

        btn.playFeedbackEffects.Add(effect);
    }

    //Injects Commander and ScoreManager instances.
    public void Set(Commander commander, ScoreManager scoreManager, MainSC mainSC) {
        this.commander = commander;
        this.scoreManager = scoreManager;
        this.mainSC = mainSC;

        Array.ForEach(buttonNotes, btn => btn.commander = commander);
    }

	// Use this for initialization
	void Start () {
        state = GUIState.Playing;
    }

	// Update is called once per frame
    void Update () {
        if (Input.GetButtonDown("Pause")) this.mainSC.Pause();
        if (Input.GetButtonDown("Reset")) this.mainSC.Reset();

        Array.ForEach(buttonNotes, btn => btn.Update());
    }

	void OnGUI () {
        //Thr rectangle of the the board
        Rect GUIRect = new Rect(0f, Screen.height * (1f - bgSize), Screen.width, Screen.height * bgSize);

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

        Rect restRect = new Rect(30, 30, 60, 60);
        GUI.DrawTexture(restRect, resetBtn);
        Rect pauseRect = new Rect(110, 30, 60, 60);
        GUI.DrawTexture(pauseRect, pauseBtn);

        if (Event.current.type == EventType.MouseUp) {
            if (restRect.Contains(Event.current.mousePosition)) {
                this.mainSC.Reset();
            }

            if (pauseRect.Contains(Event.current.mousePosition)) {
                this.mainSC.Pause();
            }
        }

        //Only for debug:
        /*
        GUI.Box(new Rect((Screen.width - 170)/2, 10, 170, 25), String.Format("T: {0}, E: {1}, C: {2}",
            scoreManager.TotalScore, scoreManager.EmotionMeter, scoreManager.ComboCount));
            */
		
	}

    private ButtonNote getButtonNote(Note note) {
        return Array.Find(buttonNotes, btn => btn.note == note);
    }

	//TODO: El tablero de botones debería estár en una clase aparte, junto con sus efectos y botones.
	private class PlayFeedbackEffect {
        public static PlayFeedbackEffect WellPlayedEffect(ButtonNote buttonNote, GUIStyle textStyle) {
            return new PlayFeedbackEffect {
                buttonNote = buttonNote,
                text = buttonNote.note.Name,
                color = buttonNote.note.Color,
                textStyle = textStyle,
            };
        }

        public static PlayFeedbackEffect MissedEffect(ButtonNote buttonNote, GUIStyle textStyle) {
            return new PlayFeedbackEffect {
                buttonNote = buttonNote,
                text = "Miss",
                color = Color.gray,
                textStyle = textStyle,
            };
        }

        public static PlayFeedbackEffect BadPlayedEffect(ButtonNote buttonNote, GUIStyle textStyle) {
            return new PlayFeedbackEffect {
                buttonNote = buttonNote,
                text =  "Bad!",
                color = Color.gray,
                textStyle = textStyle,
            };
        }

		// The button over wich the effect is showed.
		public ButtonNote buttonNote;
        public GUIStyle textStyle;
        public string text;
        public Color color;

		public float timeBeforeFadeOut = 0.8f;
		public float fadeOutTime = 0.3f;
        public float risingSpeed = 40f;

        private float elapsedTime = 0f;
        private float currentHeight = 0f;

		//Plays the effect animation.
        public void OnGUI(Rect buttonRect) {

            if (elapsedTime < timeBeforeFadeOut + fadeOutTime) {
                float dt = Time.deltaTime;
                float alpha = 1f;
                currentHeight -= risingSpeed * dt;

                if (elapsedTime > timeBeforeFadeOut) {
                    float remainingTimeToFade = elapsedTime - timeBeforeFadeOut;
                    alpha = 1f - remainingTimeToFade / fadeOutTime;
                }

                color.a = alpha;
                textStyle.normal.textColor = color;
                buttonRect.y += -buttonRect.height * 0.5f + currentHeight;
                GUI.Label(buttonRect, text, textStyle);
            }

		}

		public void Update () {
            elapsedTime += Time.deltaTime;
		}
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
        public List<PlayFeedbackEffect> playFeedbackEffects;
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
            playFeedbackEffects = new List<PlayFeedbackEffect>();
            this.buttonName = buttonName;
        }

        //This will animate the effects and check for user input.
        public void OnGUI(Rect area, Texture2D buttonImage, Texture2D noteAheadImage) {
            Color prevColor = GUI.color;
            GUI.color = note.Color;

            GUI.DrawTexture(Util.InflateRectByFactor(area, 2f, 2f), buttonImage);
            noteAheadEffects.ForEach(effect => effect.OnGUI(area, noteAheadImage));

            GUI.color = prevColor;

            playFeedbackEffects.ForEach(effect => effect.OnGUI(area));

            //if(GUI.Button(area, note.Name)) playNote();
            if (Event.current.type == EventType.MouseUp && area.Contains(Event.current.mousePosition)) {
                playNote();
            }
        }

        //This will check for user input
        public void Update () {
            noteAheadEffects.ForEach(effect => effect.Update());
            playFeedbackEffects.ForEach(effect => effect.Update());

            if (Input.GetButtonDown(buttonName)) playNote();
        }

        //Sends the PlayNoteCommand to be excecuted by the commander.
        private void playNote() {
            commander.AddCommand(new PlayNoteCommand(note));
        }
    }
}
