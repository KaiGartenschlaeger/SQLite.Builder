# SQLite.Builder
Ermöglicht das erzeugen von Objekte mit Hilfe des Builder-Patterns.

# HowTo
~~~
var table = new TableBuilder("Test")
    .WithColumn("Id", SQLiteDbType.Integer)
    .WithColumn("ZipCode", SQLiteDbType.Text)
    .WithColumn("Town", SQLiteDbType.Text)
    .WithPrimaryKey(k => k.WithColumns("ZipCode", "Town"))
    .Build();

var genertator = new SqlGenerator();
var sql = genertator.Generate(table);
~~~
