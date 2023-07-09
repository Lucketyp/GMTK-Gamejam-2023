using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScript : MonoBehaviour
{
    void Update()
    {
        // Press any key to start the game
        if(Input.anyKeyDown){
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
        }
    }
}
