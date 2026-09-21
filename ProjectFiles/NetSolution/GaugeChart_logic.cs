#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.UI;
using FTOptix.HMIProject;
using FTOptix.NetLogic;
using FTOptix.CoreBase;
using FTOptix.Core;
#endregion

public class GaugeChart_logic : BaseNetLogic
{
    private PeriodicTask periodicTask;
    private Random random = new Random();

    public override void Start()
    {
        // Tarea periódica cada 2000 ms (2 segundos)
        periodicTask = new PeriodicTask(ActualizarGrafico, 2000, LogicObject);
        periodicTask.Start();
    }

    public override void Stop()
    {
        if (periodicTask != null)
        {
            periodicTask.Dispose();
            periodicTask = null;
        }
    }

    private void ActualizarGrafico()
    {
        // 1. Leer el estado de la variable booleana Darkmode
        var darkModeVar =LogicObject.GetVariable("Darkmode");
        bool isDarkMode = darkModeVar != null && (bool)darkModeVar.Value;

        // Definir colores según el modo
        string backgroundColor = isDarkMode ? "#100C2A" : "#FFFFFF";
        string textColor = isDarkMode ? "#FFFFFF" : "#333333";

        // 2. Generar el valor aleatorio simulado
        double valorSimulado = Math.Round(random.NextDouble() * 100, 2);

        // Actualizar la variable interna GaugeValue para la HMI
        var gaugeValueVar = LogicObject.GetVariable("GaugeValue");
        if (gaugeValueVar != null)
        {
            gaugeValueVar.Value = (float)valorSimulado;
        }

        // 3. Generar la estructura JSON de la opción de ECharts
        string jsonOption = $$"""
        {
          "backgroundColor": "{{backgroundColor}}",
          "series": [
            {
              "type": "gauge",
              "axisLine": {
                "lineStyle": {
                  "width": 30,
                  "color": [
                    [0.3, "#67e0e3"],
                    [0.7, "#37a2da"],
                    [1, "#fd666d"]
                  ]
                }
              },
              "pointer": {
                "itemStyle": {
                  "color": "auto"
                }
              },
              "axisTick": {
                "distance": -30,
                "length": 8,
                "lineStyle": {
                  "color": "{{textColor}}",
                  "width": 2
                }
              },
              "splitLine": {
                "distance": -30,
                "length": 30,
                "lineStyle": {
                  "color": "{{textColor}}",
                  "width": 4
                }
              },
              "axisLabel": {
                "color": "inherit",
                "distance": 40,
                "fontSize": 20
              },
              "detail": {
                "valueAnimation": true,
                "formatter": "{value} km/h",
                "color": "inherit"
              },
              "data": [
                {
                  "value": {{valorSimulado}}
                }
              ]
            }
          ]
        }
        """;

        // 4. Enviar el JSON a la propiedad string que consume la vista HTML/ECharts
        var chartOptionVar = LogicObject.GetVariable("ChartOptionJSON");
        if (chartOptionVar != null)
        {
            chartOptionVar.Value = jsonOption;
        }
    }
}
