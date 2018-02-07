using UnityEngine;
using UnityEditor;

namespace RosSharp.RosBridgeClient
{
   
    public class CameraImagePublisher2
    {

        private string topic;
        
        private int qualityLevel = 50;

        private RosSocket rosSocket;
        private int publicationId;
        private SensorCompressedImage message;
        private int sequenceId;
        public string frameId = "camera";
         
        public CameraImagePublisher2(RosSocket _rosSocket,string _topic, int ql)
        {
            qualityLevel = ql;
            topic = _topic;
            rosSocket = _rosSocket;
            publicationId = rosSocket.Advertize(topic, "sensor_msgs/CompressedImage");
            message = new SensorCompressedImage();
            sequenceId = 0;
        }

        public void Publish(Texture2D texture2D)
        {
            // Build up the message and publish
            message.header.frame_id = frameId;
            message.header.seq = sequenceId;
            message.format = "jpeg";
            message.data = texture2D.EncodeToJPG(qualityLevel);
            rosSocket.Publish(publicationId, message);

            ++sequenceId;
        }
    }
}