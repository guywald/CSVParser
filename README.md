CsvParser
=========

A simple CSV parser. 

Has a very simple API. You may iterate it using foreach or accessing a specific row via an indexer:

    CsvParser csv = new CsvParser(@"D:\TestCsvFile.csv");
    if (!csv.IsEmpty)
    {
        int count = csv.Count;
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
    
You may iterate it using foreach or access a specific row using an indexer.

You may also query the CSV using `WhereEqual`, `WhereGreaterThan` and `WhereLessThan`:

    List<CsvRow> matches = csv.WhereEquals("status", "OK");
    matches = csv.WhereGreaterThan("status", 200);
    matches = csv.WhereLessThan("status", 500);
    
Each query will return a `List<CsvRow>` where CsvRow is a dynamic object containing the CSV's fields.
