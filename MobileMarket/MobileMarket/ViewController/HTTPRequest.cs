using MobileMarket.Model;
using MobileMarket.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileMarket.ViewController
{
    public class HTTPRequest
    {
        private static string urlBase = "https://192.168.15.10:45455";

        public static LoginTokenResult GetLoginToken(string cpf, string senha)
        {
            using (HttpClientHandler httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (HttpClient client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(urlBase);
                    string URL = urlBase + "/token";
                    FormUrlEncodedContent parametros = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string,string>("grant_type","password"),
                        new KeyValuePair<string,string>("username",cpf),
                        new KeyValuePair<string,string>("password",senha)
                    });

                    try
                    {
                        HttpResponseMessage response = client.PostAsync(URL, parametros).GetAwaiter().GetResult();
                        if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.BadRequest)
                        {
                            string resultJSON = response.Content.ReadAsStringAsync().Result;
                            LoginTokenResult result = JsonConvert.DeserializeObject<LoginTokenResult>(resultJSON);
                            return result;
                        }
                        return null;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
        }

        public static bool PostRegisterClient(Page registerPage)
        {
            using (HttpClientHandler httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (HttpClient client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(urlBase);
                    string URL = urlBase + "/api/register";
                    FormUrlEncodedContent parametros = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string,string>("cpf",ClienteInfo.CPF),
                        new KeyValuePair<string,string>("nome",ClienteInfo.Nome),
                        new KeyValuePair<string,string>("sobrenome",ClienteInfo.Sobrenome),
                        new KeyValuePair<string,string>("email",ClienteInfo.Email),
                        new KeyValuePair<string,string>("senha",ClienteInfo.Senha)
                    });

                    try
                    {
                        HttpResponseMessage response = client.PostAsync(URL, parametros).GetAwaiter().GetResult();
                        if (response.StatusCode == HttpStatusCode.Created)
                        {
                            string result = response.Content.ReadAsStringAsync().Result;
                            if (result == "\"CPF já existe.\"")
                            {
                                registerPage.DisplayAlert("CPF Inválido","Este CPF já está cadastrado.", "OK");
                                return false;
                            }
                            if (result == "\"Email já existe.\"")
                            {
                                registerPage.DisplayAlert("Email Inválido","Este email já está cadastrado.","OK");
                                return false;
                            }
                            if (result == "\"Falha ao conectar com o banco.\"")
                            {
                                DisplayConnectionError(registerPage);
                                return false;
                            }
                            if (result == "\"Cliente cadastrado.\"")
                            {
                                registerPage.DisplayAlert("Cadastro efetivado","O cadastro foi realizado com sucesso.","OK");
                                return true;
                            }
                            DisplayConnectionError(registerPage);
                            return false;
                        }
                        else
                        {
                            DisplayConnectionError(registerPage);
                            return false;
                        }
                    }
                    catch
                    {
                        DisplayConnectionError(registerPage);
                        return false;
                    }
              }
            }
        }

        public static bool UpdateClientInfo()
        {
            using (HttpClientHandler httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (HttpClient client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(urlBase);
                    string URL = urlBase + "/api/client?cpf=" + ClienteInfo.CPF;

                    try
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ClienteInfo.Token);
                        HttpResponseMessage response = client.GetAsync(URL).GetAwaiter().GetResult();
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            string result = response.Content.ReadAsStringAsync().Result;
                            if(result == "null")
                            {
                                return false;
                            }
                            else
                            {
                                Cliente clienteResult = JsonConvert.DeserializeObject<Cliente>(result);
                                ClienteInfo.CPF = clienteResult.CPF;
                                ClienteInfo.Email = clienteResult.Email;
                                ClienteInfo.Nome = clienteResult.Nome;
                                ClienteInfo.Sobrenome = clienteResult.Sobrenome;
                                ClienteInfo.Creditos = clienteResult.Creditos;
                                ClienteInfo.Senha = string.Empty;
                                return true;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
        }

        public static List<Cupom> GetAvailableCupons()
        {
            using (HttpClientHandler httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (HttpClient client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(urlBase);
                    string URL = urlBase + "/api/cupon";
                    try
                    {
                        HttpResponseMessage response = client.GetAsync(URL).GetAwaiter().GetResult();
                        if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.BadRequest)
                        {
                            string resultJSON = response.Content.ReadAsStringAsync().Result;
                            List<Cupom> result = JsonConvert.DeserializeObject<List<Cupom>>(resultJSON);
                            return result;
                        }
                        return null;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
        }

        public static List<CupomResgatado> GetMyCupons()
        {
            using (HttpClientHandler httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (HttpClient client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(urlBase);
                    string URL = urlBase + "/api/cupon?cpf=" + ClienteInfo.CPF;
                    try
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ClienteInfo.Token);
                        HttpResponseMessage response = client.GetAsync(URL).GetAwaiter().GetResult();
                        if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.BadRequest)
                        {
                            string resultJSON = response.Content.ReadAsStringAsync().Result;
                            List<CupomResgatado> result = JsonConvert.DeserializeObject<List<CupomResgatado>>(resultJSON);
                            return result;
                        }
                        return null;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
        }

        public static bool ResgatarCupom(string codigo_cupom)
        {
            using (HttpClientHandler httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (HttpClient client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(urlBase);
                    string URL = urlBase + "/api/cupon?cpf=" + ClienteInfo.CPF + "&codigo_cupom=" + codigo_cupom;

                    try
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ClienteInfo.Token);
                        HttpResponseMessage response = client.PostAsync(URL,null).GetAwaiter().GetResult();
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            string result = response.Content.ReadAsStringAsync().Result;
                            if (result == "\"Cupom resgatado com sucesso!\"")
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
        }

        public static void DisplayConnectionError(Page page)
        {
            page.DisplayAlert("Erro de conexão", "Não foi possível estabelecer a conexão com o servidor.", "OK");
        }
    }
}
