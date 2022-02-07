using DevIO.Api.Controllers;
using DevIO.Business.Interfaces;
using Elmah.Io.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace DevIO.Api.V2.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/teste")]
    public class TesteController : MainController
    {
        private readonly ILogger _logger;
        public TesteController(INotificador notificador, IUser appUser, ILogger<TesteController> logger) : base(notificador, appUser)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Valor()
        {
            // simulando problema
            //try
            //{
            //    var i = 0;
            //    var result = 22 / i;
            //}
            //catch(DivideByZeroException e)
            //{
            //    e.Ship(HttpContext); // Ship é um método eo Elmah que envia o erro para o servidor do Elmah
            //}
            //throw new Exception("Lançando erro!");

            _logger.LogTrace("Log de Trace"); // menos impactante
            _logger.LogDebug("Log de Debug"); // p/ debug
            _logger.LogInformation("Log de Informação"); // grava qlq informação que queiramos registrar
            _logger.LogWarning("Log de aviso"); // podemos reigstrar um erro 404, um warning...
            _logger.LogError("Log de erro"); // houve de fato um erro
            _logger.LogCritical("Log de problema crítico"); // erro muito prejudicial a aplicação

            return "Sow a V2";
        }
    }
}
