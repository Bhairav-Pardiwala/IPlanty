
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace BackgroundApplicationRelay
{
    class M2MQTTMod
    {
        MqttClient cl;
        string sendTopic = "iot";
        string[] reciveTopic = { "starshaqos1" };
        string diffResponse1 = "Hello World", diffResponse2 = "Hi Bhairav";
        Boolean run = true;
        String instanceid = "2";

        BackgroundApplicationRelay.StartupTask sttask;


        public bool isConnected()
        {
            return cl.IsConnected;
        }

        public void reset()
        {
            ConnectClient();
        }

        public void init(BackgroundApplicationRelay.StartupTask tsk)
        {
            cl = new MqttClient("moose.rmq.cloudamqp.com", 1883, false, MqttSslProtocols.None);
            ConnectClient();
            cl.MqttMsgPublishReceived += Cl_MqttMsgPublishReceived;
            cl.ConnectionClosed += Cl_ConnectionClosed;
            
            
            instanceid = Guid.NewGuid().ToString();
            this.sttask = tsk;
        }

        private void ConnectClient()
        {
           
            byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE };
            cl.Subscribe(reciveTopic, qosLevels);
            CommunicationMessage commMsg = new CommunicationMessage();
            commMsg.id = instanceid;
            commMsg.msg = "status";
            commMsg.response = "I started at " + DateTime.Now.ToString(); ;
            publishMessage(sendTopic, SerializeIt(commMsg));
        }

        private void Cl_ConnectionClosed(object sender, EventArgs e)
        {
            ConnectClient();
        }

        private void Cl_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            if (e.Message != null && !String.IsNullOrWhiteSpace(Encoding.UTF8.GetString(e.Message)))
            {
                CommunicationMessage mrcv = deserializeIt(e.Message);
               
                    if (mrcv.msg.Equals("StartPump"))
                    {
                        CommunicationMessage commMsg = new CommunicationMessage();
                        commMsg.id = instanceid;
                    commMsg.msg = "status";
                   sttask.getPmp().ManualStartPump();
                        commMsg.response = "I started Pump";

                        publishMessage(sendTopic, SerializeIt(commMsg));
                    }
                    else if (mrcv.msg.Equals("StopPump"))
                    {
                        CommunicationMessage commMsg = new CommunicationMessage();
                        commMsg.id = instanceid;
                    commMsg.msg = "status";
                    commMsg.response = "I stopped Pump";
                   sttask. getPmp().ManulStopPump();

                    publishMessage(sendTopic, SerializeIt(commMsg));
                    }
                    else if (mrcv.msg.Equals("DeviceStatus"))
                    {
                    CommunicationMessage commMsg = new CommunicationMessage();
                    commMsg.id = instanceid;
                    commMsg.msg = "status";
                    commMsg.response = "Heres the device status";
                    sttask.getLedBulb().Blink();
                    publishMessage(sendTopic, SerializeIt(commMsg));
                }
                else if (mrcv.msg.Equals("TimeSet"))
                {
                    DateTime.Parse(mrcv.response);
                    SetTime(DateTime.Parse(mrcv.response));
                    CommunicationMessage commMsg = new CommunicationMessage();
                    commMsg.id = instanceid;
                    commMsg.msg = "status";
                    commMsg.response = "Time set to "+DateTime.Now.ToString();
                    publishMessage(sendTopic, SerializeIt(commMsg));
                }
                else
                    {
                        //run = true;
                    }
               


            }
        }

        private void SetTime(DateTime now)
        {
            DateTimeOffset local = DateTime.SpecifyKind(now, DateTimeKind.Local);
            Windows.System.DateTimeSettings.SetSystemDateTime(local);
        }

        public void publishMessage(String topic, [System.Runtime.InteropServices.WindowsRuntime.ReadOnlyArray]byte[] message)
        {

            try
            {
                cl.Publish(topic, message);
            }
            catch (Exception)
            {


            }
        }
        public byte[] SerializeIt(Object obj)
        {
            try
            {
                //MemoryStream ms = new MemoryStream();
                //JsonSerializer serializer = new JsonSerializer();

                //// serialize product to BSON
                //BsonWriter writer = new BsonWriter(ms);
                //serializer.Serialize(writer, obj);
                //return ms.ToArray();

                return System.Text.Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
            }
            catch (Exception)
            {


            }
            return new byte[10];
        }
        public CommunicationMessage deserializeIt([System.Runtime.InteropServices.WindowsRuntime.ReadOnlyArray] byte[] b)
        {
            //MemoryStream ms = new MemoryStream(b);
            //JsonSerializer serializer = new JsonSerializer();
            //BsonReader reader = new BsonReader(ms);
            //CommunicationMessage rcv = serializer.Deserialize<CommunicationMessage>(reader);
            //return rcv;
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<CommunicationMessage>(System.Text.Encoding.UTF8.GetString(b));
            }
            catch (Exception)
            {


            }
            return new CommunicationMessage();
        }
    }
}
