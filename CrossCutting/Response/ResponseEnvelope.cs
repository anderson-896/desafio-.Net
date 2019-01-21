using Core.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossCutting.Response
{
    public class ResponseEnvelope : IResponseEnvelope
    {
        public ValidationMessage Message { get; internal set; }
        public ValidationErrorMessage Error { get; internal set; }

        public bool Success { get; internal set; } = true;

        public static ResponseEnvelope CreateResponseEnvelope(ValidationMessage? message = null)
        {
            return new ResponseEnvelope
            {
                Message = message ?? ValidationMessageHelper.Create(ValidationMessages.SUCCESS),
                Success = true
            };
        }

        public static ResponseEnvelope CreateErrorResponseEnvelope(ValidationMessage message)
        {
            return new ResponseEnvelope
            {
                Message = message,
                Success = false
            };
        }

        public static ResponseEnvelope<T> CreateResponseEnvelope<T>(T content, ValidationMessage? message = null)
        {
            var response = new ResponseEnvelope<T>
            {
                Content = content,
                Success = true,
                Message = message ?? ValidationMessageHelper.Create(ValidationMessages.SUCCESS)
            };

            return response;
        }

        public static ResponseEnvelope<T> CreateErrorResponseEnvelope<T>(ValidationErrorMessage message, T content = default(T))
        {
            var response = new ResponseEnvelope<T>
            {
                Content = content,
                Success = false,
                Error = message,
            };

            return response;
        }


    }

    public class ResponseEnvelope<T> : ResponseEnvelope, IResponseEnvelope<T>
    {
        public T Content { get; set; }
    }

    public static class ValidationMessageHelper
    {
        public static ValidationMessage Create(string messageWithId, bool isError = false)
        {
            int id;
            string[] idAndMessage = messageWithId.Split('|');
            if (int.TryParse(idAndMessage.ElementAtOrDefault(0), out id))
            {
                return new ValidationMessage(idAndMessage.ElementAtOrDefault(1).Trim(), id);
            }
            else
                throw new ApplicationException();
        }

        public static ValidationErrorMessage CreateErrorMessage(string messageWithId)
        {
            int id;
            string[] idAndMessage = messageWithId.Split('|');
            if (int.TryParse(idAndMessage.ElementAtOrDefault(0), out id))
            {
                return new ValidationErrorMessage(idAndMessage.ElementAtOrDefault(1).Trim(), id);
            }
            else
                throw new ApplicationException();
        }
    }
}
