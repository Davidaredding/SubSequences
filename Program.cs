using System;
using System.Collections.Generic;
using System.Linq;

namespace Sequences
{
    public class Program
    {
        static void Main(string[] args)
        {
            Action<List<int>> simpleLog = l=>Console.WriteLine($"[{String.Join(',',l)}]");
            /*
                abcad = [abca][d]
                abc = [a][b][c]
                aabacbacdedefg = [aabacbac][dede][f][g]
             */
            Console.WriteLine("Sequence 1");
            simpleLog(SubSequenceCount("abcad"));

            Console.WriteLine("Sequence 2");
            simpleLog(SubSequenceCount("abc"));

            Console.WriteLine("Sequence 3");
            simpleLog(SubSequenceCount("aabacbacdedefg"));
        }

        
        public static List<int> SubSequenceCount(string sequence)
        {
            Dictionary<char,(int first, int last)> subsequences = new Dictionary<char, (int first, int last)>();
            
            sequence.Select((c,i)=>(c,i)).ToList().ForEach(item=>{
                if(subsequences.TryGetValue(item.Item1, out var existing))
                    subsequences[item.Item1] = (existing.first, item.Item2);
                else
                        subsequences[item.Item1] = (item.Item2,item.Item2);

            });
//            subsequences.ToList().ForEach(kvp=>Console.WriteLine($"{kvp.Key}-[{kvp.Value.first}:{kvp.Value.last}]"));
            //merge down
            var rawSubSequences = subsequences.Values.ToList();
            var resultingSubSequences = new List<(int first, int last)>();
            while(rawSubSequences.Count>0)
            {
                rawSubSequences = MergeSubSequences(rawSubSequences);
                resultingSubSequences.Add(rawSubSequences.First());
                rawSubSequences.RemoveAt(0);
            }
            
            //resultingSubSequences.ForEach(s=>Console.WriteLine($"{s.first}:{s.last} [{sequence.Substring(s.first,(s.last-s.first)+1)}]"));
            
            //The original question called for a list of 
            //subsequences counts
            return resultingSubSequences.Select(r=>(r.last-r.first)+1).ToList();
        }

        public static List<(int first, int last)> MergeSubSequences(List<(int first, int last)> subsequences)
        {
             var orderSubSequences = subsequences.OrderBy(sub=>sub.first).ToList();
             var subsequence = orderSubSequences.First();
             orderSubSequences.RemoveAt(0);
             while(orderSubSequences.Count() > 1 && orderSubSequences.First().first < subsequence.last)
             {
                 subsequence.last = Math.Max(orderSubSequences.First().last, subsequence.last);
                 orderSubSequences.RemoveAt(0);
             }
             orderSubSequences.Insert(0,subsequence);
             return orderSubSequences;
        }


    }

    


}
