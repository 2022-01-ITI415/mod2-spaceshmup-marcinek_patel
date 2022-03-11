using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy_Missile : Enemy {

    public Transform target;
    public Rigidbody rb;
    public float turn;
    public float missileVelocity;
    void start()
    {
        rb = GetComponent<Rigidbody>();
        
    }
    public override void Move() // private void FixedUpdate()
    {
        rb.velocity = transform.up * missileVelocity;
        var targetRotation = Quaternion.LookRotation(target.position - transform.position);
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turn));
        base.Move();
    }
}
