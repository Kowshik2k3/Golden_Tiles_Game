
using UnityEngine;

public class HomeManager : MonoBehaviour
{
    // This script will just handle UI actions in the Home/Menu scene
    // For example: buttons that call MusicManager.SelectMusic() or QuitGame()

    public void OnSelectSong(int index)
    {
        // Calls the MusicManager to select and play music, then load the game scene
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.SelectMusic(index);
        }
        else
        {
            Debug.LogError("MusicManager not found in the scene!");
        }
    }

    public void OnQuitButton()
    {
        if (MusicManager.Instance != null)
            MusicManager.Instance.QuitGame();
        else
            Application.Quit();
    }
}
