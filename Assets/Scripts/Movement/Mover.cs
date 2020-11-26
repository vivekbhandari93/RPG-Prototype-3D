using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;


namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        NavMeshAgent agent;
        Animator animator;
        Health health;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();
        }


        void Update()
        {
            agent.enabled = !health.IsDead();

            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destionation)
        {
            GetComponent<ActionScheduler>().StartActionScheduler(this);
            MoveTo(destionation);
        }

        public void MoveTo(Vector3 destination)
        {
            agent.destination = destination;
            agent.isStopped = false;
        }

        public void Cancel()
        {
            agent.isStopped = true;
        }


        private void UpdateAnimator()
        {
            Vector3 velocity = agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            animator.SetFloat("forwardSpeed", speed);
        }

    }
}
