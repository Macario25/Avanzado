#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.UI;
using FTOptix.DataLogger;
using FTOptix.HMIProject;
using FTOptix.Store;
using FTOptix.NetLogic;
using FTOptix.NativeUI;
using FTOptix.InfluxDBStoreRemote;
using FTOptix.MQTTClient;
using FTOptix.Retentivity;
using FTOptix.CoreBase;
using FTOptix.InfluxDBStore;
using FTOptix.Core;
#endregion

public class RuntimeNetLogic2 : BaseNetLogic
{
    private IUAVariable msg;
    public override void Start()
    {
        // Insert code to be executed when the user-defined logic is started
        msg = Project.Current.GetVariable("Model/Texto");
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }
    [ExportMethod]
    public void setMessage()
    {
        // Insert code to be executed when the user-defined logic is updated
        string texto = "Nuevo mensaje establecido";
        msg.Value = texto;
    }
}
