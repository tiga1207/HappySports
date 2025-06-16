using System.Collections.Generic;
using Newtonsoft.Json;

[System.Serializable]
public class SongInfo
{
    [JsonProperty("_songName")]
    public string songName;
    [JsonProperty("_songFilename")]
    public string songFilename;

    [JsonProperty("_songAuthorName")]
    public string songAuthor;

    [JsonProperty("_beatsPerMinute")]
    public float bpm;
    [JsonIgnore]
    public List<DifficultyBeatmap> difficultyBeatmaps = new();

}


[System.Serializable]
public class DifficultyBeatmapSetWrapper
{
    [JsonProperty("_difficultyBeatmaps")]
    public List<DifficultyBeatmap> _difficultyBeatmaps;
}

[System.Serializable]
public class DifficultyBeatmap
{
    [JsonProperty("_difficulty")]
    public string difficulty;

    [JsonProperty("_beatmapFilename")]
    public string beatmapFilename;
}
