using System;
using System.Collections.Generic;
using System.Linq;

namespace MPAid
{
    public class NAudioController
    {
        private const string micPrefix = "mic";

        public NAudioController()
        {
            try
            {
                InitializeNAudio();
            }
            catch (Exception)
            { }
        }

        public List<NAudio.CoreAudioApi.MMDevice> DeviceList;

        private void InitializeNAudio()
        {
            // Get all Audio devices in the system
            DeviceList = new List<NAudio.CoreAudioApi.MMDevice>();
            NAudio.CoreAudioApi.MMDeviceEnumerator enumerator =
                new NAudio.CoreAudioApi.MMDeviceEnumerator();
            var devices = enumerator.EnumerateAudioEndPoints(NAudio.CoreAudioApi.DataFlow.All,
                NAudio.CoreAudioApi.DeviceState.Active);
            DeviceList.AddRange(devices.ToArray());
        }

        public bool StatusOK()
        {
            if (DeviceList == null)
                return false;
            return DeviceList.Count > 0;
        }

        private int GetNAudioMasterPeakValue(NAudio.CoreAudioApi.MMDevice device)
        {
            int result = 0;
            if (device == null)
                goto emptyReturn;

            try
            {
                result = (int)(Math.Round(device.AudioMeterInformation.MasterPeakValue * 100));
                return result;
            }
            catch (Exception)
            {
                goto emptyReturn;
            }

        emptyReturn:
            return 0;
        }

        public int GetValue()
        {
            if (GetMicDevice() != null)
                return GetNAudioMasterPeakValue(GetMicDevice());
            else
                return 0;
        }

        private NAudio.CoreAudioApi.MMDevice GetMicDevice()
        {
            if (!StatusOK())
                return null;

            foreach (NAudio.CoreAudioApi.MMDevice device in DeviceList)
                if (device.FriendlyName.ToLower().Contains(micPrefix))
                    if (GetNAudioMasterPeakValue(device) > 0)
                        return device;

            // If no audio device with "mic" keyword is found
            foreach (NAudio.CoreAudioApi.MMDevice device in DeviceList)
                if (GetNAudioMasterPeakValue(device) > 0)
                    return device;

            return null;
        }

    }
}
