using System;

namespace WorldleConsole
{

    internal class Program
    {
        //variable to keep track of correct answers for DrawAlphabet method
        public static LetterStatus[] alphabetInt = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static char[] chars = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'Å', 'Ä', 'Ö' };
        //Class to create an object that holds 5 letters and their status
        public class Letters
        {
            public LetterStatus[] alla = new LetterStatus[5];
            public Letters()
            {
                this.alla[0] = LetterStatus.Tom;
                this.alla[1] = LetterStatus.Tom;
                this.alla[2] = LetterStatus.Tom;
                this.alla[3] = LetterStatus.Tom;
                this.alla[4] = LetterStatus.Tom;
            }
        }
        //LettersStatus to see what status a guess has Tom=empty Fel=wrong, FelPosition=wrong position and Correct
        public enum LetterStatus
        {
            Tom,
            Fel,          // "This letter is not in the word."
            FelPosition,  // "This letter is in the word, but not in this position."
            Correct         // "This letter is in the word in this position."
        }
        /// <summary>
        /// reads akk words from file and returns them in a list
        /// </summary>
        /// <param name="fileName">name of file to read</param>
        /// <returns></returns>
        static List<string> ReadWordsFromFile(string fileName)
        {
            List<string> addressList = new List<string>();
            using (StreamReader file = new StreamReader(fileName))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                    addressList.Add(new string(line).ToUpper());
            }
            return addressList;
        }
        /// <summary>
        /// Draws all guesses and color codes them if they are correct or not
        /// </summary>
        /// <param name="ordet">the correct word</param>
        /// <param name="allResult">all results from checking if the guesses were correct stored in an array for every guess</param>
        /// <param name="gissningar">all the guesses stored in an array</param>
        public static void Rita(string ordet, Letters[] allResult, string[] gissningar)
        {

            Console.Clear();
            for (int j = 0; j < 6; j++)
            {
                for (int i = 0; i < 5; i++)
                {
                    //draws the letter in green if it was Correct
                    if (allResult[j].alla[i] == LetterStatus.Correct)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($"{gissningar[j][i]} ");
                    }
                    //draws the letter in yellow if it was correct but out of position
                    else if (allResult[j].alla[i] == LetterStatus.FelPosition)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write($"{gissningar[j][i]} ");
                    }
                    //draws the letter in red color if it does not exist in the word
                    else if (allResult[j].alla[i] == LetterStatus.Fel)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write($"{gissningar[j][i]} ");
                    }
                    //draws # if a guess has not yet been made
                    else if (allResult[j].alla[i] == LetterStatus.Tom)
                    {
                        Console.ResetColor();
                        Console.Write($"# ");
                    }

                }
                Console.ResetColor();
                Console.WriteLine();
            }

        }
        /// <summary>
        /// method to check if guess was correct and assign 
        /// </summary>
        /// <param name="ordet">the correct word</param>
        /// <param name="gissning">the guess</param>
        /// <returns></returns>
        public static LetterStatus[] CheckLetters(string ordet, string gissning)
        {
            //create result and sets all the Fel (incorrect)
            var result = new LetterStatus[5];
            result[0] = LetterStatus.Fel;
            result[1] = LetterStatus.Fel;
            result[2] = LetterStatus.Fel;
            result[3] = LetterStatus.Fel;
            result[4] = LetterStatus.Fel;
            //First loop to check if any letters are in the correct position, turns them into "_" if so to make sure they are not counted in the second loop
            for (int i = 0; i < gissning.Length; i++)
            {
                if (ordet[i] == gissning[i])
                {
                    result[i] = LetterStatus.Correct;
                    if (i == 0) ordet = "_" + ordet.Remove(0, 1);
                    else if (i == 1) ordet = ordet.Substring(0, 1) + "_" + ordet.Substring(2);
                    else if (i == 2) ordet = ordet.Substring(0, 2) + "_" + ordet.Substring(3);
                    else if (i == 3) ordet = ordet.Substring(0, 3) + "_" + ordet.Substring(4);
                    else if (i == 4) ordet = ordet.Substring(0, 4) + "_" + ordet.Substring(5);
                }
            }
            //Second loop to check for correct letters but that are out of place. if one is found the first instance of that letter is removed
            //from the correct word so that they are not counted twice creating incorrect information such as double yellow letters when there should be only one
            for (int i = 0; i < gissning.Length; i++)
            {
                if (ordet.Contains(gissning[i]) && result[i] != LetterStatus.Correct)
                {
                    result[i] = LetterStatus.FelPosition;
                    if (gissning[i] == ordet[1]) ordet = ordet.Substring(0, 1) + "_" + ordet.Substring(2);
                    else if (gissning[i] == ordet[2]) ordet = ordet.Substring(0, 2) + "_" + ordet.Substring(3);
                    else if (gissning[i] == ordet[3]) ordet = ordet.Substring(0, 3) + "_" + ordet.Substring(4);
                    else if (gissning[i] == ordet[4]) ordet = ordet.Substring(0, 4) + "_" + ordet.Substring(5);
                }
            }
            //Loop to check for which letters have been guessed and store in alphabetInt if they are correct(3), wrong(1) or wrongposition(2)
            for (int i = 0; i < chars.Length; i++)
            {
                if (gissning[0] == chars[i] && result[0] == LetterStatus.Correct) alphabetInt[i] = LetterStatus.Correct;
                else if (gissning[0] == chars[i] && result[0] == LetterStatus.FelPosition && alphabetInt[i] != LetterStatus.Correct) alphabetInt[i] = LetterStatus.FelPosition;
                else if (gissning[0] == chars[i] && result[0] == LetterStatus.Fel && alphabetInt[i] != LetterStatus.FelPosition && alphabetInt[i] != LetterStatus.Correct) alphabetInt[i] = LetterStatus.Fel;

                if (gissning[1] == chars[i] && result[1] == LetterStatus.Correct) alphabetInt[i] = LetterStatus.Correct;
                else if (gissning[1] == chars[i] && result[1] == LetterStatus.FelPosition && alphabetInt[i] != LetterStatus.Correct) alphabetInt[i] = LetterStatus.FelPosition;
                else if (gissning[1] == chars[i] && result[1] == LetterStatus.Fel && alphabetInt[i] != LetterStatus.FelPosition && alphabetInt[i] != LetterStatus.Correct) alphabetInt[i] = LetterStatus.Fel;

                if (gissning[2] == chars[i] && result[2] == LetterStatus.Correct) alphabetInt[i] = LetterStatus.Correct;
                else if (gissning[2] == chars[i] && result[2] == LetterStatus.FelPosition && alphabetInt[i] != LetterStatus.Correct) alphabetInt[i] = LetterStatus.FelPosition;
                else if (gissning[2] == chars[i] && result[2] == LetterStatus.Fel && alphabetInt[i] != LetterStatus.FelPosition && alphabetInt[i] != LetterStatus.Correct) alphabetInt[i] = LetterStatus.Fel;

                if (gissning[3] == chars[i] && result[3] == LetterStatus.Correct) alphabetInt[i] = LetterStatus.Correct;
                else if (gissning[3] == chars[i] && result[3] == LetterStatus.FelPosition && alphabetInt[i] != LetterStatus.Correct) alphabetInt[i] = LetterStatus.FelPosition;
                else if (gissning[3] == chars[i] && result[3] == LetterStatus.Fel && alphabetInt[i] != LetterStatus.FelPosition && alphabetInt[i] != LetterStatus.Correct) alphabetInt[i] = LetterStatus.Fel;

                if (gissning[4] == chars[i] && result[4] == LetterStatus.Correct) alphabetInt[i] = LetterStatus.Correct;
                else if (gissning[4] == chars[i] && result[4] == LetterStatus.FelPosition && alphabetInt[i] != LetterStatus.Correct) alphabetInt[i] = LetterStatus.Fel;
                else if (gissning[4] == chars[i] && result[4] == LetterStatus.Fel && alphabetInt[i] != LetterStatus.Fel && alphabetInt[i] != LetterStatus.Correct) alphabetInt[i] = LetterStatus.Fel;

            }

            Console.WriteLine(ordet);
            return result;
        }
        /// <summary>
        /// draws alphabet, if alphabetChars is equals to chars[i] print in color. if Correct green if Wrongpos yellow if wrong red
        /// </summary>
        public static void DrawAlphabet()
        {
            char[] chars = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'Å', 'Ä', 'Ö' };
            for (int i = 0; i < chars.Length; i++)
            {
                if (alphabetInt[i] == LetterStatus.Correct)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(chars[i] + " ");
                }
                else if (alphabetInt[i] == LetterStatus.FelPosition)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(chars[i] + " ");
                }
                else if (alphabetInt[i] == LetterStatus.Fel)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(chars[i] + " ");
                }
                else
                {
                    Console.ResetColor();
                    Console.Write(chars[i] + " ");
                }
            }

            Console.ResetColor();
            Console.WriteLine();
        }
        static void Main(string[] args)
        {
            //Creates a wordlist
            List<string> ordLista = new List<string>();
            //Retrieve words from file
            ordLista = ReadWordsFromFile("ordlista.txt");
        //Starting point
        goagain:
            //resetting alphabetInt
            for(int i=0; i < alphabetInt.Length; i++)
            {
                alphabetInt[i] = LetterStatus.Tom;
            }
            //Declares variable to declare results of each guess stored as Tom, Fel, FelPosition and Correct all are Tom as stand
            Letters[] allResult = { new Letters(), new Letters(), new Letters(), new Letters(), new Letters(), new Letters() };
            Random rand = new Random();
            //Pick a random word and store in ordet
            string ordet = ordLista[rand.Next(ordLista.Count())];
            //variable to store guess
            string gissning;
            //index to know where to assign guesses and results
            int index = 0;
            //
            bool end = false;
            //array to collect all guesses
            string[] gissningar = new string[6];            
            do
            {
                Rita(ordet, allResult, gissningar);
                DrawAlphabet();
            hejhopp:
                Console.WriteLine("Skriv in din gissning");
                gissning = Console.ReadLine().ToUpper();

                //Loop to check if input word exists in database break loops if it exists in database lets you reenter a new guess otherwise
                for (int i = 0; i < ordLista.Count(); i++)
                {
                    if (gissning == ordLista[i]) break;
                    else if (i == ordLista.Count() - 1)
                    {
                        Rita(ordet, allResult, gissningar);
                        DrawAlphabet();
                        Console.WriteLine("Det där är inget ord som finns i ordlistan!");
                        goto hejhopp;
                    }
                }
                //Assign guess to correct slot in array
                gissningar[index] = gissning;
                //Send guess and correct word to CheckLetters to check if they are correct and store them in index of allResult
                allResult[index].alla = CheckLetters(ordet, gissning);
                //Send word allResult and guesses to draw all of them up with color coding
                Rita(ordet, allResult, gissningar);
                DrawAlphabet();
                //Check if guess was correct
                if (ordet == gissning)
                {
                    Console.WriteLine("Du vann!!! Hurra!! Skriv igen om du vill spela en gång till annars quit");
                    string command = Console.ReadLine();
                    if (command == "igen") goto goagain;
                    else end = true;
                }
                index++;

            } while (end == false);
        }
    }
}