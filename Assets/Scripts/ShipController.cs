using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public int ship_type;
    public GameObject ship2;
    public GameObject ship3;
    public float thrust_factor;
    public float torque_factor;
    public Rigidbody2D rb;

    private float thrust;
    private float torque;

    // Wrapping coordinates
    public float top;
    public float bottom;
    public float left;
    public float right;

    ScreenWrapping sw;
    public GameObject projectile;
    public GameObject projectile2;
    public GameObject projectile3;
    public float projectile_force_factor;
    public Transform gun_point0;
    public Transform gun_point1;
    public Transform gun_point2;
    public Transform gun_point3;
    public Transform gun_point4;
    public Transform gun_point5;
    public Transform gun_point6;
    [HideInInspector] public float damage;
    private int lives;
    private float elapsed_time;
    public float time_score_delta;
    public int time_score_amount;
    public float damage_multiplier;
    UIManager uim;
    LevelManager lm;
    private bool new_life = false;
    private bool can_shoot = true;

    // Start is called before the first frame update
    void Start()
    {
        // lives and damage
        lives = 3;
        damage = 0;

        // reference wrapping ui and levem manager script
        sw = GetComponent<ScreenWrapping>();

        // referencing script attached to canvas https://forum.unity.com/threads/how-do-i-reference-a-canvas.336846/
        GameObject user_interface = GameObject.Find("User Interface");
        uim = user_interface.GetComponent<UIManager>();
        lm = GameObject.Find("Level Manager").GetComponent<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Read input
        thrust = Input.GetAxis("Vertical");
        torque = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") & can_shoot)
            Fire();

        // Wrap the ship when it exits screen
        if (sw.CheckWrapping(transform.position, top, bottom, left, right))
            transform.position = sw._new_position;

        // Add points to score after delta time https://soltveit.org/unity-call-function-every-0-1-seconds/
        elapsed_time += Time.deltaTime;
        if (elapsed_time >= time_score_delta)
        {
            elapsed_time = 0;
            if (lives > 0)
                lm.SendMessage("UpdateScore", time_score_amount);
        }

        if (new_life)
            Invoke("ContinuePlaying", 2.0f);
    }

    // Adding force dependant on input in FixedUpdate because it runs at the same rate as the physics engine
    void FixedUpdate()
    {
        rb.AddRelativeForce(Vector2.up * thrust * thrust_factor);
        rb.AddTorque(-torque * torque_factor);
    }

    void Fire()
    {
        if (ship_type == 1)
        {
            // Instantiate projectile prefab to correct position
            GameObject fired_projectile = Instantiate(projectile, gun_point0.position, gun_point0.rotation);
            fired_projectile.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * projectile_force_factor);
            SoundManager.Instance.PlaySound("laser1");

            // Destroy projectile after it exits screen
            Destroy(fired_projectile, 6.0f);
        }
        else if (ship_type == 2)
        {
            GameObject fired_projectile1 = Instantiate(projectile2, gun_point1.position, gun_point1.rotation);
            GameObject fired_projectile2 = Instantiate(projectile2, gun_point2.position, gun_point2.rotation);
            fired_projectile1.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * projectile_force_factor);
            fired_projectile2.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * projectile_force_factor);
            SoundManager.Instance.PlaySound("laser2");
            SoundManager.Instance.PlaySound("laser2");
            Destroy(fired_projectile1, 6.0f);
            Destroy(fired_projectile2, 6.0f);
        }
        else if (ship_type == 3)
        {
            GameObject fired_projectile1 = Instantiate(projectile3, gun_point3.position, gun_point3.rotation);
            GameObject fired_projectile2 = Instantiate(projectile3, gun_point4.position, gun_point4.rotation);
            GameObject fired_projectile3 = Instantiate(projectile3, gun_point5.position, gun_point5.rotation);
            GameObject fired_projectile4 = Instantiate(projectile3, gun_point6.position, gun_point6.rotation);
            fired_projectile1.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * projectile_force_factor);
            fired_projectile2.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * projectile_force_factor);
            fired_projectile3.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * projectile_force_factor);
            fired_projectile4.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * projectile_force_factor);
            for (int i = 0; i < 4; i++)
            {
                SoundManager.Instance.PlaySound("laser3");
            }
            Destroy(fired_projectile1, 6.0f); Destroy(fired_projectile3, 6.0f);
            Destroy(fired_projectile2, 6.0f); Destroy(fired_projectile4, 6.0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Get collision force and calculate damage to spaceship
        float collision_force = collision.relativeVelocity.magnitude;
        damage += collision_force * damage_multiplier;
        uim.UpdateDamageText(damage);
        //Debug.Log(collision_force);

        // Play impact sound effect depending on impact force
        float collision_force_rounded = Mathf.Round(collision_force * 10.0f) * 0.1f; // round float to 1 decimal point
        if (collision_force_rounded >= 3)
            SoundManager.Instance.PlaySound("big-impact");
        else if (collision_force_rounded >= 1.5 & collision_force_rounded <= 2.9)
            SoundManager.Instance.PlaySound("medium-impact");
        else if (collision_force_rounded <= 1.4)
            SoundManager.Instance.PlaySound("small-impact");

        // play damage warning sound effect
        int i_damage = (int)damage;
        if (i_damage >= 50 & i_damage <= 75)
            SoundManager.Instance.PlaySound("damage-warning");
        else if (i_damage >= 76 & i_damage <= 99)
            SoundManager.Instance.PlaySound("critical-warning");
                   
        if ((int)damage >= 100)
        {
            damage = 0;
            uim.UpdateDamageText(100.0f);
            lives--;
            uim.UpdateLivesText(lives);

            // Make spaceship invisible and not able to shoot
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<PolygonCollider2D>().enabled = false;
            can_shoot = false;

            // play exlosion sound effect
            SoundManager.Instance.PlaySound("explosion");

            if (lives == 0)
            {
                // Game over
                uim.UpdateDamageText(100.0f);
                // canvel invoke
                CancelInvoke("ContinuePlaying");
                // activate game over panel
                uim.game_over_panel.SetActive(true);
                // play game over bgm
                SoundManager.Instance.ChangeBGM("game_over");
            }
            else
                new_life = true;
                
        }
    }
  
    void ContinuePlaying()
    {
        new_life = false;
        GetComponent<SpriteRenderer>().enabled = true;
        // apply tranpararency after spawn that indicates ship cant be damaged
        GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 0.2f);
        uim.UpdateDamageText(damage);
        // remove momentum and reposition to center
        rb.velocity = Vector2.zero;
        transform.position = Vector2.zero;
        can_shoot = true;
        Invoke("EndSpawnProtection", 2.0f);
    }

    void EndSpawnProtection()
    {
        // enable collider and disable tranparency
        GetComponent<PolygonCollider2D>().enabled = true;
        GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 1.0f);
    }

    public void UpgradeShip(int level)
    {
        if (level == 5)
        {
            Destroy(gameObject);
            Instantiate(ship2, transform.position, transform.rotation);
            // tell level manager to refresh its reference to the new ship object
            lm.UpdateSpaceshipReference();
        }
        if (level == 10)
        {
            Destroy(gameObject);
            Instantiate(ship3, transform.position, transform.rotation);
        }
           
        damage = 0.0f;
        lives = 3;
        uim.UpdateDamageText(damage);
        uim.UpdateLivesText(lives);
        SoundManager.Instance.PlaySound("ship-upgraded");
    }
}
