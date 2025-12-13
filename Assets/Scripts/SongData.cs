using UnityEngine;

[CreateAssetMenu(menuName = "Music/Song Data")]
public class SongData : ScriptableObject
{
    public string songName;
    public Sprite artwork;
    public AudioClip demoClip;   // 20 sec preview
    public AudioClip fullClip;   // gameplay song
}
