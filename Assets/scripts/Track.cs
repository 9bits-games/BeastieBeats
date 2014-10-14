using UnityEngine;
using System;
using System.Collections;

public class Track : MonoBehaviour9Bits
{
    public delegate void NoteAhead(Note note, float timeAhead, float trackTime);
    public event NoteAhead OnNoteAhead;

    public delegate void NoteWellPlayed(Note note, float trackTime, float playTime);
    public event NoteWellPlayed OnNoteWellPlayed;

    public delegate void NoteBadPlayed(Note note, float playTime);
    public event NoteBadPlayed OnNoteBadPlayed;

    public delegate void NoteNotPlayed(Note note);
    public event NoteNotPlayed OnNoteNotPlayed;

    public AudioClip music;
  
    /* Seconds to look ahead for notes in the track.
     * If a Note is in Time + lookAheadTime the event NoteAhead is fired.
     */
    public float lookAheadTime = 1f;
    public float timeTolerancy = 0.3f;

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

    protected NoteInTrack[] notes;
   
    private AudioSource audioSource;
//    private int currentNoteIndex;
    private int aheadNoteIndex;
    private int backNoteIndex;

    void Start () {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = music;
        audioSource.priority = 0;
        audioSource.minDistance = float.MaxValue;

//        currentNoteIndex = -1;
        aheadNoteIndex = -1;
        backNoteIndex = -1;

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

//        for (int i = 0; i < notes.Length; i++) notes[i].played = false;
    }

    // Update is called once per frame
    void Update () {
        if (isPlaying) {
//            if (currentNoteIndex < notes.Length - 1) {
//                //Current Note
//                if (Time >= notes[currentNoteIndex + 1].time) {
//                    currentNoteIndex++;
////                    Debug.Log(String.Format("Current Note: {0} at {1}", notes[currentNoteIndex].note.Name, Time));
//                }
//            }

            LookAheadNotes();
            CheckBackNotes();
        }
    }

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

    private void CheckBackNotes() {
        if (backNoteIndex < notes.Length - 1) {
            bool passed = Time - lookAheadTime >= notes[backNoteIndex + 1].time;
            if (passed) {
                backNoteIndex++;
                NoteInTrack noteInTrack = notes[backNoteIndex];

                if (!noteInTrack.played) {
                    if (OnNoteNotPlayed != null) OnNoteNotPlayed(noteInTrack.note);
                }
            }
        }
    }

    private NoteInTrack[] QueryNotesInTrack(float beginTime, float endTime, Note note = null) {
        NoteInTrack[] query = Array.FindAll(
            notes,
            (NoteInTrack nit) =>
                nit.time >= beginTime && nit.time <= endTime
                && (note == null || nit.note == note)
        );

        return query;
    }

//    protected NoteInTrack? currentNoteInTrack() {
//        if (currentNoteIndex == -1) {
//            return null;
//        } else {
//            return notes[currentNoteIndex];
//        }
//    }

    protected class NoteInTrack {
        public Note note;
        public float time;
        public bool played;
    }
}
