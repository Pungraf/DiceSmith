using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Dice : MonoBehaviour
{
    // Floating rotation variables
    public float degreesPerSecond = 200.0f;
    public float degreesPerSecondX;
    public float degreesPerSecondY;
    public float degreesPerSecondZ;
    public float amplitude = 0.5f;
    public float frequency = 1f;


    // Rest of variables
    public Vector3 posOffset = new Vector3();
    public Vector3 tempPos = new Vector3();

    public List<string> namesList = new List<string>();
    public CraftController craftController;
    public List<FaceGeometry> Faces = new List<FaceGeometry>();
    public List<Face> FacesObjects = new List<Face>();
    public Rigidbody rb;
    public DiceStatus diceStatus;
    public bool diceKinematic = true;
    public bool rolled = false;

    public float choosedFaceHigh = float.MinValue;
    public Face choosedFace;
    public abstract void GenerateFaces();
    public abstract void GenerateFacePlace();
    public abstract void setChoosedFace();

}
