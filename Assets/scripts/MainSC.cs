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

	void Start () {
        commander = new Commander();

        track = this.GetComponentInChildren<Track>();
        track.Play();

        //Connecting the ScoreManager with the Track
        scoreManager = this.GetComponent<ScoreManager>();
        track.OnNoteWellPlayed += scoreManager.OnNoteWellPlayed;
        track.OnNoteBadPlayed += scoreManager.OnNoteBadPlayed;
        track.OnNoteNotPlayed += scoreManager.OnNoteNotPlayed;
//        scoreManager.Set(track);

        //Connecting the GUIManager with the Track
        guiManager = this.GetComponent<GUIManager>();
        guiManager.Set(commander, scoreManager);
        track.OnNoteAhead += guiManager.NoteAhead;

        PlayNoteCommand.OnNotePlayed += OnNotePlayed;
    }
	
	void Update () {
	}

    //When a note is played on the GUI we play it on the track.
    private void OnNotePlayed(PlayNoteCommand playNoteCommand) {
        track.PlayNote(playNoteCommand.Note);
    }
}
