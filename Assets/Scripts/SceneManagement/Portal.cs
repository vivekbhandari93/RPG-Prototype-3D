﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier { A, B, C, D, E}

        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint = null;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] float fadeOutTime = 0.5f; 
        [SerializeField] float fadeInTime = 1f; 
        [SerializeField] float fadeWaitTime = 0.5f; 


        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();

            yield return fader.FadeOut(fadeOutTime);

            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            wrapper.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            wrapper.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);

            Destroy(gameObject);
        }

        private Portal GetOtherPortal()
        {
            foreach(Portal portal in FindObjectsOfType<Portal>())
            {
                if(portal == this) { continue; }
                if(portal.destination != destination) { continue; }
                return portal;
            }
            return null;
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");

            player.GetComponent<NavMeshAgent>().enabled = false;

            player.transform.position = otherPortal.spawnPoint.position;
            player.transform.rotation = otherPortal.spawnPoint.rotation;

            player.GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}
