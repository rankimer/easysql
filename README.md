easysql
=======

Easy to use wrapper for common and basic SQL database functions

---
## How-to use in your project

- Copy/Add [EasySql.cs](/src/EasySql.cs) to your project
- Locate the following [line](/src/EasySql.cs#L22) and un-comment the database you will use

```csharp
#region Select your DB type
#define SQLSERVER
//#define MSACCESS
//#define MYSQL
//#define SQLLITE
//#define ORACLE
#endregion
```

- Use as described in the code below

---
## Code Usage
* Include namespace
```csharp
using Kimerran.EasySql;
```

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
