using System.Collections.Generic;
using CsvParser.Exceptions;
using CsvParser.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace CsvParser.Tests
{
    [TestClass]
    public class CsvParserUnitTests
    {
        [TestMethod]
        public void CsvParser_LineCountEquality_Calculated()
        {
            Parser.CsvParser parser = new Parser.CsvParser(@"C:\csv_parser_examples\TestCsvFile.csv");
            Assert.AreEqual(4, parser.Count, "There aren't 4 lines");
        }

        [TestMethod]
        public void CsvParser_CompleteRequestedAPI()
        {

            Parser.CsvParser csv = new Parser.CsvParser(@"C:\csv_parser_examples\TestCsvFile.csv");
            if (!csv.IsEmpty)
            {
                long count = csv.Count;
                for (int i = 0; i < csv.Count; ++i)
                {
                    dynamic row = csv[i];
                    Console.WriteLine("{0}: {1}", row.time, row.result);
                }
                foreach (CsvRow row in csv)
                {
                    Console.WriteLine("{0}: {1}", row["time"], row["result"]);
                }
            }

            List<CsvRow> matches = csv.WhereEquals("status", "OK");
            Assert.AreEqual(matches.Count, 0);

            matches = csv.WhereGreaterThan("status", 200);
            Assert.AreEqual(matches.Count, 3);

            matches = csv.WhereLessThan("status", 500);
            Assert.AreEqual(matches.Count, 3);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void CsvParser_PathDosentExist_FileNotFoundException()
        {
            Parser.CsvParser parser = new Parser.CsvParser(@"NotFound.csv");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CsvParser_EmptyCsvFileName_ArgumentNullException()
        {
            Parser.CsvParser parser = new Parser.CsvParser("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CsvParser_NullCsvFileName_ArgumentNullException()
        {
            Parser.CsvParser parser = new Parser.CsvParser(null);
        }

        [TestMethod]
        public void CsvParser_ShouldParseFifthRow()
        {
            Parser.CsvParser parser = new Parser.CsvParser(@"C:\csv_parser_examples\TestCsvFile.csv");
            dynamic fifthLine = parser[4];
            Assert.AreEqual(fifthLine.time, "18:04");
            Assert.AreEqual(fifthLine.status, "500");
            Assert.AreEqual(fifthLine.result, "Internal Server Error");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CsvParser_FetchAnUnreachableIndex_ArugmentOutOfRangeException()
        {
            Parser.CsvParser parser = new Parser.CsvParser(@"C:\csv_parser_examples\TestCsvFile.csv");
            dynamic tenthLine = parser[10];
        }

        [TestMethod]
        public void CsvParser_IterateUsingIndexer()
        {
            Parser.CsvParser parser = new Parser.CsvParser(@"C:\csv_parser_examples\TenThounsandRowCsv.csv");
            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < parser.Count; i++)
            {
                dynamic row = parser[i];
                Console.WriteLine("Line Count: {0} - {1}: {2}", i, row.time, row.status);
            }

            sw.Stop();
            Debug.WriteLine(sw.Elapsed);

            Assert.AreEqual(parser.Count, 9999);

        }

        [TestMethod]
        public void CsvParser_IterateUsingEnumerator()
        {
            Parser.CsvParser parser = new Parser.CsvParser(@"C:\csv_parser_examples\TenThounsandRowCsv.csv");
            int index = 0;
            var sw = new Stopwatch();
            sw.Start();
            foreach (var row in parser)
            {
                Debug.WriteLine("Index: {0} - {1}: {2}", index, row["time"], row["status"]);
                index++;
            }
            sw.Stop();
            Debug.WriteLine(sw.Elapsed);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CsvParser_AccessViaOutOfRangeIndexer_ArgumentOutOfRangeException()
        {
            Parser.CsvParser parser = new Parser.CsvParser(@"C:\csv_parser_examples\TenThounsandRowCsv.csv");
            dynamic row = parser[int.MaxValue];
        }

        [TestMethod]
        [ExpectedException(typeof(CsvParseException))]
        public void CsvParser_MoreHeadersThanRows_CsvParseException()
        {
            Parser.CsvParser parser = new Parser.CsvParser(@"D:\CsvEmptyLine.csv");
            foreach (var row in parser)
            {
                Debug.WriteLine(row["time"]);
            }
        }
    }
}
