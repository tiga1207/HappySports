using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

public class SongLoader : MonoBehaviour
{
    // 폴더 위치 : BeatSaber/Resources/Songs/
    public string rootPath = "Songs";
    public List<string> songFolders = new();
    public List<SongFolder> allSongs = new();

    void Awake()
    {
        LoadAllSongs();
    }

    public void LoadAllSongs()
    {
        allSongs.Clear();

        foreach (var folderName in songFolders)
        {
            string fullPath = $"{rootPath}/{folderName}";

            // info.json
            TextAsset infoAsset = Resources.Load<TextAsset>($"{fullPath}/info");
            if (infoAsset == null)
            {
                Debug.Log($"info.json이 해당 위치에 없습니다. {fullPath}");
                continue;
            }

            // 1) info 파싱
            SongInfo songInfo = JsonConvert.DeserializeObject<SongInfo>(infoAsset.text);

            // 2) 난이도만 따로 파싱
            var root = JsonConvert.DeserializeObject<Dictionary<string, object>>(infoAsset.text);
            var beatmapSetsJson = root["_difficultyBeatmapSets"].ToString();
            var beatmapSets = JsonConvert.DeserializeObject<List<DifficultyBeatmapSetWrapper>>(beatmapSetsJson);

            foreach (var set in beatmapSets)
            {
                songInfo.difficultyBeatmaps.AddRange(set._difficultyBeatmaps);
            }

            // 4) 커버
            Texture2D cover = Resources.Load<Texture2D>($"{fullPath}/cover");

            // 5) 리스트에 등록
            allSongs.Add(new SongFolder
            {
                folderName = folderName,
                info = songInfo,
                coverImage = cover
            });

        }
    }
}
