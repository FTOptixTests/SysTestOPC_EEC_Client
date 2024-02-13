#region Using directives
using System;
using FTOptix.CoreBase;
using FTOptix.HMIProject;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.NetLogic;
using FTOptix.UI;
using FTOptix.OPCUAServer;
using FTOptix.Retentivity;
using FTOptix.OPCUAClient;
using FTOptix.EventLogger;
using FTOptix.System;
using FTOptix.SerialPort;
using FTOptix.MicroController;
using FTOptix.ODBCStore;
#endregion

public class LoginFormOutputMessageLogic : BaseNetLogic
{
    public override void Start()
    {
        messageVariable = Owner.GetVariable("Message");
        task = new DelayedTask(() =>
        {
            if (messageVariable == null)
            {
                Log.Error("LoginFormOutputMessageLogic", "Unable to find variable Message in LoginFormOutputMessage label");
                return;
            }

            messageVariable.Value = "";
            taskStarted = false;
        }, 5000, LogicObject);
    }

    public override void Stop()
    {
        task?.Dispose();
    }

    [ExportMethod]
    public void SetOutputMessage(string message)
    {
        if (messageVariable == null)
        {
            Log.Error("LoginFormOutputMessageLogic", "Unable to find variable Message in LoginFormOutputMessage label");
            return;
        }

        messageVariable.Value = message;

        if (taskStarted)
        {
            task?.Cancel();
            taskStarted = false;
        }

        task.Start();
        taskStarted = true;
    }

    private DelayedTask task;
    private bool taskStarted = false;
    private IUAVariable messageVariable;
}
