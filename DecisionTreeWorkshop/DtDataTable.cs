using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace DtWorkshop.ID3
{
    public class DtDataTable : DataTable
    {
        public object[][] Examples;
        Dictionary<object, object>[] HashSets;
        //
        public void ReadCsv(string filePath)
        {
            ReadDelimited(filePath, ',');
        }
        public void ReadTsv(string filePath)
        {
            ReadDelimited(filePath, '\t');
        }
        public void ReadDelimited(string filePath, char delimiter)
        {
            StreamReader reader = new StreamReader(filePath);
            //
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            TableName = fileName;
            DtDelimitedImporter importer = new DtDelimitedImporter(this, filePath, delimiter);
            importer.Read();
            //
            HashSets = new Dictionary<object, object>[Columns.Count];
            for (int i = 0; i < Columns.Count; ++i )
            {
                HashSets[i] = new Dictionary<object, object>();
            }
            //
            Update();
        }
        private void Update()
        {
            List<object[]> examples = new List<object[]>();
            //
            foreach (DataRow row in Rows)
            {
                object[] example = new object[Columns.Count];
                int index = 0;
                foreach (object item in row.ItemArray)
                {
                    object condition;
                    if(!HashSets[index].TryGetValue(item, out condition)){
                        condition = item;
                        HashSets[index][item] = condition;
                    }
                    example[index] = condition;
                    ++index;
                }
                examples.Add(example);
            }
            Examples = examples.ToArray();
        }
        public string[] GetAttributeNames()
        {
            List<string> attributeList = new List<string>();
            foreach (DataColumn column in Columns)
            {
                string name = column.ColumnName;
                attributeList.Add(column.ColumnName);
            }
            return attributeList.ToArray();
        }
        public DtAttribute GetAttribute(int index)
        {
            string name = Columns[index].ColumnName;
            return new DtAttribute(name, index);
        }
        public DtAttribute GetAttribute(string name)
        {
            int index = Columns.IndexOf(name);
            return new DtAttribute(name, index);
        }
        public DtAttribute[] GetAttributes()
        {
            List<DtAttribute> attributeList = new List<DtAttribute>();
            foreach (DataColumn column in Columns)
            {
                string name = column.ColumnName;
                int index = column.Ordinal;
                attributeList.Add(new DtAttribute(name, index));
            }
            return attributeList.ToArray();
        }
    }

    public static class DtDataTableHelper
    {
        public static IQueryable<DataRow> AsQueryable(this DataRowCollection This)
        {
            return This.Cast<DataRow>().AsQueryable();
        }
    }

}
