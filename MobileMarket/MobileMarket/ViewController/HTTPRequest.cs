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
        private static string urlBase = "https://192.168.15.33:45455";

        public static LoginTokenResult GetLoginToken(string email, string senha)
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
                        new KeyValuePair<string,string>("username",email),
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
                        new KeyValuePair<string,string>("nome",ClienteInfo.Nome),
                        new KeyValuePair<string,string>("email",ClienteInfo.Email),
                        new KeyValuePair<string,string>("senha",ClienteInfo.Senha)
                    });

                    try
                    {
                        HttpResponseMessage response = client.PostAsync(URL, parametros).GetAwaiter().GetResult();
                        if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.BadRequest)
                        {
                            string result = response.Content.ReadAsStringAsync().Result;
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
                    string URL = urlBase + "/api/usuario?email=" + ClienteInfo.Email;

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
                                ClienteInfo.ID = clienteResult.ID;
                                ClienteInfo.Email = clienteResult.Email;
                                ClienteInfo.Nome = clienteResult.Nome;
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

        public static List<Ponto> GetMyPontos()
        {
            using (HttpClientHandler httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (HttpClient client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(urlBase);
                    string URL = urlBase + "/api/ponto?codigoUsuario=" + ClienteInfo.ID;
                    try
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ClienteInfo.Token);
                        HttpResponseMessage response = client.GetAsync(URL).GetAwaiter().GetResult();
                        if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.BadRequest)
                        {
                            string resultJSON = response.Content.ReadAsStringAsync().Result;
                            List<Ponto> result = JsonConvert.DeserializeObject<List<Ponto>>(resultJSON);
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

        public static bool PostRegisterPonto(Page registerPage, Ponto ponto)
        {
            using (HttpClientHandler httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (HttpClient client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(urlBase);
                    string URL = urlBase + "/api/ponto/cadastrar";
                    FormUrlEncodedContent parametros = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string,string>("nome",ponto.Nome),
                        new KeyValuePair<string,string>("descricao",ponto.Descricao),
                        new KeyValuePair<string,string>("codigoUsuario",ponto.CodigoUsuario.ToString())
                    });

                    try
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ClienteInfo.Token);
                        HttpResponseMessage response = client.PostAsync(URL, parametros).GetAwaiter().GetResult();
                        if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.BadRequest)
                        {
                            string result = response.Content.ReadAsStringAsync().Result;
                            if (result == "\"Falha ao conectar com o banco.\"")
                            {
                                DisplayConnectionError(registerPage);
                                return false;
                            }
                            if (result == "\"Ponto cadastrado.\"")
                            {
                                registerPage.DisplayAlert("Ponto criado", "O ponto de medição foi criado com sucesso.", "OK");
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

        public static bool PutUpdatePonto(Page registerPage, Ponto ponto)
        {
            using (HttpClientHandler httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (HttpClient client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(urlBase);
                    string URL = urlBase + "/api/ponto/atualizar";
                    FormUrlEncodedContent parametros = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string,string>("codigo",ponto.Codigo.ToString()),
                        new KeyValuePair<string,string>("nome",ponto.Nome),
                        new KeyValuePair<string,string>("descricao",ponto.Descricao),
                        new KeyValuePair<string,string>("codigoUsuario",ponto.CodigoUsuario.ToString())
                    });

                    try
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ClienteInfo.Token);
                        HttpResponseMessage response = client.PutAsync(URL, parametros).GetAwaiter().GetResult();
                        if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.BadRequest)
                        {
                            string result = response.Content.ReadAsStringAsync().Result;
                            if (result == "\"Falha ao conectar com o banco.\"")
                            {
                                DisplayConnectionError(registerPage);
                                return false;
                            }
                            if (result == "\"Ponto atualizado.\"")
                            {
                                registerPage.DisplayAlert("Ponto atualizado", "O ponto de medição foi atualizado com sucesso.", "OK");
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

        public static bool DeletePonto(Page registerPage, Ponto ponto)
        {
            using (HttpClientHandler httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (HttpClient client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(urlBase);
                    string URL = urlBase + "/api/ponto/deletar";
                    FormUrlEncodedContent parametros = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string,string>("codigo",ponto.Codigo.ToString()),
                        new KeyValuePair<string,string>("nome",ponto.Nome),
                        new KeyValuePair<string,string>("descricao",ponto.Descricao),
                        new KeyValuePair<string,string>("codigoUsuario",ponto.CodigoUsuario.ToString())
                    });

                    try
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ClienteInfo.Token);
                        HttpResponseMessage response = client.PostAsync(URL, parametros).GetAwaiter().GetResult();
                        if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.BadRequest)
                        {
                            string result = response.Content.ReadAsStringAsync().Result;
                            if (result == "\"Falha ao conectar com o banco.\"")
                            {
                                DisplayConnectionError(registerPage);
                                return false;
                            }
                            if (result == "\"Ponto deletado.\"")
                            {
                                registerPage.DisplayAlert("Ponto deletado", "O ponto de medição foi deletado com sucesso.", "OK");
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

        public static void DisplayConnectionError(Page page)
        {
            page.DisplayAlert("Erro de conexão", "Não foi possível estabelecer a conexão com o servidor.", "OK");
        }
    }
}
