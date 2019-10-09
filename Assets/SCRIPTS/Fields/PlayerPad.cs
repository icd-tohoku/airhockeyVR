using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AirHockey.Fields
{
    public class PlayerPad : MonoBehaviour
    {
        private const string PuckTag = "Puck";
        private const float Translation = 0.05f;
        private static readonly Vector3 RightTranslation = new Vector3(Translation, 0, 0);
        private static readonly Vector3 LeftTranslation = new Vector3(-Translation, 0, 0);
        private static readonly Vector3 UpperTranslation = new Vector3(0, 0, Translation);
        private static readonly Vector3 DownTranslation = new Vector3(0, 0, -Translation);
        
        [SerializeField] private float hitForce;
        [SerializeField] private bool enableKeyboardManipulation = false;

        private void Update()
        {
            if (enableKeyboardManipulation == false) return;

            // Read keyboard input and move myself.
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.position += RightTranslation;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position += LeftTranslation;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.position += DownTranslation;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.position += UpperTranslation;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag(PuckTag))
            {
                // Paddle and Puck have capsule collider, so we can use their position directly to detect the direction to add force
                var direction = (other.transform.position - transform.position).normalized;
                direction.y = 1.8f;

                // Add force as impulse
                other.rigidbody.velocity = Vector3.zero;
                other.rigidbody.AddForce(hitForce * direction, ForceMode.Impulse);
            }
        }
    }
}