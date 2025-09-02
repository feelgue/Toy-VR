using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int currentHp;
    public int maxHp;
    
    public void takeDamage(int damage)
    {
        currentHp -= damage;
    }
}
