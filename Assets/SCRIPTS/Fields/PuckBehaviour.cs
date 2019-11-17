using System;
using System.Collections;
using System.Collections.Generic;
using AirHockey.Data;
using AirHockey.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace AirHockey.Fields
{
    public class PuckBehaviour : MonoBehaviour
    {
        [SerializeField] private Vector3 initialForce;

        [SerializeField] private Transform positionLimit_XY_P = null;
        [SerializeField] private Transform positionLimit_XY_N = null;
        [SerializeField] private Transform centerThresholdMarker = null;
        
        private Rigidbody _rigidBody;
        private Vector3 _initialPosition;

        private void Awake()
        {
            EventManager.GameStartEvent += ActivateSelf;
            EventManager.GameFinishEvent += DeactivateSelf;
            EventManager.InitPuckEvent += Initialize;

            _initialPosition = transform.position;
            _rigidBody = gameObject.GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            Initialize();

            if (Display.displays.Length > 1)
                Display.displays[1].Activate();
        }

        private void FixedUpdate()
        {
            // Limit the position of the puck so that it does not go out of the table.
            var p = transform.position;
            var v = _rigidBody.velocity;
            
            if ((p.x > positionLimit_XY_P.position.x && v.x > 0) || (p.x < positionLimit_XY_N.position.x && v.x < 0))
            {
                _rigidBody.velocity = new Vector3(-v.x, v.y, v.z);
            }
            
            if ((p.z > positionLimit_XY_P.position.z && v.z > 0) || (p.z < positionLimit_XY_N.position.z && v.z < 0))
            {
                _rigidBody.velocity = new Vector3(v.x, v.y, -v.z);
            }
            
            // Add force when the puck is too slow and positioned at center of table.
            /*var distance = Mathf.Abs(transform.position.z);
            if (distance < centerThresholdMarker.position.z && _rigidBody.velocity.magnitude < 0.1f)
            {
                if (transform.position.z < 0)
                {
                    _rigidBody.AddForce(new Vector3(distance - transform.position.z, 0, 0));
                }
                else
                {
                    _rigidBody.AddForce(new Vector3(transform.position.z - distance, 0, 0));
                }
            }*/
        }

        private void OnDestroy()
        {
            EventManager.GameStartEvent -= ActivateSelf;
            EventManager.GameFinishEvent -= DeactivateSelf;
            EventManager.InitPuckEvent -= Initialize;
        }

        private void ActivateSelf()
        {
            gameObject.SetActive(true);
        }

        private void DeactivateSelf(PlayerType winner)
        {
            gameObject.SetActive(false);
        }

        private void OnCollisionEnter(Collision other)
        {
            // When the puck collide with goal object, change the score.
            GoalEntity entity;
            if (other.transform.TryGetComponent(out entity))
            {
                switch (entity.GoalOwner)
                {
                    case PlayerType.Desktop: GameData.AddScore(PlayerType.VR); break;
                    case PlayerType.VR: GameData.AddScore(PlayerType.Desktop); break;
                    default: throw new Exception($"[PuckCollision] Unknown player: {entity.GoalOwner}");
                }
                
                entity.GoalSound();

                Initialize(true);
            }
        }

        private void Initialize(bool inverseInitialForceDirection = false)
        {
            // Set puck to initial position and add weak force
            transform.position = _initialPosition;
            initialForce *= inverseInitialForceDirection ? -1 : 1;
            
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.AddForce(initialForce);
        }
    }
}
