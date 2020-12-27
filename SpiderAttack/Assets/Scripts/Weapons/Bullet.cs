using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

public abstract class Bullet : MonoBehaviour
{
    public int Damage = 100;
    public BulletType bulletType;
    public ItemCategory ItemCategory;

}
