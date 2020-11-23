using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            MoveToCursor();
        }
        UpdateAnimator();
    }

    private void MoveToCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);

        if (hasHit)
        {
            agent.destination = hit.point;
        }

    }

    private void UpdateAnimator()
    {
        Vector3 velocity = agent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        animator.SetFloat("forwardSpeed", speed);
    }
}
