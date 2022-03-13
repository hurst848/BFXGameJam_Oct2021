using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] public float moveSpeed;
    [SerializeField] public float health;
    public int score;

    public int skillPoints = 0;

    public GameObject sprite;

    public Text DED;

    public GameObject bulletPrefeab;
    public float bulletSpeed;

    // Gets animator from sprite
    public Animator animator;

    // Denotes which weapone behaviour to follow
    public int currentWeapon = 0;
    // used to detect mode change
    public int lastMode = 0;


    public List<AudioClip> sounds;
    private AudioSource source;

    /* GUNS TABLE */
        // Current Selected
            private float rateOfFire;
            private int clipSize;
            private float reloadSpeed;
            private float dammage;
        // Pistol               0
            public float pistol_RateOfFire;
            public int pistol_ClipSize;
            public float pistol_ReloadSpeed;
            public float pistol_Dammage; 
        // Shotgun              1 (fires 4 bullets)
            public float shotgun_RateOfFire;
            public int shotgun_ClipSize;
            public float shotgun_ReloadSpeed;
            public float shotgun_Dammage;
        // Rifle                2 (req bullet health)
            public float rifle_RateOfFire;
            public int rifle_ClipSize;
            public float rifle_ReloadSpeed;
            public float rifle_Dammage;
    // Burst Machine Gun    3 (4 shots)
            public float machineGun_RateOfFire;
            public int machineGun_ClipSize;
            public float machineGun_ReloadSpeed;
            public float machineGun_Dammage;

    // Whether or not a bullet can be fired
    [SerializeField]private bool canFire = true;
    [SerializeField]private int currentClip;
    private Vector3 spriteScale;

    void Start()
    {
        Debug.Log("start");
        currentWeapon = 0;
        rateOfFire = pistol_RateOfFire;
        clipSize = pistol_ClipSize;
        reloadSpeed = pistol_ReloadSpeed;
        spriteScale = sprite.transform.localScale;
        currentClip = clipSize;
        dammage = pistol_Dammage;
        score = 0;
        source = GetComponent<AudioSource>();
    }
    bool dn = false;
    // Update is called once per frame
    void Update()
    {

        if (health >= 0)
        {
            // reset scale
            sprite.transform.localScale = spriteScale;

            // Get mouse location and cast to world space
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
            bool facingRight = false;


            // Poll inputs
            Vector3 move = new Vector3();
            if (Input.GetKey(KeyCode.A))
            {
                move.x -= moveSpeed * Time.deltaTime;
                animator.SetBool("Left", true);
            }
            else { animator.SetBool("Left", false); }

            if (Input.GetKey(KeyCode.D))
            {
                move.x += moveSpeed * Time.deltaTime;
                animator.SetBool("Right", true);
                facingRight = true;
            }
            else { animator.SetBool("Right", false); }

            if (Input.GetKey(KeyCode.W))
            {
                move.y += moveSpeed * Time.deltaTime;
                animator.SetBool("Backward", true);
            }
            else { animator.SetBool("Backward", false); }

            if (Input.GetKey(KeyCode.S))
            {
                move.y -= moveSpeed * Time.deltaTime;
                animator.SetBool("Forward", true);
            }
            else { animator.SetBool("Forward", false); }

            if (move.x > 0 && move.y < 0)
            {
                animator.SetBool("Forward", false);
                animator.SetBool("Right", false);
                animator.SetBool("Angle1", true);
                facingRight = true;
            }
            else
            { animator.SetBool("Angle1", false); }


            if (move.x > 0 && move.y > 0)
            {
                animator.SetBool("Backward", false);
                animator.SetBool("Right", false);
                animator.SetBool("Angle2", true);
                facingRight = true;
            }
            else
            { animator.SetBool("Angle2", false); }

            if (move.x < 0 && move.y < 0)
            {
                animator.SetBool("Forward", false);
                animator.SetBool("Left", false);
                animator.SetBool("Angle1Flipped", true);
            }
            else
            { animator.SetBool("Angle1Flipped", false); }

            if (move.x < 0 && move.y > 0)
            {
                animator.SetBool("Backward", false);
                animator.SetBool("Left", false);
                animator.SetBool("Angle2Flipped", true);
            }
            else
            { animator.SetBool("Angle2Flipped", false); }

            // flip sprite if facing right
            if (facingRight)
            {
                Vector3 theScale = sprite.transform.localScale;
                theScale.x *= -1;
                sprite.transform.localScale = theScale;
            }

            // weapon switching
            if (Input.GetKey(KeyCode.Alpha1)) { currentWeapon = 0; }
            if (Input.GetKey(KeyCode.Alpha2)) { currentWeapon = 1; }
            if (Input.GetKey(KeyCode.Alpha3)) { currentWeapon = 2; }
            if (Input.GetKey(KeyCode.Alpha4)) { currentWeapon = 3; }

            if (currentWeapon != lastMode) { changeGun(currentWeapon); }

            // create and fire the bullets
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (canFire)
                {
                    canFire = false;

                    if (currentWeapon == 0) // Pistol
                    {

                        GameObject newBullet = Instantiate(bulletPrefeab, transform.position, transform.rotation, transform.parent);
                        newBullet.GetComponent<bulletController>().setupBullet(transform.position, worldPosition);
                        newBullet.GetComponent<bulletController>().bulletSpeed = bulletSpeed;
                        newBullet.GetComponent<bulletController>().owner = false;
                        newBullet.GetComponent<bulletController>().dammage = dammage;
                        newBullet.GetComponent<bulletController>().startBullet();
                        source.clip = sounds[0];
                        source.Play();
                        // Play animation
                        animator.SetBool("Shoot", true);

                        if (animator.GetBool("Forward"))
                        {
                            animator.Play("PlayerShootingFront");
                            animator.SetBool("Shoot", false);
                        }
                        else if (animator.GetBool("Backward"))
                        {
                            animator.Play("PlayerShootingBack");
                            animator.SetBool("Shoot", false);
                        }
                        else if (animator.GetBool("Left"))
                        {
                            animator.Play("PlayerShootingSideFlipped");
                            animator.SetBool("Shoot", false);
                        }
                        else if (animator.GetBool("Right"))
                        {
                            animator.Play("PlayerShootingSide");
                            animator.SetBool("Shoot", false);
                        }
                        else if (animator.GetBool("Angle1"))
                        {
                            animator.Play("PlayerShootingAngle1");
                            animator.SetBool("Shoot", false);
                        }
                        else if (animator.GetBool("Angle2"))
                        {
                            animator.Play("PlayerShootingAngle2");
                            animator.SetBool("Shoot", false);
                        }
                        else if (animator.GetBool("Angle1Flipped"))
                        {
                            animator.Play("PlayerShootingAngle1Flipped");
                            animator.SetBool("Shoot", false);
                        }
                        else if (animator.GetBool("Angle2Flipped"))
                        {
                            animator.Play("PlayerShootingAngle2Flipped");
                            animator.SetBool("Shoot", false);
                        }
                        else
                        {
                            animator.Play("PlayerShootingFront");
                            animator.SetBool("Shoot", false);
                        }


                        if (currentClip <= 0) { StartCoroutine(reloadSpeedDelayFunction()); }
                        else { StartCoroutine(rateOfFireDelayFunction()); currentClip--; }
                    }
                    else if (currentWeapon == 1) // Shotgun
                    {
                        GameObject newBullet = Instantiate(bulletPrefeab, transform.position, transform.rotation, transform.parent);
                        newBullet.GetComponent<bulletController>().setupBullet(transform.position, new Vector3((
                            worldPosition.x * Mathf.Cos(Mathf.Deg2Rad * 2.0f)) - (worldPosition.y * Mathf.Sin(Mathf.Deg2Rad * 2.0f)), (
                            worldPosition.x * Mathf.Sin(Mathf.Deg2Rad * 2.0f)) + (worldPosition.y * Mathf.Cos(Mathf.Deg2Rad * 2.0f))));
                        newBullet.GetComponent<bulletController>().bulletSpeed = bulletSpeed;
                        newBullet.GetComponent<bulletController>().owner = false;
                        newBullet.GetComponent<bulletController>().dammage = dammage;
                        newBullet.GetComponent<bulletController>().startBullet();

                        GameObject newBullet1 = Instantiate(bulletPrefeab, transform.position, transform.rotation, transform.parent);
                        newBullet1.GetComponent<bulletController>().setupBullet(transform.position, new Vector3((
                            worldPosition.x * Mathf.Cos(Mathf.Deg2Rad * -2.0f)) - (worldPosition.y * Mathf.Sin(Mathf.Deg2Rad * -2.0f)), (
                            worldPosition.x * Mathf.Sin(Mathf.Deg2Rad * -2.0f)) + (worldPosition.y * Mathf.Cos(Mathf.Deg2Rad * -2.0f))));
                        newBullet1.GetComponent<bulletController>().bulletSpeed = bulletSpeed;
                        newBullet.GetComponent<bulletController>().owner = false;
                        newBullet.GetComponent<bulletController>().dammage = dammage;
                        newBullet1.GetComponent<bulletController>().startBullet();

                        GameObject newBullet2 = Instantiate(bulletPrefeab, transform.position, transform.rotation, transform.parent);
                        newBullet2.GetComponent<bulletController>().setupBullet(transform.position, new Vector3((
                            worldPosition.x * Mathf.Cos(Mathf.Deg2Rad * 4.0f)) - (worldPosition.y * Mathf.Sin(Mathf.Deg2Rad * 4.0f)), (
                            worldPosition.x * Mathf.Sin(Mathf.Deg2Rad * 4.0f)) + (worldPosition.y * Mathf.Cos(Mathf.Deg2Rad * 4.0f))));
                        newBullet2.GetComponent<bulletController>().bulletSpeed = bulletSpeed;
                        newBullet.GetComponent<bulletController>().owner = false;
                        newBullet.GetComponent<bulletController>().dammage = dammage;
                        newBullet2.GetComponent<bulletController>().startBullet();

                        GameObject newBullet3 = Instantiate(bulletPrefeab, transform.position, transform.rotation, transform.parent);
                        newBullet3.GetComponent<bulletController>().setupBullet(transform.position, new Vector3((
                            worldPosition.x * Mathf.Cos(Mathf.Deg2Rad * -4.0f)) - (worldPosition.y * Mathf.Sin(Mathf.Deg2Rad * -4.0f)), (
                            worldPosition.x * Mathf.Sin(Mathf.Deg2Rad * -4.0f)) + (worldPosition.y * Mathf.Cos(Mathf.Deg2Rad * -4.0f))));
                        newBullet3.GetComponent<bulletController>().bulletSpeed = bulletSpeed;
                        newBullet.GetComponent<bulletController>().dammage = dammage;
                        newBullet.GetComponent<bulletController>().owner = false;
                        newBullet3.GetComponent<bulletController>().startBullet();

                        GameObject newBullet4 = Instantiate(bulletPrefeab, transform.position, transform.rotation, transform.parent);
                        newBullet4.GetComponent<bulletController>().setupBullet(transform.position, worldPosition);
                        newBullet4.GetComponent<bulletController>().bulletSpeed = bulletSpeed;
                        newBullet.GetComponent<bulletController>().owner = false;
                        newBullet.GetComponent<bulletController>().dammage = dammage;
                        newBullet4.GetComponent<bulletController>().startBullet();
                        source.clip = sounds[1];
                        source.Play();
                        // Play animation
                        animator.SetBool("Shoot", true);

                        if (animator.GetBool("Forward"))
                        {
                            animator.Play("PlayerShootingFront");
                            animator.SetBool("Shoot", false);
                        }
                        else if (animator.GetBool("Backward"))
                        {
                            animator.Play("PlayerShootingBack");
                            animator.SetBool("Shoot", false);
                        }
                        else if (animator.GetBool("Left"))
                        {
                            animator.Play("PlayerShootingSideFlipped");
                            animator.SetBool("Shoot", false);
                        }
                        else if (animator.GetBool("Right"))
                        {
                            animator.Play("PlayerShootingSide");
                            animator.SetBool("Shoot", false);
                        }
                        else if (animator.GetBool("Angle1"))
                        {
                            animator.Play("PlayerShootingAngle1");
                            animator.SetBool("Shoot", false);
                        }
                        else if (animator.GetBool("Angle2"))
                        {
                            animator.Play("PlayerShootingAngle2");
                            animator.SetBool("Shoot", false);
                        }
                        else if (animator.GetBool("Angle1Flipped"))
                        {
                            animator.Play("PlayerShootingAngle1Flipped");
                            animator.SetBool("Shoot", false);
                        }
                        else if (animator.GetBool("Angle2Flipped"))
                        {
                            animator.Play("PlayerShootingAngle2Flipped");
                            animator.SetBool("Shoot", false);
                        }
                        else
                        {
                            animator.Play("PlayerShootingFront");
                            animator.SetBool("Shoot", false);
                        }

                        if (currentClip <= 0) { StartCoroutine(reloadSpeedDelayFunction()); }
                        else { StartCoroutine(rateOfFireDelayFunction()); currentClip--; }
                    }
                    else if (currentWeapon == 2) // Rifle
                    {
                        GameObject newBullet = Instantiate(bulletPrefeab, transform.position, transform.rotation, transform.parent);
                        newBullet.GetComponent<bulletController>().setupBullet(transform.position, worldPosition);
                        newBullet.GetComponent<bulletController>().bulletSpeed = bulletSpeed;
                        newBullet.GetComponent<bulletController>().isRifle = true;
                        newBullet.GetComponent<bulletController>().owner = false;
                        newBullet.GetComponent<bulletController>().dammage = dammage;
                        newBullet.GetComponent<bulletController>().startBullet();
                        source.clip = sounds[2];
                        source.Play();
                        // Play Animation
                        animator.SetBool("Shoot", true);

                        if (animator.GetBool("Forward"))
                        {
                            animator.Play("PlayerShootingFront");
                            animator.SetBool("Shoot", false);
                        }
                        else if (animator.GetBool("Backward"))
                        {
                            animator.Play("PlayerShootingBack");
                            animator.SetBool("Shoot", false);
                        }
                        else if (animator.GetBool("Left"))
                        {
                            animator.Play("PlayerShootingSideFlipped");
                            animator.SetBool("Shoot", false);
                        }
                        else if (animator.GetBool("Right"))
                        {
                            animator.Play("PlayerShootingSide");
                            animator.SetBool("Shoot", false);
                        }
                        else if (animator.GetBool("Angle1"))
                        {
                            animator.Play("PlayerShootingAngle1");
                            animator.SetBool("Shoot", false);
                        }
                        else if (animator.GetBool("Angle2"))
                        {
                            animator.Play("PlayerShootingAngle2");
                            animator.SetBool("Shoot", false);
                        }
                        else if (animator.GetBool("Angle1Flipped"))
                        {
                            animator.Play("PlayerShootingAngle1Flipped");
                            animator.SetBool("Shoot", false);
                        }
                        else if (animator.GetBool("Angle2Flipped"))
                        {
                            animator.Play("PlayerShootingAngle2Flipped");
                            animator.SetBool("Shoot", false);
                        }
                        else
                        {
                            animator.Play("PlayerShootingFront");
                            animator.SetBool("Shoot", false);
                        }


                        if (currentClip <= 0) { StartCoroutine(reloadSpeedDelayFunction()); }
                        else { StartCoroutine(rateOfFireDelayFunction()); currentClip--; }
                    }
                    else if (currentWeapon == 3) // Machine Gun
                    {
                        StartCoroutine(machineGunBulletSpawner());
                        source.clip = sounds[3];
                        source.Play();
                        // Play Animation
                        animator.SetBool("Shoot", true);

                        if (animator.GetBool("Forward"))
                        {
                            animator.Play("PlayerShootingFront");
                            animator.SetBool("Shoot", false);
                        }
                        else if (animator.GetBool("Backward"))
                        {
                            animator.Play("PlayerShootingBack");
                            animator.SetBool("Shoot", false);
                        }
                        else if (animator.GetBool("Left"))
                        {
                            animator.Play("PlayerShootingSideFlipped");
                            animator.SetBool("Shoot", false);
                        }
                        else if (animator.GetBool("Right"))
                        {
                            animator.Play("PlayerShootingSide");
                            animator.SetBool("Shoot", false);
                        }
                        else if (animator.GetBool("Angle1"))
                        {
                            animator.Play("PlayerShootingAngle1");
                            animator.SetBool("Shoot", false);
                        }
                        else if (animator.GetBool("Angle2"))
                        {
                            animator.Play("PlayerShootingAngle2");
                            animator.SetBool("Shoot", false);
                        }
                        else if (animator.GetBool("Angle1Flipped"))
                        {
                            animator.Play("PlayerShootingAngle1Flipped");
                            animator.SetBool("Shoot", false);
                        }
                        else if (animator.GetBool("Angle2Flipped"))
                        {
                            animator.Play("PlayerShootingAngle2Flipped");
                            animator.SetBool("Shoot", false);
                        }
                        else
                        {
                            animator.Play("PlayerShootingFront");
                            animator.SetBool("Shoot", false);
                        }

                        if (currentClip <= 0) { StartCoroutine(reloadSpeedDelayFunction()); }
                        else { StartCoroutine(rateOfFireDelayFunction()); currentClip -= 4; }
                    }

                }
            }

            GetComponent<Transform>().position = GetComponent<Transform>().position += move;
        }
        else
        {
            if (!dn)
            {
                source.clip = sounds[4];
                source.Play();
                dn = true;
            }
            DED.text = "You have perished! \n Press R to return to main menu.";
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }

    IEnumerator rateOfFireDelayFunction()
    {
        yield return new WaitForSeconds(rateOfFire);
        canFire = true;
    }

    IEnumerator reloadSpeedDelayFunction()
    {
        yield return new WaitForSeconds(reloadSpeed);
        canFire = true;
        currentClip = clipSize;
    }

    IEnumerator machineGunBulletSpawner()
    {
        for (int i = 0; i < 4; i++)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

            GameObject newBullet = Instantiate(bulletPrefeab, transform.position, transform.rotation, transform.parent);
            newBullet.GetComponent<bulletController>().setupBullet(transform.position, worldPosition);
            newBullet.GetComponent<bulletController>().bulletSpeed = bulletSpeed;
            newBullet.GetComponent<bulletController>().owner = false;
            newBullet.GetComponent<bulletController>().dammage = dammage;
            newBullet.GetComponent<bulletController>().startBullet();

            yield return new WaitForSeconds(0.1f);

        }
        yield return null; ;
    }

    private void changeGun(int newMode)
    {
        switch (newMode)
        {
            case 0:
                rateOfFire = pistol_RateOfFire;
                currentClip = pistol_ClipSize;
                clipSize = pistol_ClipSize;
                reloadSpeed = pistol_ReloadSpeed;
                dammage = pistol_Dammage;
                break;
            case 1:
                rateOfFire = shotgun_RateOfFire;
                currentClip = shotgun_ClipSize;
                clipSize = shotgun_ClipSize;
                reloadSpeed = shotgun_ReloadSpeed;
                dammage = shotgun_Dammage;
                break;
            case 2:
                rateOfFire = rifle_RateOfFire;
                currentClip = rifle_ClipSize;
                clipSize = rifle_ClipSize;
                reloadSpeed = rifle_ReloadSpeed;
                dammage = rifle_Dammage;
                break;
            case 3:
                rateOfFire = machineGun_RateOfFire;
                currentClip = machineGun_ClipSize;
                clipSize = machineGun_ClipSize;
                dammage = machineGun_Dammage;
                reloadSpeed = machineGun_ReloadSpeed;
                break;
            default:
                break;

        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collider = collision.gameObject;
        if (collider.tag == "Bullet")
        {
            if (collider.GetComponent<bulletController>().owner)
            {
                GameObject.Destroy(collider);
                health -= 0.1f;
            }
        }
    }
}
