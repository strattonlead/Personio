﻿using Newtonsoft.Json;
using Personio.Api.Common;
using Personio.Api.Configuration;
using Personio.Api.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace Personio.Api
{
    public class PersonioClient
    {
        #region Properties

        private readonly PersonioClientOptions _options;

        private string _token { get; set; }

        private HttpClient _getClient
        {
            get
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (!string.IsNullOrWhiteSpace(_options.PartnerId))
                {
                    client.DefaultRequestHeaders.Add("X-Personio-Partner-ID", _options.PartnerId);
                }
                if (!string.IsNullOrWhiteSpace(_options.AppId))
                {
                    client.DefaultRequestHeaders.Add("X-Personio-App-ID", _options.AppId);
                }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                return client;
            }
        }

        private HttpClient _postClient
        {
            get
            {
                var client = _getClient;
                client.DefaultRequestHeaders.Add("content-type", "application/json");
                return client;
            }
        }

        #endregion

        #region Constructor

        public PersonioClient(PersonioClientOptions options)
        {
            _options = options;
        }

        #endregion

        #region Auth

        public async Task<string> AuthAsync()
        {
            return await AuthAsync(_options.ClientId, _options.ClientSecret);
        }

        public async Task<string> AuthAsync(string clientId, string clientSecret)
        {
            var request = new AuthRequest() { ClientId = clientId, ClientSecret = clientSecret };
            return await AuthAsync(request);
        }

        /// <summary>
        /// https://developer.personio.de/reference/post_auth
        /// </summary>
        /// <returns>Authentication token response</returns>
        public async Task<string> AuthAsync(AuthRequest request)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("content-type", "application/json");
            var content = new StringContent(JsonConvert.SerializeObject(request));

            var response = await client.PostAsync("https://api.personio.de/v1/auth", content);
            var result = await response.Content.ReadAsStringAsync();
            var authResponse = JsonConvert.DeserializeObject<AuthResponse>(result);
            _token = authResponse.Data?.Token;
            return _token;
        }

        #endregion

        #region Employees

        /// <summary>
        /// https://developer.personio.de/reference/get_company-employees
        /// </summary>
        /// <returns>List Company Employees</returns>
        public async Task GetEmployeesAsync(GetEmployeesRequest request)
        {
            var url = $"https://api.personio.de/v1/company/employees?limit={request.Limit}&offset={request.Offset}";
            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                url += $"&email={HttpUtility.UrlEncode(request.Email)}";
            }

            if (request.UpdatedSince.HasValue)
            {
                url += $"&updated_since={HttpUtility.UrlEncode(request.UpdatedSince.Value.ToString(""))}";
            }

            if (request.Attributes != null && request.Attributes.Any())
            {
                foreach (var attribute in request.Attributes)
                {
                    url += $"&attributes[]={HttpUtility.UrlEncode(attribute)}";
                }
            }

            var result = await _getClient.GetStringAsync(url);
        }

        /// <summary>
        /// https://developer.personio.de/reference/post_company-employees
        /// 
        /// Creates a new employee. If the employee's status is not provided, it will be set based 
        /// on the hire_date value - if it is in the past, status will be active, otherwise onboarding. 
        /// This endpoint responds with the id of the created employee in case of success.
        /// </summary>
        public async Task<CreateResponse> CreateEmployeeAsync(CreateEmployeeRequest request)
        {
            var content = new StringContent(JsonConvert.SerializeObject(request));
            var response = await _postClient.PostAsync("https://api.personio.de/v1/company/employees", content);
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CreateResponse>(result);
        }

        /// <summary>
        /// https://developer.personio.de/reference/patch_company-employees-employee-id
        /// 
        /// Update existing employee. Note: Updating of Email field is not currently supported.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<UpdateResponse> UpdateEmployeeAsync(UpdateEmployeeRequest request)
        {
            var content = new StringContent(JsonConvert.SerializeObject(request));
            //var response = await _postClient.PatchAsync($"https://api.personio.de/v1/company/employees/{request.Id}", content);

            var message = new HttpRequestMessage(new HttpMethod("PATCH"), $"https://api.personio.de/v1/company/employees/{request.Id}");
            message.Content = content;
            var response = await _postClient.SendAsync(message);

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UpdateResponse>(result);
        }

        /// <summary>
        /// https://developer.personio.de/reference/get_company-employees-employee-id
        /// 
        /// Show employee by ID
        /// </summary>
        public async Task<Employee> GetEmployeeAsync(int id)
        {
            var result = await _getClient.GetStringAsync($"https://api.personio.de/v1/company/employees/{id}");
            return JsonConvert.DeserializeObject<Employee>(result);
        }

        /// <summary>
        /// https://developer.personio.de/reference/get_company-employees-employee-id-profile-picture-width
        /// 
        /// Show employee's profile picture. If profile picture is missing, the 404 error will be thrown. 
        /// The Profile Picture attribute has to be whitelisted.
        public async Task<byte[]> GetProfilePictureAsync(int id, int width)
        {
            var client = _getClient;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/png"));

            var response = await client.GetAsync($"https://api.personio.de/v1/company/employees/{id}/profile-picture/{width}");
            if (response.IsSuccessStatusCode)
            {
                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var mem = new MemoryStream())
                {
                    await stream.CopyToAsync(mem);
                    return mem.ToArray();
                }
            }
            return null;
        }

        #endregion

        #region Attendances

        /// <summary>
        /// https://developer.personio.de/reference/get_company-attendances
        /// 
        /// Fetch attendance data for the company employees. The result can be
        /// paginated and filtered by period, the date and/or time they were 
        /// updated, and/or specific employee/employees. The result 
        /// contains a list of attendances.
        /// </summary>
        public async Task GetAttendancesAsync(GetAttendencesRequest request)
        {
            var url = $"https://api.personio.de/v1/company/attendances?start_date={request.StartDate.ToString(Constants.DATE_FORMAT)}&end_date={request.EndDate.ToString(Constants.DATE_FORMAT)}&limit={request.Limit}&offset={request.Offset}";
            if (request.UpdatedFrom.HasValue)
            {
                url += $"updated_from={request.UpdatedFrom.Value.ToString(Constants.DATE_FORMAT)}";
            }

            if (request.UpdatedTo.HasValue)
            {
                url += $"updated_to={request.UpdatedTo.Value.ToString(Constants.DATE_FORMAT)}";
            }

            if (request.EmployeeIds != null && request.EmployeeIds.Any())
            {
                foreach (var employeeId in request.EmployeeIds)
                {
                    url += $"&employees[]={employeeId}";
                }
            }

            var result = await _getClient.GetStringAsync(url);
#warning TODO schauen was da als datentyp zurück kommt
        }

        /// <summary>
        /// https://developer.personio.de/reference/post_company-attendances
        /// 
        /// This endpoint is responsible for adding attendance data for the 
        /// company employees. It is possible to add attendances for one or many 
        /// employees at the same time. The payload sent on the request should 
        /// be a list of attendance periods, in the form of an array containing 
        /// attendance period objects.
        /// </summary>
        public async Task<CreateResponse> CreateAttendancesAsync(AddAttendancesRequest request)
        {
            var content = new StringContent(JsonConvert.SerializeObject(request));
            var response = await _postClient.PostAsync("https://api.personio.de/v1/company/attendances", content);
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CreateResponse>(result);
        }

        /// <summary>
        /// https://developer.personio.de/reference/delete_company-attendances-id
        /// 
        /// This endpoint is responsible for deleting attendance data for the company employees.
        /// </summary>
        public async Task<DeleteResponse> DeleteAttendancesAsync(int id, bool skipAPproval = true)
        {
            var response = await _getClient.DeleteAsync($"https://api.personio.de/v1/company/attendances/{id}?skip_approval={skipAPproval}");
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DeleteResponse>(result);
        }

        #endregion

        #region Absences

        /// <summary>
        /// https://developer.personio.de/reference/get_company-time-off-types
        /// 
        /// Provides a list of absence types for absences tracked in days and 
        /// hours. For example 'Paid vacation', 'Parental leave' or 'Home office'.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GetTimeOffTypesResponse> GetTimeOffTypesAsync(GetTimeOffTypesRequest request)
        {
            var result = await _getClient.GetStringAsync($"https://api.personio.de/v1/company/time-off-types?limit={request.Limit}&offset={request.Offset}");
            return JsonConvert.DeserializeObject<GetTimeOffTypesResponse>(result);
        }

        /// <summary>
        /// https://developer.personio.de/reference/get_company-time-offs
        /// 
        /// Fetches absence periods for absences tracked in days. The result 
        /// can be paginated and filtered by period and/or specific employee/employees.
        /// The result contains a list of absence periods.
        /// </summary>
        public async Task GetTimeOffsAsync(GetTimeOffsRequest request)
        {
            var url = $"https://api.personio.de/v1/company/time-offs?start_date={request.StartDate.ToString(Constants.DATE_FORMAT)}" +
                $"&end_date={request.StartDate.ToString(Constants.DATE_FORMAT)}" +
                $"&updated_from={request.StartDate.ToString(Constants.DATE_FORMAT)}" +
                $"&updated_to={request.StartDate.ToString(Constants.DATE_FORMAT)}" +
                $"&limit={request.Limit}&offset={request.Offset}";

            if (request.EmployeeIds != null && request.EmployeeIds.Any())
            {
                foreach (var employeeId in request.EmployeeIds)
                {
                    url += $"&employees[]={employeeId}";
                }
            }

            var result = await _getClient.GetStringAsync(url);
#warning TODO hier das model checken was da zurück kommt
        }

        /// <summary>
        /// https://developer.personio.de/reference/post_company-time-offs
        /// 
        /// Adds absence data for absence types tracked in days.
        /// </summary>
        public async Task<CreateResponse> CreateTimeOffAsync(CreateTimeOffRequest request)
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "half_day_start", request.HalfDayStart.ToString().ToLower() },
                { "half_day_end", request.HalfDayEnd.ToString().ToLower() },
                { "employee_id", request.EmployeeId.ToString() },
                { "time_off_type_id", request.TimeOffTypeId.ToString() },
                { "start_date", request.StartDate.ToString(Constants.DATE_FORMAT) },
                { "end_date", request.EndDate.ToString(Constants.DATE_FORMAT) },
                { "comment", request.Comment },
                { "skip_approval", request.SkipApproval.ToString().ToLower() },
            });
            var response = await _postClient.PostAsync("https://api.personio.de/v1/company/time-offs", content);
            var result = await response.Content.ReadAsStringAsync();
