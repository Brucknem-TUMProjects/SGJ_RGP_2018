using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    MusicManager musicManager;
    public Slider sfxSlider;
    public Slider musicSlider;

	void Start () {
        musicManager = FindObjectOfType<MusicManager>();
        sfxSlider.value = GameData.Instance.sfxVolume;
        musicSlider.value = GameData.Instance.musicVolume;
	}

    public void StartLevel(int sceneID)
    {
        GameData.Instance.SaveSettings();
        SceneManager.LoadScene(sceneID);
    }

    // End the game / return to the desktop
    public void EndGame()
    {

        GameData.Instance.SaveSettings();

        //If we are running in a standalone build of the game
#if UNITY_STANDALONE
        //Quit the application
        Application.Quit();
#endif

        //If we are running in the editor
#if UNITY_EDITOR
        //Stop playing the scene
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void SetSFXLevel(float lvl)
    {
        musicManager.SetSfxLevel(lvl);
    }

    public void SetMusicLevel(float lvl)
    {
        musicManager.SetMusicLevel(lvl);
    }
}
