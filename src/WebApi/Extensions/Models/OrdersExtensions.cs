namespace WebApi.Extensions.Models
{
    using Db.Models;

    using Dtos.Orders;

    /// <summary>
    ///     Расширения для заказа.
    /// </summary>
    public static class OrdersExtensions
    {
        /// <summary>
        ///     Маппинг DTO нового или обновленного заказа в модель.
        /// </summary>
        /// <param name="createdOrderDto">DTO нового заказа.</param>
        /// <returns>Модель заказа.</returns>
        public static Order ToModel(this CreateOrUpdateOrderDto createdOrderDto)
        {
            return new Order
            {
                Coffee = createdOrderDto.Coffee.ToModel(),
                User = createdOrderDto.User?.ToModel()
            };
        }

        /// <summary>
        ///     Маппинг модели заказа в DTO.
        /// </summary>
        /// <param name="order">Модель заказа.</param>
        /// <returns>DTO заказа.</returns>
        public static OrderDto ToDto(this Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                Coffee = order.Coffee.ToDto(),
                User = order.User?.ToDto()
            };
        }

        /// <summary>
        ///     Маппинг моделей перечисления заказов в DTO.
        /// </summary>
        /// <param name="orders">Перечисление моделей заказов.</param>
        /// <returns>Перечисление DTO заказов.</returns>
        public static IAsyncEnumerable<OrderDto> ToDto(this IAsyncEnumerable<Order> orders)
        {
            return orders.Select(order => order.ToDto());
        }
    }
}