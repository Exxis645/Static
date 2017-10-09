using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Woofer : MonoBehaviour {

    public float HP;


    public LayerMask enemyMask;
    Rigidbody2D myBody;
    Transform myTrans;
    float myWidth;
    public float speed;
    public Transform groundChecker;
    public float groundRadius = 0.0002f;
    public LayerMask whatIsGround;
    public bool grounded;
    public Transform firePoint;
    public GameObject projectile;
    public float projectileSpeed;
    public float shotBuffer;
    public float fireRate;
    private float nextFire;
    private int scaler;
    public float shotLift;
    
    Animator anim;
    public bool drifting = false;
    public Transform driftCheck;
    public bool driftOut;

    // Use this for initialization
    void Start () {
        myTrans = this.transform;
        myBody = this.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Platform") {
            Flip();
        }
        if (col.gameObject.tag == "Projectile") {
            Destroy(col.gameObject);
            HP = HP-1;
        }
            

    }

    // Update is called once per frame
    void FixedUpdate () {

       

        if (HP <= 0) {
            Destroy(gameObject);
        }
    
        grounded = Physics2D.OverlapCircle(groundChecker.position, groundRadius, whatIsGround);
        driftOut = Physics2D.OverlapCircle(driftCheck.position, groundRadius, whatIsGround);

        if (!grounded) {

            anim.Play("drift");
            Vector2 myVel = myBody.velocity;
            myVel.x = -myTrans.right.x * (speed/2);
            myBody.velocity = myVel;


        }

        if (!driftOut) {
            anim.Play("Walk");
            Flip();
        }

        //always move forward
        if (grounded)
        {
            Vector2 myVel = myBody.velocity;
            myVel.x = -myTrans.right.x * speed;
            myBody.velocity = myVel;
        }

    }

   

    private void Update()
    {
        if (transform.eulerAngles.y == 180)
        {
            scaler = -1;
        }
        if (transform.eulerAngles.y == 0)
        {
            scaler = 1;
        }

      
        if (Time.time > nextFire)
        {
            GameObject bullet = (GameObject)Instantiate(
                                           projectile,
                                           transform.position,
                                           Quaternion.identity);

            // Adds velocity to the bullet

            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeed, shotLift);
            nextFire = Time.time + fireRate;
        }
    }

    void Flip() {
       
        Vector3 currRot = myTrans.eulerAngles;
        currRot.y += 180;
        myTrans.eulerAngles = currRot;
        projectileSpeed *= -1;

        Vector2 myVel = myBody.velocity;
        myVel.x = -myTrans.right.x * speed;
        myBody.velocity = myVel;


    }


}
