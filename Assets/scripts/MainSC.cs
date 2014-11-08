using UnityEngine;
using System.Collections;

/**
 * The Scene Controller of the main scene.
 * 
 * For now is being used to connect the event producers with its listeners.
 */
public class MainSC : SceneController {

    Commander commander;
    Track track;
    GUIManager guiManager;
    ScoreManager scoreManager;

    bool paused;

    void Awake() {
        paused = false;
        Time.timeScale = 1f;
        commander = new Commander();
        PlayNoteCommand.ClearOnNotePlayed();
    }

	void Start () {
        track = this.GetComponentInChildren<Track>();
        track.Play();
        track.OnTrackEnded += Win;

        //Connecting the ScoreManager with the Track
        scoreManager = this.GetComponent<ScoreManager>();
        track.OnNoteWellPlayed += scoreManager.OnNoteWellPlayed;
        track.OnNoteBadPlayed += scoreManager.OnNoteBadPlayed;
        track.OnNoteNotPlayed += scoreManager.OnNoteNotPlayed;
        scoreManager.OnEmotionBelowLimit += Loose;

        //Connecting the GUIManager with the Track
        guiManager = this.GetComponent<GUIManager>();
        guiManager.Set(commander, scoreManager, track, this);
        track.OnNoteAhead += guiManager.NoteAhead;
        track.OnNoteWellPlayed += guiManager.NoteWellPlayed;
        track.OnNoteNotPlayed += guiManager.NoteNotPlayed;
//        track.OnNoteBadPlayed += guiManager.NoteBadPlayed;

        PlayNoteCommand.OnNotePlayed += OnNotePlayed;
    }
	
	void Update () {}

    public void Loose() {
        guiManager.Loose();
        Pause();
        track.Pause();
    }

    public void Win() {
        guiManager.Win();
        Pause();
    }

    public void Pause() {
        //TODO: Estos estados deberían estár en SceneController no en GUIManager
        if (guiManager.state != GUIManager.GUIState.Win && guiManager.state != GUIManager.GUIState.Loose) {
            if (paused) {
                Time.timeScale = 1f;
                track.Play();
                guiManager.Unpause();
                paused = false;
            } else {
                Time.timeScale = 0f;
                track.Pause();
                guiManager.Pause();
                paused = true;
            }
        }
    }

    public void Reset() {
        Application.LoadLevel(Application.loadedLevelName);
    }

    //When a note is played on the GUI we play it on the track.
    private void OnNotePlayed(PlayNoteCommand playNoteCommand) {
        track.PlayNote(playNoteCommand.Note);
    }
}
