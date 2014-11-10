using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;
using HealthDemo.Service;
using HealthDemo.Models.ResponseModel;
using HealthDemo.Service.RequestModel;
using System.Threading.Tasks;
using Newtonsoft.Json;
using HealthDemo;
using HealthDemo.Models;

[assembly: Xamarin.Forms.Dependency(typeof(HealthWebService))]
namespace HealthDemo
{
    public class HealthWebService : IWebService
    {
        private RestClient client;

        protected RestClient Client
        {
            get
            {
                return client;
            }
        }

        public HealthWebService()
		{
			client = new RestClient(Constants.ApiUrl);
			client.AddDefaultHeader("Accept", "application/json");
		}

        public async void SearchDoctors(SearchDoctorRequest request, Action<DoctorResponse> onCompleted)
        {
            var asyncResult = await ExecuteServiceMethod<DoctorResponse>("doctors/SearchDoctors", Method.POST,content =>
            {
                var response = new DoctorResponse() { Result = JsonConvert.DeserializeObject<List<Doctor>>(content) };
                return response;
            }, request);
            onCompleted(asyncResult);
        }

        public async void GetCategories(Action<CategoryResponse> onCompleted)
        {
            var asyncResult = await ExecuteServiceMethod<CategoryResponse>("TipCategories", Method.GET, content =>
            {
                var response = new CategoryResponse() { Result = JsonConvert.DeserializeObject<List<HealthCategory>>(content) };
                return response;
            });
            onCompleted(asyncResult);
        }

        public async void GetHealthTipsByCategory(int categoryID, Action<HealthTipResponse> onCompleted)
        {
            var asyncResult = await ExecuteServiceMethod<HealthTipResponse>(string.Format("Tips/GetByCatId/?id={0}", categoryID), Method.GET, content =>
                {
                    var response = new HealthTipResponse() { Result = JsonConvert.DeserializeObject<List<HealthTip>>(content) };
                    return response;
                });
            onCompleted(asyncResult);
        }

        public async void GetSpeicalties(Action<PositionResponse> onCompleted)
        {
            var asyncResult = await ExecuteServiceMethod<PositionResponse>("position", Method.GET, content =>
                {
                    var response = new PositionResponse() { Result = JsonConvert.DeserializeObject<List<DocPosition>>(content) };
                    return response;
                });
            onCompleted(asyncResult);
        }

        public Task<T> ExecuteServiceMethod<T>(string resource, Method method, Func<string, T> deserialiser, object requestObject = null) where T : ResponseBase
        {
            var restRequest = new RestRequest(resource, method);
            if (requestObject != null)
            {
                restRequest.RequestFormat = DataFormat.Json;
                restRequest.AddBody(requestObject);
            }

            return Task.Run<T>(() =>
            {
                T response = Activator.CreateInstance<T>();
                var errorResponse = new ErrorResponseModel();
                try
                {
                    var restResponse = Client.Execute(restRequest);
                    this.CheckServer(restResponse.Content);
                    if (!string.IsNullOrEmpty(restResponse.Content))
                    {
                        response = deserialiser(restResponse.Content);// JsonConvert.DeserializeObject<T>(restResponse.Content);
                        if (restResponse.Content.Contains("ExceptionMessage"))
                            errorResponse = JsonConvert.DeserializeObject<ErrorResponseModel>(restResponse.Content);
                        else response.Success = true;
                    }
                    else
                    {
                        errorResponse.ExceptionMessage = Constants.NoInternetMessage;
                    }
                    if (errorResponse != null && !string.IsNullOrEmpty(errorResponse.ExceptionMessage))
                    {
                        response.Success = false;
                        response.ErrorMessage = errorResponse.ExceptionMessage;
                    }
                }
                catch (Exception)
                {
                    response.Success = false;
                    response.ErrorMessage = "Server is down please try later.";
                }
                return response;
            });
        }


        public void CheckServer(string responsString)
        {
            string htmlContent = "<!DOCTYPE";
            if (responsString.Contains(htmlContent))
                throw new Exception("Server is down please try later.");
        }
    }
}