using Castle.DynamicProxy;
using PokemonReviewApp.Controllers;
using Serilog;
using System.Linq;

namespace PokemonReviewApp.Aspect
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class LogAttribute : Attribute
    {
    }
    public class LoggingInterceptor : IInterceptor
    {
        private readonly ILogger<LoggingInterceptor> _logger;

        public LoggingInterceptor(ILogger<LoggingInterceptor> logger)
        {
            _logger = logger;
        }
        public void Intercept(IInvocation invocation)
        {

            // Attribute kontrolü
            var methodInfo = invocation.MethodInvocationTarget ?? invocation.Method;
            var hasLogAttribute = methodInfo.GetCustomAttributes(typeof(LogAttribute), true).Any() ||
                                  invocation.TargetType.GetCustomAttributes(typeof(LogAttribute), true).Any();

            if (hasLogAttribute)
            {
                // Metoda giriş logu
                var methodName = invocation.Method.Name;
                var arguments = string.Join(", ", invocation.Arguments.Select(a => a?.ToString() ?? "null"));
               // Log.Information($"Entering method {methodName} with arguments: {arguments}");
                _logger.LogInformation($"Entering method {methodName} with arguments: {arguments}");

                try
                {
                    // Metodu çağır
                    invocation.Proceed();

                    // Metottan dönüş logu
                    var returnValue = invocation.ReturnValue;
                    //Log.Information($"Exiting method {methodName} with return value: {returnValue}");
                    _logger.LogInformation($"Exiting method {methodName} with return value: {returnValue}");
                }
                catch (Exception ex)
                {
                    // Hata logu
                   // Log.Error(ex, $"Exception in method {methodName}");
                    _logger.LogError(ex, $"Exception in method {methodName}");
                    throw;
                }
            }
            else
            {
                // Metot üzerinde attribute yoksa doğrudan devam et
                invocation.Proceed();
            }
        }
    }
}
