using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Production.Service.Interfaces
{
    public interface IReceiveMessagesFactory
    {
        ReceiveMessagesNewOrderService CreateNewOrderService(string topic, string subscription, string filterName = null, string filter = null);
        //ReceiveMessagesUserWithLessOfferService CreateNewUserWithLessOfferService(string topic, string subscription, string filterName = null, string filter = null);
    }
}
