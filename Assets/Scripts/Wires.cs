using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wires : MonoBehaviour
{
    public static Wires instance;
    [SerializeField] private CompositeCollider2D[] wires;
    void Start()
    {
        instance = this;
    }

    public void ChangeWire(int oldWire, int newWire)
	{
        wires[oldWire].isTrigger = true;
        wires[newWire].isTrigger = false;
    }

    public void TurnOffAll()
	{
        foreach (var wire in wires)
            wire.isTrigger = true;
	}
}
