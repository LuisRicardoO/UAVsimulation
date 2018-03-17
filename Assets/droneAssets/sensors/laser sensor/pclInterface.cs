using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//using System;
using System.Runtime.InteropServices;
using RosSharp.RosBridgeClient;

namespace pclInterfaceName
{
    class pclInterface
    {
        private IntPtr cloud;

        //**************************************************************************************************************
        //**************************************************************************************************************
        //**************************************************************************************************************
        //**************************************************************************************************************
        [DllImport("pclUnity", CharSet = CharSet.Unicode)]
        static extern IntPtr pclConnectionConstructor();

        [DllImport("pclUnity", CharSet = CharSet.Unicode)]
        static extern void setCloudWidth(IntPtr api, int width);

        [DllImport("pclUnity", CharSet = CharSet.Unicode)]
        static extern void setCloudHeight(IntPtr api, int height);

        [DllImport("pclUnity", CharSet = CharSet.Unicode)]
        static extern void setCloudDense(IntPtr api, bool dense);

        [DllImport("pclUnity", CharSet = CharSet.Unicode)]
        static extern void pushPoint(IntPtr api, float x, float y, float z);

        [DllImport("pclUnity", CharSet = CharSet.Unicode)]
        static extern int getCloudWidth(IntPtr api);

        [DllImport("pclUnity", CharSet = CharSet.Unicode)]
        static extern int getCloudHeight(IntPtr api);

        [DllImport("pclUnity", CharSet = CharSet.Unicode)]
        static extern bool getCloudDense(IntPtr api);

        [DllImport("pclUnity", CharSet = CharSet.Unicode)]
        static extern void toRosPointCloud(IntPtr api);

        [DllImport("pclUnity", CharSet = CharSet.Unicode)]
        static extern int getRosHeight(IntPtr api);

        [DllImport("pclUnity", CharSet = CharSet.Unicode)]
        static extern int getRosWidth(IntPtr api);

        [DllImport("pclUnity", CharSet = CharSet.Unicode)]
        static extern bool getRosBigendian(IntPtr api);

        [DllImport("pclUnity", CharSet = CharSet.Unicode)]
        static extern int getRosPointStep(IntPtr api);

        [DllImport("pclUnity", CharSet = CharSet.Unicode)]
        static extern int getRosRowStep(IntPtr api);

        [DllImport("pclUnity", CharSet = CharSet.Unicode)]
        static extern bool getRosDense(IntPtr api);

        [DllImport("pclUnity", CharSet = CharSet.Unicode)]
        static extern IntPtr getRosFieldName(IntPtr api);

        [DllImport("pclUnity", CharSet = CharSet.Unicode)]
        static extern IntPtr getRosFieldOffset(IntPtr api);

        [DllImport("pclUnity", CharSet = CharSet.Unicode)]
        static extern IntPtr getRosFieldDataType(IntPtr api);

        [DllImport("pclUnity", CharSet = CharSet.Unicode)]
        static extern IntPtr getRosFieldCount(IntPtr api);

        [DllImport("pclUnity", CharSet = CharSet.Unicode)]
        static extern int getRosFieldSize(IntPtr api);

        [DllImport("pclUnity", CharSet = CharSet.Unicode)]
        static extern IntPtr getRosData(IntPtr api);

        [DllImport("pclUnity", CharSet = CharSet.Unicode)]
        static extern int getRosDataSize(IntPtr api);
        //**************************************************************************************************************
        //**************************************************************************************************************
        //**************************************************************************************************************
        //**************************************************************************************************************

        /// <summary>        
        ///   How to fill and get sensorPointCloud2
        ///   1- construct pcl::PointCloud
        ///   2- push all needed points
        ///   3- convert to pcl::PCLpointcloud2
        ///   4- copy from pcl::PCLpointcloud2 to sensorPointCloud2
        ///   5- send sensorPointCloud2
        /// </summary>
        public pclInterface()
        {

        }

        /// <summary>
        /// creates PCL::pointCloud
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="is_dense"></param>
        public void createPclCloud(int width, int height, bool is_dense)
        {
            cloud = pclConnectionConstructor();
            setCloudWidth(cloud, width);
            setCloudHeight(cloud, height);
            setCloudDense(cloud, is_dense);
        }


