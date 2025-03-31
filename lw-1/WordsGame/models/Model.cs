using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

public class Model : INotifyPropertyChanged
{
    private string _word;
    private string _hint;
    private int _remainingAttempts;
    private HashSet<char> _guessedLetters;
    private string _currentDisplayWord;

    public event PropertyChangedEventHandler PropertyChanged;

    public string Word
    {
        get => _word;
        private set
        {
            _word = value;
            OnPropertyChanged(nameof(Word));
        }
    }

    public string Hint
    {
        get => _hint;
        private set
        {
            _hint = value;
            OnPropertyChanged(nameof(Hint));
        }
    }

    public int RemainingAttempts
    {
        get => _remainingAttempts;
        private set
        {
            _remainingAttempts = value;
            OnPropertyChanged(nameof(RemainingAttempts));
        }
    }

    public string CurrentDisplayWord
    {
        get => _currentDisplayWord;
        private set
        {
            _currentDisplayWord = value;
            OnPropertyChanged(nameof(CurrentDisplayWord));
        }
    }

    public void LoadNewWord(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        var random = new Random();
        var line = lines[random.Next(lines.Length)];
        var parts = line.Split(';');
        
        Word = parts[0].Trim().ToUpper();
        Hint = parts.Length > 1 ? parts[1].Trim() : "No hint available";
        _guessedLetters = new HashSet<char>();
        RemainingAttempts = 7;
        UpdateDisplayWord();
    }

    public bool GuessLetter(char letter)
    {
        if (_guessedLetters.Contains(letter))
            return false;

        _guessedLetters.Add(letter);

        if (!Word.Contains(letter))
        {
            RemainingAttempts--;
            return false;
        }

        UpdateDisplayWord();
        return true;
    }

    public bool IsGameWon() => CurrentDisplayWord == Word;

    public bool IsGameOver() => RemainingAttempts <= 0;

    private void UpdateDisplayWord()
    {
        var displayWord = new char[Word.Length];
        for (int i = 0; i < Word.Length; i++)
        {
            displayWord[i] = _guessedLetters.Contains(Word[i]) ? Word[i] : '_';
        }
        CurrentDisplayWord = new string(displayWord);
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
