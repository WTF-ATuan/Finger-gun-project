using UniRx;
using System;

public class EventAggregator{
	private static IMessageBroker _messageBroker;
	private static IMessageBroker MessageBroker => _messageBroker ??= new MessageBroker();

	public static void Publish<T>(T message){
		MessageBroker.Publish(message);
	}

	public static IObservable<T> OnEvent<T>(){
		return MessageBroker.Receive<T>();
	}
}
