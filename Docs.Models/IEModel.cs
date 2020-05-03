using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Docs.Models
{
    public enum CustomErrorCode
    {
        Sucess,
        SucessWithError,
        NoOperation,
        InvalidParameter,
        NullParameter,
        KeyDuplicated,
        KeyNotFound,
        FieldDuplicated,
    }

    // Information Exchange Model
    // Responsible for information exchange: operation status code, payload and messages across services

    //TODO 001:
    // Update Payload field, so multiple payloads can be set along a request (array?, list?)
    //TODO 002:
    // Get HTTP Status Code alongside. Is is necessary though?
    public class IEModel<T>
    {
        public IEModel()
        {
            this.CustomMessage = null;
            this.ErrorStatusCode = CustomErrorCode.NoOperation;
            this.Payload = default;
        }
        public IEModel(T payload)
        {
            this.ErrorStatusCode = CustomErrorCode.Sucess;
        }
        public IEModel(string msg)
        {
            this.CustomMessage = msg;
            this.ErrorStatusCode = CustomErrorCode.Sucess;
            this.Payload = default;
        }
        public IEModel(string msg, CustomErrorCode status)
        {
            this.CustomMessage = msg;
            this.ErrorStatusCode = status;
            this.Payload = default;
        }
        public IEModel(string msg, CustomErrorCode status, T payload)
        {
            this.CustomMessage = msg;
            this.ErrorStatusCode = status;
            this.Payload = payload;
        }
        public CustomErrorCode ErrorStatusCode { get; set; }
        public string CustomMessage { get; set; }
        public T Payload { get; set; }
    }
}
