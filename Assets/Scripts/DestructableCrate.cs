using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableCrate : MonoBehaviour
{
    public void Damage() {
        Destroy(gameObject);
    }
}
