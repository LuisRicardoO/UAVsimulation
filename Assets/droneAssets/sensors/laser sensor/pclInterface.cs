﻿using System;
using System.Collections.Generic;
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
        static extern float getPointAtX(IntPtr api, int at);

        [DllImport("pclUnity", CharSet = CharSet.Unicode)]
        static extern float getPointAtY(IntPtr api, int at);

        [DllImport("pclUnity", CharSet = CharSet.Unicode)]
        static extern float getPointAtZ(IntPtr api, int at);

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

        [DllImport("pclUnity", CharSet = CharSet.Unicode)]
        static extern byte getRosDataAt(IntPtr api, int at);

        [DllImport("pclUnity", CharSet = CharSet.Unicode)]
        static extern byte sendTest(IntPtr api);

        [DllImport("pclUnity", CharSet = CharSet.Unicode)]
        static extern IntPtr sendTestArray(IntPtr api);

        [DllImport("pclUnity", CharSet = CharSet.Unicode)]
        static extern bool cloudIsEmpty(IntPtr api);

        [DllImport("pclUnity", CharSet = CharSet.Unicode)]
        static extern IntPtr getRosDataFast(IntPtr api);

        [DllImport("pclUnity", CharSet = CharSet.Unicode)]
        static extern IntPtr getRosDataFastByParts(IntPtr api, int start, int end);

        [DllImport("pclUnity", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.SafeArray)]
        static extern IntPtr sendTestArray2(IntPtr api);

        [DllImport("pclUnity", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.SafeArray)]
        static extern IntPtr getRosDataPtr(IntPtr api);

        [DllImport("pclUnity", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.SafeArray)]
        static extern IntPtr getRosDataPtrAt(IntPtr api, int at);

        [DllImport("pclUnity", CharSet = CharSet.Unicode)]
        static extern void sendTestArray3(IntPtr api, ref IntPtr ptr);

        
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

        //MAIN METHODS**************************************************************************************************************
        /// <summary>
        /// pushs a point to pcl::pointCloud
        /// axis are converted to the ones used in Ros
        /// </summary>
        /// <param name="pointXYZ"></param>
        public void pushPointToCloud(Vector3 pointXYZ)
        {
            pushPoint(cloud, pointXYZ.x, pointXYZ.z, pointXYZ.y);
        }
        /// <summary>
        /// Converts pcl::pointcloud to pcl::pclPointCloud2
        /// </summary>
        /// <param name="pc"></param>
        /// <param name="frame_id"></param>        
        public void convertToCloud2()
        {
            toRosPointCloud(cloud);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pc"></param>
        /// <param name="frame_id"></param>
        public void convertToRosCloud(ref SensorPointCloud2 pc, string frame_id)
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
            //copyData
            cloud2RosCloud(ref pc);

        }


        //AUX METHODS****************************************************************************************************
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
        private byte readDataFromCloud2At(int at)
        {
            return getRosDataAt(cloud, at);
        }
        private void oldGetData(ref byte[] data)
        {
            unsafe
            {
                int length = getRosDataSize(cloud);
                Array.Resize<byte>(ref data, length);
                byte* ptr = (byte*)getRosData(cloud);
                for (int i = 0; i < length; i++)
                {
                    data[i] = ptr[i];
                }
            }
        }
        private void oldCloud2RosCloud(ref SensorPointCloud2 pc)
        {
            unsafe
            {
                int length = getRosDataSize(cloud);
                byte* ptr = (byte*)getRosDataFast(cloud);
                for (int i = 0; i < length; i++)
                {
                    pc.data[i] = ptr[i];
                }
            }
        }
        /// <summary>
        /// copy from cloud2 data to ros cloud data
        /// </summary>
        /// <param name="rosCloud"></param>
        private void cloud2RosCloud(ref SensorPointCloud2 rosCloud)
        {
            unsafe
            {
                //Array.Resize<byte>(ref rosCloud.data, getRosDataSize(cloud));
                rosCloud.data = new byte[getRosDataSize(cloud)];
                IntPtr ptr = getRosDataPtr(cloud);
                Marshal.Copy(ptr, rosCloud.data, 0, getRosDataSize(cloud));
            }
        }

        /// <summary>
        /// copy entire point cloud to part of the ros cloud msg
        /// </summary>
        /// <param name="rosCloud"></param>
        /// <param name="start"></param>
        /// <param name="size"></param>
        private void cloud2PartRosCloud(ref SensorPointCloud2 rosCloud,int start, int size)
        {
            unsafe
            {                             
                IntPtr ptr = getRosDataPtr(cloud);
                Marshal.Copy(ptr, rosCloud.data,start, size);
            }
        }


        //DEBUG METHDOS**************************************************************************************************    
        public bool cloudHasPoints()
        {
            return !cloudIsEmpty(cloud);
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
            sout += "first Point" + getPointAtX(cloud, 0) + " " + getPointAtY(cloud, 0) + " " + getPointAtZ(cloud, 0);
            return sout;
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
            sout += "data size: " + getRosDataSize(cloud) + " ";
            sout += "data: ";
            byte[] data = new byte[0];
            oldGetData(ref data);
            int length = getRosDataSize(cloud);
            for (int i = 0; i < length; i++)
            {
                sout += data[i] + " ";
            }
            //byte[] data = new byte[0];
            //getDataCloud2ToArray(ref data);
            //string converted = System.Text.Encoding.UTF8.GetString(data, 0, getRosDataSize(cloud));
            //sout += "datareal: " + converted + " ";
            //Debug.LogWarning("REALDAKSHGAS: " + " " + data[0] + data[1] + data[2] + data[3] + data[4]);
            //byte[] data2 = new byte[0];
            //testByteArray(ref data2);
            ////string converted2 = System.Text.Encoding.UTF8.GetString(data2,0,20);
            //Debug.LogWarning( "testData: " +" "+data2[0] + data2[1] + data2[2] + data2[3] + data2[4]);
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
            sout += "data size: " + getRosDataSize(cloud) + " ";
            sout += "data: ";
            int length = getRosDataSize(cloud);
            for (int i = 0; i < length; i++)
                sout += pc.data[i] + " ";

            return sout;
        }
        public string compareDataCloud2andCloudRos(SensorPointCloud2 pc)
        {
            string sout = "";
            byte[] data = new byte[0];
            oldGetData(ref data);
            int length = getRosDataSize(cloud);
            for (int i = 0; i < length; i++)
            {
                sout += "(" + pc.data[i] + "," + data[i] + ") ";
            }
            return sout;

            /*
            unsafe
            {
                byte* ptr = (byte*)getRosData(cloud);
                int length = getRosDataSize(cloud);
                byte[] rosByteArray = new byte[length];
                string sout = "";
                for (int i = 0; i < length; i++)
                {
                    //rosByteArray[i] = ptr[i];
                    sout += "(" + pc.data[i] + "," + pc.data[i] + ")   ";
                }
                Debug.Log("compara(cloud2,Ros)" + sout);
            }
            */
        }

        //TEST METHODS DO NOT USE THEM************************************************************************************
        public byte testByte()
        {
            return sendTest(cloud);
        }
        public void testByteArray(ref byte[] data)
        {
            unsafe
            {
                Array.Resize<byte>(ref data, 20);
                byte* ptr = (byte*)sendTestArray(cloud);
                for (int i = 0; i < 20; i++)
                {
                    data[i] = ptr[i];
                }
            }
        }
        public void testByteArray2(ref byte[] data)
        {

            unsafe
            {
                Array.Resize<byte>(ref data, 5);
                byte* ptr = (byte*)sendTestArray2(cloud);
                for (int i = 0; i < 5; i++)
                {
                    data[i] = ptr[i];
                }
            }

        }
        public void testByteArray22(ref int[] data)
        {
            unsafe
            {
                Array.Resize<int>(ref data, 5);
                IntPtr ptr = sendTestArray2(cloud);
                int[] result = new int[5];
                Marshal.Copy(ptr, result, 0, 5);
                for (int i = 0; i < 5; i++)
                {
                    data[i] = result[i];
                }
            }
            /*
            [DllImport("wrapper_demo_d.dll")]
            public static extern IntPtr fnwrapper_intarr();

            IntPtr ptr = fnwrapper_intarr();
            int[] result = new int[3];
            Marshal.Copy(ptr, result, 0, 3);    
            */

        }
        public void testByteArray222(ref byte[] data)
        {
            unsafe
            {
                Array.Resize<byte>(ref data, getRosDataSize(cloud));
                IntPtr ptr = getRosDataPtr(cloud);
                byte[] result = new byte[getRosDataSize(cloud)];
                Marshal.Copy(ptr, result, 0, getRosDataSize(cloud));
                for (int i = 0; i < getRosDataSize(cloud); i++)
                {
                    data[i] = result[i];
                }
            }
            /*
            [DllImport("wrapper_demo_d.dll")]
            public static extern IntPtr fnwrapper_intarr();

            IntPtr ptr = fnwrapper_intarr();
            int[] result = new int[3];
            Marshal.Copy(ptr, result, 0, 3);    
            */

        }
        public void convert()
        {
            toRosPointCloud(cloud);
        }
        public void testByteArray3(ref byte[] data)
        {
            unsafe
            {
                Array.Resize<byte>(ref data, 5);
                IntPtr pointer = new IntPtr();
                sendTestArray3(cloud, ref pointer);
                byte* ok = (byte*)pointer;
                for (int i = 0; i < 5; i++)
                {
                    data[i] = ok[i];
                }
            }
        }
    }
}