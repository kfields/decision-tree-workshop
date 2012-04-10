using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DtWorkshop.ID3
{
    public delegate void OnRootChanged();

    public class DtTree
    {
        public event OnRootChanged RootChanged;
        private DtNode root;
        public DtNode Root { 
            get { return root; } 
            set { 
                root = value; 
                if(root != null)
                    RootChanged();
            } 
        }
    }
    //
    public struct DtAttribute
    {
        public string Name;
        public int Index;
        public DtAttribute(string name, int index)
        {
            Name = name;
            Index = index;
        }
        public override string ToString()
        {
            return Name;
        }
    }
    //
    public enum DtElementKind
    {
        Branch,
        Edge,
        Leaf
    }
    public abstract class DtElement
    {
        public DtElementKind Kind;
    }
    public class DtEdge : DtElement
    {
        public DtBranch From;
        public DtNode To;
        public object Condition;
        //
        public DtEdge(DtBranch from, object condition, DtNode to)
        {
            Kind = DtElementKind.Edge;
            From = from;
            Condition = condition;
            To = to;
        }
        public override string ToString()
        {
            return Condition.ToString();
        }
    }
    public abstract class DtNode : DtElement
    {
        public DtEdge Edge;
    }
    public class DtBranch : DtNode
    {
        public DtAttribute Attribute;
        public List<DtEdge> Edges = new List<DtEdge>();
        //
        public DtBranch(DtAttribute attribute)
        {
            Kind = DtElementKind.Branch;
            Attribute = attribute;
        }
        public void AddNode(object condition, DtNode node)
        {
            DtEdge edge = new DtEdge(this, condition, node);
            node.Edge = edge;
            Edges.Add(edge);
        }
        public override string ToString()
        {
            return Attribute.ToString();
        }
    }
    public class DtLeaf : DtNode
    {
        public object Value;
        //
        public DtLeaf(object value)
        {
            Kind = DtElementKind.Leaf;
            Value = value;
        }
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
