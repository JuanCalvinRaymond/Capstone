using UnityEngine;
using System.Collections;

public enum ENetworkEventTypes
{
    Log,
    Error,
    Message,
    ConnectionStatus
}

public struct SNetworkClientEvents
{
    public ENetworkEventTypes m_eventType;
    public string m_message;
    public bool m_connectionStatus;

    public SNetworkClientEvents(ENetworkEventTypes aEventType, string aMessage)
    {
        m_eventType = aEventType;
        m_message = aMessage;
        m_connectionStatus = false;
    }

    public SNetworkClientEvents(ENetworkEventTypes aEventType, bool aConnectionStatus)
    {
        m_eventType = aEventType;
        m_message = string.Empty;
        m_connectionStatus = aConnectionStatus;
    }
}
