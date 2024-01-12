using TMPro;
using UnityEngine;

public class RomanNumeralsCalculator : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _responseText;

    [SerializeField]
    private TMP_InputField _inputField;

    /*
		Use this class for your assigment. 
		Roman numerals are as follow:

		I -> 1
		V -> 5
		X -> 10
		L -> 50
		C -> 100
		D -> 500
		M -> 1000

		For this example we'll use the simple conversion, where all numbers are
		translated straigt into their integer value (IIII -> 4) but feel free
		to add the correct conversion as an extra challenge (IV -> 4)

		Example input:

		MDLXX     ->    1570
		CCCLXVII  ->    367
	
	*/


    public void CalculateRomanNumerals()
    {
        string romanNumeral = _inputField.text;
        int response = RomanToArabic(romanNumeral);
        _responseText.text = $"{response}";

        _inputField.text = " ";
        _inputField.Select();
        _inputField.ActivateInputField();
    }

    private int RomanToArabic(string romanNumber)
    {
        int arabicNumber = 0;
        //Your logic here

        char[] userInput = romanNumber.ToCharArray();
        foreach (char c in userInput)
        {

            arabicNumber += LetterTranslation(char.ToUpper(c));
        }

        return arabicNumber;

    }

    private int LetterTranslation(char character)
    {
        switch (character)
        {
            case 'I':
                return 1;

            case 'V':
                return 5;

            case 'X':
                return 10;

            case 'L':
                return 50;
            case 'C':
                return 100;
            case 'D':
                return 500;
            case 'M':
                return 1000;
        }
        return 0;
    }

}
