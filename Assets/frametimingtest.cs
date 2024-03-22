    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using System.Text;
     
    public class Timing : MonoBehaviour
    {
        // public TMP_Text screenText;
     
        FrameTiming[] frameTimings = new FrameTiming[1];
     
     
     
        uint m_frameCount = 0;
     
        const uint kNumFrameTimings = 2;
     
     
     
        // Update is called once per frame
        void Update()
        {
            ++m_frameCount;
            if (m_frameCount <= kNumFrameTimings)
            {
                return;
            }
            FrameTimingManager.CaptureFrameTimings();
            uint res = FrameTimingManager.GetLatestTimings(1, frameTimings);
            if (res < 1)
            {
                UnityEngine.Debug.LogErrorFormat("Skipping frame {0}, didn't get enough frame timings.",
                    m_frameCount);
                // screenText.text = string.Format("error frame {0}",
                // m_frameCount);
     
                return;
            }
     
     
            string text = string.Format("cpu frame time: {0}\ncpu time frame complete: {1}\ncpu time present called: {2}\ngpu frame time: {3}\nheight scale: {4}\nsync interval: {5} \nwidth scale: {6}\n{7}\n{8}\n{9}\n{10}\n{11}",
                frameTimings[0].cpuFrameTime,
                frameTimings[0].cpuTimeFrameComplete,
                frameTimings[0].cpuTimePresentCalled,
                frameTimings[0].gpuFrameTime,
                frameTimings[0].heightScale,
                frameTimings[0].syncInterval,
                frameTimings[0].widthScale,
                frameTimings[0].cpuMainThreadFrameTime,
                frameTimings[0].cpuRenderThreadFrameTime,
                frameTimings[0].cpuMainThreadPresentWaitTime,
                frameTimings[0].frameStartTimestamp,
                frameTimings[0].firstSubmitTimestamp);
            Debug.Log(text);
        }
    }
     
