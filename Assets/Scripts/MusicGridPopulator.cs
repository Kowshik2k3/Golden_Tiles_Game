using UnityEngine;

public class MusicGridPopulator : MonoBehaviour
{
    public SongCardUI songCardPrefab;
    public SongData[] songs;

    void Start()
    {
        Populate();
    }

    void Populate()
    {
        for (int i = 0; i < songs.Length; i++)
        {
            SongCardUI card = Instantiate(songCardPrefab, transform);
            card.Setup(songs[i], i);
        }
    }
}
