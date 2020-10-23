using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    public Gun theGun;
    public float waitToBeCollected = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(waitToBeCollected > 0){
            waitToBeCollected -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player" && waitToBeCollected <= 0){
            
            bool hasGun = false;
            foreach (Gun gunToCheck in PlayerController.instance.availableGuns)
            {
                if(theGun.weaponName == gunToCheck.weaponName){
                    hasGun = true;
                }
            }

            if(!hasGun){
                Debug.Log("Inside GunPickup If block....");
                Gun gunClone = Instantiate(theGun);
                gunClone.transform.parent = PlayerController.instance.gunArm;
                gunClone.transform.position = PlayerController.instance.gunArm.position;
                gunClone.transform.localRotation = Quaternion.Euler(Vector3.zero);
                gunClone.transform.localScale = Vector3.one;


                PlayerController.instance.availableGuns.Add(gunClone);
                PlayerController.instance.currentGun = PlayerController.instance.availableGuns.Count - 1;
                Debug.Log(PlayerController.instance.availableGuns[PlayerController.instance.currentGun]+" : Current Gun from GunPickup Script! Calling SwitchGun() ");
                PlayerController.instance.SwitchGun();
            }

            Destroy(gameObject);

            AudioManager.instance.PlaySFX(7);
        }
    }
}
