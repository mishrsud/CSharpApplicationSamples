# Summary

This sample application helps validate the conecptual thinking around transactions and the locks they hold on the records, tables and databases they affect.

## Objective

Validate our assumtptions that:

1. While writing to the database foo happens when bar
2. Test

## Concepts

## SQL Scripts

Veryfying the open transactions on SQL server:

1. Get open transactions on the database [SCRIPT](SQL/CheckOpenTransactions.sql)
Source: [Stackoverflow](https://stackoverflow.com/a/4449999/190476)

2. Start a named transaction

```sql
BEGIN TRAN UpdateBalanceTransaction
```

3. Two (or more) transactions with isolation level REPEATABLE READ hold locks, but do not block one another from reading, if they were ONLY reading (SELECT)

4. When multiple transactions with isolation level REPEATABLE READ hold locks, and the first one started a data manipulation (UPDATE, INSERT, DELETE), the following reads are blocked until the first one commits or rolls back.
```sql
SELECT TOP 10 * FROM sys.dm_tran_locks

SELECT cmd,* FROM sys.sysprocesses WHERE blocked > 0
```