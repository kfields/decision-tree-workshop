using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using System.Diagnostics;

namespace DtWorkshop.ID3
{
    public class DtNodeBuilder
    {
        public DtTreeBuilder TreeBuilder;
        public IQueryable<object[]> Query;
        //TODO:Parallel implementation
        //public object[][] Data;
        public DtAttribute Attribute;
        public DtAttribute[] Attributes;
        public bool Canceled
        {
            get
            {
                if (TreeBuilder.Canceled)
                    return true;
                return false;
            }
        }
        public bool Finished { 
            get {
                if (Attributes.Length == 0)
                    return true;
                IQueryable<object> vals =
                    from record in Query
                    select record[TreeBuilder.TargetAttr.Index];
                IQueryable<object> distinctVals = vals.Distinct();
                if (distinctVals.Count() == 1)
                    return true;
                return false;
                }
        }
        public DtNodeBuilder(DtTreeBuilder treeBuilder, IQueryable<object[]> query, DtAttribute attribute, DtAttribute[] attributes)
        {
            TreeBuilder = treeBuilder;
            //TODO:Parallel implementation, need to clone data, can't use same query.
            //Data = query.ToArray();
            //Query = Data.AsQueryable();
            Query = query;
            Attribute = attribute;
            Attributes = attributes.Except(new DtAttribute[] { Attribute }).ToArray();
        }
        public DtNode Build()
        {
            if (Canceled)
                return null;
            if (Finished)
                return new DtLeaf(Query.First()[TreeBuilder.TargetAttr.Index]);
            // Else
            // Choose the next best attribute to best classify our data
            DtAttribute best = SelectBest(TreeBuilder.Heuristic);
            // Create a new decision tree/node with the best attribute.
            DtBranch tree = new DtBranch(best);
            // Create a new decision tree/sub-node for each of the values in the best attribute field
            IQueryable<object> vals =
                from record in Query
                select record[best.Index];
            IQueryable<object> distinctVals = vals.Distinct();
            //
            foreach (var val in distinctVals)
            {
                if (Canceled)
                    return null;
                // Create a subtree for the current value under the "best" field
                DtNodeBuilder child = CreateNodeBuilder(this, best, val);
                DtNode subtree = child.Build();
                // Add the new subtree to our new tree/node we just created.
                if(subtree != null)
                    tree.AddNode(val, subtree);
            }
            return tree;
        }
        public DtAttribute SelectBest(DtHeuristic heuristic)
        {
            // Cycles through all the attributes and returns the attribute with the lowest entropy.
            float lowestCost = float.MaxValue;
            DtAttribute bestAttr = new DtAttribute();

            foreach (var attr in Attributes)
            {
                float cost = heuristic(this, attr);
                if (cost <= lowestCost)
                {
                    bestAttr = attr;
                    lowestCost = cost;
                }
            }
            return bestAttr;
        }
        DtNodeBuilder CreateNodeBuilder(DtNodeBuilder parent, DtAttribute attr, object value)
        {
            // Returns a list of all the records in <data> with the value of <attr>
            // matching the given value.
            IQueryable<object[]> records =
                from record in parent.Query
                where record[attr.Index] == value
                select record;

            DtNodeBuilder builder = new DtNodeBuilder(parent.TreeBuilder, records, attr, parent.Attributes);
            return builder;
        }
    }
    public class DtTreeBuilder
    {
        public bool Prepruning = true;
        public IQueryable<object[]> Query;
        public DtAttribute TargetAttr;
        public DtAttribute[] Attributes;
        public DtHeuristic Heuristic;
        Thread thread;
        public bool Running = false;
        public bool Canceled = false;
        DtTree Tree;
        //
        public DtTreeBuilder(DtTree tree, IQueryable<object[]> query, DtAttribute targetAttr, DtAttribute[] attributes, DtHeuristicKind heuristicKind)
        {
            Tree = tree;
            Query = query;
            TargetAttr = targetAttr;
            Attributes = attributes;
            //Heuristic = DtHeuristics.GainHeuristic;
            Heuristic = DtHeuristics.Instances[(int)heuristicKind];
        }
        public void Build()
        {
            thread = new Thread(new ThreadStart(() => { this.ThreadedBuild(); }));
            thread.Start();
        }
        public void ThreadedBuild()
        {
            Running = true;
            Trace.WriteLine("Build Started");
            DtNodeBuilder child = new DtNodeBuilder(this, Query, TargetAttr, Attributes);
            DtNode root = child.Build();
            Tree.Root = root;
            if(root != null)
                Trace.WriteLine("Build Finished");
        }
        public void Cancel()
        {
            Canceled = true;
            Running = false;
            Trace.WriteLine("Build Canceled");
        }
    }
}
