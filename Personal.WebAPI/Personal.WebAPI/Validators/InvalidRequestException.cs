using System.Runtime.Serialization;
using System.Text.Json;

namespace Personal.WebAPI.Validators
{
    public class InvalidRequestException : Exception
    {
        public InvalidRequestException()
        {
        }

        public InvalidRequestException(IEnumerable<string> errorMessages)
            : base(JsonSerializer.Serialize(errorMessages.Distinct()))
        {
        }

        public InvalidRequestException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
