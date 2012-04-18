using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DtWorkshop.Plugin.ID3
{
    public delegate void OnRootChanged();

    public class DtTree
    {
        public event OnRootChanged RootChanged = delegate { };
        private DtBranch root;
        public DtBranch Root
        { 
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
        public DtBranch Parent
        {
            get
            {
                if (Edge == null)
                    return null;
                //else
                return Edge.From;
            }
        }
        public bool IsBranch
        {
            get { return Kind == DtElementKind.Branch; }
        }    
        public bool IsLeaf
        {
            get { return Kind == DtElementKind.Leaf; }
        }    
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
        public void AddChild(object condition, DtNode node)
        {
            DtEdge edge = new DtEdge(this, condition, node);
            node.Edge = edge;
            Edges.Add(edge);
        }
        public void AddChild(DtNode node)
        {
            DtEdge edge = node.Edge;
            edge.From = this;
            Edges.Add(edge);
        }
        public void RemoveChild(DtNode node)
        {
            DtEdge edge = Edges.Find( (e) => e.To == node);
            Edges.Remove(edge);
        }
        public IEnumerable<DtNode> Children
        {
            get
            {
                return new ChildEnumerable(this);
            }
        }
        public bool HasSingleParent { get { return Parent != null; } }
        public bool HasSingleChild
        {
            get
            {
                if (Edges.Count != 1)
                    return false;
                /*try
                {
                    DtBranch child = Children.Single() as DtBranch;
                }
                catch (System.InvalidOperationException)
                {
                    //Console.WriteLine("The collection does not contain exactly one element.");
                    return false;
                }*/
                return true;
            }
        }
        /*public bool HasLeafChild
        {
            get
            {
                if (!Edges.Any())
                    return false;

                if (Children.First().Kind != DtElementKind.Leaf)
                    return false;
                return true;
            }
        }*/
        public bool HasLeafChildren
        {
            get
            {
                if (!Edges.Any())
                    return false;
                foreach (DtNode child in Children)
                    if (!child.IsLeaf)
                        return false;
                return true;
            }
        }
        public override string ToString()
        {
            return Attribute.ToString();
        }
    }
    public class ChildEnumerable : IEnumerable<DtNode>
    {
        DtBranch Parent;
        public ChildEnumerable(DtBranch parent){
            Parent = parent;
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public IEnumerator<DtNode> GetEnumerator()
        {
            foreach (DtEdge edge in Parent.Edges)
            {
                yield return edge.To;
            }
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
