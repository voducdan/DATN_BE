using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace DATN.Infrastructure.Responses
{
    public class ErrMessage
    {
        public string Name { get; set; }
        public List<string> Errs { get; set; } = new List<string>();
    }
    public class ActionResponse
    {
        public int? PageIndex { get; set; }
        public int? TotalPage { get; set; }
        public string Type
        {
            get
            {
                if (errMessages.Count > 0)
                    return "Detect error by Team";
                return null;
            }
        }
        public string Title
        {
            get
            {
                if (errMessages.Count > 0)
                    return "One or more validation errors occurred.";
                return null;
            }
        }
        public int StatusCode { get; set; } = 200;
        public object Data { get; set; }
        public OrderedDictionary Errors
        {
            get
            {
                if (errMessages.Count == 0)
                    return null;
                OrderedDictionary errors = new OrderedDictionary();
                foreach (var errMessage in errMessages)
                {
                    errors.Add(errMessage.Name, errMessage.Errs);
                }
                return errors;
            }
        }
        public string Message { get; set; }
        public void SetMessage(string message)
        {
            Message = message;
        }
        public void SetCreatedObject(object createdObject)
        {
            this.Data = createdObject;
            this.StatusCode = 201;
        }

        private List<ErrMessage> errMessages = new List<ErrMessage>();
        private ErrMessage GetErrMessage(string name)
        {
            ErrMessage errMessage = errMessages.FirstOrDefault(dd => dd.Name == name);
            if (errMessage == null)
            {
                errMessage = new ErrMessage() { Name = name };
                errMessages.Add(errMessage);
            }
            return errMessage;
        }
        public void AddMessageErr(string name, string message)
        {
            ErrMessage errMessage = GetErrMessage(name);
            errMessage.Errs.Add(message);
        }
        public void AddRequirementErr(string name)
        {
            AddMessageErr(name, $"The {name} field is required");
        }
        public void AddNotAllowedErr()
        {
            StatusCode = 403;
            AddMessageErr("Permission", $"Your permission's denied");
        }
        public void AddNotFoundErr(string name)
        {
            StatusCode = 404;
            AddMessageErr(name, $"The {name} field's not found");
        }
        public void SetNoContent()
        {
            StatusCode = 204;
            Data = null;
        }
        public void AddExpiredErr(string name)
        {
            StatusCode = 400;
            AddMessageErr(name, $"The {name} field exceeds expiring time");
        }
        public void AddInvalidErr(string name)
        {
            StatusCode = 400;
            AddMessageErr(name, $"The {name} field's invalid");
        }
        public void AddExistedErr(string name)
        {
            StatusCode = 400;
            AddMessageErr(name, $"The {name} field already exists");
        }
        public IActionResult ToIActionResult()
        {
            return new ObjectResult(this) { StatusCode = this.StatusCode };
        }
    }
}
