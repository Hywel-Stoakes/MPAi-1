﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace MPAid.Cores
{
    [Serializable]
    public class SysCfg : ISerializable
    {
        public SysCfg()
        {
            audioFolderAddr = new FolderAddress("Audio");
            videoFolderAddr = new FolderAddress("Video");
            recordingFolderAddr = new FolderAddress("Recording");
            reportFolderAddr = new FolderAddress("Report");
            annieFolderAddr = new FolderAddress("Annie");
        }
        public SysCfg(string path)
            : this()
        {
            this.filePath = path;
        }
        #region Serialization Control
        protected SysCfg(SerializationInfo info, StreamingContext context)
        {
            if (info == null) { throw new System.ArgumentNullException("info"); }
            this.audioFolderAddr = info.GetValue("Audio Recording Folder Address", typeof(FolderAddress)) as FolderAddress;
            this.videoFolderAddr = info.GetValue("Video Recording Folder Address", typeof(FolderAddress)) as FolderAddress;
            this.recordingFolderAddr = info.GetValue("Personal Recording Folder Address", typeof(FolderAddress)) as FolderAddress;
            this.reportFolderAddr = info.GetValue("Personal Report Folder Address", typeof(FolderAddress)) as FolderAddress;
            this.annieFolderAddr = info.GetValue("Annie's HTK Folder Address", typeof(FolderAddress)) as FolderAddress;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) { throw new System.ArgumentNullException("info"); }
            info.AddValue("Audio Recording Folder Address", this.audioFolderAddr);
            info.AddValue("Video Recording Folder Address", this.videoFolderAddr);
            info.AddValue("Personal Recording Folder Address", this.recordingFolderAddr);
            info.AddValue("Personal Report Folder Address", this.reportFolderAddr);
            info.AddValue("Annie's HTK Folder Address", this.annieFolderAddr);
        }
        #endregion

        [NonSerialized]
        private String filePath;
        [NonSerialized]
        public static readonly String path = "./SystemConfig.ini";

        private FolderAddress audioFolderAddr;
        public FolderAddress AudioFolderAddr
        {
            get {return audioFolderAddr; }
            set{ audioFolderAddr = value; }
        }

        private FolderAddress videoFolderAddr;
        public FolderAddress VideoFolderAddr
        {
            get{return videoFolderAddr;}
            set {videoFolderAddr = value;}
        }

        private FolderAddress recordingFolderAddr;
        public FolderAddress RecordingFolderAddr
        {
            get { return recordingFolderAddr; }
            set { recordingFolderAddr = value; }
        }

        private FolderAddress reportFolderAddr;
        public FolderAddress ReportFolderAddr
        {
            get { return reportFolderAddr; }
            set { reportFolderAddr = value; }
        }

        private FolderAddress annieFolderAddr;
        public FolderAddress AnnieFolderAddr
        {
            get { return annieFolderAddr; }
            set { annieFolderAddr = value; }
        }
    }

    [Serializable]
    public class FolderAddress : ISerializable
    {
        public FolderAddress()
        {}

        public FolderAddress(String subFolder)
            : this()
        {
            this.subFolder = subFolder;
        }

        #region Serialization Control
        protected FolderAddress(SerializationInfo info, StreamingContext context)
        {
            if (info == null) { throw new System.ArgumentNullException("info"); }
            this.baseFolder = info.GetString("Base Folder Address");
            this.subFolder = info.GetString("Sub Folder Address");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) { throw new System.ArgumentNullException("info"); }
            info.AddValue("Base Folder Address", this.baseFolder);
            info.AddValue("Sub Folder Address", this.subFolder);
        }
        #endregion

        private String baseFolder;
        public String BaseFolder
        {
            get { return baseFolder == null ? System.Windows.Forms.Application.StartupPath : baseFolder; }
            set { baseFolder = value; }
        }

        private String subFolder;
        public String SubFolder
        {
            get { return subFolder; }
            set { subFolder = value; }
        }

        private String folderAddr;
        public String FolderAddr
        {
            get
            {
                //by default, the url is ./Video
                folderAddr = folderAddr == null ? Path.Combine(BaseFolder, SubFolder) : folderAddr;
                System.IO.Directory.CreateDirectory(folderAddr);
                return folderAddr;
            }
            set
            {
                folderAddr = value;
            }
        }
    }
}
