using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{

    [SerializeField] GameObject[] levels;

    void Start()
    {
        Instantiate(levels[Random.Range(0, levels.Length)], transform);
    }
}
