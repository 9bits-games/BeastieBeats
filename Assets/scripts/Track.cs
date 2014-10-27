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
    public bool isPlaying { get{ return bgAudio.isPlaying; } }

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

    void Start () {
        SetUpAudioSources();

        SetUpEventsForMelody();

        aheadNoteIndex = -1;
        backNoteIndex = -1;

        //Here we declare the notes in the track.
        notes = new NoteInTrack[]{
            new NoteInTrack{note = Note.RE, time = 4.7f},
            new NoteInTrack{note = Note.RE, time = 7.8f},
            new NoteInTrack{note = Note.RE, time = 11.5f},
            new NoteInTrack{note = Note.DO, time = 15f},
            new NoteInTrack{note = Note.MI, time = 16f},
            new NoteInTrack{note = Note.MI, time = 17f},
            new NoteInTrack{note = Note.LA, time = 17.5f},
            new NoteInTrack{note = Note.DO, time = 18f},

            new NoteInTrack{note = Note.DO, time = 19f},
            new NoteInTrack{note = Note.RE, time = 20f},
            new NoteInTrack{note = Note.MI, time = 21f},
            new NoteInTrack{note = Note.SOL, time = 22f},
            new NoteInTrack{note = Note.LA, time = 23f},
            new NoteInTrack{note = Note.DO, time = 24f},
            new NoteInTrack{note = Note.RE, time = 25f},
            new NoteInTrack{note = Note.MI, time = 26f},
            new NoteInTrack{note = Note.SOL, time = 27f},
            new NoteInTrack{note = Note.LA, time = 28f},
            new NoteInTrack{note = Note.LA, time = 29f},

            new NoteInTrack{note = Note.DO, time = 30f},
            new NoteInTrack{note = Note.RE, time = 31f},
            new NoteInTrack{note = Note.MI, time = 32f},
            new NoteInTrack{note = Note.SOL, time = 33f},
            new NoteInTrack{note = Note.LA, time = 34f},
            new NoteInTrack{note = Note.DO, time = 35f},
            new NoteInTrack{note = Note.RE, time = 36f},
            new NoteInTrack{note = Note.MI, time = 37f},
            new NoteInTrack{note = Note.SOL, time = 38f},
            new NoteInTrack{note = Note.LA, time = 39f},

            new NoteInTrack{note = Note.DO, time = 40f},
            new NoteInTrack{note = Note.RE, time = 41f},
            new NoteInTrack{note = Note.MI, time = 42f},
            new NoteInTrack{note = Note.SOL, time = 43f},
            new NoteInTrack{note = Note.LA, time = 44f},
            new NoteInTrack{note = Note.DO, time = 45f},
            new NoteInTrack{note = Note.RE, time = 46f},
            new NoteInTrack{note = Note.MI, time = 47f},
            new NoteInTrack{note = Note.SOL, time = 48f},
            new NoteInTrack{note = Note.LA, time = 49f},

            new NoteInTrack{note = Note.DO, time = 50f},
            new NoteInTrack{note = Note.RE, time = 51f},
            new NoteInTrack{note = Note.MI, time = 52f},
            new NoteInTrack{note = Note.SOL, time = 53f},
            new NoteInTrack{note = Note.LA, time = 54f},
            new NoteInTrack{note = Note.DO, time = 55f},
            new NoteInTrack{note = Note.RE, time = 56f},
            new NoteInTrack{note = Note.MI, time = 57f},
            new NoteInTrack{note = Note.SOL, time = 58f},
            new NoteInTrack{note = Note.LA, time = 59f},

            new NoteInTrack{note = Note.DO, time = 60f},
            new NoteInTrack{note = Note.RE, time = 61f},
            new NoteInTrack{note = Note.MI, time = 62f},
            new NoteInTrack{note = Note.SOL, time = 63f},
            new NoteInTrack{note = Note.LA, time = 64f},
            new NoteInTrack{note = Note.DO, time = 65f},
            new NoteInTrack{note = Note.RE, time = 66f},
            new NoteInTrack{note = Note.MI, time = 67f},
            new NoteInTrack{note = Note.SOL, time = 68f},
            new NoteInTrack{note = Note.LA, time = 69f},

            new NoteInTrack{note = Note.DO, time = 70f},
            new NoteInTrack{note = Note.RE, time = 71f},
            new NoteInTrack{note = Note.MI, time = 72f},
            new NoteInTrack{note = Note.SOL, time = 73f},
            new NoteInTrack{note = Note.LA, time = 74f},
            new NoteInTrack{note = Note.DO, time = 75f},
            new NoteInTrack{note = Note.RE, time = 76f},
            new NoteInTrack{note = Note.MI, time = 77f},
            new NoteInTrack{note = Note.SOL, time = 78f},
            new NoteInTrack{note = Note.LA, time = 79f}
        };

    }

    // Update is called once per frame
    void Update () {
        if (isPlaying) {
            LookAheadNotes();
            CheckBackNotes();
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
