namespace WebApi.Extensions.Models
{
    using Db.Models;

    using Dtos.Coffees;

    /// <summary>
    ///     Расширения для кофе.
    /// </summary>
    public static class CoffeeExtensions
    {
        /// <summary>
        ///     Маппинг DTO нового или обновленного кофе в модель.
        /// </summary>
        /// <param name="createdCoffeeDto">DTO нового кофе.</param>
        /// <returns>Модель кофе.</returns>
        public static Coffee ToModel(this CreateOrUpdateCoffeeDto createdCoffeeDto)
        {
            return new Coffee
            {
                Name = createdCoffeeDto.Name,
                Price = createdCoffeeDto.Price
            };
        }

        /// <summary>
        ///     Маппинг модели кофе в DTO.
        /// </summary>
        /// <param name="coffee">Модель кофе.</param>
        /// <returns>DTO кофе.</returns>
        public static CoffeeDto ToDto(this Coffee coffee)
        {
            return new CoffeeDto
            {
                Id = coffee.Id,
                Name = coffee.Name,
                Price = coffee.Price
            };
        }

        /// <summary>
        ///     Обновление модели кофе на основании обновленной модели кофе.
        /// </summary>
        /// <param name="existingCoffee">Существующий кофе.</param>
        /// <param name="updatedCoffee">Модель обновленного кофе.</param>
        public static void UpdateFromModel(this Coffee existingCoffee, Coffee updatedCoffee)
        {
            existingCoffee.Name = updatedCoffee.Name;
            existingCoffee.Price = updatedCoffee.Price;
        }

        /// <summary>
        ///     Маппинг DTO кофе в модель.
        /// </summary>
        /// <param name="coffeeDto">DTO кофе.</param>
        /// <returns>Модель кофе.</returns>
        public static Coffee ToModel(this CoffeeDto coffeeDto)
        {
            return new Coffee
            {
                Id = coffeeDto.Id,
                Name = coffeeDto.Name,
                Price = coffeeDto.Price
            };
        }

        /// <summary>
        ///     Маппинг моделей перечисления кофе в DTO.
        /// </summary>
        /// <param name="coffees">Перечисление моделей кофе.</param>
        /// <returns>Перечисление DTO кофе.</returns>
        public static IAsyncEnumerable<CoffeeDto> ToDto(this IAsyncEnumerable<Coffee> coffees)
        {
            return coffees.Select(coffee => coffee.ToDto());
        }
    }
}