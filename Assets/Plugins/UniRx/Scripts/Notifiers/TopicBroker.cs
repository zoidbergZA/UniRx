using System;
using System.Collections;
using System.Collections.Generic;

namespace UniRx
{
    public interface ITopicPublisher
    {
        /// <summary>
        /// Send Message to all receiver.
        /// </summary>
        void Publish(string topic, object message);
    }

    public interface ITopicReceiver
    {
        /// <summary>
        /// Subscribe typed message.
        /// </summary>
        IObservable<object> Receive(string topic);
    }

    public interface ITopicBroker : ITopicPublisher, ITopicReceiver
    {
    }

    public class TopicBroker : ITopicBroker
    {
        /// <summary>
        /// TopicBroker in Global scope.
        /// </summary>
        public static readonly ITopicBroker Default = new TopicBroker();

        bool isDisposed = false;
        readonly Dictionary<string, object> notifiers = new Dictionary<string, object>();

        public void Publish(string topic, object message)
        {
            object notifier;
            lock (notifiers)
            {
                if (isDisposed) return;

                if (!notifiers.TryGetValue(topic, out notifier)) return;
            }
            ((ISubject<object>)notifier).OnNext(message);
        }

        public IObservable<object> Receive(string topic)
        {
            object notifier;
            lock (notifiers)
            {
                if (isDisposed) throw new ObjectDisposedException("TopicBroker");

                if (!notifiers.TryGetValue(topic, out notifier))
                {
                    ISubject<object> n = new Subject<object>().Synchronize();
                    notifier = n;
                    notifiers.Add(topic, notifier);
                }
            }

            return ((IObservable<object>)notifier).AsObservable();
        }

        public void Dispose()
        {
            lock (notifiers)
            {
                if (!isDisposed)
                {
                    isDisposed = true;
                    notifiers.Clear();
                }
            }
        }
    }
}
