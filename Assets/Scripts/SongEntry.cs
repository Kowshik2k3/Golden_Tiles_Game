using UnityEngine;
using UnityEngine.UI;
using TMPro;   // IMPORTANT for TMP support

public class SongEntry : MonoBehaviour
{
    public TextMeshProUGUI songLabel;  // <-- TMP instead of Text
    private int index;
    private Button mainButton;

    void Awake()
    {
        mainButton = GetComponent<Button>();
    }

    public void Setup(int songIndex, string songName)
    {
        index = songIndex;

        if (songLabel != null)
            songLabel.text = songName;

        mainButton.onClick.RemoveAllListeners();
        mainButton.onClick.AddListener(OnSongSelected);
    }

    private void OnSongSelected()
    {
        MusicManager.Instance?.SelectMusic(index);
    }
}
