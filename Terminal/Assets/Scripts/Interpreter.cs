using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interpreter : MonoBehaviour
{
    public TMP_InputField tmpInput;
    private List<string> wordList = new List<string>();
    public List<string> cats = new List<string>();

    public int WordListLength => CountNonEmptyWords();

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        wordList.Clear();
        cats.Clear();

        if (tmpInput != null)
        {
            tmpInput.onEndEdit.AddListener(HandleInputEndEdit);
        }
        else
        {
            Debug.LogError("TMP_InputField is not assigned!");
        }
    }

    private void HandleInputEndEdit(string userInput)
    {
        InterpretCondition(userInput.Split());
    }

    private void InterpretCondition(string[] inputArray)
    {
        if (tmpInput == null)
        {
            Debug.LogError("TMP_InputField is not assigned!");
            return;
        }

        ClearLists();

        foreach (string word in inputArray)
        {
            if (!string.IsNullOrWhiteSpace(word))
            {
                wordList.Add(word.ToLower()); // Convert to lowercase for case-insensitivity
            }
        }

        Debug.Log($"Word List Length (Ignoring Spaces): {WordListLength}");

        if (wordList.Count > 0)
        {
            string firstWord = wordList[0];

            if (firstWord == "help")
            {
                InitializeHelpCommands();
            }
            else if (firstWord == "cd" && wordList.Count > 1)
            {
                HandleCdCommand();
            }
            else
            {
                HandleUnknownCommand(firstWord);
            }
        }

        tmpInput.text = "";
    }

    private void ClearLists()
    {
        wordList.Clear();
        cats.Clear();
    }

    private void InitializeHelpCommands()
    {
        cats.Clear();
        cats.Add("You can type 'help' to get a list of commands."); //Just Print
        cats.Add("You can type 'cd' to change directories."); //Do Something
        cats.Add("You can type 'ls' to list the contents of a directory."); //Just Print
        // ... Add other help commands
    }

    private void HandleCdCommand()
    {
        string secondWord = wordList[1];

        if (secondWord == "apple" || secondWord == "orange")
        {
            cats.Clear();
            cats.Add($"You can type 'cd' to change directories to {secondWord}.");
        }
        else
        {
            cats.Clear();
            cats.Add($"Directory {secondWord} not found.");
        }
    }

    private void HandleUnknownCommand(string command)
    {
        cats.Clear();
        cats.Add($"Command {command} not found.");
    }

    private int CountNonEmptyWords()
    {
        int count = 0;
        foreach (string word in wordList)
        {
            if (!string.IsNullOrWhiteSpace(word))
            {
                count++;
            }
        }
        return count;
    }
}
