using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.Collections;

public class renderOutputLogger : MonoBehaviour
{
    [SerializeField] RenderTexture finalTexture;
    private VideoFrameSHMInterface unitycameraSHMInterface;
    NativeArray<byte> output;
    // int frame_i = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        unitycameraSHMInterface = new VideoFrameSHMInterface("../tmp_shm_structure_JSONs/unitycam_shmstruct.json");
    }

    // Update is called once per frame
    void Update()
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        SaveFinalTextureToImage(Time.frameCount);
        Debug.Log($"Saving frame in {stopwatch.ElapsedTicks / (System.TimeSpan.TicksPerMillisecond / 1000)} μs");
        stopwatch.Stop();
    }

 void SaveFinalTextureToImage(int frame_i)
{
    // Convert the RenderTexture to a Texture2D
    Texture2D texture = new Texture2D(1070, 800, TextureFormat.RGB24, false, true);
    RenderTexture.active = finalTexture;
    texture.ReadPixels(new Rect(425, 250, 1070, 800), 0, 0);
    texture.Apply();

    var imageBytes = texture.GetRawTextureData();

    float frameCount = Time.frameCount;
    float frameTime = Time.realtimeSinceStartup;
    byte[] packBytes = Encoding.UTF8.GetBytes("<{" + $"N:I,ID:{frameCount},PCT:{frameTime}" + "}>\r\n");
    unitycameraSHMInterface.AddFrame(imageBytes, packBytes);

    // Clean up
    RenderTexture.active = null;
    Destroy(texture);

}



    // void SaveFinalTextureToImage(int frame_i)
    // {
    //     // Convert the RenderTexture to a Texture2D
    //     Texture2D texture = new Texture2D(1070, 800, TextureFormat.RGB24, false, true);
    //     RenderTexture.active = finalTexture;
    //     texture.ReadPixels(new Rect(425, 250, 1070, 800), 0, 0);
    //     texture.Apply();

    //     // Create a NativeArray to hold the data
    //     output = new NativeArray<byte>(texture.GetRawTextureData().Length*3, Allocator.Persistent);

    //     // Request an asynchronous readback of the data from GPU to CPU
    //     AsyncGPUReadback.RequestIntoNativeArray(ref output, finalTexture, 0, texture.format, ReadbackCompleted);

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

    //     byte[] packBytes = Encoding.UTF8.GetBytes("<{" + $"N:I,ID:{Time.frameCount},PCT:1241938576" + "}>\r\n");
    //     unitycameraSHMInterface.AddFrame(imageBytes, packBytes);

    //     // Dispose the NativeArray
    //     output.Dispose();
    // }


}
