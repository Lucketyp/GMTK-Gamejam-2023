using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScript : MonoBehaviour
{
    void OnEnable()
    {
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(enabled && Input.anyKeyDown){
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
