using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class enemyController : MonoBehaviour
{
    public float accuracy;
    public float bulletSpeed;
    public float moveSpeed;
    public float rateOfFire;
    public float reloadSpeed;
    public float clipSize;
    public float range;

    public GameObject playerRef;
    public GameObject bulletPrefab;
    public Animator animator;

    public GameObject pathTarget;

    private bool canFire = true;
    private float currentClip;


    private Vector3 lastPos;

    public List<AudioClip> clips;
    private AudioSource source;

    public float enemyHealth = 1;

    private Vector3 targetOffset;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        currentClip = clipSize;
        lastPos = transform.position;

        float randomDist = Random.Range(0.5f, 0.9f);
        float randomRot = Random.Range(0, 360);
        targetOffset = new Vector3((
            (range * randomDist) * Mathf.Cos(Mathf.Deg2Rad * randomRot)) - ((range * randomDist) * Mathf.Sin(Mathf.Deg2Rad * randomRot)), (
            (range * randomDist) * Mathf.Sin(Mathf.Deg2Rad * randomRot)) + ((range * randomDist) * Mathf.Cos(Mathf.Deg2Rad * randomRot)));
    }

    bool isAlive = true;
    // Update is called once per frame
    void Update()
    {

        if (isAlive)
        {
            // set new target for the pathFinder
            pathTarget.transform.position = playerRef.transform.position + targetOffset;

            // Animate enemey based on movement
            Vector3 posDiff = transform.position - lastPos;

            if (posDiff.x > 0 && posDiff.y > 0) // up right
            {
                animator.SetBool("goingRight", true);
                animator.SetBool("goingUp", true);
                animator.SetBool("goingLeft", false);
                animator.SetBool("goingDown", false);
            }
            else if (posDiff.x > 0 && posDiff.y < 0) // down right
            {
                animator.SetBool("goingRight", true);
                animator.SetBool("goingDown", true);
                animator.SetBool("goingLeft", false);
                animator.SetBool("goingUp", false);
            }
            else if (posDiff.x < 0 && posDiff.y < 0) // down left
            {
                animator.SetBool("goingLeft", true);
                animator.SetBool("goingDown", true);
                animator.SetBool("goingRight", false);
                animator.SetBool("goingUp", false);
            }
            else if (posDiff.x < 0 && posDiff.y > 0) // up left
            {
                animator.SetBool("goingLeft", true);
                animator.SetBool("goingUp", true);
                animator.SetBool("goingRight", false);
                animator.SetBool("goingDown", false);
            }
            else if (posDiff.x > 0) // right
            {
                animator.SetBool("goingRight", true);
                animator.SetBool("goingDown", false);
                animator.SetBool("goingLeft", false);
                animator.SetBool("goingUp", false);
            }
            else if (posDiff.x < 0) // left
            {
                animator.SetBool("goingLeft", true);
                animator.SetBool("goingDown", false);
                animator.SetBool("goingRight", false);
                animator.SetBool("goingUp", false);
            }
            else if (posDiff.y > 0) // up
            {
                animator.SetBool("goingUp", true);
                animator.SetBool("goingDown", false);
                animator.SetBool("goingLeft", false);
                animator.SetBool("goingRight", false);
            }
            else if (posDiff.y < 0) // down
            {
                animator.SetBool("goingDown", true);
                animator.SetBool("goingRight", false);
                animator.SetBool("goingLeft", false);
                animator.SetBool("goingUp", false);
            }

            if (Vector2.Distance(playerRef.transform.position, transform.position) <= range)
            {
                // disable pathfinding
                if (canFire)
                {
                    animator.SetBool("isShooting", true);

                    canFire = false;

                    float aimOffest = Random.Range(-accuracy, accuracy);
                    GameObject newBullet = Instantiate(bulletPrefab, transform.position, transform.rotation, transform.parent);
                    newBullet.GetComponent<bulletController>().setupBullet(transform.position, new Vector3((
                            playerRef.transform.position.x * Mathf.Cos(Mathf.Deg2Rad * aimOffest)) - (playerRef.transform.position.y * Mathf.Sin(Mathf.Deg2Rad * aimOffest)), (
                            playerRef.transform.position.x * Mathf.Sin(Mathf.Deg2Rad * aimOffest)) + (playerRef.transform.position.y * Mathf.Cos(Mathf.Deg2Rad * aimOffest))));
                    newBullet.GetComponent<bulletController>().bulletSpeed = bulletSpeed;
                    newBullet.layer = 11;
                    newBullet.GetComponent<bulletController>().owner = true;
                    newBullet.GetComponent<bulletController>().startBullet();

                    source.clip = clips[0];
                    source.Play();

                    if (currentClip <= 0) { StartCoroutine(reloadSpeedDelayFunction()); }
                    else { StartCoroutine(rateOfFireDelayFunction()); currentClip--; }

                }
                else
                {
                    animator.SetBool("isShooting", false);
                }

            }

            lastPos = transform.position;

            if (enemyHealth <= 0)
            {
                source.clip = clips[1];
                source.Play();
                Debug.Log("Killed Enemy");
                animator.SetBool("isShooting", false);
                animator.SetBool("goingDown", false);
                animator.SetBool("goingRight", false);
                animator.SetBool("goingLeft", false);
                animator.SetBool("goingUp", false);
                animator.SetBool("isDead", true);
                isAlive = false;
                gameObject.layer = 9;
                StartCoroutine(destroyEnemy());
            } 
        }
    }

    IEnumerator destroyEnemy()
    {
        
        yield return new WaitForSeconds(1f);
        playerRef.GetComponent<playerController>().score++;
        source.pitch = source.pitch / 2;
        Destroy(gameObject);
    }

    IEnumerator rateOfFireDelayFunction()
    {
        yield return new WaitForSeconds(rateOfFire);
        canFire = true;
    }

    IEnumerator reloadSpeedDelayFunction()
    {
        Debug.Log("ENEMY RELOADING");
        yield return new WaitForSeconds(reloadSpeed);
        canFire = true;
        currentClip = clipSize;

        float randomDist = Random.Range(0.5f, 0.9f);
        float randomRot = Random.Range(0, 360);
        targetOffset = new Vector3((
            (range * randomDist) * Mathf.Cos(Mathf.Deg2Rad * randomRot)) - ((range * randomDist) * Mathf.Sin(Mathf.Deg2Rad * randomRot)), (
            (range * randomDist) * Mathf.Sin(Mathf.Deg2Rad * randomRot)) + ((range * randomDist) * Mathf.Cos(Mathf.Deg2Rad * randomRot)));
    }

   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collider = collision.gameObject;
        Debug.Log(collider.tag);
        if (collider.tag == "Bullet")
        {
            if (!collider.GetComponent<bulletController>().owner)
            {
                Debug.Log("Owner = " + collider.GetComponent<bulletController>().owner);
                
                if (collider.GetComponent<bulletController>().isRifle)
                {
                    if (collider.GetComponent<bulletController>().rifleHealth <= 0)
                    {
                        // kill enemey and bullet
                        enemyHealth -= collider.GetComponent<bulletController>().dammage;
                        GameObject.Destroy(collider);
                        
                    }
                    else
                    {
                        collider.GetComponent<bulletController>().rifleHealth--;
                        GameObject.Destroy(gameObject);
                    }
                }
                else
                {
                    // kill enemey and bullet
                    enemyHealth -= collider.GetComponent<bulletController>().dammage;
                    GameObject.Destroy(collider);
                    
                }
            }
        }
    }
}
