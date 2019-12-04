using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koch_PathMaker : MonoBehaviour
{
    private string axiom = "F";
    private float angle;
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
        rules.Add ('F', "F - F+ F−F−F + F");
        currentString = axiom;
        angle = 90.0f;
        length = 10f;

        Generate();
        Generate();
        Generate();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Generate()
    {
        string newString = "";

        char[] stringCharacters = currentString.ToCharArray();

        for (int i = 0; i < stringCharacters.Length; i++)
        {
            char currentCharacter = stringCharacters[i];

            //if its an f add the string
            if (rules.ContainsKey(currentCharacter))
            {
                //returns value for rule
                newString += rules[currentCharacter];
            }
            else
            {
                newString += currentCharacter.ToString();
            }
        }
        currentString = newString;
        Debug.Log(currentString);

        stringCharacters = currentString.ToCharArray();


        for (int i = 0; i < stringCharacters.Length; i++)
        {
            char currentCharacter = stringCharacters[i];

            if (currentCharacter == 'F')
            {
                Vector3 initialPosition = transform.position;
                transform.Translate(Vector3.forward * length);
                Debug.DrawLine(initialPosition, transform.position, Color.white, 1000000f, false);

            }
            else if (currentCharacter == '+')
            {
                transform.Rotate(Vector3.up * angle);
            }
            else if (currentCharacter == '-')
            {
                transform.Rotate(Vector3.up * -angle);
            }
          
        }
        Debug.Log(currentString);

    }
}
