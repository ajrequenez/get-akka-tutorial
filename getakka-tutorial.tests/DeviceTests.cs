using Akka.TestKit.Xunit2;
using Xunit;
using FluentAssertions;
using Akka.Actor;

namespace getakka_tutorial.tests;

public class DeviceTests : TestKit
{
    [Fact]
    public void Device_actor_must_reply_with_empty_reading_if_no_temperature_is_known()
    {
        var probe = CreateTestProbe();
        var deviceActor = Sys.ActorOf(Device.Props("group", "device"));

        deviceActor.Tell(new ReadTemperature(requestId: 42), probe.Ref);
        var response = probe.ExpectMsg<RespondTemperature>();

        response.RequestId.Should().Be(42);
        response.Value.Should().Be(null);
    }

    [Fact]
    public void Device_actor_must_reply_with_last_reading_received()
    {
        var probe = CreateTestProbe();
        var deviceActor = Sys.ActorOf(Device.Props("group", "device"));
        double lastTemp = 98.2;

        // Set the new temperature
        deviceActor.Tell(new RecordTemperature(requestId: 1, lastTemp), probe.Ref);
        probe.ExpectMsg<TemperatureRecorded>(s => s.RequestId == 1);

        deviceActor.Tell(new ReadTemperature(requestId: 2), probe.Ref);
        var readResponse = probe.ExpectMsg<RespondTemperature>();

        readResponse.RequestId.Should().Be(2);
        readResponse.Value.Should().Be(lastTemp);

        // Set the new temperature
        deviceActor.Tell(new RecordTemperature(requestId: 3, lastTemp + 1.0), probe.Ref);
        probe.ExpectMsg<TemperatureRecorded>(s => s.RequestId == 3);

        deviceActor.Tell(new ReadTemperature(requestId: 4), probe.Ref);
        var readResponse2 = probe.ExpectMsg<RespondTemperature>();

        readResponse2.RequestId.Should().Be(4);
        readResponse2.Value.Should().Be(lastTemp + 1.0);
    }

    [Fact]
    public void Device_actor_must_reply_to_registration_requests()
    {
        var probe = CreateTestProbe();
        var deviceActor = Sys.ActorOf(Device.Props("group", "device"));

        deviceActor.Tell(new RegisterDevice("group", "device"), probe.Ref);
        probe.ExpectMsg<DeviceRegistered>();
        probe.LastSender.Should().Be(deviceActor);
    }

    [Fact]
    public void Device_actor_must_ignore_wrong_registration_requests()
    {
        var probe = CreateTestProbe();
        var deviceActor = Sys.ActorOf(Device.Props("group", "device"));

        deviceActor.Tell(new RegisterDevice("wrongGroup", "device"), probe.Ref);
        probe.ExpectNoMsg(TimeSpan.FromMilliseconds(500));

        deviceActor.Tell(new RegisterDevice("group", "Wrongdevice"), probe.Ref);
        probe.ExpectNoMsg(TimeSpan.FromMilliseconds(500));
    }
}
