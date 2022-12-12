using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Text_Grab.Utilities;

public static class StringBuilderExtensions
{
    public static void CharDictionaryReplace(this StringBuilder stringBuilder, Dictionary<char, char> charDictionary)
    {
        foreach (Char key in charDictionary.Keys)
        {
            stringBuilder.Replace(key, charDictionary[key]);
        }
    }

    public static void ReplaceGreekOrCyrillicWithLatin(this StringBuilder stringBuilder)
    {
        stringBuilder.CharDictionaryReplace(StringMethods.greekCyrillicLatinMap);
    }

    public static void TryFixToLetters(this StringBuilder stringBuilder)
    {
        stringBuilder.CharDictionaryReplace(StringMethods.NumbersToLetters);
    }

    public static void TryToFixToNumbers(this StringBuilder stringBuilder)
    {
        stringBuilder.CharDictionaryReplace(StringMethods.LettersToNumbers);
    }

    public static string TryFixEveryWordLetterNumberErrors(this StringBuilder stringToFix)
    {
        string[] listOfWords = stringToFix.ToString().Split(' ');
        List<string> fixedWords = new();

        foreach (string word in listOfWords)
        {
            string newWord = word.TryFixNumberLetterErrors();
            fixedWords.Add(newWord);
        }
        string joinedString = string.Join(' ', fixedWords.ToArray());
        joinedString = joinedString.Replace("\t ", "\t");
        joinedString = joinedString.Replace("\r ", "\r");
        joinedString = joinedString.Replace("\n ", "\n");
        return joinedString.Trim();
    }

    public static void ReverseWordsForRightToLeft(this StringBuilder text)
    {
        string[] textListLines = text.ToString().Split(new char[] { '\n', '\r' });
        Regex regexSpaceJoiningWord = new(@"(^[\p{L}-[\p{Lo}]]|\p{Nd}$)|.{2,}");

        _ = text.Clear();
        foreach (string textLine in textListLines)
        {
            bool firstWord = true;
            bool isPrevWordSpaceJoining = false;
            List<string> wordArray = textLine.Split().ToList();
            wordArray.Reverse();

            foreach (string wordText in wordArray)
            {
                bool isThisWordSpaceJoining = regexSpaceJoiningWord.IsMatch(wordText);

                if (firstWord || (!isThisWordSpaceJoining && !isPrevWordSpaceJoining))
                    _ = text.Append(wordText);
                else
                    _ = text.Append(' ').Append(wordText);

                firstWord = false;
                isPrevWordSpaceJoining = isThisWordSpaceJoining;
            }

            if (textLine.Length > 0)
                _ = text.Append(Environment.NewLine);
        }
    }
}