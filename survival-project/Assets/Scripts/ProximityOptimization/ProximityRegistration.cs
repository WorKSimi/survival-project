using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Unity.Netcode;

namespace WizardsCode.Optimization
{
    /// <summary>
    /// This component will cause the object to self register with the `ProximityActivationManager`
    /// in the scene when it awakes.
    /// </summary>
    public class ProximityRegistration : NetworkBehaviour
    {
        [SerializeField, Tooltip("The distance at which the object is considered nearby and thus should be enabled.")]
        float m_NearDistance = 25;
        [SerializeField, Tooltip("The distance at which the object is considered far away and thus should be disabled.")]
        float m_FarDistance = 50;

        /// <summary>
        /// The square of the distance at which the object is considered nearby and thus should be enabled.
        /// </summary>
        public float NearDistanceSqr
        {
            get; private set;
        }

        /// <summary>
        /// The square of the distance at which the object is considered far away and thus should be disabled.
        /// </summary>
        public float FarDistanceSqr
        {
            get; private set;
        }

        /// <summary>
        /// Indicates whether this object has been disabled due to proximity to the target or not.
        /// </summary>
        public bool DisabledByProximity
        {
            get; private set;
        }

        /// <summary>
        /// Disable this object because of its proximity check.
        /// </summary>
        public void Disable()
        {
            if (IsHost)
            {
                DisabledByProximity = true;
                gameObject.SetActive(false); //Turn object off
                TurnOffObjectClientRpc(); //Turn off object for all clients
            }
            else Debug.Log("Your not the host dawg");
        }

        /// <summary>
        /// Disable this object because of its proximity check.
        /// </summary>
        public void Enable()
        {
            if (IsHost)
            {
                DisabledByProximity = false;
                gameObject.SetActive(true); //Turn object on
                TurnOnObjectClientRpc();
            }
            else Debug.Log("Your not the host dawg");
        }
        private void Awake()
        {
            NearDistanceSqr = m_NearDistance * m_NearDistance;
            FarDistanceSqr = m_FarDistance * m_FarDistance;

            //OPTIMIZATION: make the ProximityActivationManager a singleton
            ProximityActivationManager manager = GameObject.FindObjectOfType<ProximityActivationManager>();
            manager.Add(this);
        }

        [ClientRpc]
        public void TurnOffObjectClientRpc()
        {
            DisabledByProximity = true;
            gameObject.SetActive(false); //Turn object off
        }

        [ClientRpc]
        public void TurnOnObjectClientRpc()
        {
            DisabledByProximity = false;
            gameObject.SetActive(true); //Turn object on
        }
    }
}
