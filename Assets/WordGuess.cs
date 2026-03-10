using UnityEngine;
using TMPro;
using System.IO;
using System.Linq;

public class WordGuess : MonoBehaviour
{
    [SerializeField] GameObject resetButton;
    [SerializeField] GameObject submitButton;
    [SerializeField] TMP_Text title;
    [SerializeField] TMP_Text wordAttempt;
    [SerializeField] private TMP_InputField guessInput;
    private string randomWord;
    private string result;
    private int randomIndex;
    private int remainingGuesses = 3;
    private string filepath;
    private int lAmount;
    private string[] lines;
    char[] Chosenletters;
    char[] inputletters;
    char[] outputL;
    private string guess;
    bool match;


    private void Start()
    {
        filepath = Application.dataPath + "/wordlist.txt";
        LoadWords();
        ChooseWord();

    }

    public void ResetGame()
    {
        submitButton.GetComponent<UnityEngine.UI.Button>().interactable = true;
        remainingGuesses = 3;
        title.text = "Guess the Word!";
        wordAttempt.text = " ";
        result = "";
        outputL = null;
        ChooseWord();


    }
    public void LoadWords() 
    {
        lines = System.IO.File.ReadAllLines(filepath);
       
    }

    public void ChooseWord()
    {
        randomIndex = Random.Range(0, lines.Length);
        randomWord = lines[randomIndex];
        Debug.Log("the word is " + randomWord);

        Chosenletters = randomWord.ToCharArray();
    }

    public void checkGuess()
    {
        guess = guessInput.text;

        if (string.IsNullOrEmpty(guess))
        {
            title.text = "Please Enter a valid input";
            return;
        }

        char[] previousOutput = outputL;
        inputletters = guess.ToCharArray();
        lAmount = Chosenletters.Length;
        match = inputletters.Any(c => Chosenletters.Contains(c));
        char[] newOutput = new char[lAmount];

        for (int i = 0; i < lAmount; i++)
            newOutput[i] = (previousOutput != null && previousOutput[i] != '_') ? previousOutput[i] : '_';

        for (int i = 0; i < lAmount; i++)
        {
            if (inputletters.Contains(Chosenletters[i]))
            {
                newOutput[i] = Chosenletters[i];
            }
        }

        outputL = newOutput;
        if (guess.ToLower() == randomWord.ToLower())
        {
            title.text = "Congrats! You Win!";
            wordAttempt.text = randomWord;
            submitButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
            remainingGuesses = 0;
            guessInput.text = "";
            return;
        }

        remainingGuesses--;

        if (remainingGuesses > 0)
        {
            title.text = "You have " + remainingGuesses + " guesses left.";

            if (match)
            {
                wordAttempt.text = new string(outputL);
            }
            else
            {
                wordAttempt.text = guess;
            }
        }
        else
        {
            title.text = "No Guesses remaining.";
            wordAttempt.text = "The word was: " + randomWord;
            submitButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
        }

        guessInput.text = "";
    }
}
