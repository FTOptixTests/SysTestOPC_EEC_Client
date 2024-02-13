#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.NetLogic;
using FTOptix.CoreBase;
using FTOptix.UI;
using FTOptix.NativeUI;
using FTOptix.WebUI;
using FTOptix.Recipe;
using FTOptix.EventLogger;
using FTOptix.DataLogger;
using FTOptix.SQLiteStore;
using FTOptix.Store;
using FTOptix.ODBCStore;
using FTOptix.Report;
using FTOptix.OPCUAClient;
using FTOptix.RAEtherNetIP;
using FTOptix.System;
using FTOptix.Retentivity;
using FTOptix.CommunicationDriver;
using FTOptix.UI;
using FTOptix.SerialPort;
using FTOptix.Alarm;
using FTOptix.Core;
#endregion

public class ODBC_DB_CLEAR : BaseNetLogic
{
    public override void Start()
    {
        bool bypassOdbc = LogicObject.GetVariable("FlagODBC").Value;
        if (!bypassOdbc)
            return;
        var odbc= GetStore();
        // var odbc  = LogicObject.Get<ODBCStore>("NodePointer1");
        

        string DBName = LogicObject.GetVariable("DBName").Value;
        int EoiNr = LogicObject.GetVariable("EOINr").Value;         
        string query = String.Format("DELETE FROM {0} WHERE EOI_nr={1} OR EOI_nr = 5", DBName, EoiNr);
        
        object[,] resultSet;
        string[] Header;
         odbc.Query(query, out Header, out resultSet);
         Log.Info(String.Format("DELETE FROM {0:sql_identifier} WHERE EOI_nr={1} OR EOI_nr = 5 successfull", DBName, EoiNr));

    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }
    private ODBCStore GetStore()
    {
        var myODBCStore = LogicObject.GetVariable("NodePointer1");
        if (myODBCStore == null)
            throw new Exception("NodePointer1 variable not found");

        var nodeId = (NodeId)myODBCStore.Value;
        if (nodeId == null)
            throw new Exception("NodePointer1 not set");

        var store = InformationModel.Get(nodeId);
        if (store == null)
            throw new Exception("Store not found");

        var myStore = store as ODBCStore;
        if (myStore == null)
            throw new Exception(myStore.BrowseName + " is not an ODBCStore");

        return myStore;
    }
}
