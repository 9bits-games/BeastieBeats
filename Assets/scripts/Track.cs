using UnityEngine;
using System;
using System.Collections;

/**
 * The Track holds the song being played, the notes and its position in the song,
 * and manages the play of the notes.
 */
public class Track : MonoBehaviour9Bits
{
    //Triggered when a note is detected ahead in the track.
    public delegate void NoteAhead(Note note, float timeAhead, float trackTime);
    public event NoteAhead OnNoteAhead;

    //Triggered when a note is correcty played.
    public delegate void NoteWellPlayed(Note note, float trackTime, float playTime);
    public event NoteWellPlayed OnNoteWellPlayed;

    //Triggered when a note is moissplayed.
    public delegate void NoteBadPlayed(Note note, float playTime);
    public event NoteBadPlayed OnNoteBadPlayed;

    //Triggered when a note is not played.
    public delegate void NoteNotPlayed(Note note);
    public event NoteNotPlayed OnNoteNotPlayed;

    //Triggered when track header is at the end.
    public delegate void TrackEnded();
    public event TrackEnded OnTrackEnded;

//    public Boolean autoPlay = false;

    // The background musinc to play in the track.
    public AudioClip bgMusic;
    // The melody with the notes that the player must play
    public AudioClip melody;
    // The audio effect of a note bad played
    public AudioClip errorSound;

    /* Seconds to look ahead for notes in the track.
     * If a Note is in Time + lookAheadTime the event NoteAhead is fired.
     */
    public float lookAheadTime = 1f;
    /* The quanty of seconds of tolerancy to play a note,
     * If the player don't plays a note exacly in the time that
     * the note is in the header but plays it arround the tolerancy
     * time is considered a note well played.
     */
    public float timeTolerancy = 0.3f;

    // The position of the header in the track.
    public float Time {
        get{ return bgAudio.time; }
        set{ bgAudio.time = value; }
    }

    public float PercentagePlayed {
        get{ return bgAudio.time / bgMusic.length * 100f; }
    }

    public bool isPlaying { get{ return bgAudio.isPlaying; } }

    public Boolean ended { get; set; }

    public void Play() {
        //        Playing = true;
        bgAudio.Play();
        melodyAudio.Play();
    }

    public void Pause() {
        //        Playing = false;
        bgAudio.Pause();
        melodyAudio.Pause();
    }

    public void Stop() {
        //        Playing = false;
        bgAudio.Stop();
        melodyAudio.Stop();
    }

    public int NotesCount { get { return this.notes.Length; } }

    /* Plays a note in the track, if the note is correclty played the NoteWellPlayed is triggered,
     * otherwise NoteBadPlayed is triggered.
     */
    public bool PlayNote(Note note) {
        NoteInTrack[] nearNotesInTrack = QueryNotesInTrack(Time - timeTolerancy, Time + timeTolerancy, note);

        bool hit = false;

        foreach(NoteInTrack nit in nearNotesInTrack) {
            hit = true;
            if (!nit.played) {
                nit.played = true;
                if (OnNoteWellPlayed != null)
                    OnNoteWellPlayed(nit.note, nit.time, Time);

                //Only one wellPlay by note played.
                break;
            }
        }

        if (!hit) {
            if (OnNoteBadPlayed != null) OnNoteBadPlayed(note, Time);
        }

        return hit;
    }

    //The notes in the track.
    protected NoteInTrack[] notes;
   
    private AudioSource bgAudio;
    private AudioSource melodyAudio;
    private AudioSource audioEffect;
    private int aheadNoteIndex;
    private int backNoteIndex;

