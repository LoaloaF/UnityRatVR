using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.Collections;
using FSM;

public class renderOutputLogger : MonoBehaviour
{
    [SerializeField] RenderTexture finalTexture;
    private VideoFrameSHMInterface unityCameraSHMInterface;
    public GameObject ExperimentCore;
    private BaseStateMachine _stateMachine;

    Texture2D texture;
    Texture2D ScalableTex;    
    private byte[] imageBytes;
    private byte[] packBytes;

    // int frame_i = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        _stateMachine = ExperimentCore.GetComponent<BaseStateMachine>();
        unityCameraSHMInterface = new VideoFrameSHMInterface("unitycam_shmstruct.json");
        texture = new Texture2D(1000, 800, TextureFormat.RGB24, false, true);
    }


    // Update is called once per frame
    void Update()
    {
        if (_stateMachine._sessionManager.frameLoggerFlag)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            SaveFinalTextureToImage();
            // Debug.Log($"Saving frame in {stopwatch.ElapsedTicks / (System.TimeSpan.TicksPerMillisecond / 1000)} μs");
            stopwatch.Stop();
        }
    }

    // void SaveFinalTextureToImage(int frame_i)
    // {
    //     // Convert the RenderTexture to a Texture2D

    //     RenderTexture.active = finalTexture;
    //     texture.ReadPixels(new Rect(425, 250, 1070, 800), 0, 0);
    //     texture.Apply();

    //     var imageBytes = texture.GetRawTextureData();

    //     float frameCount = Time.frameCount;
    //     float frameTime = Time.realtimeSinceStartup;
    //     byte[] packBytes = Encoding.UTF8.GetBytes("<{" + $"N:I,ID:{frameCount},PCT:{frameTime}" + "}>\r\n");
    //     unityCameraSHMInterface.AddFrame(imageBytes, packBytes);

    //     // Clean up
    //     RenderTexture.active = null;
    //     Destroy(texture);

    // }
    void SaveFinalTextureToImage()
    {
        // Ensure the finalTexture is not null and has the correct size
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        texture = new Texture2D(1000, 800, TextureFormat.RGB24, false, true);
        // Read RenderTexture data into the Texture2D
        RenderTexture.active = finalTexture;
        texture.ReadPixels(new Rect(460, 250, 1000, 800), 0, 0);
        texture.Apply();


        TextureScaler.scale(texture,500,400,FilterMode.Trilinear);

        // Debug.Log($"TS1 {stopwatch.ElapsedTicks / (System.TimeSpan.TicksPerMillisecond / 1000)} μs");
        // Debug.Log($"TS1 {stopwatch.ElapsedTicks / (System.TimeSpan.TicksPerMillisecond / 1000)} μs");


        // Get raw texture data bytes

        imageBytes = texture.GetRawTextureData();
        // Debug.Log($"TS2 {stopwatch.ElapsedTicks / (System.TimeSpan.TicksPerMillisecond / 1000)} μs");

        // Prepare metadata packet bytes
        float frameCount = Time.frameCount;
        // float frameTime = Time.time;
        DateTime currentDateTime = DateTime.UtcNow;
        long ticksSinceEpoch = currentDateTime.Ticks - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
        long unixTimestampMicroseconds = ticksSinceEpoch / 10;

        string metadata = "<{" + $"N:I,ID:{frameCount},PCT:{unixTimestampMicroseconds}" + "}>\r\n";
        packBytes = Encoding.UTF8.GetBytes(metadata);
        // Debug.Log($"TS3 {stopwatch.ElapsedTicks / (System.TimeSpan.TicksPerMillisecond / 1000)} μs");

        // Send image and metadata to SHM interface
        unityCameraSHMInterface.AddFrame(imageBytes, packBytes);
        // Debug.Log($"TS4 {stopwatch.ElapsedTicks / (System.TimeSpan.TicksPerMillisecond / 1000)} μs");
        RenderTexture.active = null;
        Destroy(texture);

    }

    // void OnDestory()
    // {
    //     RenderTexture.active = null;
    //     if (texture != null)
    //     {
    //         Destroy(texture);
    //         texture = null;
    //     }
    // }


    // void SaveFinalTextureToImage(int frame_i)
    // {
    //     // Convert the RenderTexture to a Texture2D
    //     Texture2D texture = new Texture2D(1070, 800, TextureFormat.RGB24, false, true);
    //     RenderTexture.active = finalTexture;
    //     texture.ReadPixels(new Rect(425, 250, 1070, 800), 0, 0);
    //     texture.Apply();
    //     Debug.Log("size" + texture.GetRawTextureData().Length);


    //     // Create a NativeArray to hold the data
    //     output = new NativeArray<byte>(texture.width*texture.height*3, Allocator.Persistent);

    //     // Request an asynchronous readback of the data from GPU to CPU
    //     AsyncGPUReadback.RequestIntoNativeArray(ref output, finalTexture, 0, TextureFormat.RGB24, ReadbackCompleted);

    //     // Clean up
    //     RenderTexture.active = null;
    //     Destroy(texture);
    // }

    // void ReadbackCompleted(AsyncGPUReadbackRequest request)
    // {
    //     if (request.hasError)
    //     {
    //         Debug.Log("Failed to read GPU texture");
    //         return;
    //     }

    //     var imageBytes = request.GetData<byte>().ToArray();
    //     Debug.Log(imageBytes.Length);
    //     float frameCount = Time.frameCount;
    //     float frameTime = Time.realtimeSinceStartup;
    //     byte[] packBytes = Encoding.UTF8.GetBytes("<{" + $"N:I,ID:{frameCount},PCT:frameTime" + "}>\r\n");
    //     unitycameraSHMInterface.AddFrame(imageBytes, packBytes);

    //     // Dispose the NativeArray
    //     // output.Dispose();
    // }


}
