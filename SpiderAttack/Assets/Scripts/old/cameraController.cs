using UnityEngine;
using System.Collections;

public class cameraController : MonoBehaviour
{
    public float smooth = 0.1f;//скорость сопровождения
    public GameObject target;//объект сопровождения
    public GameObject Trees;
    public GameObject Forest;
    public GameObject Field1;
    public GameObject Field2;
    public Animator animator;

   



    public float speedField1 = 1;
    public float speedField1Vertical = 1;
    public float xPosField1;
    public float yPosField1;
    public float zPosField1;

    public float speedField2;
    public float speedField2Vertical = 1;
    public float xPosField2;
    public float yPosField2;
    public float zPosField2;


    public float speedForest = 1;
    public float speedForestVertical = 1;
    public float xPosForest;
    public float yPosForest;
    public float zPosForest;

    public float speedTrees = 1;
    public float speedTreesVertical = 1;
    public float xPosLes;
    public float yPosLes;
    public float zPosLes;

    public float xPos;
    public float yPos;
    public float zPos;


    private void Start()
    {
        animator = GetComponent<Animator>();

    }
    void LateUpdate()
    {
        transform.position = new Vector3(target.transform.position.x + xPos, target.transform.position.y + yPos, target.transform.position.z + zPos);
        Trees.transform.position = new Vector3(target.transform.position.x * speedTrees + xPosLes, target.transform.position.y * speedTreesVertical + yPosLes, Trees.transform.position.z);
        Forest.transform.position = new Vector3(target.transform.position.x * speedForest + xPosForest, target.transform.position.y * speedForestVertical + yPosForest, Forest.transform.position.z);
        Field1.transform.position = new Vector3(target.transform.position.x * speedField1 + xPosField1, target.transform.position.y * speedField1Vertical + yPosField1, Field1.transform.position.z);
        Field2.transform.position = new Vector3(target.transform.position.x * speedForest + xPosField2, target.transform.position.y * speedField2Vertical + yPosField2, Field2.transform.position.z);
      

        if (Input.GetKey(KeyCode.R)) animator.Play("Day");
        if (Input.GetKey(KeyCode.T)) animator.Play("Night");
    }
}

