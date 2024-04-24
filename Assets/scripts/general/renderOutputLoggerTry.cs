using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.Collections;

public class renderOutputLoggerTry : MonoBehaviour
{
    [SerializeField] RenderTexture finalTexture;
    private VideoFrameSHMInterface unitycameraSHMInterface;

    private byte[] packBytes;
    private NativeArray<byte> output;

    // int frame_i = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        unitycameraSHMInterface = new VideoFrameSHMInterface("../tmp_shm_structure_JSONs/unitycam_shmstruct.json");
        
    }

    // Update is called once per frame
    void Update()
    {
        RenderTexture.active = finalTexture;
        output = new NativeArray<byte>(1920*1080*3, Allocator.Persistent);
        AsyncGPUReadback.RequestIntoNativeArray(ref output, finalTexture, 0, TextureFormat.RGB24, ReadbackCompleted);
    }

    void ReadbackCompleted(AsyncGPUReadbackRequest request)
    {
        if (request.hasError)
        {
            Debug.Log("Failed to read GPU texture");
            return;
        }

        NativeArray<byte> resultData = request.GetData<byte>();

        // Prepare the metadata packet
        float frameCount = Time.frameCount;
        float frameTime = Time.realtimeSinceStartup;
        string metadata = $"<{{N:I,ID:{frameCount},PCT:{frameTime}}}>\r\n";
        byte[] metadataBytes = Encoding.UTF8.GetBytes(metadata);

        // Send data to shared memory interface
        unitycameraSHMInterface.AddFrame(resultData.ToArray(), metadataBytes);

        // Dispose the NativeArray
        resultData.Dispose();
        // Dispose the NativeArray
        // output.Dispose();
    }

   void OnDestory()
    {
        RenderTexture.active = null;
    }
}
