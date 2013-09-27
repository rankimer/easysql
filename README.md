easysql
=======

Easy to use wrapper for common and basic SQL database functions


Usage:

* Creating a database object
```csharp
  EasySql db = new EasySql("Server=.\\sqlexpress;Database=USER;Trusted_Connection=True;");
```

* SELECT query
```csharp
            DataTable dt = db.ExecuteQuery("SELECT * FROM Employees");

            foreach (DataRow row in dt.Rows)
            {
                Console.WriteLine(String.Format("Name:{0}, Age:{1}", row[0], row[1]));
            }
```

* INSERT query
```csharp
  db.ExecuteNonQuery("INSERT INTO Employees VALUES ('Mark Zuckerberg', '28')");
```
