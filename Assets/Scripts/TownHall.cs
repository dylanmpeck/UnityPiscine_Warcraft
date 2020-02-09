using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownHall : Building
{
    [SerializeField] GameObject unit;
    [SerializeField] Transform spawnLocation;
    float timer = 0.0f;

    // Update is called once per frame
    void Update()
    {
       if (timer >= 10.0f)
        {
            timer = 0.0f;
            GameObject spawnedUnit = Instantiate(unit, spawnLocation.position, Quaternion.identity);
            // for some reason footmen are spawning on with outline component enabled
            if (spawnedUnit.tag == "Player")
                spawnedUnit.GetComponent<CharacterController>().OnDeselect();
        }

        timer += Time.deltaTime;
    }
}
