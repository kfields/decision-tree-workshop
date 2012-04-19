using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using System.Diagnostics;

namespace DtWorkshop.Plugin.ID3
{
    public delegate void DtBuildEvent();

    public enum DtTaskStatus
    {
        Running,
        Canceled,
        Succeeded,
        Failed
    }
    public class DtTreeBuilder
    {
        public event DtBuildEvent BuildStarted = delegate { };
        public event DtBuildEvent BuildCanceled = delegate { };
        public event DtBuildEvent BuildFinished = delegate { };
        //
        public bool Prepruning = true;
        public bool Postpruning = true;
        public IQueryable<object[]> Query;
        public DtAttribute TargetAttr;
        public DtAttribute[] Attributes;
        public DtHeuristic Heuristic;
        Thread thread;
        public bool Running = false;
        public bool Canceled = false;
        public DtTree Tree;
        //
        public DtTreeBuilder()
        {
        }
        public void Build(DtTree tree, IQueryable<object[]> query, DtAttribute targetAttr, DtAttribute[] attributes, DtHeuristicKind heuristicKind)
        {
            Tree = tree;
            Query = query;
            TargetAttr = targetAttr;
            Attributes = attributes;
            //Heuristic = DtHeuristics.GainHeuristic;
            Heuristic = DtHeuristics.Instances[(int)heuristicKind];
            //
            BuildStarted();
            //
            thread = new Thread(new ThreadStart(() => { this.ThreadedBuild(); }));
            thread.Start();
        }
        public void ThreadedBuild()
        {
            Running = true;
            Trace.WriteLine("Build Started");
            //
            //DtAttribute[] childAttributes = Attributes.Except(new DtAttribute[] { TargetAttr }).ToArray();
            //List<DtAttribute> _childAttributes = Attributes.Except(new DtAttribute[] { TargetAttr }).ToList();
            //_childAttributes.Add(TargetAttr);
            DtAttribute[] childAttributes = Attributes;
            DtNodeBuilder child = new DtNodeBuilder(this, null, Query, null, childAttributes);
            child.Build();
            if (Canceled)
                return;
            //else
            if (Postpruning)
                Postprune();
            BuildFinished();
            Trace.WriteLine("Build Finished");
        }
        private struct Replacement
        {
            public DtNode ToRemove, ToAdd;
            public Replacement(DtNode toRemove, DtNode toAdd)
            {
                ToRemove = toRemove;
                ToAdd = toAdd;
            }
        }
        private void Postprune()
        {
            List<Replacement> replacements = new List<Replacement>();
            Postprune(Tree.Root, replacements);
            foreach (Replacement replacement in replacements)
            {
                DtBranch removeParent = replacement.ToRemove.Parent;
                removeParent.RemoveChild(replacement.ToRemove);

                DtBranch addParent = replacement.ToAdd.Parent;
                addParent.RemoveChild(replacement.ToAdd);

                replacement.ToAdd.Edge = replacement.ToRemove.Edge;
                replacement.ToAdd.Edge.To = replacement.ToAdd;

                removeParent.AddChild(replacement.ToAdd);
            }
        }
        private DtBranch Postprune(DtBranch branch, List<Replacement> replacements)
        {
            DtBranch bottom = branch;
            //
            if (branch.HasLeafChildren)
                return branch;
            else if (branch.HasSingleChild)
            {
                DtBranch child = branch.Children.Single() as DtBranch;
                bottom = Postprune(child, replacements);
                replacements.Add(new Replacement(branch, bottom));
            }
            else
                foreach (DtNode child in branch.Children)
                    if(child.Kind == DtElementKind.Branch)
                        Postprune(child as DtBranch, replacements);
            //
            return bottom;
        }
        public void Cancel()
        {
            Canceled = true;
            Running = false;
            BuildCanceled();
            Trace.WriteLine("Build Canceled");
        }
    }
}
