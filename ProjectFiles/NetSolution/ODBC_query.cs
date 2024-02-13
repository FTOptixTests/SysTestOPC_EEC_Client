#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.UI;
using FTOptix.DataLogger;
using FTOptix.NativeUI;
using FTOptix.Store;
using FTOptix.ODBCStore;
using FTOptix.Retentivity;
using FTOptix.CoreBase;
using FTOptix.Core;
using FTOptix.NetLogic;
using FTOptix.SQLiteStore;
using FTOptix.Recipe;
using FTOptix.OPCUAClient;
#endregion

public class ODBC_query : BaseNetLogic
{

    public override void Start()
    {
        ///
    }

    // [ExportMethod]
    // public void ExecuteQuery()
    // {
    //     int[] PE = {LogicObject.GetVariable("PE1").Value, LogicObject.GetVariable("PE2").Value,
    //                 LogicObject.GetVariable("PE3").Value, LogicObject.GetVariable("PE4").Value};
    //     string[] PE_odbc;
    //     PE_odbc = new string[4] {"NULL", "NULL", "NULL", "NULL"};
    //     int EOI_nr = LogicObject.GetVariable("EOI_nr").Value;

    //     var tableObject = GetTable();
    //     var storeObject = GetStoreObject(tableObject);
    //     //var selectQuery = $"SELECT COUNT(*) as test_column FROM test_db.odbc_prod_small_db ORDER BY test_column";
    //     var selectQuery = $"SELECT PE_pattern as test_column FROM odbc_prod_small_db ";

    //     for(var i = 0; i < PE.Length; i++ )
    //     {
    //         if (PE[i] == 0)
    //         {
    //             selectQuery = $"SELECT PE_pattern as test_column FROM odbc_prod_small_db WHERE EOI_nr = {EOI_nr} and LoggedTagValue = 1 and PE_pattern = 0";
    //         }
    //         else
    //         {
    //             selectQuery = $"SELECT PE_pattern as test_column FROM odbc_prod_small_db WHERE EOI_nr = {EOI_nr} and LoggedTagValue = 1 and PE_pattern = 1";     
    //         }  

    //         storeObject.Query(selectQuery, out string[] header, out object[,] resultSet);

    //         if (header == null || resultSet == null)
    //             throw new Exception("Unable to execute SQL query, malformed result");

        

    //         var rowCount = resultSet.GetLength(0);
    //         var columnCount = resultSet.GetLength(1);

    //         for (var r = 0; r < rowCount; ++r)
    //         {
    //         var currentRow = new string[columnCount];

    //         for (var c = 0; c < columnCount; ++c)
    //             {//currentRow[c] = resultSet[r, c]?.ToString() ?? "NULL";
    //             currentRow[c] = resultSet[r, c]?.ToString();
    //             PE_odbc[i]= currentRow[c];
    //             }
    //          }
    //     }

    //     // PE_odbc[i] = Convert.ToInt32(resultSet[0, 2]);
    //     LogicObject.GetVariable("PE1_ODBC").Value = Convert.ToInt32(PE_odbc[0]);
    //     LogicObject.GetVariable("PE2_ODBC").Value = Convert.ToInt32(PE_odbc[1]);
    //     LogicObject.GetVariable("PE3_ODBC").Value = Convert.ToInt32(PE_odbc[2]);
    //     LogicObject.GetVariable("PE4_ODBC").Value = Convert.ToInt32(PE_odbc[3]);


    // }

    [ExportMethod]
    public void CountRecordsInClearDB()
    {
        var tableObject = GetTable();
        var storeObject = GetStoreObject(tableObject);
        int EOI_nr = LogicObject.GetVariable("EOI_nr").Value;
        var selectQuery = $"SELECT COUNT(*) as test_column FROM odbc_prod_small_db WHERE EOI_nr = {EOI_nr} ORDER BY test_column";
        

        storeObject.Query(selectQuery, out string[] header, out object[,] resultSet);

        if (header == null || resultSet == null)
            throw new Exception("Unable to execute SQL query, malformed result");

    

        var rowCount = resultSet.GetLength(0);
        var columnCount = resultSet.GetLength(1);

        for (var r = 0; r < rowCount; ++r)
        {
        var currentRow = new string[columnCount];

            for (var c = 0; c < columnCount; ++c)
            {   
                currentRow[c] = resultSet[r, c]?.ToString() ?? "NULL";
                LogicObject.GetVariable("RecordsInDB").Value = currentRow[c];
            }
        }

    }


    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }

    public Table GetTable()
    {
        var tableVariable = LogicObject.GetVariable("Table");

        if (tableVariable == null)
            throw new Exception("Table variable not found");

        NodeId tableNodeId = tableVariable.Value;
        if (tableNodeId == null || tableNodeId == NodeId.Empty)
            throw new Exception("Table variable is empty");

        var tableNode = InformationModel.Get<Table>(tableNodeId);
        if (tableNode == null)
            throw new Exception("The specified table node is not an instance of Store Table type");

        return tableNode;
    }

    public Store GetStoreObject(Table tableNode)
    {
        return tableNode.Owner.Owner as Store;
    }

    

}
