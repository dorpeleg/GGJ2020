using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "TVSceneList", menuName = "ScriptableObjects/TV Scenes", order = 1)]
public class TVScenes : ScriptableObject
{
    public List<VideoAudio> Scenes;
}

[Serializable]
public class VideoAudio
{
    public VideoClip VideoFile;
    public string AudioEvent;
}