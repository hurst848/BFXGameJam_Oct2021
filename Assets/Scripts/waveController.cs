using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class waveController : MonoBehaviour
{
    // holds all spawners
    public List<GameObject> spawners;

    public List<GameObject> enemies;

   
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI waveText;



    public GameObject playerRef;

    public int enemyCount;
    public int wave;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    public float enemyHealth;


    public GameObject currWeaponFrame;
    public List<GameObject> currWeaponImage;

    public Slider healthBar;
   // public Image fill;
   // public Gradient gradient;

    bool alive = true;
    bool started = false;
    // Start is called before the first frame update
    void Start()
    {
        wave = 1;
        enemyHealth = 1;
        enemies = new List<GameObject>();
        startWaves();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (playerRef.GetComponent<playerController>().health <= 0)
        {
            alive = false;
        }
        scoreText.text = "Score " + playerRef.GetComponent<playerController>().score;
        waveText.text = "Wave " + wave;


        switch (playerRef.GetComponent<playerController>().lastMode)
        {
            case 0:
                currWeaponFrame.GetComponent<Image>().sprite = currWeaponImage[0].GetComponent<Image>().sprite;
                break;
            case 1:
                currWeaponFrame.GetComponent<Image>().sprite = currWeaponImage[1].GetComponent<Image>().sprite;
                break;
            case 2:
                currWeaponFrame.GetComponent<Image>().sprite = currWeaponImage[2].GetComponent<Image>().sprite;
                break;
            case 3:
                currWeaponFrame.GetComponent<Image>().sprite = currWeaponImage[3].GetComponent<Image>().sprite;
                break;
        }

        healthBar.value = playerRef.GetComponent<playerController>().health;
        //fill.color = gradient.Evaluate(slider.normalizedValue);

    }

    public void startWaves()
    {
        StartCoroutine(spawnWaves());
    }

    IEnumerator spawnWaves()
    {

        yield return new WaitForSeconds(startWait);
        while(alive)
        {
            Debug.Log("spawn wait: " + spawnWait + " wave: " + waveWait + " enemy: " + enemyCount);
            for (int i = 0; i < enemyCount; i ++)
            {
                int spawnerLoc = Random.Range(0, spawners.Count);
                GameObject new_enemy = spawners[spawnerLoc].GetComponent<enemySpawner>().spawnEnemy();
                new_enemy.GetComponent<enemyController>().playerRef = playerRef;
                new_enemy.GetComponent<enemyController>().enemyHealth = enemyHealth;
                enemies.Add(new_enemy);
                
                yield return new WaitForSeconds(spawnWait);
                
            }

            enemyCount ++;
            wave++;

            if (wave%3 == 0 && spawnWait >= 0.3f)
            {
                spawnWait -= 0.1f;
                if (waveWait >= 1)
                {
                    waveWait -= 1;
                }
            }
            if (wave % 2 == 0)
            {
                enemyHealth++;
            }

            yield return new WaitForSeconds(waveWait);
            playerRef.GetComponent<playerController>().skillPoints++;
        }
        
    }
}
