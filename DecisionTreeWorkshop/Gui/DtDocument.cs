using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using DtWorkshop.ID3;

namespace DtWorkshop.GUI
{
    public class DtDocument
    {
        //
        public string FilePath;
        public string FileName;
        public DtDataTable DataTable = new DtDataTable();
        public DtTree Tree = new DtTree();
        public DtTreeBuilder TreeBuilder = new DtTreeBuilder();
        //
        List<DtPage> Pages = new List<DtPage>();
        //
        public void Load(string filePath)
        {
            FilePath = filePath;
            FileName = Path.GetFileNameWithoutExtension(filePath);
            //DataTable.ReadCsv(filePath);
            DataTable.ReadTsv(filePath);
            RefreshPages(RefreshKind.Load);
        }
        public void Build(DtHeuristicKind heuristicKind, string _targetAttr)
        {
            DtAttribute targetAttr = DataTable.GetAttribute(_targetAttr);
            DtAttribute[] attributes = DataTable.GetAttributes();
            TreeBuilder.Build(Tree, DataTable.Examples.AsQueryable(), targetAttr, attributes, heuristicKind);
            RefreshPages(RefreshKind.Total);
        }
        public void Close()
        {
            RefreshPages(RefreshKind.Final);
            //TODO:  Fix and call to TerminateThreads?
            //CancelBuild();
        }
        public void CancelBuild()
        {
            if (TreeBuilder != null)
            {
                TreeBuilder.Cancel();
            }
        }
        public void AddPage(DtPage page)
        {
            Pages.Add(page);
        }
        public void RefreshPages(RefreshKind kind)
        {
            foreach (var page in Pages)
            {
                page.Refresh(kind);
            }
        }
    }
}
