using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Sprite Bone1Sprite;
    public Sprite Bone2Sprite;
    public Sprite Bone3Sprite;
    public Sprite Blood1Sprite;
    public Sprite Blood2Sprite;
    public Sprite Blood3Sprite;
    public Sprite Night1Sprite;
    public Sprite Night2Sprite;
    public Sprite Night3Sprite;
}
