using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteHolder : MonoBehaviour
{
    public GameObject LockedSprite, UnlockedSprite;

    public void Unlock()
    {
        LockedSprite.SetActive(false);
        UnlockedSprite.SetActive(true);
    }
}
