using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary1
{
    
    public class Class1
    {
        public static int threshold1 = 0;
        public static int threshold2 = 100;
        public static int prevSensorVal = 0;
        public static int time1 = 180;
        public static int time2 = 90;
        public static int time3 = 30;
        public static List<SensorValue> sensorValues = new List<SensorValue>();

        static void Main(string[] args)
        {
            int sensor1Value = 1;
            int sensor2Value = 1;
            int prevTickCounter = 1;
            int lastestTickCounter = 2;
                var SENSOR_VALUE = GetRefinedValue(sensor1Value, sensor2Value);
                if (sensorValues.Count >= 15)
                {
                    if (prevSensorVal >= threshold1)
                    {
                        var currentVal = 0;
                        var preVal = prevSensorVal;
                        if (preVal == 0)
                        {
                            var tempBuffer = sensorValues.OrderBy(list => GetRefinedValue(list.Sensor1Value, list.Sensor2Value));
                            var t = tempBuffer.ElementAt(sensorValues.Count() / 2);
                            currentVal = GetRefinedValue(t.Sensor1Value, t.Sensor2Value);
                            prevSensorVal = currentVal;
                        }
                        else
                        {
                            var settlingTime = time3;
                            var tickCounterDiff = Math.Abs(lastestTickCounter - prevTickCounter);
                            if (tickCounterDiff > 1 && tickCounterDiff < (settlingTime / 2))
                            {
                                settlingTime = settlingTime / tickCounterDiff;
                            }
                            else if (tickCounterDiff > (time3 / 2))
                            {
                                settlingTime = time3 / 2;
                            }

                            currentVal = preVal + (SENSOR_VALUE - preVal) / settlingTime;
                            prevSensorVal = currentVal;
                        }
                    }
                    else
                    {
                        var steppedVal = 0;
                        var settlingTime2 = time2;
                        var preVal = prevSensorVal;
                        if (preVal <= 0)
                        {
                            steppedVal = SENSOR_VALUE;
                            prevSensorVal = steppedVal;
                        }
                        else
                        {
                            var settlingTimeR = time1;
                            var tickCounterDiff = Math.Abs(lastestTickCounter - prevTickCounter);
                            if (tickCounterDiff > 1 && tickCounterDiff < (time1 / 2))
                            {
                                settlingTimeR = settlingTimeR / tickCounterDiff;
                            }
                            else if (tickCounterDiff > (time1 / 2))
                            {
                                settlingTimeR = time1 / 2;
                            }

                            steppedVal = preVal + (SENSOR_VALUE - preVal) / settlingTimeR;
                            prevSensorVal = steppedVal;
                        }
                    }
                }
            sensorValues.Add(new SensorValue { Sensor1Value = sensor1Value, Sensor2Value = sensor2Value });
        }

        public static int GetRefinedValue(int sensor1Value, int sensor2Value)
        {
             var sensorDiff = Math.Abs(sensor1Value - sensor2Value);
             if (sensorDiff > 500)
             {
                return sensor1Value <= sensor2Value ? sensor1Value : sensor2Value;
             }
             return (sensor1Value + sensor2Value) / 2;

        }
    }

    public class SensorValue
    {
        public int Sensor1Value { get; set; }
        public int Sensor2Value { get; set; }
    }
}
