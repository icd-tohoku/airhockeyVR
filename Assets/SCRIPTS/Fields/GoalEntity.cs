using System;
using System.Collections;
using System.Collections.Generic;
using AirHockey.Data;
using AirHockey.Systems;
using UnityEngine;

namespace AirHockey.Fields
{
    public class GoalEntity : MonoBehaviour
    {
        [SerializeField] private AudioClip clip;
        [SerializeField] private PlayerType goalOwner;
        public PlayerType GoalOwner => goalOwner;

        private AudioSource _source;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        public void GoalSound()
        {
            _source.PlayOneShot(clip);
        }
    }
}