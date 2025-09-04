using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public void takeDamage(int damage)
    {
        GameManager.instance.playercurrenthp -= damage;
    }
}
