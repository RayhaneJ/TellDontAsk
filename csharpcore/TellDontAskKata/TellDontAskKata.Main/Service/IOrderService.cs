using System.Collections.Generic;
using TellDontAskKata.Main.Dtos;

namespace TellDontAskKata.Main.Service
{
    public interface IOrderService
    {
        void ApproveOrder(int orderId);
        void CreateOrder(List<CreateOrderDto> orderDtos);
        void RejectOrder(int orderId);
        void ShipOrder(int orderId);
    }
}