using System;
using Akka.Actor;
using Akka.TestKit.Xunit2;
using FluentAssertions;
using Xunit;

namespace getakka_tutorial.tests
{
	public class DeviceManagerTests : TestKit
    {
        [Fact]
        public void DeviceManager_actor_must_be_able_to_register_a_device_actor()
        {
            var probe = CreateTestProbe();
            var managerActor = Sys.ActorOf(DeviceManager.Props());

            managerActor.Tell(new RegisterDevice("group", "device1"), probe.Ref);
            probe.ExpectMsg<DeviceRegistered>();
            var deviceActor1 = probe.LastSender;

            managerActor.Tell(new RegisterDevice("group", "device2"), probe.Ref);
            probe.ExpectMsg<DeviceRegistered>();
            var deviceActor2 = probe.LastSender;
            deviceActor1.Should().NotBe(deviceActor2);

            // Check that the device actors are working
            deviceActor1.Tell(new RecordTemperature(requestId: 0, value: 1.0), probe.Ref);
            probe.ExpectMsg<TemperatureRecorded>(s => s.RequestId == 0);
            deviceActor2.Tell(new RecordTemperature(requestId: 1, value: 2.0), probe.Ref);
            probe.ExpectMsg<TemperatureRecorded>(s => s.RequestId == 1);
        }

        // Not needed as Device Manager will handle al Groups
        // [Fact]
        // public void DeviceManager_actor_must_ignore_requests_for_wrong_groupId()

        [Fact]
        public void DeviceManager_actor_must_return_same_actor_for_same_deviceId()
        {
            var probe = CreateTestProbe();
            var managerActor = Sys.ActorOf(DeviceManager.Props());

            managerActor.Tell(new RegisterDevice("group", "device1"), probe.Ref);
            probe.ExpectMsg<DeviceRegistered>();
            var deviceActor1 = probe.LastSender;

            managerActor.Tell(new RegisterDevice("group", "device1"), probe.Ref);
            probe.ExpectMsg<DeviceRegistered>();
            var deviceActor2 = probe.LastSender;

            deviceActor1.Should().Be(deviceActor2);
        }

    }
}

