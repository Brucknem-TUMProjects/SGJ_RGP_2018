using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour {
    private void Update()
    {
        if (Input.GetButton("ControllerAButton") || Input.anyKey)
            SceneManager.LoadScene("Main");
    }
}
