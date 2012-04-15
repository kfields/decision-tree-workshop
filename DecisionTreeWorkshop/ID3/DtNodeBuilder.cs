using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Data;
using System.Threading;
using System.Diagnostics;

namespace DtWorkshop.ID3
{
    public class DtNodeBuilder
    {
        public DtTreeBuilder TreeBuilder;
        private DtBranch ParentNode;
        public IQueryable<object[]> Query;
        //TODO:Parallel implementation
        public object[][] Data;
        public object Value;
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
        public bool IsLeaf
        {
            get
            {
                if (Attributes.Length == 0)
                    return true;
                if(!Query.Any())
                    return true;
                //Prune if all of the records have the same target-attribute value.
                if (TreeBuilder.Prepruning)
                {
                    IQueryable<object> vals =
                        from record in Query
                        select record[TreeBuilder.TargetAttr.Index];
                    IQueryable<object> distinctVals = vals.Distinct();
                    if (distinctVals.Count() == 1)
                        return true;
                }
                //
                return false;
            }
        }
        public DtNodeBuilder(DtTreeBuilder treeBuilder, DtBranch parentNode, IQueryable<object[]> query, object value, DtAttribute[] attributes)
        {
            TreeBuilder = treeBuilder;
            ParentNode = parentNode;
            //TODO:Parallel implementation, need to clone data, can't use same query.
            //Data = query.ToArray();
            //Query = Data.AsQueryable();
            Query = query;
            Value = value;
            Attributes = attributes;
        }
        public void Build()
        {
            if (Canceled)
                return ;
            if (IsLeaf)
            {
                object result = Query.First()[TreeBuilder.TargetAttr.Index];
                //object result = Query.Single()[TreeBuilder.TargetAttr.Index];
                if (ParentNode != null)
                    ParentNode.AddNode(Value, new DtLeaf(result));
                return;
            }
            // Else
            // Choose the next best attribute to best classify our data
            Attribute = SelectBest(TreeBuilder.Heuristic);
            // Create a new decision tree/node with the best attribute.
            DtBranch tree = new DtBranch(Attribute);

            if (ParentNode != null)
                ParentNode.AddNode(Value, tree);
            else
                TreeBuilder.Tree.Root = tree;
            //
            DtAttribute[] childAttributes = Attributes.Except(new DtAttribute[] { Attribute }).ToArray();

            // Create a new decision tree/sub-node for each of the values in the best attribute field
            IQueryable<object> vals =
                from record in Query
                select record[Attribute.Index];
            IQueryable<object> distinctVals = vals.Distinct();
            //
            foreach (var val in distinctVals)
            {
                if (Canceled)
                    return;
                // Returns a list of all the records in <data> with the value of <attr>
                // matching the given value.
                IQueryable<object[]> matchingRecords =
                    from record in Query
                    where record[Attribute.Index] == val
                    select record;

                DtNodeBuilder child = new DtNodeBuilder(TreeBuilder, tree, matchingRecords, val, childAttributes);

                // Create a subtree for the current value under the "best" field
                child.Build();
            }
        }
        public DtAttribute SelectBest(DtHeuristic heuristic)
        {
            // Cycles through all the attributes and returns the attribute with the lowest entropy.
            float lowestCost = float.MaxValue;
            DtAttribute bestAttr = new DtAttribute();

            foreach (var attr in Attributes)
            {
                float cost = 0;
                if (attr.Index == TreeBuilder.TargetAttr.Index)
                    cost = float.MaxValue;
                else
                    cost = heuristic(this, attr);
                if (cost <= lowestCost)
                {
                    bestAttr = attr;
                    lowestCost = cost;
                }
            }
            return bestAttr;
        }
    }
}
