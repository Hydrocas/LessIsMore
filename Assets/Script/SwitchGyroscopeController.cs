using nn.hid;
using UnityEngine;

public class SwitchGyroscopeController : MonoBehaviour
{
    private int sixAxisHandleCount;
    private SixAxisSensorHandle[] sixAxisHandle;
    private SixAxisSensorState[] sixAxisStates;

    private void Awake()
    {
        sixAxisHandle = new SixAxisSensorHandle[2];
        sixAxisStates = new SixAxisSensorState[2];
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        sixAxisHandleCount = SixAxisSensor.GetHandles(sixAxisHandle, 2, NpadId.No1, NpadStyle.JoyRight);

        //Start sensor
        for (int i = 0; i < sixAxisHandleCount; i++)
        {
            SixAxisSensor.Start(sixAxisHandle[i]);
        }
    }

#if UNITY_SWITCH
    private void Update()
    {
        nn.util.Float4 float4TempSensorData = new nn.util.Float4();

        for (int i = 0; i < sixAxisHandleCount; i++)
        {
            //Crash in UNITY_EDITOR
            SixAxisSensor.GetState(ref sixAxisStates[i], sixAxisHandle[i]);
            sixAxisStates[i].GetQuaternion(ref float4TempSensorData);

            //Convert to an Unity Quaternion
            Quaternion rawSensorRotation = new Quaternion(
                float4TempSensorData.x, 
                float4TempSensorData.z, 
                float4TempSensorData.y, 
                float4TempSensorData.w * -1f);

            Debug.Log(rawSensorRotation);
        }
    }
#endif
}
