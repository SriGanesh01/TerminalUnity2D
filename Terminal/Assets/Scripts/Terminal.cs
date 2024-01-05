using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class Terminal : MonoBehaviour
{
    public GameObject valueUserInput;
    public GameObject valueUserOutput;
    public GameObject valueUserOutput2;
    public Transform parentPanel;
    public TMP_InputField tmpInputField;
    public RectTransform targetRectTransform;
    public RectTransform toAdjustRectTransform;
    public float minHeight = 2160f;
    private int i = 1;

    private Interpreter interpreter; // Declare interpreter at the class level

    void Start()
    {
        interpreter = GetComponent<Interpreter>();

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
        i = interpreter.WordListLength;

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
        Printer(userInput, interpreter.cats); 
    }

    private void Printer(string userInput, List<string> cats)
    {
        foreach (string catText in cats)
        {
            GameObject newPanel2 = Instantiate(valueUserOutput2, parentPanel);
            TextMeshProUGUI[] textComponents2 = newPanel2.GetComponentsInChildren<TextMeshProUGUI>();
            textComponents2[0].text = catText;
        }

        GameObject newPanel = Instantiate(valueUserInput, parentPanel);
        TextMeshProUGUI[] textComponents = newPanel.GetComponentsInChildren<TextMeshProUGUI>();
        textComponents[1].text = userInput;

        float targetY = targetRectTransform.anchoredPosition.y;
        float newHeight = Mathf.Max(minHeight, -targetY + 60f);
        toAdjustRectTransform.anchoredPosition = new Vector2(toAdjustRectTransform.anchoredPosition.x, (newHeight + 60) / 2);

        tmpInputField.text = "";
        valueUserInput.transform.SetAsLastSibling();

        VerticalLayoutGroup verticalLayoutGroup = parentPanel.GetComponent<VerticalLayoutGroup>();
        if (verticalLayoutGroup != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(parentPanel.GetComponent<RectTransform>());
        }
    }

}