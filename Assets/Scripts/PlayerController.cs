using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float moveSpeed;
    private Vector2 moveInput;

    private float activeMoveSpeed;
    public float dashSpeed = 10f, dashLength = 0.5f, dashCoolDown = 1f, dashInvincibility = 0.5f;
    [HideInInspector]
    public float dashCounter;
    private float dashCoolCounter;

    public Rigidbody2D rb;

    public Transform gunArm;

    private Camera theCam;

    public Animator anim;

    // These variables are related to shooting mechanisms
    /*public GameObject bulletToFire;
    public Transform firePoint; 
    public float timeBetweenShots;
    private float shotCounter;*/

    // To make player body bit transparent as soon as it gets damage to denote the invincible state for a brief amount of time
    public SpriteRenderer bodySR;

    [HideInInspector]
    public bool canMove = true;

    public List<Gun> availableGuns = new List<Gun>();
    [HideInInspector]
    public int currentGun;

    private void Awake()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        //theCam = Camera.main;

        activeMoveSpeed = moveSpeed;

        UIController.instance.currentGun.sprite = availableGuns[currentGun].gunUI;
        UIController.instance.gunText.text = availableGuns[currentGun].weaponName;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(canMove && !LevelManager.instance.isPaused){
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");

            moveInput.Normalize();

            //transform.position += new Vector3(moveInput.x * Time.deltaTime * moveSpeed, moveInput.y * Time.deltaTime * moveSpeed, 0f);
            rb.velocity = moveInput * activeMoveSpeed;

            Vector3 mousePos = Input.mousePosition;
            Vector3 screenPoint = CameraController.instance.mainCamera.WorldToScreenPoint(transform.localPosition);

            // Changing the face of the player to opposite
            if(mousePos.x < screenPoint.x){
                transform.localScale = new Vector3(-1f, 1f, 1f);
                gunArm.localScale = new Vector3(-1f, -1f, 1f);
            }else{
                transform.localScale = Vector3.one;
                gunArm.localScale = Vector3.one;
            }

            // Rotate gun arm
            Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
            float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            gunArm.rotation = Quaternion.Euler(0, 0, angle);

            // Spawn bullet
            /*if(Input.GetMouseButtonDown(0)){
                Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
                shotCounter = timeBetweenShots;
                AudioManager.instance.PlaySFX(12);
            }

            if(Input.GetMouseButton(0)){
                shotCounter -= Time.deltaTime;
                if(shotCounter <= 0){
                    Instantiate(bulletToFire, firePoint.position, firePoint.rotation); 
                    AudioManager.instance.PlaySFX(12);
                    shotCounter = timeBetweenShots;
                }
            }*/

            if(Input.GetKeyDown(KeyCode.Tab)){
                if(availableGuns.Count > 0){
                    currentGun++;
                    if(currentGun >= availableGuns.Count){
                        currentGun = 0;
                    }

                    SwitchGun();
                }else{
                    Debug.Log("Player has no guns!");
                }
            }

            // Dashing Logic for Player Movement 
            if(Input.GetKeyDown(KeyCode.Space)){
                if(dashCoolCounter <= 0 && dashCounter <= 0){
                    activeMoveSpeed = dashSpeed;
                    dashCounter = dashLength;

                    anim.SetTrigger("dash");

                    PlayerHealthController.instance.MakeInvincible(dashInvincibility);

                    AudioManager.instance.PlaySFX(8);
                }
            }

            if(dashCounter > 0){
                dashCounter -= Time.deltaTime;
                if(dashCounter <= 0){
                    activeMoveSpeed = moveSpeed;
                    dashCoolCounter = dashCoolDown;
                }
            }

            if(dashCoolCounter > 0){
                dashCoolCounter -= Time.deltaTime;
            }

            // Transitioning from Player Idle - Walking Animation
            if(moveInput != Vector2.zero){
                anim.SetBool("isMoving", true);
            }else{
                anim.SetBool("isMoving", false);
            }
        }else{
            // To stop the movement of the Player
            rb.velocity = Vector2.zero;
            anim.SetBool("isMoving", false);
        }
         
    }

    public void SwitchGun(){
        foreach (Gun theGun in availableGuns)
        {   
            Debug.Log(theGun.gameObject+ " : Deactivating Gun!");
            theGun.gameObject.SetActive(false);
        }
        Debug.Log(availableGuns[currentGun]+ " : Activating Gun!");
        availableGuns[currentGun].gameObject.SetActive(true);

        UIController.instance.currentGun.sprite = availableGuns[currentGun].gunUI;
        UIController.instance.gunText.text = availableGuns[currentGun].weaponName;

        AudioManager.instance.PlaySFX(6);
    }
}
