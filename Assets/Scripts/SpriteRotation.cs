﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRotation : MonoBehaviour {

    public Space rotationSpace = Space.Self;
    public Vector3 rotationSpeed;

	void Update ()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime, rotationSpace);	
	}
}
