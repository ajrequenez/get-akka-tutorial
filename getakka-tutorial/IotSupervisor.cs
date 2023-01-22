using System;
using Akka.Actor;
using Akka.Event;

namespace getakka_tutorial
{
    public class IotSupervisor : UntypedActor
    {
        public ILoggingAdapter Log { get; } = Context.GetLogger();

        protected override void PreStart() => Log.Info("IoT Application Started");
        protected override void PostStop() => Log.Info("IoT Application Stopped");

        protected override void OnReceive(object message)
        {
            // No need to handle any messages
        }

        public static Props Props() => Akka.Actor.Props.Create<IotSupervisor>();
    }
}

