using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;


public class LSystemsGenerator : MonoBehaviour
{
    [SerializeField]
    public bool Tree;
    [SerializeField]
    GameObject branch;
    [SerializeField]
    GameObject leaf;
    

    private string axiom;
    private float angle;
    private bool isGenerating = false;
    private string currentString;
    private Dictionary<char, string> rules = new Dictionary<char, string>();
    private Stack<TransformInfo> transformStack = new Stack<TransformInfo>();

    


    private float length;

    // Start is called before the first frame update
    void Start()
    {

        ///F = Move forward
        ///+ = turn right by 25%
        ///- = turn left by 25%
        ///[=save position
        ///]= go back to saved position
        axiom = "F" + "L";
        rules.Add('F', "FF(+[(F+FL-)FL-)FL]-)[-F)F+(FL+(FL]");
        angle = 25.0f;
        length = 10f;
       

        
        currentString = axiom;

        StartCoroutine(GenerateLSystem());

    } 

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator GenerateLSystem()
    {
        int count = 0;
        
        while (count < 5)
        {
            if(!isGenerating)
            {
                isGenerating = true;
                StartCoroutine(Generate());
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }

        }
    }

    IEnumerator Generate()
    {
        length /= 2f;
        
        string newString = "";

        char[] stringCharacters = currentString.ToCharArray();

        for (int i = 0; i < stringCharacters.Length; i++)
        {
            char currentCharacter = stringCharacters[i];

            //if its an f add the string
            if(rules.ContainsKey(currentCharacter))
            {
                //returns value for rule
                newString += rules[currentCharacter];
            }else
            {
                newString += currentCharacter.ToString();
            }
        }
        currentString = newString;
        Debug.Log(currentString);

        stringCharacters = currentString.ToCharArray();

    
        for(int i = 0; i < stringCharacters.Length; i++)
        {
            char currentCharacter = stringCharacters[i];

            if (currentCharacter == 'F')
            {
                Vector3 initialPosition = transform.position;
                transform.Translate(Vector3.up * length);
                Vector3 branchScale = new Vector3(1f, initialPosition[1] - transform.position[1], 1f);
                Vector3 branchOrigin = new Vector3(transform.position[0] + initialPosition[0], transform.position[1] + initialPosition[1], transform.position[2] + initialPosition[2]) / 2;
                branch.transform.localScale = branchScale;
                if (Tree)
                {
                    Instantiate(branch, branchOrigin, transform.rotation);
                }else
                {
                    Debug.DrawLine(initialPosition, transform.position, Color.white, 1000000f, false);
                }
                yield return null;
            } else if (currentCharacter == '+')
            {
                transform.Rotate(Vector3.right * angle);

            } else if(currentCharacter == 'L') 
            {
                if(Tree)
                {
                    Instantiate(leaf, transform.position, transform.rotation);
                }
            } else if (currentCharacter == '-')
            {
                transform.Rotate(Vector3.right * -angle);
            } else if (currentCharacter == ')')
            {
                transform.Rotate(Vector3.forward * (angle));
            }
            else if (currentCharacter == '(')
            {
                transform.Rotate(Vector3.forward * -angle);
            } else if (currentCharacter == '[')
            {
                TransformInfo ti = new TransformInfo();
                ti.position = transform.position;
                ti.rotation = transform.rotation;
                transformStack.Push(ti);
            } else if (currentCharacter == ']')
            {
                TransformInfo ti = transformStack.Pop();
                transform.position = ti.position;
                transform.rotation = ti.rotation;
            }
        }
        Debug.Log(currentString);

        isGenerating = false;

    }
}
