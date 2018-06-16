using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

	public enum Type
    {
        TRIANGLE, SQUARE, CIRCLE, DIAMOND
    }

    private readonly Type type;

    public Key(Type type)
    {
        this.type = type;
    }
}