        /// <summary>
        /// pushs a point to pcl::pointCloud
        /// </summary>
        /// <param name="pointXYZ"></param>
        public void pushPointToCloud(Vector3 pointXYZ)
        {
            pushPoint(cloud, pointXYZ.x, pointXYZ.y, pointXYZ.z);
        }


        /// <summary>
        /// Reads parameters of pcl::pointCloud
        /// </summary>
        /// <returns></returns>
        public string readCloudParameters()
        {
            string sout = "";
            sout += "width: " + getCloudWidth(cloud).ToString() + " ";
            sout += "height: " + getCloudHeight(cloud).ToString() + " ";
            sout += "is dense: " + getCloudDense(cloud).ToString();
            return sout;
        }

        /// <summary>
        /// Converts pcl::pointcloud to pcl::pclPointCloud2
        /// </summary>
        /// <param name="pc"></param>
        /// <param name="frame_id"></param>
        public void convertToRosMsgFromCloud(SensorPointCloud2 pc, string frame_id)
        {
            toRosPointCloud(cloud);
            copyToSensorPointCloud2(pc, frame_id);
        }


        /// <summary>
        /// Reads parameters of pcl::pclPointCloud2
        /// </summary>
        /// <returns></returns>
        public string readCloud2Parameters()
        {
            //::pcl::PCLHeader    headerX
            //pcl::uint32_t   heightX
            //pcl::uint32_t   widthX
            //std::vector < ::pcl::PCLPointField > fieldsX
            //      std::string nameX
            //      pcl::uint32_t offsetX
            //      pcl::uint8_t datatypeX
            //      pcl::uint32_t countX
            //pcl::uint8_t is_bigendianX
            //pcl::uint32_t point_stepX  
            //pcl::uint32_t row_stepX
            //std::vector<pcl::uint8_t> data
            //pcl::uint8_t is_denseX
            string sout = "";
            sout += "height: " + getRosHeight(cloud).ToString() + " ";
            sout += "width: " + getRosWidth(cloud).ToString() + " ";
            sout += "is_big-endian: " + getRosBigendian(cloud).ToString() + " ";
            sout += "point step: " + getRosPointStep(cloud).ToString() + " ";
            sout += "row step: " + getRosRowStep(cloud).ToString() + " ";
            sout += "is dense: " + getRosDense(cloud).ToString() + " ";
            sout += "Field parameters->";
            List<string> names = getFieldsStrings();
            List<int> offsets = getFieldsIntegers(1);
            List<int> dataTypes = getFieldsIntegers(2);
            List<int> counts = getFieldsIntegers(3);
            sout += "names: " + names[0] + " " + names[1] + " " + names[2] + " ";
            sout += "offsets: " + offsets[0] + " " + offsets[1] + " " + offsets[2] + " ";
            sout += "datatypes: " + dataTypes[0] + " " + dataTypes[1] + " " + dataTypes[2] + " ";
            sout += "counts: " + counts[0] + " " + counts[1] + " " + counts[2] + " ";            

            //unsafe
            //{
            //    byte* ptr = (byte*)getRosData(cloud);
            //    int length = getRosDataSize(cloud);
            //    byte[] rosByteArray = new byte[length];
            //    for (int i = 0; i < length; i++)
            //    {
            //        rosByteArray[i] = ptr[i];
            //    }                
            //    Debug.LogWarning("1data|"+rosByteArray[0]+"|"); 
            //}
            return sout;
        }

