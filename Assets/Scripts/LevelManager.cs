using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    UIManager uim;
    ShipController sc;

    public int level;
    public int asteroid_count;
    private int score;

    // large asteroids
    public GameObject Asteroid_3_1;
    public GameObject Asteroid_3_2;
    public GameObject Asteroid_3_3;

    // Start is called before the first frame update
    void Start()
    {
        // Because im using this script for menu scene aswell
        try
        {
            uim = GameObject.Find("User Interface").GetComponent<UIManager>();
            sc = GameObject.FindGameObjectWithTag("Player").GetComponent<ShipController>();
        
            level = 1;
            score = 0;
            asteroid_count = 3; // first level: 3 large asteroids, after every level asteroid count +3
            uim.UpdateLevelText(level);
            ChangeMusic();
        }
        catch (System.Exception) { }
    }

    // Update score
    void UpdateScore(int new_score)
    {
        score += new_score;
        uim.UpdateScoreText(score);
    }

    public void UpdateAsteroidCount(int net_change)
    {
        asteroid_count += net_change;
        if (asteroid_count < 0)
            asteroid_count = 0;

        if (asteroid_count == 0)
        {
            // Double check asteroid count (sometimes the count is wrong)
            if (NoAsteroidsLeft())
            {
                CancelInvoke("CallUpdateAsteroidCountIndirectly");
                Invoke("ChangeLevel", 3.0f);
            }
            else
                Invoke("CallUpdateAsteroidCountIndirectly", 0.5f);  
        }
    }

     // Necessery because Invoke doesnt work with funtions with parameters (even optional params) and
     // i dont want to mess with coroutines
    void CallUpdateAsteroidCountIndirectly()
    {
        UpdateAsteroidCount(0);
    }

    bool NoAsteroidsLeft()
    {
        // http://answers.unity.com/answers/1279524/view.html
        List<GameObject> all_objects = new List<GameObject>();
        Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects(all_objects);

        int real_asteroid_count = 0;
        foreach(GameObject go in all_objects)
        {
            if (go.CompareTag("Asteroid"))
                real_asteroid_count++;
        }
        Debug.Log(string.Format("[level {0}] real asteroid count {1}", level, real_asteroid_count));
        if (real_asteroid_count == 0)
            return true;
        else
            return false;
    }

    public void ChangeLevel()
    {
        level++;
        uim.UpdateLevelText(level);
        
        // Spawn new asteroids
        for (int i = 0; i < level; i++)
        {
            Instantiate(Asteroid_3_1, new Vector2(Random.Range(-10.50f, 10.50f), 6.30f), Quaternion.identity);
            Instantiate(Asteroid_3_2, new Vector2(Random.Range(-10.50f, 10.50f), 6.30f), Quaternion.identity);
            Instantiate(Asteroid_3_3, new Vector2(Random.Range(-10.50f, 10.50f), 6.30f), Quaternion.identity);
            asteroid_count += 3;
        }
        // check for ship upgrade
        if (level == 5)
            sc.UpgradeShip(level);
        if (level == 10)
            sc.UpgradeShip(level);

        // set damage to the ship to 0 and update damage ui text
        sc.damage = 0.0f;
        uim.UpdateDamageText(sc.damage);

        // check if music needs to be changed
        ChangeMusic();
    }

    public void UpdateSpaceshipReference()
    {
        //sc = GameObject.FindObjectWithTag("Player").GetComponent<ShipController>();
        sc = GameObject.Find("Spaceship 2(Clone)").GetComponent<ShipController>();
    }

    public void RestartGame()
    {
        SoundManager.Instance.PlaySound("button-press");
        SceneManager.LoadScene("MainScene");
    }

    public void ReturnToMenu()
    {
        SoundManager.Instance.PlaySound("button-press");
        SceneManager.LoadScene("MenuScene");
    }

    public void StartGame()
    {
        SoundManager.Instance.PlaySound("button-press");
        SceneManager.LoadScene("MainScene");
    }

    public void QuitGame()
    {
        SoundManager.Instance.PlaySound("button-press");
        Invoke("Quit", 1.5f);
    }

    void Quit()
    {
        Application.Quit();
    }

    void ChangeMusic()
    {
        switch (level)
        {
            case 1:
                SoundManager.Instance.ChangeBGM("level1_2");
                break;
            case 3:
                SoundManager.Instance.ChangeBGM("level3_4");
                break;
            case 5:
                SoundManager.Instance.ChangeBGM("level5_6");
                break;
            case 7:
                SoundManager.Instance.ChangeBGM("level7_8");
                break;
            case 9:
                SoundManager.Instance.ChangeBGM("level9_10");
                break;
            case 11:
                SoundManager.Instance.ChangeBGM("level11_inf");
                break;
            default:
                break;
        }
    }
}
