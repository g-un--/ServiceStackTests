using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;

namespace ServiceStackAsync
{
    class MessageBus : IObserver<string>
    {
        #region static

        private static readonly MessageBus bus = new MessageBus();

        public static MessageBus Instance
        {
            get
            {
                return bus;
            }
        }

        #endregion

        private MessageBus() {}

        private Subject<string> messagesSubject = new Subject<string>();

        public IObservable<string> Bus
        {
            get
            {
                return Observable.Create<string>(observer =>
                    {
                        return messagesSubject.SubscribeSafe<string>(observer);
                    });
            }
        }

        public void OnCompleted()
        {
            messagesSubject.OnCompleted();
        }

        public void OnError(Exception error)
        {
            messagesSubject.OnError(error);
        }

        public void OnNext(string value)
        {
            messagesSubject.OnNext(value);
        }
    }
}
