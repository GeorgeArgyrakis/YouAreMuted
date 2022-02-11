using NAudio.CoreAudioApi;

namespace GlobalMicMute
{
    public class DeviceVolumeNotificationHandler
    {
        private readonly string _deviceId;
        private Action<string, AudioVolumeNotificationData>? _handleAction;

        public DeviceVolumeNotificationHandler(string deviceId)
        {
            _deviceId = deviceId;
        }

        public void Handle(AudioVolumeNotificationData data)
        {
            _handleAction?.Invoke(_deviceId, data);
        }

        public void SetHandleAction(Action<string, AudioVolumeNotificationData> action)
        {
            _handleAction = action;
        }
    }
}
