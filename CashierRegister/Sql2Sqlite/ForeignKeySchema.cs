using System;
using System.Collections.Generic;
using System.Text;

namespace CashierRegister.Sql2Sqlite
{
	public class ForeignKeySchema
	{
	    public string TableName;

		public string ColumnName;

		public string ForeignTableName;

		public string ForeignColumnName;

	    public bool CascadeOnDelete;

	    public bool IsNullable;
	}
}
