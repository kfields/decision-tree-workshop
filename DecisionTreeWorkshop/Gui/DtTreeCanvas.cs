using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Xml;
using System.Windows.Markup;

using GraphLayout;

using DtWorkshop.ID3;

namespace DtWorkshop.GUI
{
    public class DtTreeCanvas : TreeCanvas, INameScope
    {
        public DtDocument Document;
        private Dictionary<string, object> Scope = new Dictionary<string, object>();
        //
        public DtTreeCanvas(){
            NameScope.SetNameScope(this, this);
        }
        public void Build()
        {
            if (Document.Tree.Root == null)
                return;
            TreeCanvasNode tnRoot = CreateElementNode(null, Document.Tree.Root);
        }
        public TreeCanvasNode CreateElementNode(TreeCanvasNode parent, DtElement element)
        {
            switch (element.Kind)
            {
                case DtElementKind.Branch:
                    return CreateBranchNode(parent, element);
                case DtElementKind.Edge:
                    return CreateEdgeNode(parent, element);
                case DtElementKind.Leaf:
                    return CreateLeafNode(parent, element);
            }
            return null;
        }
        public TreeCanvasNode CreateBranchNode(TreeCanvasNode parent, DtElement element)
        {
            DtBranch branch = element as DtBranch;
            TreeCanvasNode canvasNode = CreateNode(parent, branch);
            foreach (var edge in branch.Edges)
            {
                CreateEdgeNode(canvasNode, edge);
            }
            return canvasNode;
        }
        public TreeCanvasNode CreateEdgeNode(TreeCanvasNode parent, DtElement element)
        {
            DtEdge edge = element as DtEdge;
            TreeCanvasNode canvasNode = CreateNode(parent, element);
            CreateElementNode(canvasNode, edge.To);
            return canvasNode;
        }
        public TreeCanvasNode CreateLeafNode(TreeCanvasNode parent, DtElement element)
        {
            return CreateNode(parent, element);
        }
        public TreeCanvasNode CreateNode(TreeCanvasNode parent, DtElement element)
        {
            if (element == null)
                return null;
            //else
            /*Rectangle rect = new Rectangle();
            rect.Height = 32;
            rect.Width = 32;
            rect.Stroke = System.Windows.Media.Brushes.Black;
            rect.Fill = System.Windows.Media.Brushes.SkyBlue;*/
            //
            TextBlock text = new TextBlock();
            text.Text = element.ToString();
            if(element is DtBranch)
                text.Background = System.Windows.Media.Brushes.LightSteelBlue;
            else if (element is DtLeaf)
                text.Background = System.Windows.Media.Brushes.LightSeaGreen;
            //
            TreeCanvasNode canvasNode;
            if (parent == null)
                canvasNode = AddRoot(text);
            else
                canvasNode = AddNode(text, parent);
            return canvasNode;
        }
        #region INameScope Members

        object INameScope.FindName(string name)
        {
            return Scope[name];
            //throw new NotImplementedException();
        }

        void INameScope.RegisterName(string name, object scopedElement)
        {
            Scope[name] = scopedElement;
            //throw new NotImplementedException();
        }

        void INameScope.UnregisterName(string name)
        {
            Scope.Remove(name);
            //throw new NotImplementedException();
        }

        #endregion
    }
}
