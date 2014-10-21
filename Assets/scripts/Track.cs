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

    // The musing/song to play in the track.
    public AudioClip music;
  
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
        get{ return audioSource.time; }
        set{ audioSource.time = value; }
    }
    public bool isPlaying { get{ return audioSource.isPlaying; } }

    public void Play() {
        //        Playing = true;
        audioSource.Play();
    }

    public void Pause() {
        //        Playing = false;
        audioSource.Pause();
    }

    public void Stop() {
        //        Playing = false;
        audioSource.Stop();
    }

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
   
    private AudioSource audioSource;
    private int aheadNoteIndex;
    private int backNoteIndex;

    void Start () {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = music;
        audioSource.priority = 0;
        audioSource.minDistance = float.MaxValue;

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
            new NoteInTrack{note = Note.DO, time = 18f}
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