#warning TODO hier das model checken was da zurück kommt
            return JsonConvert.DeserializeObject<CreateResponse>(result);
        }

        /// <summary>
        /// https://developer.personio.de/reference/delete_company-time-offs-id
        /// 
        /// Deletes absence period data for absence types tracked in days.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DeleteResponse> DeleteTimeOffAsync(int id)
        {
            var response = await _getClient.DeleteAsync($"https://api.personio.de/v1/company/time-offs/{id}");
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DeleteResponse>(result);
        }

        /// <summary>
        /// https://developer.personio.de/reference/get_company-time-offs-id
        /// 
        /// Gets an absence period for absences tracked in days.
        /// </summary>
        public async Task<GetTimeOffResponse> GetTimeOffAsync(int id)
        {
            var response = await _getClient.GetAsync($"https://api.personio.de/v1/company/time-offs/{id}");
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<GetTimeOffResponse>(result);
        }

        /// <summary>
        /// https://developer.personio.de/reference/get_company-absence-periods
        /// 
        /// Fetches absence periods for absences tracked in hours. The result can be 
        /// paginated and filtered by period and/or specific employee/employees. 
        /// The result contains a list of hourly absence periods.
        /// </summary>
        public async Task GetAbsencePeriodsAsync(GetAbsencePeriodsRequest request)
        {
            var url = $"https://api.personio.de/v1/company/absence-periods" +
                $"?start_date={request.StartDate.ToString(Constants.DATE_FORMAT)}" +
                $"&end_date={request.EndDate.ToString(Constants.DATE_FORMAT)}" +
                $"&updated_from={HttpUtility.UrlEncode(request.UpdatedFrom.ToString(Constants.DATE_TIME_FORMAT))}" +
                $"&updated_to={HttpUtility.UrlEncode(request.UpdatedTo.ToString(Constants.DATE_TIME_FORMAT))}" +
                $"&limit=1&offset=1";

            if (request.EmployeeIds != null && request.EmployeeIds.Any())
            {
                foreach (var employeeId in request.EmployeeIds)
                {
                    url += $"&employees[]={employeeId}";
                }
            }

            if (request.AbsenceTypes != null && request.AbsenceTypes.Any())
            {
                foreach (var absenceType in request.AbsenceTypes)
                {
                    url += $"&absence_types[]={absenceType}";
                }
            }

            if (request.AbsencePeriods != null && request.AbsencePeriods.Any())
            {
                foreach (var absencePeriod in request.AbsencePeriods)
                {
                    url += $"&absence_periods[]={absencePeriod}";
                }
            }

            var response = await _getClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
#warning TODO result anschauen
        }

        /// <summary>
        /// https://developer.personio.de/reference/post_company-absence-periods
        /// 
        /// Adds absence data for absence types tracked in hours. Note that creating 
        /// periods for absence types with certificate requirement enabled is not supported!
        /// </summary>
        public async Task CreateAbsencePeriod(CreateAbsencePeriodRequest request)
        {
            var parameters = new Dictionary<string, string>
            {
                { "employee_id", request.EmployeeId.ToString() },
                { "time_off_type_id", request.TimeOffTypeId.ToString() },
                { "start_date", request.StartDate.ToString(Constants.DATE_FORMAT) },
                { "end_date", request.EndDate.ToString(Constants.DATE_FORMAT)  },
                { "half_day_start", request.HalfDayStart.ToString().ToLower() },
                { "half_day_end", request.HalfDayEnd.ToString().ToLower() },
                { "comment", request.Comment },
                { "skip_approval", request.SkipApproval.ToString().ToLower() },
            };

            if (request.StartTime.HasValue)
            {
                parameters.Add("start_time", request.StartTime.Value.ToString(Constants.TIME_FORMAT));
            }

            if (request.EndTime.HasValue)
            {
                parameters.Add("end_time", request.EndTime.Value.ToString(Constants.TIME_FORMAT));
            }

            var content = new FormUrlEncodedContent(parameters);
            var response = await _postClient.PostAsync("https://api.personio.de/v1/company/absence-periods", content);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
#warning TODO result anschauen
        }

        /// <summary>
        /// https://developer.personio.de/reference/delete_company-absence-periods-id
        /// 
        /// Deletes absence period data for absence types tracked in hours.
        /// </summary>
        public async Task<DeleteResponse> DeleteAbsencePeriodAsync(string id)
        {
            var response = await _getClient.DeleteAsync($"https://api.personio.de/v1/company/absence-periods/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DeleteResponse>(result);
        }

        #endregion
    }
}