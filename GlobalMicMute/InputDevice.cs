using NAudio.CoreAudioApi;

namespace GlobalMicMute
{
    public class InputDevice
    {

        public MMDevice Device { get; set; }
        public string Name;
        public bool Muted { get; private set; }

        public void SetMuted(bool value)
        {
            Muted = Device.AudioEndpointVolume.Mute = value;

        }

        public InputDevice(MMDevice inputDdevice)
        {
            Device = inputDdevice;
            Name = Device.FriendlyName;
            Muted = Device.AudioEndpointVolume.Mute;

        }

        public static List<InputDevice> GetInputDevices()
        {
            var enumerator = new MMDeviceEnumerator();
            var items = new List<InputDevice>();


            //cycle through all audio devices
            foreach (MMDevice device in enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active))
            {
                items.Add(new InputDevice(device));
            }

            enumerator.Dispose();

            return items;
        }
    }
}
