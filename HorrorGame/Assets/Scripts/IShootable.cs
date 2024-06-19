using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//IShootable ist ein Interface, das unterstützt werden kann, wenn GameObjects auf Schüsse des Players reagieren sollen
public interface IShootable
{
    public void GetShot(Vector3 knockback = default(Vector3));
}
