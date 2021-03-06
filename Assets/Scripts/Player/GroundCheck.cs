﻿using UnityEngine;

//Written by Mandy

public class GroundCheck : MonoBehaviour
{
    public bool IsGrounded = false;

    private void OnCollisionEnter2D(Collision2D collision) => IsGrounded = true;

    private void OnCollisionExit2D(Collision2D collision) => IsGrounded = false;
}
