using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RatVR.ExcelData
{
    public class ExcelSceneMetaData
    {
        public Vector2 size;
        public float baseLength;
        public int wallZone;

        public ExcelWallData wallTop;
        public ExcelWallData wallRight;
        public ExcelWallData wallBot;
        public ExcelWallData wallLeft;

        public bool cylinder;
 
        public ExcelSceneMetaData(  Vector2 size,
                                    float baseLength,
                                    int wallZone,
                                    ExcelWallData wallTop, 
                                    ExcelWallData wallRight, 
                                    ExcelWallData wallBot, 
                                    ExcelWallData wallLeft
                                    ){
            this.size = size;
            this.baseLength = baseLength;
            this.wallZone = wallZone;
            this.wallTop = wallTop;
            this.wallRight = wallRight;
            this.wallBot = wallBot;
            this.wallLeft = wallLeft;
        }
    }

    public class ExcelSessionMetaData
    {
        public int rewardPostSoundDelay;
        public int rewardAmount;
        public int punishmentLength;
        public int punishmentInactivationLength;
        public string onWallZoneEntry;
        public string onInterTrialInterval;
        public float interTrialIntervalLength;
        public int abortInterTrialIntervalLength;
        public float successSequenceLength;
        public int maximumTrialLength;
        public string trialPackageVariables;
        public int rewardedPillarsMin;
        public int rewardedPillarsMax;
        public int pillarTransparencyMin;
        public int pillarTransparencyMax;
        public int pillarPunishmentMin;
        public int pillarPunishmentMax;
        public int sessionFREEVAR1;
        public int sessionFREEVAR2;
        public string sessionFREEVAR3;
        public string sessionFREEVAR4;

        // Agent parameters
        public int agentFREEVAR1;
        public int agentFREEVAR2;
        public int agentFREEVAR3;
        public int agentFREEVAR4;
        public string agentFREEVAR5;
        public string agentFREEVAR6;
        public string agentFREEVAR7;
        public string agentFREEVAR8;


        public ExcelSessionMetaData(int rewardPostSoundDelay, 
                                    int rewardAmount, 
                                    int punishmentLength, 
                                    int punishmentInactivationLength, 
                                    string onWallZoneEntry, 
                                    string onInterTrialInterval, 
                                    float interTrialIntervalLength, 
                                    int abortInterTrialIntervalLength, 
                                    float successSequenceLength, 
                                    int maximumTrialLength,
                                    string trialPackageVariables, 
                                    int rewardedPillarsMin, 
                                    int rewardedPillarsMax, 
                                    int pillarTransparencyMin, 
                                    int pillarTransparencyMax, 
                                    int pillarPunishmentMin, 
                                    int pillarPunishmentMax,
                                    int sessionFREEVAR1,
                                    int sessionFREEVAR2,
                                    string sessionFREEVAR3,
                                    string sessionFREEVAR4,
                                    int agentFREEVAR1,
                                    int agentFREEVAR2,
                                    int agentFREEVAR3,
                                    int agentFREEVAR4,
                                    string agentFREEVAR5,
                                    string agentFREEVAR6,
                                    string agentFREEVAR7,
                                    string agentFREEVAR8
                                    ){

            this.rewardPostSoundDelay = rewardPostSoundDelay;
            this.rewardAmount = rewardAmount;
            this.punishmentLength = punishmentLength;
            this.punishmentInactivationLength = punishmentInactivationLength;
            this.onWallZoneEntry = onWallZoneEntry;
            this.onInterTrialInterval = onInterTrialInterval;
            this.interTrialIntervalLength = interTrialIntervalLength;
            this.abortInterTrialIntervalLength = abortInterTrialIntervalLength;
            this.successSequenceLength = successSequenceLength;
            this.maximumTrialLength = maximumTrialLength;
            this.trialPackageVariables = trialPackageVariables;
            this.rewardedPillarsMin = rewardedPillarsMin;
            this.rewardedPillarsMax = rewardedPillarsMax;
            this.pillarTransparencyMin = pillarTransparencyMin;
            this.pillarTransparencyMax = pillarTransparencyMax;
            this.pillarPunishmentMin = pillarPunishmentMin;
            this.pillarPunishmentMax = pillarPunishmentMax;
            this.sessionFREEVAR1 = sessionFREEVAR1;
            this.sessionFREEVAR2 = sessionFREEVAR2;
            this.sessionFREEVAR3 = sessionFREEVAR3;
            this.sessionFREEVAR4 = sessionFREEVAR4;
            this.agentFREEVAR1 = agentFREEVAR1;
            this.agentFREEVAR2 = agentFREEVAR2;
            this.agentFREEVAR3 = agentFREEVAR3;
            this.agentFREEVAR4 = agentFREEVAR4;
            this.agentFREEVAR5 = agentFREEVAR5;
            this.agentFREEVAR6 = agentFREEVAR6;
            this.agentFREEVAR7 = agentFREEVAR7;
            this.agentFREEVAR8 = agentFREEVAR8;

        }
    }

    public class ExcelWallData
    {
        public float height;
        public string texture;

        public ExcelWallData(float height, string texture)
        {
            this.height = height;
            this.texture = texture;
        }

        public static implicit operator WallData(ExcelWallData excelWall)
        {
            return new WallData(1, excelWall.texture, excelWall.height);
        }
    }

    public class ExcelObjectData
    {
        public string object_name;
        public float radius;
        public float height;
        public float zPos;
        public string texture;
        public int rewardRadius;
        public int pillarFREEVAR1;
        public int pillarFREEVAR2;

        public float transparency;
        public int isReward;
        public int isAirpuff;


        public ExcelObjectData(List<string> values)
        {
            this.object_name = values[0];
            this.radius = float.Parse(values[1], System.Globalization.CultureInfo.InvariantCulture);
            this.height = float.Parse(values[2], System.Globalization.CultureInfo.InvariantCulture);
            this.zPos = float.Parse(values[3], System.Globalization.CultureInfo.InvariantCulture);
            this.texture = values[4];
            this.rewardRadius = int.Parse(values[5]);
            this.pillarFREEVAR1 = -1;
            this.pillarFREEVAR2 = -1;
            this.transparency = 1;
            this.isReward = 1;
            this.isAirpuff = 0;
            
        }

        public override string ToString()
        {
            return object_name + "r: " + radius.ToString() + ", h: " + height.ToString() + ", zPos: " + zPos.ToString();
        }
    }
    

    
}