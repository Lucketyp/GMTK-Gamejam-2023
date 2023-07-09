using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HunterManager : MonoBehaviour
{

    [SerializeField] GameObject hunterPrefab;
    [SerializeField] GameObject currentPlayer;
    int numberOfHunters;
    int deadHunters;
    [SerializeField] Vector2 spawnSize;
    
    void Start()
    {
        numberOfHunters = 1;
        deadHunters = 0;

        if(PlayerPrefs.HasKey("wins")) {
            numberOfHunters = 1 + PlayerPrefs.GetInt("wins");
        }

        for(int i = 0; i < numberOfHunters; i++){
            GameObject hunterObject = Instantiate(
                hunterPrefab, 
                transform.position + new Vector3(Random.Range(-spawnSize.x, spawnSize.x), 0, Random.Range(-spawnSize.y, spawnSize.y)) / 2,
                Quaternion.identity
            );

            hunterObject.GetComponent<BulletShooter>().target = currentPlayer;
            hunterObject.GetComponent<BulletShooter>().hunterManager = gameObject;
        }
    }

    public void ReportDeath() {
        deadHunters++;
        if(deadHunters >= numberOfHunters){
            if(PlayerPrefs.HasKey("wins")) {
                int lastWins = PlayerPrefs.GetInt("wins");
                PlayerPrefs.SetInt("wins", lastWins + 1);
                
            } else {
                PlayerPrefs.SetInt("wins", 1);
            }
            SceneManager.LoadScene("Win Screen");
        }
    }
}
