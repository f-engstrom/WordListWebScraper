using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using static System.Console;

namespace WordListWebScraper
{
    class Program
    {
        static void Main(string[] args)
        {


            Dictionary<string, int> sortedWords = new Dictionary<string, int>();

            List<string> words = new List<string>();

            string location = @"C:\Users\Frans\Dropbox\Projects\WordListWebScraper\WordsToReadIn.txt";

            string ignorelistLocation = @"C:\Users\Frans\Dropbox\Projects\WordListWebScraper\ignorelist.txt";

            List<string> ignoreList = WordsToIgnore(ignorelistLocation);

            string scrapedWords = TextfileReader(location);

            words = WordScraper(scrapedWords);


            sortedWords = WordCountSorter(words, ignoreList);



            foreach (var word in sortedWords.Where(x => x.Value > 100))
            {

                WriteLine($"{word.Key}   {word.Value}");


            }

        }

        private static List<string> WordsToIgnore(string location)
        {
            string word = "";

            List<string> ignoreWords = new List<string>();

            using (StreamReader reader = new StreamReader(location))
            {

                while (!reader.EndOfStream)
                {
                    word = reader.ReadLine();

                    ignoreWords.Add(word);

                }

            }


            return ignoreWords;
        }


        private static string TextfileReader(string location)
        {
            string words = "";

            List<string> lines = new List<string>();

            using (StreamReader reader = new StreamReader(location))
            {

                while (!reader.EndOfStream)
                {
                    words = reader.ReadLine();

                    lines.Add(words);

                }

            }

            words = string.Join(" ", lines);

            return words;

        }

        private static List<string> WordScraper(string scrapedWords)
        {

            //char[] ignoreList = new[] { ' ', '^', '*', '!', '?', '.', ',', '-', '/', ':' };


            scrapedWords = Regex.Replace(scrapedWords, "[^a-öA-Ö]", " ");

            List<string> allWords = scrapedWords.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();



            return allWords;

        }

        private static Dictionary<string, int> WordCountSorter(List<string> wordList, List<string> ignoreList)
        {

            Dictionary<string, int> sortedWords = new Dictionary<string, int>();

            wordList.RemoveAll(word => ignoreList.Any(x => word.ToUpper() == x));


            var sortedCountedWords = wordList
                .GroupBy(q => q.ToUpper(), (c, cs) => new { key = c, count = cs.Count() })
                .OrderByDescending(q => q.count
                );


            foreach (var word in sortedCountedWords)
            {

                sortedWords.Add(word.key, word.count);
            }

            return sortedWords;


        }
    }

}