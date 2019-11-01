using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Console;

namespace WordListWebScraper
{
    class Program
    {
        public static string ignorelistLocation = @"C:\Users\Frans\Dropbox\Projects\WordListWebScraper\ignorelist.txt";
        public static string TxtToSaveTo = @"C:\Users\Frans\Dropbox\Projects\WordListWebScraper\WordsFromPage.txt";
        public static string GatheredWordsAndCount =
            @"C:\Users\Frans\Dropbox\Projects\WordListWebScraper\GatheredWordsAndCount.csv";


        static readonly HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            WordProsessing.SortedWords = TextFileProsessing.ReadFromGatherdWOrdsAndCOuntxt();

            Menu().Wait();

        }


        static async Task Menu()
        {
            string source;
            bool shouldNotExit = true;

            while (shouldNotExit)
            {


                Clear();

                WriteLine("1. Read Web source");
                WriteLine("2. Read text source");
                WriteLine("3. List words");
                WriteLine("4. Exit");

                ConsoleKeyInfo keyPressed = ReadKey(true);


                switch (keyPressed.Key)
                {

                    case ConsoleKey.D1:

                        try
                        {
                            Clear();

                            Write("Enter websource uri: ");
                            source = ReadLine();

                            string scrapedWebdWords = await client.GetStringAsync(source);

                             WordProsessing.ProcessWordsFromScrapedString(scrapedWebdWords);

                            TextFileProsessing.WriteToGatherdWOrdsAndCOuntxt();
                        }
                        catch (Exception e)
                        {
                            WriteLine(e);
                            ReadKey();
                        }


                        break;

                    case ConsoleKey.D2:

                        try
                        {

                            Clear();
                            Write("Enter textfile path: ");
                            source = ReadLine();

                            string scrapedTextFiledWords = TextFileProsessing.TextfileReader($"{source}");

                            WordProsessing.ProcessWordsFromScrapedString(scrapedTextFiledWords);

                            TextFileProsessing.WriteToGatherdWOrdsAndCOuntxt();


                        }
                        catch (Exception e)
                        {
                            WriteLine(e);
                            ReadKey();
                        }

                        break;

                    case ConsoleKey.D3:

                        Clear();

                        WordProsessing.ListWords();

                        break;

                    case ConsoleKey.D4:

                        shouldNotExit = false;

                        break;


                }

                    WordProsessing.DeleteNewIgnoredWords();


            }

            TextFileProsessing.WriteToGatherdWOrdsAndCOuntxt();
        }
    }

}