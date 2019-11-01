using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WordListWebScraper
{
    class TextFileProsessing
    {
        public static Dictionary<string, int> ReadFromGatherdWOrdsAndCOuntxt()
        {
            Dictionary<string, int> readwordsAndCOunt = new Dictionary<string, int>();


            using (StreamReader sr = new StreamReader(Program.GatheredWordsAndCount))
            {
                string[] fileData = new string[2];

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    fileData = line.Split(' ');

                    string word = fileData[0];
                    int count = Int32.Parse(fileData[1]);

                    readwordsAndCOunt.Add(word, count);


                }


                sr.Close();


            }

            return readwordsAndCOunt;

        }

        public static void WriteToGatherdWOrdsAndCOuntxt()
        {

            string[] words = WordProsessing.SortedWords.Select(x => x.Key + " " + x.Value).ToArray();

            //string[] words = sortedWords.Where(pair => pair.Value > 100).Select(x => x.Key).ToArray();


            using (StreamWriter sr = new StreamWriter(Program.GatheredWordsAndCount, false))
            {

                foreach (var word in words)
                {
                    sr.WriteLine(word);

                }


                sr.Close();


            }


        }

        public static void WritCurrentListToTxt(List<string> chosenWords)
        {

            // string words = string.Join(";", sortedWords.Select(x => x.Key + "=" + x.Value).ToArray());

            string[] words = chosenWords.ToArray();


            using (StreamWriter sr = new StreamWriter(Program.TxtToSaveTo,false))
            {

                foreach (var word in words)
                {
                    sr.WriteLine(word);

                }



                sr.Close();


            }

        }

        public static string TextfileReader(string location)
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

            words = String.Join(" ", lines);

            return words;

        }

        public static List<string> WordsToIgnore(string location)
        {
            string word = "";

            List<string> ignoreWords = new List<string>();

            using (StreamReader reader = new StreamReader(location))
            {

                while (!reader.EndOfStream)
                {
                    word = reader.ReadLine();

                    ignoreWords.Add(word.ToUpper( ).Normalize());

                }

            }


            return ignoreWords;
        }
    }
}