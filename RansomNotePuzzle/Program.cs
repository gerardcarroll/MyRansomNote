using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;

namespace RansomNotePuzzle
{
    class Program
    {
        public static char[] Note = "bcxdy".ToCharArray();
        public static char[] Newspaper = "axybcde".ToCharArray();
        public static bool bombed = false;
        public static Stopwatch sw = new Stopwatch();

        static void Main(string[] args)
        {
            MakeNoteSortArrays(Note,Newspaper);

            sw.Start();
            MakeNoteSortArrays(Note, Newspaper);
            sw.Stop();
            ReportResult("Sorting");

            sw.Restart();
            MakeNoteHashCount(Note,Newspaper);
            sw.Stop();
            ReportResult("Hashing");
            
            sw.Restart();
            MakeNoteDictCount(Note, Newspaper);
            sw.Stop();
            ReportResult("Dictionary<>");

            sw.Restart();
            MakeNoteNestedFors(Note,Newspaper);
            sw.Stop();
            ReportResult("NestedFors");

            sw.Restart();
            CheckNote(Note.ToString(), Newspaper.ToString());
            sw.Stop();
            ReportResult("Ronan Clancy");

            sw.Restart();
            MagazineAsnote(Note.ToString(), Newspaper.ToString());
            sw.Stop();
            ReportResult("Marco Leite");

            sw.Restart();
            CanBuildMessage(Note.ToString(), Newspaper.ToString());
            sw.Stop();
            ReportResult("Dana Diac");
            Console.ReadLine();
        }

        #region Dana Diac
        public static int[] CountSort(String note)
        {
            var a = new int[128];
            foreach (var charachter in note)
            {
                a[((int)charachter)] += 1;
            }
            return a;
        }

        public static bool Compare(int[] note, int[] magazine)
        {
            for (int i = 0; i < 128; i++)
            {
                if (note[i] > magazine[i])
                    return false;
            }
            return true;
        }

        public static bool CanBuildMessage(string message, string magazine)
        {
            if (message.Length > magazine.Length)
                return false;

            var noteMessage = CountSort(message);
            var magazineText = CountSort(magazine);
            return Compare(noteMessage, magazineText);
        }
        #endregion
        #region Marco Leite

        delegate bool FindLetter(char find);
        public static bool MagazineAsnote(string note, string magazine)
        {
            bool[] used = new bool[magazine.Length];
            FindLetter fl = l =>
            {
                bool ans;
                for (int i = 0; i < magazine.Length; i++)
                {
                    if (l.Equals(magazine[i]))
                    {
                        if (used[i] != true)
                        {
                            used[i] = true;
                            return true;
                        }
                    }
                }
                return false;
            };

            foreach (char letter in note)
            {
                if (!fl(letter))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion
        #region Ronan Clancy
        private static bool CheckNote(string note, string paper)
        {
            var noteList = new List<char>(note);
            var paperList = new List<char>(paper);
            var letterSuccess = 0;

            //note interation
            foreach (var i in noteList)
            {
                if (!paperList.Contains(i)) break;

                //increment successfull letter found, reset paperChar letter
                letterSuccess++;
                paperList.Remove(i);
            }

            //check letters count
            return (letterSuccess != noteList.Count);
        }
        #endregion

        #region jk
        
        // Sort both strings and step through until no match
        public static bool MakeNoteSortArrays(char[] noteIn, char[] newspaperIn)
        {
            char[] note = (char[])(noteIn.Clone());
            Array.Sort<Char>(note);
            char[] newspaper = (char[])(newspaperIn.Clone());
            Array.Sort<Char>(newspaper);
            int idxNewsPaper = 0;
            for (int i = 0; i < Note.Length; i++)
            {
                while (!bombed && note[i] != newspaper[idxNewsPaper])
                {
                    if (newspaper[idxNewsPaper] < note[i]) idxNewsPaper++;
                    bombed = (idxNewsPaper >= newspaper.Length-1) ? true : false;
                }
                idxNewsPaper++;
                if (bombed || idxNewsPaper>=newspaper.Length) return false;
            }
            return true;
        }

        public static bool MakeNoteHashCount(char[] Note, char[] Newspaper)
        {
            Hashtable countLet = new Hashtable();
            for (int i = 0; i < Newspaper.Length; i++)
            {
                if (countLet.ContainsKey(Newspaper[i]))
                {
                    int cnt = Convert.ToInt16(countLet[Newspaper[i]]);
                    countLet[Newspaper[i]] = cnt++;
                }
                else
                {
                    countLet.Add(Newspaper[i],1);
                }
            }
            for (int i = 0; i < Note.Length; i++)
            {
                if (countLet.ContainsKey(Note[i]))
                {
                    int cnt = Convert.ToInt16(countLet[Newspaper[i]])-1;
                    countLet[Newspaper[i]] = cnt;
                    if (cnt < 0) bombed = true;
                }
            }
            return bombed;
        }

        // Dictionary<>
        public static bool MakeNoteDictCount(char[] Note, char[] Newspaper)
        {
            var countLet = new Dictionary<char, int>();
            for (int i = 0; i < Newspaper.Length-1; i++)
            {
                if (countLet.ContainsKey(Newspaper[i])) countLet[Newspaper[i]]++;
                else countLet.Add(Newspaper[i],1);
            }
            for (int i = 0; i < Note.Length; i++)
            {
                bombed = (((countLet[Note[i]]--)<0)?true:false);
                
            }
            return bombed;
        }

        // For each Note letter, search for same in Newspaper and mark as used
        public static bool MakeNoteNestedFors(char[] noteIn, char[] newspaperIn)
        {
            char[] note = (char[])(noteIn.Clone());
            char[] newspaper = (char[])(newspaperIn.Clone());
            int idxNote = 0;
            do
            {
                int idxNews = 0;
                do
                {
                    if (note[idxNote] == newspaper[idxNews])
                    {
                        newspaper[idxNews] = '*';
                        break;
                    }
                    idxNews++;
                } while (idxNews < newspaper.Length);
                if (idxNews == newspaper.Length) bombed = true;
                idxNote++;
            } while (!bombed && idxNote < note.Length);

            return bombed;
        }

        public static void ReportResult(string type)
        {
            Console.Write(type+": ");
            Console.WriteLine(bombed ? "No can do." : "Note written.");
            Console.WriteLine(sw.Elapsed.TotalMilliseconds+"ms");
        }
        #endregion
    }
}
