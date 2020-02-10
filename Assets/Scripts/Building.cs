using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] int maxHp;
    int currentHp;

    public enum Races { Human, Orc };

    [SerializeField] public Races race;

    [HideInInspector] public bool isAlive = true;

    [SerializeField] AudioClip explode;

    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHp;
    }

    public void ReceiveDamage(int damage)
    {
        if (isAlive)
        {
            currentHp -= damage;

            Debug.Log(transform.name + "[" + currentHp + "/" + maxHp + "]HP has been attacked.");

            if (race == Races.Orc)
            {
                EnemyAI.baseAttacked = true;
            }

            if (currentHp <= 0)
            {
                if (race == Races.Human)
                {
                    GameManager.HumanBuildingCount--;
                    TownHall.HumanSpawnTime += 2.5f;
                }
                else
                {
                    GameManager.OrcBuildingCount--;
                    TownHall.OrcSpawnTime += 2.5f;
                }

                GameObject.Find("GameManager").GetComponent<AudioSource>().PlayOneShot(explode);
                isAlive = false;
                //StartCoroutine(WaitAndDestroy());
                this.gameObject.SetActive(false);
                Navigator.RefreshTileMap();
            }
        }
    }

    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(0.15f);
        Destroy(this.gameObject);
    }
}
