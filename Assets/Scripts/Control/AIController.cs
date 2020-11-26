using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;


namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;

        Fighter fighter;
        GameObject player;
        Health health;
        Mover mover;

        Vector3 guardPosition;

        float timeSinceLastSawPlayer = Mathf.Infinity;
        float suspicionTime = 2f;

        private void Start()
        {
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();

            guardPosition = transform.position;
        }

        private void Update()
        {
            if (health.IsDead()) { return; }

            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0;
                AttackBehaviour();
            }

            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspiciousBehaviour();
            }

            else
            {
                GuardBehaviour();
            }

            timeSinceLastSawPlayer += Time.deltaTime;
        }

        private void AttackBehaviour()
        {
            fighter.Attack(player);
        }


        private void SuspiciousBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }


        private void GuardBehaviour()
        {
            mover.StartMoveAction(guardPosition);
        }


        private bool InAttackRangeOfPlayer()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
