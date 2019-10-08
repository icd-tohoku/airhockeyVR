using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AirHockey.Fields
{
    public class PlayerPad : MonoBehaviour
    {
        private const string PuckTag = "Puck";
        
        [SerializeField] private float hitForce;
        [SerializeField] private bool enableKeyboardManipulation = false;

        private void Update()
        {
            if (enableKeyboardManipulation == false) return;
            
            var translate = new Vector3(
                Convert.ToInt32(Input.GetKey(KeyCode.RightArrow)) - Convert.ToInt32(Input.GetKey(KeyCode.LeftArrow)),
                Convert.ToInt32(Input.GetKey(KeyCode.DownArrow)) - Convert.ToInt32(Input.GetKey(KeyCode.UpArrow)),
                0
                );
            transform.Translate(translate * 0.05f);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag(PuckTag))
            {
                var direction = (other.transform.position - transform.position).normalized;
                direction.y = 1.8f;
                other.rigidbody.AddForce(hitForce * direction, ForceMode.Impulse);
            }
        }
    }
}