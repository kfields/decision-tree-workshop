using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace DtWorkshop
{
    public class DtDelimitedImporter
    {
        StreamReader reader;
        DataTable Table;
        string FilePath;
        char[] Delimiter;
        //
        public DtDelimitedImporter(DataTable table, string filePath, char delimiter)
        {
            Table = table;
            FilePath = filePath;
            Delimiter = new char[] { delimiter };
        }
        public void Read()
        {
            reader = new StreamReader(FilePath);
            //
            ReadColumnNames();
            ReadColumnTypes();
            ReadDataRows();
            //
            reader.Close();
        }
        private void ReadColumnNames()
        {
            string[] columnNames = ReadItems();
            foreach (var columnName in columnNames)
            {
                Table.Columns.Add(columnName);
            }
        }
        private void ReadColumnTypes()
        {
            string[] items = ReadItems();
            List<Type> types = new List<Type>();
            for (int t = 0; t < items.Length; ++t)
            {
                Table.Columns[t].DataType = ReadColumnType(items[t]);
            }
            ReadRow(items);
        }
        private Type ReadColumnType(string item)
        {
            object sample = null;
            try
            {
                sample = float.Parse(item);
            }
            catch (FormatException)
            {
                try
                {
                    sample = int.Parse(item);
                }
                catch (FormatException)
                {
                    try
                    {
                        sample = bool.Parse(item);
                    }
                    catch (FormatException)
                    {
                        sample = item;
                    }
                }
            }
            return sample.GetType();
        }
        private void ReadDataRows()
        {
            while (ReadRow(ReadItems()) != null)
            {
            }
        }
        private DataRow ReadRow(string[] items)
        {
            if (items == null)
                return null;
            //
            DataRow row = Table.NewRow();
            for (int t = 0; t < items.Length; ++t)
            {
                row[t] = items[t];
            }
            Table.Rows.Add(row);
            //
            return row;
        }
        private string[] ReadItems()
        {
            string[] items;
            string line = reader.ReadLine();
            if (line == null)
                return null;
            items = line.Split(Delimiter);
            return items;
        }
    }
}
