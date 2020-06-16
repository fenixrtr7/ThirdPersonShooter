using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class PlayerAgent : MonoBehaviour
{
    public PlayerCharacter playerCharacterData;

    private void Awake() {
        PlayerCharacter tmp = new PlayerCharacter();
        tmp.name = "Juan Gabriel";
        tmp.health = 100;
        tmp.defense = 50;
        tmp.description = "El mejor personaje";
        tmp.dexterity = 30;
        tmp.intelligence = 80;
        tmp.strength = 40;

        playerCharacterData = tmp;
    }

    private void Update() {
        if(playerCharacterData.health < 0f)
        {
            playerCharacterData.health = 0;

            transform.GetComponent<DragonCharacterController>().die = true;
        }
    }
}
