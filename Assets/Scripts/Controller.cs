using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public GameObject Dice;
    public GameObject Face;

    public Dice DiceObject;

    public TMP_InputField w;
    public float wValue;
    public TMP_InputField x;
    public float xValue;
    public TMP_InputField y;
    public float yValue;
    public TMP_InputField z;
    public float zValue;

    public TMP_InputField xp;
    public float xpValue;
    public TMP_InputField yp;
    public float ypValue;
    public TMP_InputField zp;
    public float zpValue;
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RotateFace()
    {
        wValue = float.Parse(w.text);
        xValue = float.Parse(x.text);
        yValue = float.Parse(y.text);
        zValue = float.Parse(z.text);

        //Face.transform.eulerAngles = new Vector3(xValue, yValue, zValue);

        Quaternion baseRot = Quaternion.Euler(-90f, 0, 0);
        Quaternion rot = baseRot * new Quaternion(xValue, -yValue, -zValue, wValue);
        Face.transform.rotation = rot;
    }

    public void PositionFace()
    {
        xpValue = float.Parse(xp.text);
        ypValue = float.Parse(yp.text);
        zpValue = float.Parse(zp.text);

        Face.transform.position = new Vector3(xpValue, ypValue, zpValue);
    }

    public void GenerateFacesController()
    {
        DiceObject.GenerateFaces();
    }
}
