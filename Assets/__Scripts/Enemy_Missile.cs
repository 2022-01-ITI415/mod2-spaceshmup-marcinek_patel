using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy_Missile : Enemy {

    // public Transform target;
    // public Rigidbody rb;
    public float missileVelocity;
    // public float rotateSpeed = 30f;
    public float birthTime;
    
    void start()
    {
        // rb = GetComponent<Rigidbody>();
        birthTime = Time.time;
        missileVelocity = Enemy.speed;
    }

    public override void Move() // private void FixedUpdate()
    {
        float step = Time.deltaTime * missileVelocity;
        var target = GameObject.Find("_Hero");
        var toTarget  = (target.transform.position - transform.position).normalized;
        // if (toTarget != Vector3.zero) {
        var targetRotation = Quaternion.LookRotation(toTarget);
        targetRotation *= Quaternion.Euler(90, 90, 20);
        // transform.rotation = targetRotation; 
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
        // base.Move();
    }
}