    void Awake () {
        OnNoteAhead = null;
        OnNoteWellPlayed = null;
        OnNoteBadPlayed = null;
        OnNoteNotPlayed = null;
        OnTrackEnded = null;
        ended = false;
        SetUpAudioSources();

        SetUpEventsForMelody();

        aheadNoteIndex = -1;
        backNoteIndex = -1;

        //Here we declare the notes in the track.
        notes = new NoteInTrack[]{
            new NoteInTrack{note = Note.RE, time = 18.76f},
            new NoteInTrack{note = Note.FA, time = 18.96f},
            new NoteInTrack{note = Note.SOL, time = 19.20f},
            new NoteInTrack{note = Note.LA, time = 19.56f},
            new NoteInTrack{note = Note.RE, time = 22.56f},
            new NoteInTrack{note = Note.FA, time = 22.80f},
            new NoteInTrack{note = Note.SOL, time = 23.04f},
            new NoteInTrack{note = Note.LA, time = 23.40f},
            new NoteInTrack{note = Note.DO, time = 23.76f},
            new NoteInTrack{note = Note.LA, time = 24.24f},
            new NoteInTrack{note = Note.SOL, time = 24.48f},
            new NoteInTrack{note = Note.FA, time = 24.72f},
            new NoteInTrack{note = Note.RE, time = 26.40f},
            new NoteInTrack{note = Note.FA, time = 26.64f},
            new NoteInTrack{note = Note.SOL, time = 26.88f},
            new NoteInTrack{note = Note.LA, time = 27.24f},
            new NoteInTrack{note = Note.DO, time = 28.56f},
            new NoteInTrack{note = Note.SOL, time = 29.04f},
            new NoteInTrack{note = Note.LA, time = 29.52f},
            new NoteInTrack{note = Note.FA, time = 30.00f},
            new NoteInTrack{note = Note.RE, time = 30.72f},
            new NoteInTrack{note = Note.FA, time = 30.96f},
            new NoteInTrack{note = Note.SOL, time = 31.20f},
            new NoteInTrack{note = Note.LA, time = 31.44f},
            new NoteInTrack{note = Note.SOL, time = 31.68f},
            new NoteInTrack{note = Note.FA, time = 31.92f},
            new NoteInTrack{note = Note.SOL, time = 32.16f},
            new NoteInTrack{note = Note.FA, time = 32.28f},
            new NoteInTrack{note = Note.DO, time = 32.64f},
            new NoteInTrack{note = Note.SOL, time = 32.88f},
            new NoteInTrack{note = Note.LA, time = 33.12f},
            new NoteInTrack{note = Note.FA, time = 33.36f},
            new NoteInTrack{note = Note.SOL, time = 33.60f},
            new NoteInTrack{note = Note.RE, time = 33.84f},
            new NoteInTrack{note = Note.FA, time = 34.32f},
            new NoteInTrack{note = Note.SOL, time = 34.80f},
            new NoteInTrack{note = Note.LA, time = 35.04f},
            new NoteInTrack{note = Note.RE, time = 35.28f},
            new NoteInTrack{note = Note.DO, time = 36.48f},
            new NoteInTrack{note = Note.SOL, time = 36.72f},
            new NoteInTrack{note = Note.LA, time = 36.96f},
            new NoteInTrack{note = Note.FA, time = 37.20f},
            new NoteInTrack{note = Note.SOL, time = 37.44f},
            new NoteInTrack{note = Note.RE, time = 37.68f},
            new NoteInTrack{note = Note.FA, time = 38.16f},
            new NoteInTrack{note = Note.SOL, time = 38.64f},
            new NoteInTrack{note = Note.RE, time = 38.88f},
            new NoteInTrack{note = Note.FA, time = 39.12f},
            new NoteInTrack{note = Note.SOL, time = 39.36f},
            new NoteInTrack{note = Note.LA, time = 39.60f},
            new NoteInTrack{note = Note.SOL, time = 39.84f},
            new NoteInTrack{note = Note.DO, time = 40.32f},
            new NoteInTrack{note = Note.SOL, time = 40.56f},
            new NoteInTrack{note = Note.LA, time = 40.80f},
            new NoteInTrack{note = Note.FA, time = 41.04f},
            new NoteInTrack{note = Note.SOL, time = 41.28f},
            new NoteInTrack{note = Note.RE, time = 41.56f},
            new NoteInTrack{note = Note.FA, time = 42.00f},
            new NoteInTrack{note = Note.SOL, time = 42.48f},
            new NoteInTrack{note = Note.RE, time = 42.72f},
            new NoteInTrack{note = Note.FA, time = 42.96f},
            new NoteInTrack{note = Note.SOL, time = 43.20f},
            new NoteInTrack{note = Note.LA, time = 43.32f},
            new NoteInTrack{note = Note.SOL, time = 43.44f},
            new NoteInTrack{note = Note.FA, time = 43.56f},
            new NoteInTrack{note = Note.RE, time = 43.68f},
            new NoteInTrack{note = Note.FA, time = 43.80f},
            new NoteInTrack{note = Note.SOL, time = 43.92f},
            new NoteInTrack{note = Note.DO, time = 44.16f},
            new NoteInTrack{note = Note.RE, time = 44.40f},
            new NoteInTrack{note = Note.FA, time = 44.64f},
            new NoteInTrack{note = Note.LA, time = 44.88f},
            new NoteInTrack{note = Note.SOL, time = 45.12f},
            new NoteInTrack{note = Note.RE, time = 45.36f},
            new NoteInTrack{note = Note.FA, time = 45.60f},
            new NoteInTrack{note = Note.SOL, time = 45.84f},
            new NoteInTrack{note = Note.RE, time = 46.08f},
            new NoteInTrack{note = Note.FA, time = 46.32f},
            new NoteInTrack{note = Note.SOL, time = 46.56f},
            new NoteInTrack{note = Note.LA, time = 46.80f},
            new NoteInTrack{note = Note.DO, time = 47.04f},
            new NoteInTrack{note = Note.LA, time = 47.28f},
            new NoteInTrack{note = Note.SOL, time = 47.52f},
            new NoteInTrack{note = Note.FA, time = 47.64f},
            new NoteInTrack{note = Note.FA, time = 48.00f},
            new NoteInTrack{note = Note.RE, time = 48.12f},
            new NoteInTrack{note = Note.FA, time = 48.36f},
            new NoteInTrack{note = Note.RE, time = 48.48f},
            new NoteInTrack{note = Note.FA, time = 48.72f},
            new NoteInTrack{note = Note.RE, time = 48.84f},
            new NoteInTrack{note = Note.FA, time = 49.08f},
            new NoteInTrack{note = Note.RE, time = 49.20f},
            new NoteInTrack{note = Note.FA, time = 49.44f},
            new NoteInTrack{note = Note.SOL, time = 49.68f},
            new NoteInTrack{note = Note.FA, time = 49.92f},
            new NoteInTrack{note = Note.RE, time = 50.04f},
            new NoteInTrack{note = Note.FA, time = 50.28f},
            new NoteInTrack{note = Note.RE, time = 50.40f},
            new NoteInTrack{note = Note.FA, time = 50.64f},
            new NoteInTrack{note = Note.RE, time = 50.76f},
            new NoteInTrack{note = Note.FA, time = 51.00f},
            new NoteInTrack{note = Note.RE, time = 51.12f},
            new NoteInTrack{note = Note.FA, time = 51.36f},
            new NoteInTrack{note = Note.SOL, time = 51.48f},
            new NoteInTrack{note = Note.LA, time = 51.60f},
            new NoteInTrack{note = Note.SOL, time = 51.72f},
            new NoteInTrack{note = Note.FA, time = 51.84f},
            new NoteInTrack{note = Note.RE, time = 51.96f},
            new NoteInTrack{note = Note.FA, time = 52.20f},
            new NoteInTrack{note = Note.RE, time = 52.32f},
            new NoteInTrack{note = Note.FA, time = 52.56f},
            new NoteInTrack{note = Note.RE, time = 52.68f},
            new NoteInTrack{note = Note.FA, time = 52.92f},
            new NoteInTrack{note = Note.RE, time = 53.04f},
            new NoteInTrack{note = Note.FA, time = 53.28f},
            new NoteInTrack{note = Note.SOL, time = 53.52f},
            new NoteInTrack{note = Note.FA, time = 53.76f},
            new NoteInTrack{note = Note.RE, time = 53.88f},
            new NoteInTrack{note = Note.FA, time = 54.12f},
            new NoteInTrack{note = Note.RE, time = 54.24f},
            new NoteInTrack{note = Note.FA, time = 54.48f},
            new NoteInTrack{note = Note.RE, time = 54.60f},
            new NoteInTrack{note = Note.FA, time = 54.84f},
            new NoteInTrack{note = Note.LA, time = 54.96f},
            new NoteInTrack{note = Note.SOL, time = 55.20f},
            new NoteInTrack{note = Note.FA, time = 55.68f},
            new NoteInTrack{note = Note.RE, time = 55.80f},
            new NoteInTrack{note = Note.FA, time = 56.04f},
            new NoteInTrack{note = Note.RE, time = 56.16f},
            new NoteInTrack{note = Note.FA, time = 56.40f},
            new NoteInTrack{note = Note.RE, time = 56.52f},
            new NoteInTrack{note = Note.FA, time = 56.76f},
            new NoteInTrack{note = Note.RE, time = 56.88f},
            new NoteInTrack{note = Note.FA, time = 57.12f},
            new NoteInTrack{note = Note.SOL, time = 57.36f},
            new NoteInTrack{note = Note.FA, time = 57.60f},
            new NoteInTrack{note = Note.RE, time = 57.72f},
            new NoteInTrack{note = Note.FA, time = 57.96f},
            new NoteInTrack{note = Note.RE, time = 58.08f},
            new NoteInTrack{note = Note.FA, time = 58.32f},
            new NoteInTrack{note = Note.RE, time = 58.44f},
            new NoteInTrack{note = Note.FA, time = 58.68f},
            new NoteInTrack{note = Note.RE, time = 58.80f},
            new NoteInTrack{note = Note.FA, time = 59.04f},
            new NoteInTrack{note = Note.SOL, time = 59.16f},
            new NoteInTrack{note = Note.LA, time = 59.28f},
            new NoteInTrack{note = Note.DO, time = 59.40f},
            new NoteInTrack{note = Note.FA, time = 59.52f},
            new NoteInTrack{note = Note.RE, time = 59.64f},
            new NoteInTrack{note = Note.FA, time = 59.88f},
            new NoteInTrack{note = Note.RE, time = 60f},
            new NoteInTrack{note = Note.FA, time = 60.24f},
            new NoteInTrack{note = Note.RE, time = 60.36f},
            new NoteInTrack{note = Note.FA, time = 60.60f},
            new NoteInTrack{note = Note.RE, time = 60.72f},
            new NoteInTrack{note = Note.FA, time = 60.96f},
            new NoteInTrack{note = Note.SOL, time = 61.20f},
            new NoteInTrack{note = Note.FA, time = 61.44f},
            new NoteInTrack{note = Note.RE, time = 61.56f},
            new NoteInTrack{note = Note.FA, time = 61.80f},
            new NoteInTrack{note = Note.RE, time = 61.92f},
            new NoteInTrack{note = Note.FA, time = 62.28f},
            new NoteInTrack{note = Note.SOL, time = 62.40f},
            new NoteInTrack{note = Note.LA, time = 62.52f},
            new NoteInTrack{note = Note.DO, time = 62.64f},
            new NoteInTrack{note = Note.RE, time = 65.52f},
            new NoteInTrack{note = Note.FA, time = 65.76f},
            new NoteInTrack{note = Note.LA, time = 66.00f},
            new NoteInTrack{note = Note.SOL, time = 66.24f},
            new NoteInTrack{note = Note.DO, time = 66.48f},
            new NoteInTrack{note = Note.FA, time = 66.72f},
            new NoteInTrack{note = Note.SOL, time = 66.96f},
            new NoteInTrack{note = Note.FA, time = 73.20f},
            new NoteInTrack{note = Note.RE, time = 73.44f},
            new NoteInTrack{note = Note.FA, time = 73.68f},
            new NoteInTrack{note = Note.SOL, time = 73.92f},
            new NoteInTrack{note = Note.LA, time = 74.16f},
            new NoteInTrack{note = Note.DO, time = 74.40f},
            new NoteInTrack{note = Note.LA, time = 76.32f},
            new NoteInTrack{note = Note.FA, time = 76.56f},
            new NoteInTrack{note = Note.SOL, time = 78.24f},
            new NoteInTrack{note = Note.RE, time = 78.48f},
            new NoteInTrack{note = Note.RE, time = 80.16f},
            new NoteInTrack{note = Note.FA, time = 80.40f},
            new NoteInTrack{note = Note.SOL, time = 80.64f},
            new NoteInTrack{note = Note.LA, time = 81.00f},
            new NoteInTrack{note = Note.SOL, time = 81.36f},
            new NoteInTrack{note = Note.FA, time = 81.84f},
            new NoteInTrack{note = Note.SOL, time = 82.08f},
            new NoteInTrack{note = Note.FA, time = 82.20f},
            new NoteInTrack{note = Note.RE, time = 82.56f},
            new NoteInTrack{note = Note.RE, time = 82.92f},
            new NoteInTrack{note = Note.RE, time = 84.00f},
            new NoteInTrack{note = Note.FA, time = 84.24f},
            new NoteInTrack{note = Note.SOL, time = 84.48f},
            new NoteInTrack{note = Note.LA, time = 84.84f},
            new NoteInTrack{note = Note.DO, time = 85.44f},
            new NoteInTrack{note = Note.RE, time = 85.68f},
            new NoteInTrack{note = Note.FA, time = 85.92f},
            new NoteInTrack{note = Note.LA, time = 86.16f},
            new NoteInTrack{note = Note.SOL, time = 8640f},
            new NoteInTrack{note = Note.FA, time = 86.64f},
            new NoteInTrack{note = Note.SOL, time = 86.88f},
            new NoteInTrack{note = Note.FA, time = 87.12f},
            new NoteInTrack{note = Note.SOL, time = 87.36f},
            new NoteInTrack{note = Note.LA, time = 87.48f},
            new NoteInTrack{note = Note.SOL, time = 87.60f},
            new NoteInTrack{note = Note.FA, time = 87.84f},
            new NoteInTrack{note = Note.RE, time = 88.08f},
            new NoteInTrack{note = Note.FA, time = 88.32f},
            new NoteInTrack{note = Note.RE, time = 88.44f},
            new NoteInTrack{note = Note.FA, time = 88.68f},
            new NoteInTrack{note = Note.RE, time = 88.80f},
            new NoteInTrack{note = Note.FA, time = 89.04f},
            new NoteInTrack{note = Note.RE, time = 89.16f},
            new NoteInTrack{note = Note.FA, time = 89.40f},
            new NoteInTrack{note = Note.RE, time = 89.52f},
            new NoteInTrack{note = Note.FA, time = 89.76f},
            new NoteInTrack{note = Note.RE, time = 90.00f},
            new NoteInTrack{note = Note.SOL, time = 90.24f},
            new NoteInTrack{note = Note.LA, time = 90.36f},
            new NoteInTrack{note = Note.SOL, time = 90.48f},
            new NoteInTrack{note = Note.FA, time = 90.60f},
            new NoteInTrack{note = Note.RE, time = 90.72f},
            new NoteInTrack{note = Note.FA, time = 90.84f},
            new NoteInTrack{note = Note.SOL, time = 90.96f},
            new NoteInTrack{note = Note.LA, time = 91.08f},
            new NoteInTrack{note = Note.SOL, time = 91.20f},
            new NoteInTrack{note = Note.FA, time = 91.32f},
            new NoteInTrack{note = Note.SOL, time = 91.44f},
            new NoteInTrack{note = Note.LA, time = 91.56f},
            new NoteInTrack{note = Note.DO, time = 91.68f},
            new NoteInTrack{note = Note.LA, time = 91.80f},
            new NoteInTrack{note = Note.SOL, time = 91.92f},
            new NoteInTrack{note = Note.FA, time = 92.04f},
            new NoteInTrack{note = Note.FA, time = 92.16f},
            new NoteInTrack{note = Note.SOL, time = 92.64f},
            new NoteInTrack{note = Note.LA, time = 93.12f},
            new NoteInTrack{note = Note.DO, time = 93.60f},
            new NoteInTrack{note = Note.RE, time = 94.08f}
        };
        Array.ForEach(notes, nit => nit.time -= 7.7f);
        Debug.Log("Total Notes: " + notes.Length);

        //Cosas para pussys, quitar
        /*NoteInTrack[] nits = new NoteInTrack[notes.Length];
        float lastNoteTime = float.MinValue;
        int count = 0;
        foreach (NoteInTrack nit in notes) {
            if(lastNoteTime + 0.35f < nit.time) {
                nits[count++] = nit;
                lastNoteTime = nit.time;
            }
        }
        notes = new NoteInTrack[count];
        Array.Copy(nits, notes, count);*/
    }

