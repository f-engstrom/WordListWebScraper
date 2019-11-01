using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Console;

namespace WordListWebScraper
{
    class WordProsessing
    {
        public static Dictionary<string, int> SortedWords { get; set; } = new Dictionary<string, int>();


        public static void ProcessWordsFromScrapedString(string scrapedWords)
        {


            Dictionary<string, int> sortedCountedWords = WordCountSorter(WordScraper(scrapedWords));

            SortedWords = DeleteIgnoredWords(TextFileProsessing.WordsToIgnore(Program.ignorelistLocation), sortedCountedWords);


        }

        public static void DeleteNewIgnoredWords()
        {
            SortedWords = DeleteIgnoredWords(TextFileProsessing.WordsToIgnore(Program.ignorelistLocation), SortedWords);


        }


        private static List<string> WordScraper(string scrapedWords)
        {

            char[] ignoreList = new[] { '_', ' ', '^', '*', '!', '?', '.', ',', '-', '/', ':', '·' };


            scrapedWords = Regex.Replace(scrapedWords, "[^a-öA-Ö]", " ");

            List<string> allWords = scrapedWords.Split(ignoreList, StringSplitOptions.RemoveEmptyEntries).ToList();



            return allWords;

        }

        private static Dictionary<string, int> WordCountSorter(List<string> wordList)
        {



            var sortedCountedWords = wordList
                .GroupBy(q => q.ToUpper(), (c, cs) => new { key = c.Normalize(), count = cs.Count() })
                .OrderByDescending(q => q.count
                );

            Dictionary<string, int> sortedWords = SortedWords;

            foreach (var word in sortedCountedWords)
            {
                if (!(sortedWords.Keys.Contains(word.key)))
                {
                    sortedWords.Add(word.key, word.count);

                }

                sortedWords[word.key] += word.count;


            }

            return sortedWords;


        }

        private static Dictionary<string, int> DeleteIgnoredWords(List<string> ignoreList, Dictionary<string, int> sortedCountedWords)
        {

            foreach (var ignoreWord in ignoreList)
            {
                sortedCountedWords.Remove(ignoreWord);
            }

            return sortedCountedWords;

            // allWords.Remove(key => ignoreList.Any(x => key.ToUpper() == x));

        }






        public static void ListWords()
        {
            TextInfo tx = CultureInfo.CurrentCulture.TextInfo;
            int amount = 0;
            bool exitLoop = false;
            List<string> chosenWords = new List<string>();
            Func<KeyValuePair<string, int>, bool> filter = pair => String.IsNullOrEmpty(pair.Key);

            var filterdWords = SortedWords.Where(filter).ToList();
            int wordsCount = filterdWords.Count();

            do
            {
                Clear();
                filterdWords = SortedWords.Where(filter).ToList();

                WriteLine("Word        Count          % of total words");
                WriteLine("-----------------------------------------");



                for (int i = 0; i < wordsCount; i++)
                {
                    WriteLine(tx.ToTitleCase(filterdWords[i].Key.ToLower()).PadRight(15, ' ') + filterdWords[i].Value.ToString().PadRight(15, ' ') + ((filterdWords[i].Value / SortedWords.Count) * 100));

                    chosenWords.Add(tx.ToTitleCase(filterdWords[i].Key.ToLower()));


                }



                WriteLine("Press 1 to sort out specified number of top words");
                WriteLine("Press 2 to sort out words with count more than specified number");
                WriteLine("Press 3 to display all words");
                WriteLine("Press enter to save current list to txt");
                WriteLine("Press escape to exit to main menu");

                ConsoleKeyInfo input;
                bool isValidKey;

                do
                {

                    input = ReadKey(true);

                    isValidKey = input.Key == ConsoleKey.D1 || input.Key == ConsoleKey.D2 || input.Key == ConsoleKey.D3 || input.Key == ConsoleKey.Escape || input.Key == ConsoleKey.Enter;


                } while (!isValidKey);

                if (input.Key == ConsoleKey.D1)
                {

                    Write("\nDisplay top: ");
                    wordsCount = Convert.ToInt32(ReadLine());

                    filter = pair => !String.IsNullOrEmpty(pair.Key);

                }
                if (input.Key == ConsoleKey.D2)
                {

                    WriteLine("\nCount more than: ");
                    amount = Convert.ToInt32(ReadLine());

                    filter = pair => pair.Value > amount;
                }
                if (input.Key == ConsoleKey.D3)
                {

                    filter = pair => !String.IsNullOrEmpty(pair.Key);
                }


                if (input.Key == ConsoleKey.Escape)
                {
                    exitLoop = true;
                }



                if (input.Key == ConsoleKey.Enter)
                {
                    TextFileProsessing.WritCurrentListToTxt(chosenWords);
                }

            } while (!exitLoop);







        }
    }
}