using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wires : MonoBehaviour
{
    public static Wires instance;
    [SerializeField] private Collider2D[] wires;

    void Start()
    {
        instance = this;
    }

    public void ChangeWire(int oldWire, int newWire)
	{
        wires[oldWire].enabled = false;
        wires[newWire].enabled = true;
    }

    public void TurnOffAll()
	{
        foreach (var wire in wires)
            wire.enabled = false;
	}
    
}
