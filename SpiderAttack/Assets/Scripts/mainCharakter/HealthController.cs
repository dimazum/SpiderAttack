using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace mainCharakter
{
    public class HealthController : MonoBehaviour
    {




        public List<Vector3> tileWorldLocations;


        public bool IsAlive = true;
        private float _decelerationTolerance = 12.0f;
        public Vector3 Velocity;
        private Rigidbody2D rb;

        public Slider slider;

        private void Awake()
        {
            slider.value = 1;
            rb = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            if (IsAlive)
            {
                var dist = Vector3.Distance(rb.velocity, Velocity);
                IsAlive = Vector3.Distance(rb.velocity, Velocity) < _decelerationTolerance;

                if (!IsAlive)
                {
                    slider.value -= (dist/100) ;
                    IsAlive = true;
                }
                Velocity = rb.velocity;

                //Debug.Log(dist);
            }
        }
    }
}
