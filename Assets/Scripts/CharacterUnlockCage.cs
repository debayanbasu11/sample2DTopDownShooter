using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUnlockCage : MonoBehaviour
{
    private bool canUnlock;
    public GameObject message;

    public CharacterSelector[] charSelects;
    private CharacterSelector playerToUnlock;

    // To show the player body inside the cage
    public SpriteRenderer cagedSR;

    // Start is called before the first frame update
    void Start()
    {
        playerToUnlock = charSelects[Random.Range(0, charSelects.Length)];

        cagedSR.sprite = playerToUnlock.playerToSpawn.bodySR.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if(canUnlock){
            if(Input.GetKeyDown(KeyCode.E)){
                // Logic: 0 => Character is locked; 1 => Character is unlocked
                PlayerPrefs.SetInt(playerToUnlock.playerToSpawn.name, 1);

                Instantiate(playerToUnlock, transform.position, transform.rotation);

                gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player"){
            canUnlock = true;
            message.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.tag == "Player"){
            canUnlock = false;
            message.SetActive(false);
        }
    }
}
