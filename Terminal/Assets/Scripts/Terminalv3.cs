using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Terminalv3 : MonoBehaviour
{
    public GameObject valueUserInput;
    public GameObject valueUserOutput;
    public GameObject valueUserError;

    public TMP_InputField tmpInputField;

    public Transform parentPanel;
    public RectTransform targetRectTransform;
    public RectTransform toAdjustRectTransform;

    public List<string> inputArray;
    public List<string> outputArray = new List<string>();
    public List<string> lsValues = new List<string> { "A", "B", "C", "D", "E" };

    public float minHeight = 2160f;
    public int i = 1;
    public string username = "user";

    void Start()
    {
        inputArray = new List<string>();
        outputArray = new List<string>();

        tmpInputField.onEndEdit.AddListener(HandleInputEndEdit);
        tmpInputField.Select();
        tmpInputField.ActivateInputField();

        VerticalLayoutGroup verticalLayoutGroup = parentPanel.GetComponent<VerticalLayoutGroup>();
        if (verticalLayoutGroup != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(parentPanel.GetComponent<RectTransform>());
        }
    }

    void Update()
    {
        //i = parentPanel.childCount;
        if (targetRectTransform != null && toAdjustRectTransform != null)
        {
            float targetY = targetRectTransform.anchoredPosition.y;
            float newHeight = Mathf.Max(minHeight, -targetY + (60f * i));
            toAdjustRectTransform.sizeDelta = new Vector2(toAdjustRectTransform.sizeDelta.x, newHeight);
        }
        else
        {
            Debug.LogError("Target RectTransforms are not assigned!");
        }
    }

    private void FixedUpdate()
    {
        tmpInputField.Select();
        tmpInputField.ActivateInputField();
    }

    private void HandleInputEndEdit(string userInput)
    {
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            ExecuteCommand(userInput);
        }
    }

    private void ExecuteCommand(string userInput)
    {
        Printer(userInput);
    }

    private void Printer(string userInput)
    {
        GameObject inputPanel = Instantiate(valueUserInput, parentPanel);
        inputPanel.tag = "InstantiatedPanel";
        TextMeshProUGUI[] InputComponents = inputPanel.GetComponentsInChildren<TextMeshProUGUI>();
        InputComponents[0].text = $"TS {username}~$ ";
        InputComponents[1].text = userInput;
        tmpInputField.text = "";

        inputArray = new List<string>(userInput.Split());

        if (inputArray.Count > 0)
        {
            if (inputArray[0] == "Help" || inputArray[0] == "help")
            {
                outputArray.Clear();
                outputArray.Add("You can type 'help' to get a list of commands."); //Just Print //Done
                outputArray.Add("You can type 'cd' to change directories."); //Do Something
                outputArray.Add("You can type 'ls' to list the contents of a directory."); //Just Print //Done
                outputArray.Add("You can type 'cat' to read the contents of a file."); //Do Something
                outputArray.Add("You can type 'echo' to print text to the screen."); //Just Print //Done
                outputArray.Add("You can type 'pwd' to print the current working directory."); //Just Print
                outputArray.Add("You can type 'mkdir' to create a new directory."); //Do Something //Done
                outputArray.Add("You can type 'cp' to copy a file or directory."); //Do Something
                outputArray.Add("You can type 'rm' to remove a file or directory."); //Do Something
                outputArray.Add("You can type 'rmdir' to remove a directory."); //Do Something
                outputArray.Add("You can type 'touch' to create a new file."); //Do Something
                outputArray.Add("You can type 'clear' to clear the screen."); //Do Something //Done
                outputArray.Add("You can type 'exit' to exit the terminal."); //Do Something
            }
            else if (inputArray[0] == "ls")
            {
                outputArray.Clear();
                foreach (string word in lsValues)
                {
                    outputArray.Add(word);
                }
            }
            else if (inputArray[0] == "echo" && inputArray.Count > 1)
            {
                outputArray.Clear();
                outputArray.Add(inputArray[1]);
            }
            else if (inputArray[0] == "echo" && inputArray.Count <= 1)
            {
                outputArray.Clear();
                outputArray.Add("echo: missing operand");
            }
            else if (inputArray[0] == "mkdir" && inputArray.Count > 1)
            {
                outputArray.Clear();
                lsValues.Add(inputArray[1]);
                outputArray.Add($"Directory {inputArray[1]} created");
            }
            else if (inputArray[0] == "mkdir" && inputArray.Count <= 1){
                outputArray.Clear();
                outputArray.Add("mkdir: missing operand");
            }
            else if (inputArray[0] == "clear")
            {
                ClearTerminal();
            }
            else
            {
                outputArray.Clear();
                GameObject errorPanel = Instantiate(valueUserError, parentPanel);
                errorPanel.tag = "InstantiatedPanel";
            }
        }

        foreach (string word in outputArray)
        {
            GameObject outputPanel = Instantiate(valueUserOutput, parentPanel);
            outputPanel.tag = "InstantiatedPanel";
            TextMeshProUGUI[] OutputComponents = outputPanel.GetComponentsInChildren<TextMeshProUGUI>();
            OutputComponents[0].text = word;
        }

        AdjustHeight();
    }

    private void ClearTerminal()
    {
        // Destroy only the instantiated input and output panels
        foreach (Transform child in parentPanel.transform)
        {
            if (child.CompareTag("InstantiatedPanel"))
            {
                Destroy(child.gameObject);
            }
        }

        // Reset the input and output arrays
        inputArray.Clear();
        outputArray.Clear();

        // Adjust the height after clearing
        AdjustHeight();
    }


    private void AdjustHeight()
    {
        float targetY = targetRectTransform.anchoredPosition.y;
        float newHeight = Mathf.Max(minHeight, -targetY + 60f);
        toAdjustRectTransform.anchoredPosition = new Vector2(toAdjustRectTransform.anchoredPosition.x, (newHeight + 60) / 2);

        valueUserInput.transform.SetAsLastSibling();

        VerticalLayoutGroup verticalLayoutGroup = parentPanel.GetComponent<VerticalLayoutGroup>();
        if (verticalLayoutGroup != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(parentPanel.GetComponent<RectTransform>());
        }
    }
}
