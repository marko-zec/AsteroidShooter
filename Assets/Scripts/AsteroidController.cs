using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    // Asteroid prefab referneces
    // Small
    public GameObject Asteroid_1_1;
    public GameObject Asteroid_1_2;
    public GameObject Asteroid_1_3;
    // medium
    public GameObject Asteroid_2_1;
    public GameObject Asteroid_2_2;
    public GameObject Asteroid_2_3;

    GameObject[] small_asteroids;
    GameObject[] medium_asteroids;

    // Wrapping coordinates
    public float c_top;
    public float c_bottom;
    public float c_left;
    public float c_right;

    public float speed;
    public Rigidbody2D rb;
    ScreenWrapping sw;
    private int points; // Scoring: large - 50, medium - 100, small - 200

    //GameObject spaceship;
    LevelManager lm;

    // Start is called before the first frame update
    void Start()
    {
        // Iinitialize asteroid arrays for splitting/spawning
        small_asteroids = new GameObject[3] { Asteroid_1_1, Asteroid_1_2, Asteroid_1_3 };
        medium_asteroids = new GameObject[3] { Asteroid_2_1, Asteroid_2_2, Asteroid_2_3 };

        sw = GetComponent<ScreenWrapping>();
        points = 0;

        // https://forum.unity.com/threads/creating-a-random-direction-vector.220427/
        Vector2 direction = Random.insideUnitCircle;

        // Add force to asteroid
        rb.AddForce(direction * speed);
        //rb.AddForce(new Vector2(0, 1) * speed);

        // find spaceship
        //spaceship = GameObject.FindGameObjectWithTag("Player");

        // reference level manager script
        lm = GameObject.Find("Level Manager").GetComponent<LevelManager>();

        /*  I LIKE IT BETTER WITHOUT ASTEROID COLLISION SFX IN THE END
        // disable collider at start to avoid sounds of a lot of asteroid crashing against one another as zhey spawn
        GetComponent<PolygonCollider2D>().enabled = false;
        // enable it after 0.2 seconds
        Invoke("EnableCollider", 0.2f);
        */
    }

    // Update is called once per frame
    void Update()
    {
        // Check for warpping
        if (sw.CheckWrapping(transform.position, c_top, c_bottom, c_left, c_right))
            transform.position = sw._new_position;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag is "Projectile")
        {
            // play sound effect
            SoundManager.Instance.PlaySound("asteroid-break");

            // Destroy projectile when it hits an asteroid
            Destroy(collision.gameObject);

            // Split asteroid on imapct with projectile according to its type
            splitOrDestroyAsteroid();
        }
    }

    /*
    private void EnableCollider()
    {
        GetComponent<PolygonCollider2D>().enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Asteroid"))
            SoundManager.Instance.PlaySound("asteroid-collision");
    }
    */

    void splitOrDestroyAsteroid()
    {
        // Load information about asteroid that is attached to script
        string asteroid_name = gameObject.name;
        char size = asteroid_name[9];
        char type = asteroid_name[11];

        switch (size)
        {
            case '3':
                // destroy it and spawn 2 medium asteroids of same type
                Destroy(gameObject);
                foreach (GameObject asteroid in medium_asteroids)
                {
                    if (asteroid.name[11] == type)
                    {
                        Instantiate(asteroid, new(transform.position.x + 0.7f, transform.position.y + 0.7f), transform.rotation);
                        Instantiate(asteroid, transform.position, transform.rotation);
                        break;
                    }
                        
                }
                //score points
                points += 50;
                SendScore();
                //update number of asteroids in level manager
                lm.UpdateAsteroidCount(1);
                break;

            case '2':
                // Destroy it and spawn 2 small ones of same type
                Destroy(gameObject);
                foreach (GameObject asteroid in small_asteroids)
                {
                    if (asteroid.name[11] == type)
                    {
                        Instantiate(asteroid, new(transform.position.x + 0.4f, transform.position.y + 0.4f), transform.rotation);
                        Instantiate(asteroid, transform.position, transform.rotation);
                        break;
                    }
                }
                points += 100;
                SendScore();
                lm.UpdateAsteroidCount(1);
                break;

            default:
                // destroy small asteroid and score points
                Destroy(gameObject);
                points += 200;
                SendScore();
                lm.UpdateAsteroidCount(-1);
                break;
        }

    }

    void SendScore()
    {
        // Tell spaceship to update score
        lm.SendMessage("UpdateScore", points);
    }
}
