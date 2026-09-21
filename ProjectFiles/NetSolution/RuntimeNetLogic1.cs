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

public class RuntimeNetLogic1 : BaseNetLogic
{
    private double var1 = 0;
    private double var2 = 0;
    private bool enable = false;
    private double arg = 0;
    private PeriodicTask periodicTask;
    public override void Start()
    {
        // Insert code to be executed when the user-defined logic is started
        arg = 0.0;
        periodicTask = new PeriodicTask(simulacion,250,LogicObject);
        periodicTask.Start();
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
        if (periodicTask != null)
        {
            periodicTask.Dispose();
            periodicTask = null;
        }
    }
    public void simulacion()
    {
        // Obtener referencia a la variable de habilitación
        enable = LogicObject.GetVariable("run").Value;

        if (enable)
        {
            // Incrementar el argumento en cada ciclo para generar el movimiento de la onda
            arg += 0.05; 

            var sin = Math.Sin(arg) * 100;
            var cos = Math.Cos(arg) * 50;

            // Asignar los valores calculados a las variables de salida
            var var1Node = LogicObject.GetVariable("Variable1");
            var var2Node = LogicObject.GetVariable("Variable2");

            if (var1Node != null)
                var1Node.Value = (float)sin;

            if (var2Node != null)
                var2Node.Value = (float)cos;
        }
    }
}
