using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoteData
{
    public float _time;
    public int _lineIndex;
    public int _lineLayer;
    public int _type;
    public int _cutDirection;
}


[System.Serializable]
public class BeatMapData
{
    public float _beatsPerMinute;
    public List<NoteData> _notes;
}


[System.Serializable]
public class SongFolder
{
    public string folderName;
    public SongInfo info;
    public Texture2D coverImage;
}
