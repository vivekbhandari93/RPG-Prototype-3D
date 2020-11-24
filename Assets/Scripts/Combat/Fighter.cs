using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttack = 1f;
        
        Transform target;
        float timeSinceLastAttack = 0;


        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (!target) { return; }

            if (!GetIsCloseEnough())
            {
                GetComponent<Mover>().MoveTo(target.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            if(timeSinceLastAttack > timeBetweenAttack)
            {
                GetComponent<Animator>().SetTrigger("attack");
                timeSinceLastAttack = 0f;
            }
        }

        private bool GetIsCloseEnough()
        {
            return Vector3.Distance(transform.position, target.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartActionScheduler(this);
            target = combatTarget.transform;
        }

        public void Cancel()
        {
            target = null;
        }

        //called on the animation
        void Hit()
        {

        }
    }
}
