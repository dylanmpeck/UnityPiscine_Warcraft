using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] GameObject clickTargetAnimation;

    static List<GameObject> units = new List<GameObject>();

    GameObject lastSelectedUnit;
    int selectedSameUnitCount = 0;

    bool whichAudioSource;

    [SerializeField] List<AudioSource> audioSources;

    [SerializeField] List<AudioClip> selectedSounds;
    [SerializeField] List<AudioClip> acknowledgeSounds;
    [SerializeField] List<AudioClip> annoyedSounds;

    // Update is called once per frame
    void Update()
    {
        // On Left Click either select or move units
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.tag == "Player" && hit.collider.GetComponent<CharacterController>().isAlive)
                {
                    if (Input.GetKey(KeyCode.LeftControl) == false)
                        ClearUnitSelection();
                    SelectUnit(hit.collider.gameObject);
                }
                else if (hit.collider.tag == "Enemy" || hit.collider.tag == "EnemyBuilding")
                {
                    Debug.Log(hit.collider.name);
                    OrderAttack(hit.collider.gameObject);
                }
                else
                {
                    //if (hit.collider != null) Debug.Log(hit.collider.gameObject.name);
                    //Debug.Log(mousePos2D);
                    StartCoroutine(SpawnClickTargetAnimation(hit.collider.transform.position));
                    MoveSelectedUnits(hit.collider.transform.position);
                }
            }

        }

        // On Right Click - clear all selected units
        if (Input.GetMouseButtonDown(1))
        {
            ClearUnitSelection();
        }
    }

    void MoveSelectedUnits(Vector3 tilePos)
    {
        foreach (GameObject unit in units)
            unit.GetComponent<CharacterController>().StartMove(tilePos);
        PlaySound(acknowledgeSounds);
    }

    void SelectUnit(GameObject unit)
    {
        units.Add(unit);
        unit.GetComponent<CharacterController>().OnSelect();

        if (lastSelectedUnit && GameObject.ReferenceEquals(unit, lastSelectedUnit))
            selectedSameUnitCount++;

        if (selectedSameUnitCount > 7)
        {
            selectedSameUnitCount = 0;
            PlaySound(annoyedSounds);
        }
        else
            PlaySound(selectedSounds);

        lastSelectedUnit = unit;
    }

    static void ClearUnitSelection()
    {
        foreach (GameObject unit in units)
        {
            unit.GetComponent<CharacterController>().OnDeselect();
        }
        units.Clear();
    }

    public static void RemoveUnit(GameObject unitToRemove)
    {
        units.Remove(unitToRemove);
    }

    void OrderAttack(GameObject target)
    {
        foreach (GameObject unit in units)
        {
            unit.GetComponent<CharacterController>().Attack(target);
        }
        PlaySound(acknowledgeSounds);
    }

    IEnumerator SpawnClickTargetAnimation(Vector2 mousePos)
    {
        clickTargetAnimation.SetActive(true);
        clickTargetAnimation.transform.position = mousePos;
        yield return new WaitForSeconds(0.35f);
        clickTargetAnimation.SetActive(false);
    }

    // Crossfades one audio source with the other for smooth clip transitions.
    void PlaySound(List<AudioClip> soundChoices)
    {
        StartCoroutine(StartFade(audioSources[Convert.ToInt32(whichAudioSource)], 0.16f, 0.0f));
        whichAudioSource = !whichAudioSource;
        audioSources[Convert.ToInt32(whichAudioSource)].volume = 1.0f;
        audioSources[Convert.ToInt32(whichAudioSource)].PlayOneShot(soundChoices[UnityEngine.Random.Range(0, soundChoices.Count)]);
    }

    public IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}
