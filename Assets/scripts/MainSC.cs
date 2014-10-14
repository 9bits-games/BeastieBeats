using UnityEngine;
using System.Collections;

public class MainSC : SceneController {

    Commander commander;
    Track track;
    GUIManager guiManager;
    ScoreManager scoreManager;

	// Use this for initialization
	void Start () {
        commander = new Commander();

        track = this.GetComponentInChildren<Track>();
        track.Play();

        scoreManager = this.GetComponent<ScoreManager>();
        track.OnNoteWellPlayed += scoreManager.OnNoteWellPlayed;
        track.OnNoteBadPlayed += scoreManager.OnNoteBadPlayed;
        track.OnNoteNotPlayed += scoreManager.OnNoteNotPlayed;
//        scoreManager.Set(track);

        guiManager = this.GetComponent<GUIManager>();
        guiManager.Set(commander, scoreManager);
        track.OnNoteAhead += guiManager.NoteAhead;

        PlayNoteCommand.OnNotePlayed += OnNotePlayed;
    }
	
	// Update is called once per frame
	void Update () {
	}

    private void OnNotePlayed(PlayNoteCommand playNoteCommand) {
        track.PlayNote(playNoteCommand.Note);
    }
}
