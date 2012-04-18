using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DtWorkshop.Plugin.ID3
{
    public delegate float DtHeuristic(DtNodeBuilder context, DtAttribute attribute);

    public enum DtHeuristicKind
    {
        Gain,
        Uniform
    }

    public static class DtHeuristics
    {
        public static DtHeuristic[] Instances = 
        { 
            GainHeuristic,
            UniformHeuristic 
        };
        public static float GainHeuristic(DtNodeBuilder context, DtAttribute attr)
        {
            DtAttribute targetAttr = context.TreeBuilder.TargetAttr;
            // Calculates the information gain (reduction in entropy) that would
            // result by splitting the data on the chosen attribute (attr).
            float subsetEntropy = 0.0f;
            // Calculate the frequency of each of the values in the target attribute
            var query = from record in context.Query
                        group record by record[attr.Index] into g
                        select new { Count = g.Count(), Value = g.Key };
            // Calculate the sum of the entropy for each subset of records weighted
            // by their probability of occuring in the training set.
            float invSum = 1.0f / query.Sum(g => g.Count);
            // Subtract the entropy of the chosen attribute from the entropy of the
            // whole data set with respect to the target attribute (and return it)
            foreach (var g in query)
            {
                float valProb = g.Count * invSum;
                IQueryable<object[]> subQuery =
                    from record in context.Query
                    where record[attr.Index] == g.Value
                    select record;
                subsetEntropy += valProb * Entropy(subQuery, targetAttr);
            }
            float gain = Entropy(context.Query, targetAttr) - subsetEntropy;
            //This is a heuristic, so cost = negative gain.
            return -gain;
        }
        public static float Entropy(IQueryable<object[]> query, DtAttribute targetAttr)
        {
            // Calculates the entropy of the given data set for the target attribute.
            float entropy = 0.0f;
            // Calculate the frequency of each of the values in the target attribute.
            var frequencies =
                from record in query
                group record by record[targetAttr.Index] into g
                select g.Count();
            // Calculate the entropy of the data for the target attribute
            float invSum = 1.0f / frequencies.Sum(x => x);
            foreach (var freq in frequencies)
            {
                entropy += (-freq * invSum) * (float)Math.Log(freq * invSum, 2.0f);
            }
            return entropy;
        }

        public static float Gini(IQueryable<object[]> query, DtAttribute attr)
        {
            // Calculates the entropy of the given data set for the target attribute.
            float entropy = 0.0f;
            // Calculate the frequency of each of the values in the target attribute.
            var frequencies =
                from record in query
                group record by record[attr.Index] into g
                select g.Count();
            // Calculate the entropy of the data for the target attribute
            float invSum = 1.0f / frequencies.Sum(x => x);
            foreach (var freq in frequencies)
            {
                float valProb = freq * invSum;
                entropy += (float)Math.Pow(valProb, 2);
            }
            return 1 - entropy;
        }
        public static float ClassificationError(IQueryable<object[]> query, DtAttribute attr)
        {
            // Calculates the entropy of the given data set for the target attribute.
            float entropy = 0.0f;
            // Calculate the frequency of each of the values in the target attribute.
            var frequencies =
                from record in query
                group record by record[attr.Index] into g
                select g.Count();
            float invSum = 1.0f / frequencies.Sum(x => x);
            var probabilities =
                from freq in frequencies
                select (freq * invSum);
            // Calculate the entropy of the data for the target attribute
            entropy = probabilities.Max();
            return 1 - entropy;
        }
        //
        public static float UniformHeuristic(DtNodeBuilder context, DtAttribute attr)
        {
            return 1.0f;
        }
    }
}
