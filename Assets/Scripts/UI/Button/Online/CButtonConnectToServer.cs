using UnityEngine;
using System.Collections;
using System;

/*
Description: Button functionality to connect to the online leaderboard server
Creator: Alvaro Chavez Mixco
Creation Date: Sunday, January 22, 2017
*/
public class CButtonConnectToServer : AButtonFunctionality
{
    /*
    Description: Button functionality to connect to the online leaderboard server
    Creator: Alvaro Chavez Mixco
    Creation Date: Sunday, January 22, 2017
    */
    public override void OnButtonExecution()
    {
        //If there is an online manager
        if(COnlineManager.s_instanceOnlineManager!=null)
        {
            //Connect to the online leaderbaord server
            COnlineManager.s_instanceOnlineManager.ConnectToLeaderboardServer();
        }
    }

}
