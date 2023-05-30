﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ModelsApi;
using MyCar.Server.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using System.Windows;

namespace MyCar.Web.Core
{
    
        public class Api
        {
            static HttpClient client = new HttpClient(); //
            static string server = "http://localhost:5243/api/";
            static JsonSerializerOptions jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            public static async Task<T> GetListAsync<T>(string controller)
            {
                var answer = await client.GetAsync(server + controller);
                string answerText = await answer.Content.ReadAsStringAsync();
                var result = (T)JsonSerializer.Deserialize(answerText, typeof(T), jsonOptions);
                return result;
            }

            public static async Task<T> GetAsync<T>(int id, string controller)
            {
                var answer = await client.GetAsync(server + controller + $"/{id}");
                string answerText = await answer.Content.ReadAsStringAsync();
                var result = (T)JsonSerializer.Deserialize(answerText, typeof(T), jsonOptions);
                return result;
            }

            public static async Task<T> SearchAsync<T>(string type, string? text, string controller)
            {
                var answer = await client.GetAsync(server + controller + $"/Type, Text?type={type}&text={text}");
                string answerText = await answer.Content.ReadAsStringAsync();
                var result = (T)JsonSerializer.Deserialize(answerText, typeof(T), jsonOptions);
                return result;
            }
            public static async Task<T> SearchFilterAsync<T>(string type, string? text, string controller, string? filter)
            {
                var answer = await client.GetAsync(server + controller + $"/Type, Text, Filter?type={type}&text={text}&filter={filter}");
                string answerText = await answer.Content.ReadAsStringAsync();
                var result = (T)JsonSerializer.Deserialize(answerText, typeof(T), jsonOptions);
                return result;
            }

            public static async Task<UserApi> Enter<UserApi>(string UserName, string Password, string controller)
            {
                var answer = await client.GetAsync(server + controller + $"/UserName, Password?userName={UserName}&Password={Password}");
                if (answer.StatusCode == System.Net.HttpStatusCode.NotFound || answer.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    //тут хз че в ретерне
                    return default(UserApi);
                string answerText = await answer.Content.ReadAsStringAsync();
                var result = (UserApi)JsonSerializer.Deserialize(answerText, typeof(UserApi), jsonOptions);
                return result;
            }

            public static async Task<int> RegistrationAsync<T>(T value, string controller)
            {
                var str = JsonSerializer.Serialize(value, typeof(T));
                var answer = await client.PostAsync(server + controller, new StringContent(str, Encoding.UTF8, "application/json"));
                string answerText = await answer.Content.ReadAsStringAsync();
                if (!int.TryParse(answerText, out int result))
                    return -1;
                return result;
            }

            public static async Task<ModelApi> GetModelApi<ModelApi>(string markName, string controller)
            {
                var answer = await client.GetAsync(server + controller + $"/Mark?markName={markName}");
                string answerText = await answer.Content.ReadAsStringAsync();
                var result = (ModelApi)JsonSerializer.Deserialize(answerText, typeof(ModelApi), jsonOptions);
                return result;
            }

            public static async Task<int> PostAsync<T>(T value, string controller) //
            {
                var str = JsonSerializer.Serialize(value, typeof(T));
                var answer = await client.PostAsync(server + controller, new StringContent(str, Encoding.UTF8, "application/json"));
                if (answer.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    return -1;
                }
                string answerText = await answer.Content.ReadAsStringAsync();
                if (!int.TryParse(answerText, out int result))
                    return -1;
                return result;
            }

            public static async Task<bool> PutAsync<T>(T value, string controller) where T : ModelsApi.ApiBaseType
            {
                var str = JsonSerializer.Serialize(value, typeof(T));
                var answer = await client.PutAsync(server + controller + $"/{value.ID}", new StringContent(str, Encoding.UTF8, "application/json"));
                return answer.StatusCode == System.Net.HttpStatusCode.OK;
            }

            public static async Task<bool> DeleteAsync<T>(T value, string controller) where T : ModelsApi.ApiBaseType
            {
                var str = JsonSerializer.Serialize(value, typeof(T));
                var answer = await client.DeleteAsync(server + controller + $"/{value.ID}");
                return answer.StatusCode == System.Net.HttpStatusCode.OK;
            }
            
    }
}
