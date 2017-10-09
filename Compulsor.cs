using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compulsor : MonoBehaviour {

    public float HP;


    public LayerMask enemyMask;
    Rigidbody2D myBody;
    Transform myTrans;
    float myWidth;
    public float speed;
    public Transform groundChecker;
    public float groundRadius = 0.0002f;
    public LayerMask whatIsGround;
    private bool grounded;

    // Use this for initialization
    void Start () {
        myTrans = this.transform;
        myBody = this.GetComponent<Rigidbody2D>();
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

        if (!grounded) {
            Flip();

        }

        //always move forward

        Vector2 myVel = myBody.velocity;
        myVel.x = -myTrans.right.x *speed;
        myBody.velocity = myVel;


    }

    void Flip() {
        Vector3 currRot = myTrans.eulerAngles;
        currRot.y += 180;
        myTrans.eulerAngles = currRot;
        
    }
}
