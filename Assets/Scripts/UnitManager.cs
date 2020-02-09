using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] GameObject clickTargetAnimation;

    List<GameObject> units = new List<GameObject>();

    GameObject lastSelectedUnit;
    int selectedSameUnitCount = 0;

    // Update is called once per frame
    void Update()
    {
        // On Left Click either select or move units
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null && hit.collider.tag == "Player")
            {
                Debug.Log(hit.collider.gameObject.name);
                if (Input.GetKey(KeyCode.LeftControl) == false) 
                    ClearUnitSelection();
                SelectUnit(hit.collider.gameObject);
            }
            else if (hit.collider != null)
            {
                //if (hit.collider != null) Debug.Log(hit.collider.gameObject.name);
                //Debug.Log(mousePos2D);
                StartCoroutine(SpawnClickTargetAnimation(hit.collider.transform.position));
                MoveSelectedUnits(hit.collider.transform.position, mousePos);
            }
        }

        // On Right Click - clear all selected units
        if (Input.GetMouseButtonDown(1))
        {
            ClearUnitSelection();
        }
    }

    void MoveSelectedUnits(Vector3 tilePos, Vector3 mousePos)
    {
        foreach (GameObject unit in units)
            unit.GetComponent<CharacterController>().StartMove(tilePos, mousePos);
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
            unit.GetComponent<CharacterController>().annoyed = true;
        }

        lastSelectedUnit = unit;
    }

    void ClearUnitSelection()
    {
        foreach (GameObject unit in units)
        {
            unit.GetComponent<CharacterController>().OnDeselect();
        }
        units.Clear();
    }

    IEnumerator SpawnClickTargetAnimation(Vector2 mousePos)
    {
        clickTargetAnimation.SetActive(true);
        clickTargetAnimation.transform.position = mousePos;
        yield return new WaitForSeconds(0.35f);
        clickTargetAnimation.SetActive(false);
    }
}
