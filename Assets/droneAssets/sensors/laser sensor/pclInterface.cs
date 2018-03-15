using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System;
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

        public void createPclCloud(int width, int height, bool is_dense)
        {
            cloud = pclConnectionConstructor();
            setCloudWidth(cloud, width);
            setCloudHeight(cloud, height);
            setCloudDense(cloud, is_dense);
        }

        public void pushPointToCloud(Vector3 pointXYZ)
        {
            pushPoint(cloud, pointXYZ.x, pointXYZ.y, pointXYZ.z);
        }

        public string readCloudParameters()
        {
            string sout = "";
            sout += "width: " + getCloudWidth(cloud).ToString() + " ";
            sout += "height: " + getCloudHeight(cloud).ToString() + " ";
            sout += "is dense: " + getCloudDense(cloud).ToString();
            return sout;
        }

        public void getRosMsgFromCloud(SensorPointCloud2 pc)
        {

            toRosPointCloud(cloud);
            copyToSensorPointCloud2(pc);
        }

        public string readCloud2Parameters()
        {
            //::pcl::PCLHeader    headerX
            //pcl::uint32_t   heightX
            //pcl::uint32_t   widthX
            //std::vector < ::pcl::PCLPointField > fields
            //      std::string name
            //      pcl::uint32_t offset
            //      pcl::uint8_t datatype
            //      pcl::uint32_t count
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
            return sout;
        }

        private void copyToSensorPointCloud2(SensorPointCloud2 pc)
        {


        }


    }
}
