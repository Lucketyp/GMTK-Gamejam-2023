using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptiveAudio : MonoBehaviour
{

    [SerializeField] private AudioSource simpleDrums;
    [SerializeField] private AudioSource heartbeatDrums;


    void Update()
    {
        DrumLogic();
    }

    void DrumLogic() {
        Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        simpleDrums.mute = movement == Vector2.zero ? true : false;
        bool sprinting = Input.GetKey(KeyCode.LeftShift);
        heartbeatDrums.mute = sprinting ? false : true;
    }
}