    // Update is called once per frame
    void Update () {
        if (isPlaying) {
            AutoPlay();
            LookAheadNotes();
            CheckBackNotes();
        }

        //Cheking for the end of file:
        if (!ended && bgAudio.time >= bgMusic.length) {
            ended = true;
            if (OnTrackEnded != null)
                OnTrackEnded();
        }
    }

    /**
     * Look for notes ahead of the actual position in the track.
     * The maximun time to look ahead is defined by lookAheadTime.
     * If a note is detected then the NoteAhead event is triggered.
     */
    private void LookAheadNotes() {
        if (aheadNoteIndex < notes.Length - 1) {
            bool isNear = Time + lookAheadTime >= notes[aheadNoteIndex + 1].time;
            if (isNear) {
                aheadNoteIndex++;

                NoteInTrack noteInTrack = notes[aheadNoteIndex];
                if (OnNoteAhead != null) OnNoteAhead(noteInTrack.note, this.lookAheadTime, noteInTrack.time);
            }
        }
    }

    private void SetUpAudioSources() {
        AudioSource[] audioSources = GetComponents<AudioSource>();

        bgAudio = audioSources[0];
        bgAudio.clip = bgMusic;
        bgAudio.priority = 0;
        bgAudio.minDistance = float.MaxValue;

        melodyAudio = audioSources[1];
        melodyAudio.clip = melody;
        melodyAudio.priority = 0;
        melodyAudio.minDistance = float.MaxValue;

        audioEffect = audioSources[2];
        audioEffect.clip = errorSound;
        audioEffect.priority = 0;
        audioEffect.minDistance = float.MaxValue;

    }

