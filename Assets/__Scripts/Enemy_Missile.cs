using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy_Missile : Enemy {

    public Transform target;
    public Rigidbody rb;
    public float force;
    public float rotationForce;
    private float x0; // The initial x value of pos
    private float birthTime;
    public float waveFrequency = 3;
    // sine wave width in meters
    public float waveWidth = 5;
    public float waveRotY = 40;

    void start()
    {
        rb = GetComponent<Rigidbody>();
       
        x0 = pos.x;

        birthTime = Time.time;

    }
    public override void Move() // private void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 direction = target.position - rb.position;
            direction.Normalize();
            Vector3 rotatioAmount = Vector3.Cross(transform.forward, direction);
            rb.angularVelocity = rotatioAmount * rotationForce;
            rb.velocity = transform.forward * force;
        }
    }
    public void FixUpdate()
    {
        Vector3 tempPos = pos;

        float age = Time.time - birthTime;
        float theta = Mathf.PI * 2 * age / waveFrequency;
        float sin = Mathf.Sin(theta);
        tempPos.x = x0 + waveWidth * sin;
        pos = tempPos;

        //rotate a bit about y
        Vector3 rot = new Vector3(0, sin * waveRotY, 0);
        this.transform.rotation = Quaternion.Euler(rot);

        // base.Move() still handles the movement down in y
        base.Move();

        // print (bndCheck.isOnScreen);
    }

    public void OnCollisionEnter (Collision collision)
    {
        Destroy(gameObject);
    }
      
}

