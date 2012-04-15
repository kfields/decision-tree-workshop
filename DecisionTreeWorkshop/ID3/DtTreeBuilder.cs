using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using System.Diagnostics;

namespace DtWorkshop.ID3
{
    public delegate void DtBuildEvent();

    public class DtTreeBuilder
    {
        public event DtBuildEvent BuildStarted = delegate { };
        public event DtBuildEvent BuildCanceled = delegate { };
        public event DtBuildEvent BuildFinished = delegate { };
        //
        public bool Prepruning = true;
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
            BuildFinished();
            Trace.WriteLine("Build Finished");
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
