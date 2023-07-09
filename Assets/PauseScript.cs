using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{

    bool paused = false;
    [SerializeField] GameObject pauseMenu;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Space)){
            paused = !paused;
            Time.timeScale = paused ? 0 : 1;
            pauseMenu.SetActive(paused);
            //Set all audio sources to paused
            if (paused) {
                foreach(AudioSource audioSource in FindObjectsOfType<AudioSource>()){
                    audioSource.Pause();
                }
            } else {
                foreach(AudioSource audioSource in FindObjectsOfType<AudioSource>()){
                    audioSource.UnPause();
                }
            }
        }
    }
}