        /// <summary>
        /// copy from pcl::pclPointCloud2 to sensorPointCloud2 (i.e. sensor_msgs/PointCloud2)
        /// </summary>
        /// <param name="pc"></param>
        /// <param name="frame_id"></param>
        private void copyToSensorPointCloud2(SensorPointCloud2 pc, string frame_id)
        {

            //header should be already set            
            pc.header.frame_id = frame_id;

            pc.height = getRosHeight(cloud);
            pc.width = getRosWidth(cloud);
            pc.is_bigendian = getRosBigendian(cloud);
            pc.point_step = getRosPointStep(cloud);
            pc.row_step = getRosRowStep(cloud);
            pc.is_dense = getRosDense(cloud);

            int size = getRosFieldSize(cloud);
            List<string> names = getFieldsStrings();
            List<int> offsets = getFieldsIntegers(1);
            List<int> dataTypes = getFieldsIntegers(2);
            List<int> counts = getFieldsIntegers(3);
            //Debug.Log("***************************************");
            //Debug.Log("antes"+pc.fields.Length);
            Array.Resize<SensorPointField>(ref pc.fields, 3);
            //Debug.LogWarning("depois "+pc.fields.Length);
            for (int i = 0; i < size; i++)
            {
                SensorPointField field = new SensorPointField();
                field.datatype = dataTypes[i];
                field.name = names[i];
                field.offset = offsets[i];
                field.count = counts[i];
                pc.fields[i] = field;
            }

            //copy data
            unsafe
            {
                byte* ptr = (byte*)getRosData(cloud);
                int length = getRosDataSize(cloud);
                byte[] rosByteArray = new byte[length];
                for (int i = 0; i < length; i++)
                {
                    rosByteArray[i] = ptr[i];
                }
                pc.data = rosByteArray;
            }
        }

        /// <summary>
        /// Reads paramters of sensorPointCloud2
        /// </summary>
        /// <param name="pc"></param>
        /// <returns></returns>
        public string readCloudRosParameters(SensorPointCloud2 pc)
        {
            string sout = "";
            sout += "height: " + pc.height.ToString() + " ";
            sout += "width: " + pc.width.ToString() + " ";
            sout += "is_big-endian: " + pc.is_bigendian.ToString() + " ";
            sout += "point step: " + pc.point_step.ToString() + " ";
            sout += "row step: " + pc.row_step.ToString() + " ";
            sout += "is dense: " + pc.is_dense.ToString() + " ";
            sout += "Field parameters->";
            sout += "names: " + pc.fields[0].name + " " + pc.fields[1].name + " " + pc.fields[2].name + " ";
            sout += "offsets: " + pc.fields[0].offset + " " + pc.fields[1].offset + " " + pc.fields[2].offset + " ";
            sout += "datatypes: " + pc.fields[0].datatype + " " + pc.fields[1].datatype + " " + pc.fields[2].datatype + " ";
            sout += "counts: " + pc.fields[0].count + " " + pc.fields[1].count + " " + pc.fields[2].count + " ";
            sout += "data size: " + getRosDataSize(cloud);
            
            //Debug.LogWarning("1RRRR|" + pc.data[0]+"|");
            

            return sout;
        }


        /// <summary>
        /// Gets a string name list of every field in pcl::pclPointCloud
        /// </summary>
        /// <returns></returns>
        private List<string> getFieldsStrings()
        {
            unsafe
            {
                List<string> list = new List<string>();
                char** names = (char**)getRosFieldName(cloud);
                int size = getRosFieldSize(cloud);
                for (int i = 0; i < size; i++)
                {
                    list.Add(new string(names[i], 0, 1));
                }
                return list;
            }
        }

        /// <summary>
        /// Gets a integer list of every field in pcl::pclPointCloud
        /// </summary>
        /// <param name="parameter">1= offset
        /// 2=datatype
        /// 3=count</param>
        /// <returns></returns>
        private List<int> getFieldsIntegers(int parameter)
        {

            unsafe
            {
                List<int> list = new List<int>();
                int* ints = null;
                switch (parameter)
                {
                    case 1:
                        ints = (int*)getRosFieldOffset(cloud);
                        break;
                    case 2:
                        ints = (int*)getRosFieldDataType(cloud);
                        break;
                    case 3:
                        ints = (int*)getRosFieldCount(cloud);
                        break;
                    default:
                        break;
                }

                int size = getRosFieldSize(cloud);
                for (int i = 0; i < size; i++)
                {
                    list.Add(ints[i]);
                }
                return list;
            }
        }
        
        public void compareDataCloud2andCloudRos(SensorPointCloud2 pc)
        {
            unsafe
            {
                byte* ptr = (byte*)getRosData(cloud);
                int length = getRosDataSize(cloud);
                byte[] rosByteArray = new byte[length];
                string sout = "";
                for (int i = 0; i < length; i++)
                {
                    rosByteArray[i] = ptr[i];
                    sout += "(" + rosByteArray[i] + "," + pc.data[i] + ")   ";
                }
                Debug.Log("compara(cloud2,Ros)" + sout);
            }
        }

    }
}