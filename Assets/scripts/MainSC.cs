﻿using UnityEngine;
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

	void Start () {
        paused = false;
        commander = new Commander();

        track = this.GetComponentInChildren<Track>();
        track.Play();

        //Connecting the ScoreManager with the Track
        scoreManager = this.GetComponent<ScoreManager>();
        track.OnNoteWellPlayed += scoreManager.OnNoteWellPlayed;
        track.OnNoteBadPlayed += scoreManager.OnNoteBadPlayed;
        track.OnNoteNotPlayed += scoreManager.OnNoteNotPlayed;

        //Connecting the GUIManager with the Track
        guiManager = this.GetComponent<GUIManager>();
        guiManager.Set(commander, scoreManager, this);
        track.OnNoteAhead += guiManager.NoteAhead;
        track.OnNoteWellPlayed += guiManager.NoteWellPlayed;
        track.OnNoteNotPlayed += guiManager.NoteNotPlayed;
//        track.OnNoteBadPlayed += guiManager.NoteBadPlayed;

        PlayNoteCommand.ClearOnNotePlayed();
        PlayNoteCommand.OnNotePlayed += OnNotePlayed;
    }
	
	void Update () {}

    public void Pause() {
        if (paused) {
            Time.timeScale = 1f;
            track.Play();
            paused = false;
        } else {
            Time.timeScale = 0f;
            track.Pause();
            paused = true;
        }
        Debug.Log("Time scale " + Time.timeScale);
    }

    public void Reset() {
        Application.LoadLevel(Application.loadedLevelName);
    }

    //When a note is played on the GUI we play it on the track.
    private void OnNotePlayed(PlayNoteCommand playNoteCommand) {
        track.PlayNote(playNoteCommand.Note);
    }
}
