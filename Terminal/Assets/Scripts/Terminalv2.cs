using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Terminalv2 : MonoBehaviour
{
    //GameObjects of printable panels
    public GameObject valueUserInput;
    public GameObject valueUserOutput;
    public GameObject valueUserError;

    //Input value
    public TMP_InputField tmpInputField;

    //Transfoem of terminal and input field
    public Transform parentPanel;
    public RectTransform targetRectTransform;
    public RectTransform toAdjustRectTransform;

    //Script
    public Interpreterv2 interpreterv2;

    public List<string> outputArray = new List<string>();

    //Variables
    public float minHeight = 2160f;
    public int i = 1;

    void Start()
    {
        interpreterv2 = GetComponent<Interpreterv2>();
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
        if (targetRectTransform != null && toAdjustRectTransform != null)
        {
            float targetY = targetRectTransform.anchoredPosition.y;
            float newHeight = Mathf.Max(minHeight, -targetY + (60f*i));
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
            Printer(userInput);
        }
    }

    public void Printer(string userInput1)
    {
        GameObject inputPanel = Instantiate(valueUserInput, parentPanel);
        TextMeshProUGUI[] InputComponents = inputPanel.GetComponentsInChildren<TextMeshProUGUI>();
        InputComponents[1].text = userInput1;
        tmpInputField.text = "";

        outputArray = new List<string>(interpreterv2.inputArray);

        foreach (string word in outputArray)
        {
            Debug.Log(word);
        }
        GameObject outputPanel = Instantiate(valueUserOutput, parentPanel);
        TextMeshProUGUI[] OutputComponents = outputPanel.GetComponentsInChildren<TextMeshProUGUI>();
        OutputComponents[0].text = "userInput1";

        
        
        float targetY = targetRectTransform.anchoredPosition.y;

        // Calculate the new height with a minimum of minHeight
        float newHeight = Mathf.Max(minHeight, -targetY + 60f);
        toAdjustRectTransform.anchoredPosition = new Vector2(toAdjustRectTransform.anchoredPosition.x, (newHeight+60)/2);

        

        

        // Move the new panel to the end of the parentPanel's children
        valueUserInput.transform.SetAsLastSibling();

        // Force layout rebuild
        VerticalLayoutGroup verticalLayoutGroup = parentPanel.GetComponent<VerticalLayoutGroup>();
        if (verticalLayoutGroup != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(parentPanel.GetComponent<RectTransform>());
        }
    }
}