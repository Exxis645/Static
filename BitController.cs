using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class BitController : MonoBehaviour
    {
        public Transform target;
        public GameObject projectile;
        public Transform firePoint;
        public float damping = 1;
        public float lookAheadFactor = 3;
        public float lookAheadReturnSpeed = 0.5f;
        public float lookAheadMoveThreshold = 0.1f;
        public float projectileSpeed;
        public float shotBuffer;
        public float fireRate;
        public Vector2 playerVel;

        public AudioClip pewPew;
        public AudioSource musicPlayer;
        
        
        

        private float m_OffsetZ;
        private Vector3 m_LastTargetPosition;
        private Vector3 m_CurrentVelocity;
        private Vector3 m_LookAheadPos;
        private float nextFire;

        // Use this for initialization
        private void Start()
        {
            m_LastTargetPosition = target.position;
            m_OffsetZ = (transform.position - target.position).z;
            transform.parent = null;
            musicPlayer.clip = pewPew;
            


        }


        // Update is called once per frame
        private void Update()
        {
            playerVel = GameObject.Find("Player").GetComponent<Rigidbody2D>().velocity;
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            //fire at mouse direction
            if (Input.GetButton("Fire1") && Time.time > nextFire) {
                Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                Vector2 direction = (Vector2)((worldMousePos - transform.position));
                direction.Normalize();

               

                // Creates the bullet locally
                GameObject bullet = (GameObject)Instantiate(
                                        projectile,
                                        transform.position + (Vector3)(direction * shotBuffer),
                                        Quaternion.identity);

                // Adds velocity to the bullet

                musicPlayer.Play();

                bullet.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed + playerVel;
                nextFire = Time.time + fireRate;
            }
            // only update lookahead pos if accelerating or changed direction
            float xMoveDelta = (target.position - m_LastTargetPosition).x;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                m_LookAheadPos = lookAheadFactor*Vector3.right*Mathf.Sign(xMoveDelta);
            }
            else
            {
                m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime*lookAheadReturnSpeed);
            }

            Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.forward*m_OffsetZ;
            Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

            transform.position = newPos;

            m_LastTargetPosition = target.position;
        }
    }
}
