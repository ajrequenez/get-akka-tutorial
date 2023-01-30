using System;
namespace getakka_tutorial
{
    public sealed class ReadTemperature
    {
        public ReadTemperature(long requestId)
        {
            RequestId = requestId;
        }

        public long RequestId { get; }
    }

    public sealed class RespondTemperature
    {
        public RespondTemperature(long requestId, double? value)
        {
            RequestId = requestId;
            Value = value;
        }

        public long RequestId { get; }
        public double? Value { get; }
    }

    public sealed class RecordTemperature
    {
        public RecordTemperature(long requestId, double value)
        {
            RequestId = requestId;
            Value = value;
        }

        public long RequestId { get; }
        public double Value { get; }
    }

    public sealed class TemperatureRecorded
    {
        public TemperatureRecorded(long requestId)
        {
            RequestId = requestId;
        }

        public long RequestId { get; }
    }

    public sealed class RegisterDevice
    {
        public RegisterDevice(string groupId, string deviceId)
        {
            GroupId = groupId;
            DeviceId = deviceId;
        }

        public string GroupId { get; }
        public string DeviceId { get; }
    }

    public sealed class DeviceRegistered
    {
        public static DeviceRegistered Instance { get; } = new DeviceRegistered();
        private DeviceRegistered() { }
    }
}

