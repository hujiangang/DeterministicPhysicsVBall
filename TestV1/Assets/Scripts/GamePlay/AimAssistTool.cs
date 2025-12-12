using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAssistTool
{
    private LineRenderer aimLine;
    private LineRenderer trajectoryLine;
    private Transform ghostBall;

    public void Init(){
        // 初始化瞄准线和轨迹线
        if (aimLine == null){
            aimLine = GameObject.FindGameObjectWithTag(GameTags.GetTagName(GameTag.AimLine)).GetComponent<LineRenderer>();
        }
        
        if (trajectoryLine == null) {
            trajectoryLine = GameObject.FindGameObjectWithTag(GameTags.GetTagName(GameTag.TrajectoryLine)).GetComponent<LineRenderer>();
        }
        // 初始化ghostBall
        if (ghostBall == null){
            ghostBall = GameObject.FindGameObjectWithTag(GameTags.GetTagName(GameTag.GhostBall)).transform;
        }
    }

    public void Reset(){
        ghostBall.position = Vector3.zero;
        aimLine.positionCount = 0;
        trajectoryLine.positionCount = 0;
    }


    public void Open()
    {
        Init();
        GameEvents.RegisterEvent<Vector3, Vector3, Vector3, Vector3, Vector3>(GameBasicEvent.DrawAimLine, DrawAimLine);
        GameEvents.RegisterBasicEvent(GameBasicEvent.HideCuestick, Reset);
    }
    
    public void Close()
    {
        GameEvents.UnregisterEvent<Vector3, Vector3, Vector3, Vector3, Vector3>(GameBasicEvent.DrawAimLine, DrawAimLine);
        GameEvents.UnregisterBasicEvent(GameBasicEvent.HideCuestick, Reset);
    }

    public void DrawAimLine( Vector3 ghostBallPos, Vector3 aimLineStartPos, Vector3 aimLineEndPos, 
        Vector3 trajectoryLineStart, Vector3 trajectoryLineEnd){

        aimLine.positionCount = 2;
        aimLine.SetPosition(0, aimLineStartPos);
        aimLine.SetPosition(1, aimLineEndPos);

        trajectoryLine.positionCount = 2;
        trajectoryLine.SetPosition(0, trajectoryLineStart);
        trajectoryLine.SetPosition(1, trajectoryLineEnd);

        ghostBall.position = ghostBallPos;
    }
}
