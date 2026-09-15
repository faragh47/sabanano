using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Infrastructure.Services.SMSProvider.Faraz;
using Common;

using DataTransferObjects.SharedModels;
using Newtonsoft.Json;
using X.PagedList;

namespace Services.IServices.OutgoingApi
{
    public class HttpHeader 
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public abstract class ApiClientFactory
    {
        private readonly IHttpClientFactory _clientFactory;
        public static AccessTokenFaraz AccessTokenFaraz { get; set; }

        public ApiClientFactory(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public virtual async Task<HttpResponseMessage> GetAsync(string url, string token = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = _clientFactory.CreateClient();
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            HttpResponseMessage response = await client.SendAsync(request);
            return response;
        }

        public virtual async Task<HttpResponseMessage> GetAsync(string url, HttpHeader httpHeader = null, string token = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = _clientFactory.CreateClient();
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            if (httpHeader is not null)
            {
                client.DefaultRequestHeaders.Add(httpHeader.Name, httpHeader.Value);
            }
            HttpResponseMessage response = await client.SendAsync(request);
            return response;
        }

        //Task<ApiResult<List<TListDto>>> GetAllAsync(string url);
        //Task<ApiResult<IList<TListDto>>> SearchAsync(string url, TSearchDto searchObj);
        public virtual async Task<HttpResponseMessage> PostAsync<T>(string url, T objToCreate, HttpHeader httpHeader=null , string token = null) where T:class,new()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (objToCreate != null)
            {
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(objToCreate), Encoding.UTF8, "application/json");
            }
            else
            {
                return null;
            }
            var client = _clientFactory.CreateClient();
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            if (httpHeader is not null)
            {
                client.DefaultRequestHeaders.Add(httpHeader.Name, httpHeader.Value);
            }

            HttpResponseMessage response = await client.SendAsync(request);
            return response;
        }
        //Task<ApiResult<TListDto>> UpdateAsync(string url, TCuDto objToUpdate);
        //Task<ApiResult<TListDto>> DeleteAsync(string url, TKey Id);
        //Task<ApiResult<IPagedList<List<TListDto>>>> GetPagedListAsync(string url);

        protected async Task<ApiResult<T>> GenerateResultValueAsync<T>(HttpResponseMessage responseMessage, bool base64 = false) where T : class
        {
            try
            {
                var jsonString = await responseMessage.Content.ReadAsStringAsync();

                switch (responseMessage.StatusCode)
                {
                    case HttpStatusCode.OK:
                        var apiResult = JsonConvert.DeserializeObject<T>(jsonString);
                        return new ApiResult<T>(true, ApiResultStatusCode.Success, apiResult);
                    case HttpStatusCode.Created:
                        apiResult = JsonConvert.DeserializeObject<T>(jsonString);
                        return new ApiResult<T>(true, ApiResultStatusCode.Success, apiResult);
                    case HttpStatusCode.NotFound:
                        return new ApiResult<T>(false, ApiResultStatusCode.NotFound, null);
                    case HttpStatusCode.InternalServerError:
                        apiResult = JsonConvert.DeserializeObject<T>(jsonString);
                        return new ApiResult<T>(false, ApiResultStatusCode.ServerError, apiResult);
                    case HttpStatusCode.Unauthorized:
                        return new ApiResult<T>(false, ApiResultStatusCode.UnAuthorized, null);
                    default:
                        return new ApiResult<T>(false, ApiResultStatusCode.LogicError, null);
                }
            }
            catch (Exception ex)
            {
                return new ApiResult<T>(isSuccess: false, ApiResultStatusCode.ServerError, null);
            }

        }
    }
}
