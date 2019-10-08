using System;
using System.Collections;
using System.Collections.Generic;
using AirHockey.Data;
using AirHockey.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace AirHockey.Fields
{
    public class PuckCollision : MonoBehaviour
    {
        [SerializeField] private Vector3 initialForce = new Vector3(0, 0, -0.5f);
        
        private Rigidbody _rigidBody;
        private Vector3 _initialPosition;

        private void Start()
        {
            _initialPosition = transform.position;
            _rigidBody = gameObject.GetComponent<Rigidbody>();
            _rigidBody.AddForce(initialForce);

            if (Display.displays.Length > 1)
                Display.displays[1].Activate();
        }

        private void FixedUpdate()
        {
            Vector3 pos = _rigidBody.position;
            if (pos.z > -1.3 && pos.z < 0)
            {
                _rigidBody.AddForce(new Vector3(0, 0, -0.02f));
            }
            else if (pos.z < 1.3 && pos.z > 0)
            {
                _rigidBody.AddForce(new Vector3(0, 0, 0.02f));
            }

            if (pos.z > 2.3)
            {
                _rigidBody.AddForce(new Vector3(0, 0, -10f));
            }
            else if (pos.z < -2.3)
            {
                _rigidBody.AddForce(new Vector3(0, 0, 10f));
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            GoalEntity entity;
            if (other.transform.TryGetComponent(out entity))
            {
                switch (entity.GoalOwner)
                {
                    case PlayerType.Player1: GameData.Instance.AddScore(PlayerType.Player2); break;
                    case PlayerType.Player2: GameData.Instance.AddScore(PlayerType.Player1); break;
                    case PlayerType.None: Debug.LogWarning($"[PuckCollision] Goal owner has not set in gameObject: {entity.name}"); break;
                    default: throw new Exception($"[PuckCollision] Unknown player: {entity.GoalOwner}");
                }

                // Set puck to initial position and add weak force
                transform.position = _initialPosition;
                _rigidBody.AddForce(initialForce);
            }
            
            /*if (other.gameObject.name == "AirHockeyPad" || other.gameObject.name == "AirHockeyPad (1)")
            {
                Vector3 dir = transform.position - (other.contacts[0].point + new Vector3(0, transform.position.y, 0));
                _rigidBody.AddForce(dir * hitForce);
            }
            /*else if (other.gameObject.name == "AirHockeyTable")
            {
                Vector3 dir = transform.position -
                              (new Vector3(other.contacts[0].point.x, transform.position.y, other.contacts[0].point.z));
                dir.z = -dir.z;
                dir.y = -dir.y;
                //puckBody.AddForce(puckBody.velocity.normalized * 5f);
                Debug.Log("ca tape la table");
            }*/
        }
    }
}
