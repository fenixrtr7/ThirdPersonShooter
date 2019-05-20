using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static int livesRemaining;
    public static int currentCherryCount;
    public int tempCurrentCherryCount;
    public bool isCollectingCherries;

    public static bool hasDead;

    public Transform[] spawningZones;

    // Start is called before the first frame update
    void Start()
    {
        livesRemaining = 3;
        currentCherryCount = 1000000;
        tempCurrentCherryCount = 0;
        isCollectingCherries = false;
        hasDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCollectingCherries)
        {
            // Cada 60 frames
            if (tempCurrentCherryCount >= 60)
            {
                currentCherryCount++;
                PointsManager.AddPoints(5);
                tempCurrentCherryCount = 0;
            }
            else
            {
                // Aún no hemos llegado al frame número 60
                tempCurrentCherryCount++;
            }
        }

        if (HealthManager.currentHealth <= 0 && !hasDead)
        {
            hasDead = true;
            livesRemaining--;

            if (livesRemaining == 2)
            {
                Destroy(GameObject.Find("Life3"));
                GetComponent<Animator>().Play("Die");
                StartCoroutine(RespawnPlayer());

            }
            if (livesRemaining == 1)
            {
                Destroy(GameObject.Find("Life2"));
                GetComponent<Animator>().Play("Die");
                StartCoroutine(RespawnPlayer());
            }
            if (livesRemaining == 0)
            {
                Destroy(GameObject.Find("Life1"));
            }
        }
    }

    IEnumerator RespawnPlayer()
    {
        // Calculamos aleartoriamnete en qué posición debemos aparecer
        int randomPos = Random.Range(0, spawningZones.Length);
        // Esperamos 4 segundos que dura la muerte
        yield return new WaitForSecondsRealtime(4f);
        // Movemos al jugador a la zona de spawning
        this.transform.position = spawningZones[randomPos].transform.position;
        // Volvemos a poner al jugador con su animación de Idle
        GetComponent<Animator>().Play("Idle_Shoot");
        HealthManager.currentHealth = 100;
        hasDead = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MunitionBox"))
        {
            // Aqui el personaje esta en la zona
            isCollectingCherries = true;
            currentCherryCount++;
            PointsManager.AddPoints(5);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("MunitionBox"))
        {
            isCollectingCherries = false;
        }
    }
}
