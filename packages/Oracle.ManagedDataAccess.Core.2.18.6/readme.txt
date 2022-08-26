Oracle.ManagedDataAccess.Core NuGet Package 2.18.6 README
=========================================================

March 2019

For documentation, go to https://docs.oracle.com/en/database/oracle/oracle-data-access-components/18.3/

Bug Fixes since Oracle.ManagedDataAccess.Core 2.18.5
====================================================
Bug 29242017 - PARAMETER IN NVL FUNCTION RESULTS IN READONLY COLUMNS IN DATASET
Bug 29412269 - ODP.NET DRIVER DOES NOT SHARE CURSORS FOR EXECUTIONS WITH NULL / NON-NULL PARAMETER VALUES
Bug 29314539 - ODP.NET ORACLE CONNECTION FAILS IF TABS EXIST BEFORE / AFTER USER ID
Bug 28728040 - ODP.NET MANAGED DRIVER FAILED TO SWITCH OVER TO STANDBY DB IN DATAGUARD ENVIRONMENT 
Bug 28632559 - CONNECTION TO STANDBY DB CAUSES ORA-01219 


ODP.NET Core Tips, Limitations, and Known Issues
================================================
1) ODP.NET 12c and 18c PL/SQL CHAR Binding Error "ORA-12899: Value Too Large for Column" When Inserting into a Table
Issue:
After patching managed Oracle Data Provider for .NET (ODP.NET) 12.x / 18.x or ODP.NET Core version 18.x, customers may encounter the "ORA-12899: Value Too Large for Column" error when binding CHAR parameters in PL/SQL which in turn inserts that value to a character based column in a database, that uses a multi-byte character set, such as AL32UTF8 where one character can take up as much as 4 bytes.

Explanation:
Due to a fix to a bug that forces ODP.NET to bind data with the "max byte size", the PL/SQL layer will create a blank-padded value that is beyond the size that the column can accept, since ODP.NET is required to provide the size of the parameter in terms of characters multiplied by the character expansion ratio.

Imagine that you have the following table created in a database with the AL32UTF8 mult-byte character set:
create table testchar_tab (char_column char(40));

And let us assume that you have the following PL/SQL stored procedure:
CREATE OR REPLACE PROCEDURE insert_row(param1 IN CHAR) AS
BEGIN
  INSERT INTO testchar_tab (CHAR_COLUMN) VALUES(param1);
END;
/

With the previous version of managed ODP.NET 12.x / 18.x  or ODP.NET Core 18.x, the application would have been able to insert character based data up to 40 characters.  However, starting with this patch, you will not able to insert more than 10 characters in this particular example, since PL/SQL layer will blank pad the data to 10 (characters) * 4 (character expansion ratio), which is 40 bytes, the max length of the CHAR column.

Resolution:
One workaround for this issue is to change the PL/SQL parameter type from CHAR to VARCHAR2, which eliminates the blank padding in the PL/SQL layer:

CREATE OR REPLACE PROCEDURE insert_row(param1 IN VARCHAR2) AS
BEGIN
  INSERT INTO testchar_tab (CHAR_COLUMN) VALUES(param1);
END;
/

The alternative approach is to bypass PL/SQL and insert directly into the table using SQL to avoid the blank padding by the PL/SQL layer.

