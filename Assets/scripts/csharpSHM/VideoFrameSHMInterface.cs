using System;
using System.IO.MemoryMappedFiles;
using Newtonsoft.Json;
using System.IO; // For StreamReader and FileNotFoundException
using UnityEngine;

public class VideoFrameSHMInterface
{
    private MemoryMappedFile _memory;
    private MemoryMappedViewAccessor _accessor;
    private string _shmName;
    private int _totalNbytes;
    private int _packageNbytes;
    private string _frameType;
    private int _xRes;
    private int _yRes;
    private int _nchannels;

    public VideoFrameSHMInterface(string shmStructureJsonFilename)
    {
        string unityProjectPath = Path.GetDirectoryName(Application.dataPath);
        // this adjusts the path when exec from build subfolder 
        if (Directory.Exists(Path.Combine(unityProjectPath, "Assets")) == false) {
            unityProjectPath = Path.Combine(unityProjectPath, "..");
        }
        string shmStructureJsonFullFilename = Path.Combine(unityProjectPath, "..", "tmp_shm_structure_JSONs", shmStructureJsonFilename);
        if (!File.Exists(shmStructureJsonFullFilename)) {
            string errorMessage = $"Error: Shared memory has not been created. Could not find JSON file: {shmStructureJsonFullFilename}";
            throw new Exception(errorMessage);
        }

        dynamic shmStructure = LoadShmStructureJson(shmStructureJsonFullFilename);

        _shmName = shmStructure.shm_name;
        _totalNbytes = shmStructure.total_nbytes;
        _packageNbytes = shmStructure.fields.package_nbytes;
        _frameType = shmStructure.field_types.frame_type;
        _xRes = shmStructure.metadata.x_resolution;
        _yRes = shmStructure.metadata.y_resolution;
        _nchannels = shmStructure.metadata.nchannels;

        // _memory = MemoryMappedFile.CreateFromFile("/dev/shm/termflag", System.IO.FileMode.Open);
        // _accessor = _memory.CreateViewAccessor();
        if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
        {
            _memory = MemoryMappedFile.CreateFromFile($"/dev/shm/{_shmName}", System.IO.FileMode.Open);
        }
        else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
        {
            _memory = MemoryMappedFile.OpenExisting(_shmName);
        } 
        // OSX
        else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
        {
            _memory = MemoryMappedFile.CreateFromFile($"/tmp/{_shmName}", System.IO.FileMode.Open);
        }
        
        if (_memory == null)
        {
            Console.WriteLine($"Failed to create MemoryMappedFile from shmName: {_shmName}\n\n");
            Environment.Exit(1);
        }
        
        _accessor = _memory.CreateViewAccessor();
    }

    private byte[] Frame
    {
        get
        {
            byte[] buffer = new byte[_totalNbytes - _packageNbytes];
            _accessor.ReadArray(_packageNbytes, buffer, 0, buffer.Length);
            return buffer;
        }
        set
        {
            // var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            _accessor.WriteArray(_packageNbytes, value, 0, value.Length);
            // Debug.Log($"SHM executed in {stopwatch.ElapsedTicks / (System.TimeSpan.TicksPerMillisecond / 1000)} μs");
        }
    }

    private byte[] Package
    {
        get
        {
            byte[] buffer = new byte[_packageNbytes];
            _accessor.ReadArray(0, buffer, 0, buffer.Length);
            return buffer;
        }
        set
        {

            byte[] package = new byte[_packageNbytes];
            Array.Copy(value, package, value.Length);
            _accessor.WriteArray(0, package, 0, package.Length);

        }
    }

    public void AddFrame(byte[] img, byte[] package)
    {
        Frame = img;
        Package = package;
    }

    public void CloseShm()
    {
        _memory.Dispose();
    }

    private dynamic LoadShmStructureJson(string filename)
    {
        using (StreamReader r = new StreamReader(filename))
        {
            string json = r.ReadToEnd();
            return JsonConvert.DeserializeObject(json);
        }
    }
}