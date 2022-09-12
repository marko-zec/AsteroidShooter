using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text lives;
    public Text damage;
    public Text score;
    public Text level;
    public GameObject game_over_panel;
    public int starting_lives;

    // Start is called before the first frame update
    void Start()
    {
        // Reset text boxes
        UpdateLivesText(starting_lives);
        UpdateDamageText(0.0f);
        UpdateScoreText(0);  
    }

    public void UpdateLivesText(int lives_num)
    {
        lives.text = string.Format("Lives: {0}", lives_num);
    }

    public void UpdateDamageText(float damage_amount)
    {
        damage.text = string.Format("Damage: {0}%", (int)damage_amount);
    }

    public void UpdateScoreText(int score_amount)
    {
        score.text = string.Format("Score: {0}", score_amount);
    }

    public void UpdateLevelText(int level_num)
    {
        level.text = string.Format("Level: {0}", level_num);
    }
}
