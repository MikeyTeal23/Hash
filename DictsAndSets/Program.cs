using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DictsAndSets
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> lines = System.IO.File.ReadAllLines(@"..\words.txt").ToList();
            int[] bitList = new int[100000000];
            bitList.Fill(0);

            addSomeWords(lines, bitList);
            Console.WriteLine(getFalsePositiveCounter(lines, bitList));
            
            Console.ReadLine();
        }

        public static void addSomeWords(List<string> words, int[] bitList)
        {
            for (int i = 0; i < 200000; i++)
            {
                foreach(var hash in getListOfHashes(words[i]))
                {
                    bitList[hash] = 1;
                }
            }
        }

        public static int getFalsePositiveCounter(List<string> words, int[] bitList)
        {
            var falsePositiveCounter = 0;
            for (int i = 0; i < 100000; i++)
            {
                var isFalsePositive = true;
                foreach (var hash in getListOfHashes(words[i + 200000]))
                {
                    if(bitList[hash] == 0)
                    {
                        isFalsePositive = false;
                        break;
                    }
                }

                if (isFalsePositive)
                {
                    falsePositiveCounter++;
                }
            }
            return falsePositiveCounter;
        }

        public static List<Int64> getListOfHashes(string input)
        {
            var hashes = new List<Int64>();
            var longHash = CalculateMD5Hash(input);
            hashes.Add(Int64.Parse(longHash.Substring(0, 8)));
            hashes.Add(Int64.Parse(longHash.Substring(10, 8)));
            hashes.Add(Int64.Parse(longHash.Substring(20, 8)));
            return hashes;
        }

        public static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input

            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString());
            }

            return sb.ToString();
        }
    }
}

public static class ArrayExtensions
{
    public static void Fill<T>(this T[] originalArray, T with)
    {
        for (int i = 0; i < originalArray.Length; i++)
        {
            originalArray[i] = with;
        }
    }
}
