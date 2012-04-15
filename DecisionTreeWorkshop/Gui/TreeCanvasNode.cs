using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GraphLayout;

namespace DtWorkshop.GUI
{
	public class TreeCanvasNode : ContentControl, ITreeNode
	{
		#region Dependency Properties
		#region Collapsed
		public static readonly DependencyProperty CollapsedProperty =
			DependencyProperty.Register(
				"Collapsed",
				typeof(bool),
				typeof(TreeCanvasNode),
				new FrameworkPropertyMetadata(
					false,
					FrameworkPropertyMetadataOptions.AffectsMeasure |
					FrameworkPropertyMetadataOptions.AffectsArrange |
					FrameworkPropertyMetadataOptions.AffectsParentMeasure |
					FrameworkPropertyMetadataOptions.AffectsParentArrange |
					FrameworkPropertyMetadataOptions.AffectsRender |
					0,
					CollapsePropertyChange,
					CollapsePropertyCoerce,
					true
				),
				null
			);

		private static object CollapsePropertyCoerce(DependencyObject d, object value)
		{
			TreeCanvasNode tn = (TreeCanvasNode)d;
			bool fCollapsed = (bool)value;
			if (!tn.Collapsible)
			{
				fCollapsed = false;
			}
			return fCollapsed;
		}

		static public void CollapsePropertyChange(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			TreeCanvasNode tn = o as TreeCanvasNode;
			if (tn != null && tn.Collapsible)
			{
				bool fCollapsed = ((bool)e.NewValue);
				foreach (TreeCanvasNode tnCur in LayeredTreeDraw.VisibleDescendants<TreeCanvasNode>(tn))
				{
					tnCur.Visibility = fCollapsed ? Visibility.Hidden : Visibility.Visible;
				}
			}
		}

		public bool Collapsed
		{
			get { return (bool)GetValue(CollapsedProperty); }
			set { SetValue(CollapsedProperty, value); }
		}
		#endregion

		#region Collapsible
		public static readonly DependencyProperty CollapsibleProperty =
			DependencyProperty.Register(
				"Collapsible",
				typeof(bool),
				typeof(TreeCanvasNode),
				new FrameworkPropertyMetadata(
					true,
					FrameworkPropertyMetadataOptions.AffectsMeasure |
					FrameworkPropertyMetadataOptions.AffectsArrange |
					FrameworkPropertyMetadataOptions.AffectsParentMeasure |
					FrameworkPropertyMetadataOptions.AffectsParentArrange |
					FrameworkPropertyMetadataOptions.AffectsRender |
					0,
					CollapsiblePropertyChange,
					null,
					true
				),
				null
			);

		static public void CollapsiblePropertyChange(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			TreeCanvasNode tn = o as TreeCanvasNode;
			if (((bool)e.NewValue) == false && tn != null)
			{
				tn.Collapsed = false;
			}
		}

		public bool Collapsible
		{
			get { return (bool)GetValue(CollapsibleProperty); }
			set { SetValue(CollapsibleProperty, value); }
		}
		#endregion

		#region TreeParent
		public static readonly DependencyProperty TreeParentProperty =
			DependencyProperty.Register(
				"TreeParent",
				typeof(string),
				typeof(TreeCanvasNode),
				new FrameworkPropertyMetadata(
					null,
					FrameworkPropertyMetadataOptions.AffectsMeasure |
					FrameworkPropertyMetadataOptions.AffectsArrange |
					FrameworkPropertyMetadataOptions.AffectsParentMeasure |
					FrameworkPropertyMetadataOptions.AffectsParentArrange |
					FrameworkPropertyMetadataOptions.AffectsRender |
					0,
					null,
					null,
					true
				),
				null
			);

		public static TreeCanvasNode GetParentElement(TreeCanvasNode tn)
		{
			TreeCanvas tc;
			TreeCanvasNode tnParent;

			if (tn == null)
			{
				return null;
			}
			tc = tn.Parent as TreeCanvas;
			if (tc == null)
			{
				return null;
			}
			string strParent = tn.TreeParent;
			if (strParent == null)
			{
				return null;
			}

			tnParent = tc.FindName(strParent) as TreeCanvasNode;
			if (tnParent == null)
			{
				return null;
			}
			return tnParent;
		}

		public string TreeParent
		{
			get { return (string)GetValue(TreeParentProperty); }
			set { SetValue(TreeParentProperty, value); }
		}
		#endregion
		#endregion

		#region Constructors
		public TreeCanvasNode()
		{
			TreeChildren = new TreeNodeGroup();
			Background = Brushes.Transparent;
		}

		static TreeCanvasNode()
		{
		}
		#endregion

		#region Parenting
		internal void ClearParent()
		{
			TreeChildren = new TreeNodeGroup();
		}

		internal bool SetParent()
		{
			TreeCanvasNode tn = GetParentElement(this);
			if (tn == null)
			{
				return false;
			}
			tn.TreeChildren.Add(this);
			return true;
		}
		#endregion

		#region ITreeNode Members
		public object PrivateNodeInfo { get; set; }

		public TreeNodeGroup TreeChildren { get; private set; }

		internal Size NodeSize()
		{
			return DesiredSize;
		}

		public double TreeHeight
		{
			get
			{
				return NodeSize().Height;
			}
		}

		public double TreeWidth
		{
			get
			{
				return NodeSize().Width;
			}
		}
		#endregion
	}
}
