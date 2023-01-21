namespace WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    ///     Тестовый контроллер для обработки запросов.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TestController
    {
        /// <summary>
        ///     Тестовый метод для проверки работы контроллера.
        /// </summary>
        /// <returns>Приветствие для пользователя.</returns>
        [HttpGet]
        public string Hello()
        {
            return "Hello ASP.NET";
        }
    }
}