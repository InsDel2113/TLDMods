using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLDLoader;
using UnityEngine;

namespace HelicopterMod
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(carscript))]
    [RequireComponent(typeof(seatscript))]
    class HelicopterScript : MonoBehaviour
    {
        public float HelicopterForce = 2800f;
        public float HelicopterTorque = 200f;
        public float HelicopterTurnSpeed = 350;
        public int rotorSpinSpeed = 15;

        public Rigidbody rb;
        public carscript car;
        public seatscript seat;
        public AudioSource helisource;
        public bool helicopterGrounded = true;
        public bool initialized = false;

        void Setup()
        {
            if ( gameObject.transform.GetChild(0).name != "SeatPos" )
            {
               QuickInit();
               return;
            }
            initialized = true;
            // get components, cached calls to save cpu time
            rb = gameObject.GetComponent<Rigidbody>();
            car = gameObject.GetComponent<carscript>();
            seat = gameObject.GetComponent<seatscript>();

            // rigidbody setup
            rb.mass = 5;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            // seat setup
            seat.sitPos = gameObject.transform.GetChild(0);
            seat.inUse = false;
            seat.drivingActive = true;
            seat.RB = rb;
            seat.limitLessRot = true; // unlock camera in seat
            car.driverSeat = seat;

            // scaling and position
            gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f); // just a bit bigger

            // audio setup
            helisource = gameObject.GetComponent<AudioSource>();
        }

        public void Start()
        {
            Setup();
        }
        public void DoEffects()
        {
            helisource.PlayOneShot(helisource.clip, 0.20f); // play sound
            gameObject.transform.GetChild(1).transform.Rotate(0, rotorSpinSpeed, 0); // spin rotors
        }
        public void Update()
        {
            if (!initialized)
                return;
            if (Physics.Raycast(seat.transform.position, transform.TransformDirection(-Vector3.up), 1.0f))
            {
                helicopterGrounded = true;
            }
            else
            {
                helicopterGrounded = false;
            }
            if (rb.velocity.magnitude > 0.85f && !helicopterGrounded)
            {
                DoEffects();
            }
            else
            {
                if (!Input.GetKey(KeyCode.Space))
                {
                    helisource.Stop();
                }
            }
        }
        public void FixedUpdate()
        {
            if (!initialized)
                return;
            if (gameObject && seat.inUse)
            {
                DoMovement();
            }
        }

        public void OnGUI()
        {

        }

        public GameObject QuickInit()
        {
            HelicopterMod newHeliMod = new HelicopterMod();
            newHeliMod.OnLoad();
            GameObject obj = UnityEngine.Object.Instantiate(newHeliMod.obj);
            obj.AddComponent<HelicopterScript>();
            return obj;
        }

        private void DoMovement()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                rb.AddForce(gameObject.transform.up * HelicopterForce * Time.deltaTime);
                DoEffects(); // just in case the ground check is true but user still pressing space
            }
            if (Input.GetKey(KeyCode.W))
            {
                rb.AddTorque(gameObject.transform.forward * -HelicopterTorque * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S))
            {
                rb.AddTorque(gameObject.transform.forward * HelicopterTorque * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                rb.AddTorque(gameObject.transform.right * -HelicopterTorque * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.A))
            {
                rb.AddTorque(gameObject.transform.right * HelicopterTorque * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.LeftControl))
            {
                rb.AddTorque(-gameObject.transform.up * HelicopterTurnSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                rb.AddTorque(gameObject.transform.up * HelicopterTurnSpeed * Time.deltaTime);
            }
        }
    }
}
