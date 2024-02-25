using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cathei.BakingSheet.Internal;
using ExcelDataReader;
using System.IO;
using System.Data;
using System;
using System.ComponentModel;
using RatVR.ExcelData;

namespace Cathei.BakingSheet
{
    public class ExcelSceneFileHandler
    {
        // Scene description takes place over a excel file with multiple tables.
        // The table "Environment" contains a x-y-Grid with keywords. These keywords should match the objects listed under hyperparameters in the object definition section
        // The table "Hyperparameters" contains the object definition and scene metadata information

        private string path;
        private Dictionary<string, Page> pages;

        public ExcelSceneFileHandler(string path)
        {
            this.path = path;
            pages = new Dictionary<string, Page>();
            LoadData();
        }
        private class Page
        {
            private DataTable _table;

            public string SubName { get; }

            public Page(DataTable table, string subName)
            {
                _table = table;
                SubName = subName;
            }

            public string GetCell(int col, int row)
            {
                if (col >= _table.Columns.Count || row >= _table.Rows.Count)
                    return null;

                return _table.Rows[row][col].ToString();
            }
        }

        private void LoadData()
        {
            // return File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            //var files = _fileSystem.GetFiles(_loadPath, _extension);

            pages.Clear();

            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var dataset = reader.AsDataSet(new ExcelDataSetConfiguration
                {
                    UseColumnDataType = false,
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = false,
                    }
                });

                for (int i = 0; i < dataset.Tables.Count; ++i)
                {
                    var table = dataset.Tables[i];
                    var tableName = table.TableName;

                    if (tableName.StartsWith(Config.Comment))
                        continue;

                    var (sheetName, subName) = Config.ParseSheetName(tableName);

                    pages.Add(sheetName, new Page(table, subName));

                }
            }

            //return Task.FromResult(true);
        }

        public Dictionary<Tuple<int, int>, string> ScenePlacementCoords()
        {
            Dictionary<Tuple<int, int>, string> scenePlacement = new Dictionary<Tuple<int, int>, string>();

            if (!pages.ContainsKey("Environment")) throw new Exception("Environment table missing!!");

            for (int x = 1; x <= 200; ++x) // TODO change to real environment size
            {
                for (int y = 1; y <= 200; ++y)
                {
                    var cellContent = pages["Environment"].GetCell(x, y);
                    if (cellContent == null) continue;
                    if (cellContent == "x") break;

                    if (cellContent != null)
                    {
                        scenePlacement.Add(new Tuple<int, int>(x - 1, y - 1), cellContent);
                    }
                }
            }
            return scenePlacement;
        }

        public Dictionary<string, List<Vector2>> ScenePlacement()
        {
            Dictionary<string, List<Vector2>> result = new Dictionary<string, List<Vector2>>();

            Dictionary<Tuple<int, int>, string> scenePlacementCoords = ScenePlacementCoords();

            foreach (var pair in scenePlacementCoords)
            {
                if (result.ContainsKey(pair.Value))
                {
                    result[pair.Value].Add(new Vector2(pair.Key.Item1, pair.Key.Item2));
                }
                else
                {
                    result[pair.Value] = new List<Vector2> { new Vector2(pair.Key.Item1, pair.Key.Item2) };
                }
            }
            return result;
        }

       public List<ExcelObjectData> GetExcelSceneObjects()
       {
            List<ExcelObjectData> excelObjects = new List<ExcelObjectData>();
            if (!pages.ContainsKey("Hyperparameters")) throw new Exception("Hyperparameters table missing!!");

            int row = 1;
            while (true)
            {
                // check if there is still an entry:
                var cellContent = pages["Hyperparameters"].GetCell(0, row);
                if (cellContent == null || cellContent == "") break;
                
                List<string> rowValues = new List<string>();
                
                for (int col=0; col<8; ++col)
                {
                    cellContent = pages["Hyperparameters"].GetCell(col, row);
                    if (cellContent == null)
                    {
                        rowValues.Add("");
                    }
                    else
                    {
                        rowValues.Add(cellContent);
                    }
                }
                row++;
                excelObjects.Add(new ExcelObjectData(rowValues));
            }
            return excelObjects;
        }


        public ExcelSceneMetaData GetExcelSceneMetaData()
        {
            if (!pages.ContainsKey("Hyperparameters")) throw new Exception("Hyperparameters table missing!!");
            var hyperparams = pages["Hyperparameters"];

            Vector2 size = new Vector2(int.Parse(hyperparams.GetCell(11,1)), int.Parse(hyperparams.GetCell(12,1)));
            int deathzone = int.Parse(hyperparams.GetCell(11, 3));
            float baseLength = float.Parse(hyperparams.GetCell(11, 2), System.Globalization.CultureInfo.InvariantCulture);
            Vector2 startLocation = new Vector2(int.Parse(hyperparams.GetCell(11, 4)), int.Parse(hyperparams.GetCell(12, 4)));
            Vector2 agentLocation = new Vector2(int.Parse(hyperparams.GetCell(11, 5)), int.Parse(hyperparams.GetCell(12, 5)));
            int lengthFlash = int.Parse(hyperparams.GetCell(11, 7));
            int lengthSound = int.Parse(hyperparams.GetCell(11, 8));

            ExcelWallData topWall = new ExcelWallData(float.Parse(hyperparams.GetCell(16, 1), System.Globalization.CultureInfo.InvariantCulture.NumberFormat), hyperparams.GetCell(1, 17));
            ExcelWallData rightWall = new ExcelWallData(float.Parse(hyperparams.GetCell(16, 2), System.Globalization.CultureInfo.InvariantCulture.NumberFormat), hyperparams.GetCell(2, 17));
            ExcelWallData botWall = new ExcelWallData(float.Parse(hyperparams.GetCell(16, 3), System.Globalization.CultureInfo.InvariantCulture.NumberFormat), hyperparams.GetCell(3, 17));
            ExcelWallData leftWall = new ExcelWallData(float.Parse(hyperparams.GetCell(16, 4), System.Globalization.CultureInfo.InvariantCulture.NumberFormat), hyperparams.GetCell(4, 17));

            bool cylinder = bool.Parse(hyperparams.GetCell(16, 6));

            return new ExcelSceneMetaData(size, baseLength, deathzone, startLocation, agentLocation, lengthFlash, lengthSound, topWall, rightWall, botWall, leftWall, cylinder);
        }
    }
}