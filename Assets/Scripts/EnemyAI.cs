using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    CharacterController controller;
    GameObject HumanTownCenter;
    GameObject OrcTownCenter;

    GameObject currentTarget = null;

    static public bool targetingPlayerBase = true;

    public static bool baseAttacked = false;

    [HideInInspector] public bool inactive;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        inactive = true;
        controller = GetComponent<CharacterController>();
        HumanTownCenter = GameObject.Find("HumanTownCenter");
        OrcTownCenter = GameObject.Find("OrcTownCenter");
    }

    private void Update()
    {
        if (Vector3.Distance(OrcTownCenter.transform.position, transform.position) <= 4.0f)
            baseAttacked = false;

        if (baseAttacked && targetingPlayerBase)
        {
            controller.StartMove(OrcTownCenter.transform.position, false);
        }

        if (inactive)
        {
            if (targetingPlayerBase)
            {
                controller.StartMove(HumanTownCenter.transform.position, false);
            }
            else
            {
                controller.StartMove(OrcTownCenter.transform.position, false);
            }
            inactive = false;
        }

        if (animator.GetBool("Attack") == false && animator.GetBool("Moving") == false)
            inactive = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (baseAttacked)
        {
            if (Vector3.Distance(transform.position, OrcTownCenter.transform.position) < 2.0f)
            {
                baseAttacked = false;
            }
        }
        else
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 6.0f, 1 << LayerMask.NameToLayer("Building") | 1 << LayerMask.NameToLayer("Unit"));
            float distanceToTarget = Mathf.Infinity;
            GameObject closestTarget = null;
            foreach (Collider2D hit in hits)
            {
                if (hit && (hit.tag == "Player" || hit.tag == "PlayerBuilding"))
                {
                    if (Vector3.Distance(hit.transform.position, this.transform.position) < distanceToTarget)
                    {
                        distanceToTarget = Vector3.Distance(hit.transform.position, transform.position);
                        closestTarget = hit.gameObject;
                    }
                }
            }
            if ((currentTarget == null && closestTarget != null) || (closestTarget && closestTarget != currentTarget))
            {
                currentTarget = closestTarget;
                controller.Attack(closestTarget);
            }
        }
    }
}
