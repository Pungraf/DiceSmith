using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TwentyFace : Dice
{
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        GenerateFacePlace();
        choosedFaceHigh = float.MinValue;
        if(SceneManager.GetActiveScene().name == "Smith")
        {
            diceKinematic = false;
        }
        else
        {
            diceKinematic = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rolled = false;
        posOffset = transform.position;
        degreesPerSecondX = RandomRotation();
        degreesPerSecondY = RandomRotation();
        degreesPerSecondZ = RandomRotation();
    }

    // Update is called once per frame
    void Update()
    {
        if (diceKinematic)
        {
            FloatingMovemen();
        }
    }


    public override void GenerateFacePlace()
    {
        Faces.Add(new FaceGeometry(0.0000000f, 0.0695000f, 0.0000000f, 0.000000f, 0.000000f, 0.000000f, 1.000000f)); //1
        Faces.Add(new FaceGeometry(0.0401059f, -0.0517777f, 0.0231524f, 0.809045f, 0.467047f, -0.356810f, 0.000000f)); //2
        Faces.Add(new FaceGeometry(-0.0247868f, 0.0231583f, -0.0606198f, 0.000043f, 0.577331f, 0.755795f, 0.308971f)); //3
        Faces.Add(new FaceGeometry(-0.0247868f, -0.0231583f, 0.0606198f, -0.308972f, 0.755795f, -0.577331f, 0.000042f)); //4
        Faces.Add(new FaceGeometry(0.0401059f, 0.0231524f, 0.0517777f, -0.000010f, 0.577368f, -0.645485f, -0.499997f)); //5
        Faces.Add(new FaceGeometry(-0.0648927f, -0.0231547f, -0.0088457f, -0.499990f, 0.645503f, -0.288661f, 0.500012f)); //6
        Faces.Add(new FaceGeometry(0.0401059f, 0.0517777f, -0.0231524f, 0.309018f, -0.178385f, -0.934178f, -0.000013f)); //7
        Faces.Add(new FaceGeometry(0.0000000f, -0.0517741f, -0.0463130f, 0.809007f, 0.467086f, 0.178425f, -0.309038f)); //9
        Faces.Add(new FaceGeometry(-0.0648927f, 0.0231547f, 0.0088457f, 0.000019f, 0.577354f, -0.110253f, 0.809017f)); //9
        Faces.Add(new FaceGeometry(0.0401059f, -0.0231524f, -0.0517777f, -0.309002f, -0.755756f, -0.288697f, 0.500008f)); //10
        Faces.Add(new FaceGeometry(-0.0401059f, 0.0231524f, 0.0517777f, -0.000010f, -0.577368f, 0.645485f, -0.499998f)); //11
        Faces.Add(new FaceGeometry(0.0648927f, -0.0231547f, -0.0088457f, 0.309034f, -0.755754f, -0.288698f, 0.499991f)); //12
        Faces.Add(new FaceGeometry(0.0000000f, 0.0517741f, 0.0463130f, -0.000002f, 0.356847f, -0.934165f, -0.000004f)); //13
        Faces.Add(new FaceGeometry(-0.0401059f, -0.0517777f, 0.0231524f, -0.809024f, -0.467087f, -0.178422f, -0.308997f)); //14
        Faces.Add(new FaceGeometry(0.0648927f, 0.0231547f, 0.0088457f, 0.000025f, -0.577354f, 0.110244f, 0.809019f)); //15
        Faces.Add(new FaceGeometry(-0.0401059f, -0.0231524f, -0.0517777f, 0.309009f, -0.755753f, -0.288692f, -0.500011f)); //16
        Faces.Add(new FaceGeometry(0.0247868f, 0.0231583f, -0.0606198f, 0.499989f, -0.288656f, -0.645504f, -0.500017f)); //17
        Faces.Add(new FaceGeometry(0.0247868f, -0.0231583f, 0.0606198f, -0.500018f, 0.645503f, -0.288656f, -0.499989f)); //18
        Faces.Add(new FaceGeometry(-0.0401059f, 0.0517777f, -0.0231524f, 0.308997f, -0.178422f, 0.467086f, -0.809025f)); //19
        Faces.Add(new FaceGeometry(0.0000000f, -0.0694654f, -0.0000037f, 0.499997f, 0.866030f, 0.000023f, -0.000013f)); //20
    }

    float RandomRotation()
    {
        float random = Random.Range(-degreesPerSecond, degreesPerSecond);

        return random;
    }

    void FloatingMovemen()
    {
        // Spin object around Y-Axis
        transform.Rotate(new Vector3(Time.deltaTime * degreesPerSecondX, Time.deltaTime * degreesPerSecondY, Time.deltaTime * degreesPerSecondZ), Space.World);

        // Float up/down with a Sin()
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;
    }

    public override void GenerateFaces()
    {
        GameObject currentFace;
        foreach (Face face in FacesObjects)
        {
            Destroy(face.gameObject);
        }
        FacesObjects.Clear();
        for (int i = 0; i < Faces.Count; i++)
        {
            if(namesList[i] == "" || namesList[i] == "Empty")
            {
                currentFace = (GameObject)Instantiate(Resources.Load("20Faces/Empty"), this.gameObject.transform);
            }
            else
            {
                currentFace = (GameObject)Instantiate(Resources.Load("20Faces/" + namesList[i]), this.gameObject.transform);
            }
            FacesObjects.Add(currentFace.GetComponent<Face>());

            Face face = currentFace.GetComponent<Face>();
            face.FaceIndex = i;
            face.FaceName = namesList[i];

            currentFace.transform.position = new Vector3(Faces[i].x, Faces[i].y, Faces[i].z);

            Quaternion baseRot = Quaternion.Euler(-90f, 0, 0);
            Quaternion rot = baseRot * new Quaternion(Faces[i].rx, -Faces[i].ry, -Faces[i].rz, Faces[i].rw);
            currentFace.transform.rotation = rot;
        }
    }

    

    public override void setChoosedFace()
    {
        foreach (Face face in FacesObjects)
        {
            face.blinking = false;
            face.shadow = false;
            face.EndBlinking();
            if (face.gameObject.transform.position.y >= choosedFaceHigh)
            {
                choosedFace = face;
                choosedFaceHigh = face.gameObject.transform.position.y;
            }
        }
        foreach (Face face in FacesObjects)
        {
            if (choosedFace == face)
            {
                face.blinking = true;
            }
            else
            {
                face.shadow = true;
            }
        }
    }
}
