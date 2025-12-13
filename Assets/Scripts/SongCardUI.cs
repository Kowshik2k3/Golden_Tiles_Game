using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SongCardUI : MonoBehaviour
{
    [Header("UI References")]
    public Image artworkImage;
    public TextMeshProUGUI songNameText;

    [Header("Demo UI")]
    public Button demoButton;
    public Image playIcon;
    public Image discIcon;
    public float discRotationSpeed = 180f;

    [Header("Play Button")]
    public Button playButton;

    private SongData songData;
    private int songIndex;
    private bool demoPlaying = false;

    void Update()
    {
        if (demoPlaying && discIcon != null)
        {
            discIcon.transform.Rotate(0f, 0f, -discRotationSpeed * Time.deltaTime);
        }
    }

    public void Setup(SongData data, int index)
    {
        songData = data;
        songIndex = index;

        artworkImage.sprite = data.artwork;
        songNameText.text = data.songName;

        playIcon.gameObject.SetActive(true);
        discIcon.gameObject.SetActive(false);

        demoButton.onClick.RemoveAllListeners();
        demoButton.onClick.AddListener(ToggleDemo);

        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(PlayFullSong);
    }

    void ToggleDemo()
    {
        if (demoPlaying)
            StopDemo();
        else
            PlayDemo();
    }

    void PlayDemo()
    {
        demoPlaying = true;
        playIcon.gameObject.SetActive(false);
        discIcon.gameObject.SetActive(true);

        MusicManager.Instance.PlayDemo(songData.demoClip, this);
    }

    void StopDemo()
    {
        demoPlaying = false;
        playIcon.gameObject.SetActive(true);
        discIcon.gameObject.SetActive(false);

        MusicManager.Instance.StopDemo();
    }

    void PlayFullSong()
    {
        if (demoPlaying)
            StopDemo();

        MusicManager.Instance.SelectMusic(songIndex);
    }

    public void ForceStopDemo()
    {
        demoPlaying = false;
        playIcon.gameObject.SetActive(true);
        discIcon.gameObject.SetActive(false);
    }
}
