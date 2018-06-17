using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndsceneController : MonoBehaviour {
    public void EndGame()
    {
        SceneManager.LoadScene("Menu");
    }
}
