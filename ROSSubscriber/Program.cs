using System;
using System.Net;
using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.MessageTypes.Geometry;
using RosbridgeNet.RosbridgeClient;
using RosSharp.RosBridgeClient.Protocols;

class Program
{
    static void Main(string[] args)
    {
        string ipAddress = "34.125.47.0"; // IP address of your ROS machine
        int port = 9090; // port used by ROSBridge
        string topic = "/cmd_vel"; // topic to publish the message to

        // Create a new instance of the RosBridgeWebSocketClient
        WebSocketNetProtocol protocol = new WebSocketNetProtocol($"ws://{ipAddress}:{port}");
        RosSocket rosSocket = new RosSocket(protocol);



        // Create a new instance of the Twist message
        Twist twist = new Twist()
        {
            linear = new Vector3()
            {
                x = 0.5f,
                y = 0.0f,
                z = 0.0f
            },
            angular = new Vector3()
            {
                x = 0.0f,
                y = 0.0f,
                z = 0.5f
            }
        };

        rosSocket.Publish(topic, twist);

    }
}