    private void SetUpEventsForMelody() {
        OnNoteWellPlayed -= MelodyNoteWellPlayed;
        OnNoteWellPlayed += MelodyNoteWellPlayed;
        
        OnNoteBadPlayed -= MelodyNoteBadPlayed;
        OnNoteBadPlayed += MelodyNoteBadPlayed;

        OnNoteNotPlayed -= MelodyNoteNotPlayed;
        OnNoteNotPlayed += MelodyNoteNotPlayed;
        
    }

    private void MelodyNoteWellPlayed(Note note, float trackTime, float playTime) {
        melodyAudio.volume = 1f;
    }

    private void MelodyNoteBadPlayed(Note note, float playTime) {
        melodyAudio.volume = 0f;
        audioEffect.Stop();
        audioEffect.Play();
    }

    private void MelodyNoteNotPlayed(Note note) {
        melodyAudio.volume = 0f;
    }

    /**
     * Check if some note has passed without being played, in that case NoteNotPlayed
     * is triggered.
     * Only notes beyond timeTolerancy are considered bad played.
     */
    private void CheckBackNotes() {
        if (backNoteIndex < notes.Length - 1) {
            bool passed = Time - timeTolerancy >= notes[backNoteIndex + 1].time;
            if (passed) {
                backNoteIndex++;
                NoteInTrack noteInTrack = notes[backNoteIndex];

                if (!noteInTrack.played) {
                    if (OnNoteNotPlayed != null) OnNoteNotPlayed(noteInTrack.note);
                }
            }
        }
    }

    private void AutoPlay() {
        NoteInTrack nit = notes[backNoteIndex + 1];
        if (Time >= nit.time) {
            backNoteIndex++;
            this.PlayNote(nit.note);
        }
    }

    /**
     * Check if some note has passed without being played, in that case NoteNotPlayed
     * is triggered.
     * Only notes beyond timeTolerancy are considered bad played.
     */
    private NoteInTrack[] QueryNotesInTrack(float beginTime, float endTime, Note note = null) {
        NoteInTrack[] query = Array.FindAll(
            notes,
            (NoteInTrack nit) =>
                nit.time >= beginTime && nit.time <= endTime
                && (note == null || nit.note == note)
        );

        return query;
    }

    /**
     * Represents a note in the track.
     * Every note start as not played, if the player plays it
     * then is marked as played.
     **/
    protected class NoteInTrack {
        public Note note;
        public float time;
        public bool played;
    }
}
