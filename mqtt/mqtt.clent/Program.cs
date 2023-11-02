using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

class Program
{
    static MqttClient mqttClient;

    static async Task Main(string[] args)
    {
        _= Task.Run(() =>
        {
            mqttClient = new MqttClient("172.18.64.1");
            mqttClient.MqttMsgPublishReceived += MqttClient_MqttMsgPublishReceived;
            mqttClient.Subscribe(new string[] { "BoschOne" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
            mqttClient.Connect("");
        });

        Console.ReadLine(); 
    }
    private static void MqttClient_MqttMsgPublishReceived(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
    {
        var message = Encoding.UTF8.GetString(e.Message);
        Console.WriteLine(message);
    }
}