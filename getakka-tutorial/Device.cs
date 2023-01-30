using System;
using Akka.Actor;
using Akka.Event;

namespace getakka_tutorial
{
	public class Device : UntypedActor
	{
        private double? _lastTemperatureReading = null;

        protected string GroupId { get; }
        protected string DeviceId { get; }

        protected ILoggingAdapter Log { get; } = Context.GetLogger();

        protected override void PreStart() => Log.Info($"Device actor {GroupId}-{DeviceId} is STARTING");
        protected override void PostStop() => Log.Info($"Device actor {GroupId}-{DeviceId} has STOPPED");

        public Device(string groupId, string deviceId)
		{
            GroupId = groupId;
            DeviceId = deviceId;
		}

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case RegisterDevice req when req.GroupId.Equals(GroupId) && req.DeviceId.Equals(DeviceId):
                    Sender.Tell(DeviceRegistered.Instance);
                    break;
                case RegisterDevice req:
                    Log.Warning($"Ignoring TrackDevice request for {req.GroupId}-{req.DeviceId}.This actor is responsible for {GroupId}-{DeviceId}.");
                    break;

                case ReadTemperature read:
                    Sender.Tell(new RespondTemperature(read.RequestId, _lastTemperatureReading));
                    break;
                case RecordTemperature record:
                    Log.Info($"Recorded temperature reading {record.Value} with {record.RequestId}");
                    _lastTemperatureReading = record.Value;
                    Sender.Tell(new TemperatureRecorded(record.RequestId));
                    break;
            }
        }

        public static Props Props(string groupId, string deviceId) =>
            Akka.Actor.Props.Create(() => new Device(groupId, deviceId));
    }
}

