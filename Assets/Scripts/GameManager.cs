using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject HumanTown;
    [SerializeField] GameObject OrcTown;

    [SerializeField] AudioClip OrcWinSound;
    [SerializeField] AudioClip HumanWinSound;

    public static int HumanBuildingCount;
    public static int OrcBuildingCount;
    // Start is called before the first frame update
    void Start()
    {
        HumanBuildingCount = HumanTown.transform.childCount - 1;
        OrcBuildingCount = OrcTown.transform.childCount - 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (HumanBuildingCount == 0)
        {
            Debug.Log("The Orc Team Wins.");
            HumanBuildingCount--;
            GetComponent<AudioSource>().PlayOneShot(HumanWinSound);
        }

        if (OrcBuildingCount == 0)
        {
            Debug.Log("The Human Team Wins.");
            OrcBuildingCount--;
            GetComponent<AudioSource>().PlayOneShot(OrcWinSound);
        }

    }
}
