using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpreterv2 : MonoBehaviour
{
    public Terminalv2 terminalv2;
    public List<string> inputArray;
    public List<string> outputValue = new List<string>();

    void Start()
    {
        outputValue.Clear();
        inputArray.Clear();
        terminalv2 = FindObjectOfType<Terminalv2>(); // Find the Terminalv2 component dynamically
        inputArray = new List<string>();
        terminalv2.tmpInputField.onEndEdit.AddListener(HandleInputEndEdit); // Subscribe to onEndEdit event
    }

    void Update()
    {
        // You can put your update logic here if needed
    }

    private void HandleInputEndEdit(string userInput)
    {
        inputArray = new List<string>(userInput.Split());

        foreach (string word in inputArray)
        {
            Debug.Log(word);
        }
        // Process the inputArray as needed

        if (inputArray.Count > 0)
        {
            if (inputArray[0] == "Help" || inputArray[0] == "help")
            {
                outputValue.Add("help");
                outputValue.Add("help again");
            }
            else
            {
                outputValue.Add("error");
            }
        }
    }
}
