﻿using MobileMarket.Model;
using MobileMarket.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
        #if DEBUG
            private static string urlBase = "https://192.168.15.33:45455";
        #else
            private static string urlBase = "https://guilherme2109300258.bateaquihost.com.br";
        #endif

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

        public static bool PutUpdateKWH(Ponto ponto)
        {
            using (HttpClientHandler httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (HttpClient client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(urlBase);
                    string URL = urlBase + "/api/ponto/atualizarKWH";
                    FormUrlEncodedContent parametros = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string,string>("codigo",ponto.Codigo.ToString()),
                        new KeyValuePair<string,string>("precoKWH",ponto.PrecoKWH.ToString("F",CultureInfo.CreateSpecificCulture("en-US"))),
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
                                return false;
                            }
                            if (result == "\"Ponto atualizado.\"")
                            {
                                return true;
                            }
                            return false;
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

        public static bool DeletePonto(Page registerPage, Ponto ponto)
        {
            using (HttpClientHandler httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (HttpClient client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(urlBase);
                    string URL = urlBase + "/api/ponto/deletar?" + "codigoUsuario=" + ponto.CodigoUsuario + "&codigoPonto=" + ponto.Codigo;
                    try
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ClienteInfo.Token);
                        HttpResponseMessage response = client.DeleteAsync(URL).GetAwaiter().GetResult();
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

        #region Alarmes
        public static List<Alarme> BuscarAlarmesPorPonto(int codigoPonto)
        {
            using (HttpClientHandler httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (HttpClient client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(urlBase);
                    string URL = urlBase + "/api/alarme?codigoPonto=" + codigoPonto;
                    try
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ClienteInfo.Token);
                        HttpResponseMessage response = client.GetAsync(URL).GetAwaiter().GetResult();
                        if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.BadRequest)
                        {
                            string resultJSON = response.Content.ReadAsStringAsync().Result;
                            List<Alarme> result = JsonConvert.DeserializeObject<List<Alarme>>(resultJSON);
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

        public static bool PostRegisterAlarme(Page registerPage, Alarme alarme)
        {
            using (HttpClientHandler httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (HttpClient client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(urlBase);
                    string URL = urlBase + "/api/alarme/cadastrar";
                    FormUrlEncodedContent parametros = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string,string>("nome",alarme.Nome),
                        new KeyValuePair<string,string>("descricao",alarme.Descricao),
                        new KeyValuePair<string,string>("tipoCondicao",alarme.TipoCondicao.ToString()),
                        new KeyValuePair<string,string>("tipoMedicao",alarme.TipoMedicao.ToString()),
                        new KeyValuePair<string,string>("valorCondicao",alarme.ValorCondicao.ToString()),
                        new KeyValuePair<string,string>("codigoPonto",alarme.CodigoPonto.ToString())
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
                            if (result == "\"Alarme cadastrado.\"")
                            {
                                registerPage.DisplayAlert("Alarme criado", "O alarme foi criado com sucesso.", "OK");
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

        public static bool PutUpdateAlarme(Page registerPage, Alarme alarme)
        {
            using (HttpClientHandler httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (HttpClient client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(urlBase);
                    string URL = urlBase + "/api/alarme/atualizar";
                    FormUrlEncodedContent parametros = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string,string>("codigo",alarme.Codigo.ToString()),
                        new KeyValuePair<string,string>("nome",alarme.Nome),
                        new KeyValuePair<string,string>("descricao",alarme.Descricao),
                        new KeyValuePair<string,string>("tipoCondicao",alarme.TipoCondicao.ToString()),
                        new KeyValuePair<string,string>("tipoMedicao",alarme.TipoMedicao.ToString()),
                        new KeyValuePair<string,string>("valorCondicao",alarme.ValorCondicao.ToString()),
                        new KeyValuePair<string,string>("codigoPonto",alarme.CodigoPonto.ToString())
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
                            if (result == "\"Alarme atualizado.\"")
                            {
                                registerPage.DisplayAlert("Alarme atualizado", "O alarme foi atualizado com sucesso.", "OK");
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

        public static bool DeleteAlarme(Page registerPage, Alarme alarme)
        {
            using (HttpClientHandler httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (HttpClient client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(urlBase);
                    string URL = urlBase + "/api/alarme/deletar?" + "codigo=" + alarme.Codigo + "&codigoPonto=" + alarme.CodigoPonto;
                    try
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ClienteInfo.Token);
                        HttpResponseMessage response = client.DeleteAsync(URL).GetAwaiter().GetResult();
                        if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.BadRequest)
                        {
                            string result = response.Content.ReadAsStringAsync().Result;
                            if (result == "\"Falha ao conectar com o banco.\"")
                            {
                                DisplayConnectionError(registerPage);
                                return false;
                            }
                            if (result == "\"Alarme deletado.\"")
                            {
                                registerPage.DisplayAlert("Alarme deletado", "O alarme foi deletado com sucesso.", "OK");
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
        #endregion

        #region Notificação
        public static List<Notificacao> BuscarNotificacaoPorPonto(int codigoPonto, DateTime? horarioInicial = null, DateTime? horarioFinal = null)
        {
            using (HttpClientHandler httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (HttpClient client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(urlBase);
                    string URL = urlBase + "/api/notificacao?codigoPonto=" + codigoPonto + "&horarioInicial=" + horarioInicial + "&horarioFinal=" + horarioFinal;
                    try
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ClienteInfo.Token);
                        HttpResponseMessage response = client.GetAsync(URL).GetAwaiter().GetResult();
                        if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.BadRequest)
                        {
                            string resultJSON = response.Content.ReadAsStringAsync().Result;
                            List<Notificacao> result = JsonConvert.DeserializeObject<List<Notificacao>>(resultJSON);
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

        public static bool PostRegisterNotificacao(Notificacao notificacao)
        {
            using (HttpClientHandler httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (HttpClient client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(urlBase);
                    string URL = urlBase + "/api/notificacao/cadastrar";
                    FormUrlEncodedContent parametros = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string,string>("nome",notificacao.Nome),
                        new KeyValuePair<string,string>("descricao",notificacao.Descricao),
                        new KeyValuePair<string,string>("tipo",notificacao.Tipo.ToString()),
                        new KeyValuePair<string,string>("codigoPonto",notificacao.CodigoPonto.ToString())
                    });

                    try
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ClienteInfo.Token);
                        HttpResponseMessage response = client.PostAsync(URL, parametros).GetAwaiter().GetResult();
                        if (response.StatusCode == HttpStatusCode.Created)
                        {
                            return true;
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

        public static bool DeleteNotificacao(Notificacao notificacao)
        {
            using (HttpClientHandler httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (HttpClient client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(urlBase);
                    string URL = urlBase + "/api/notificacao/deletar?codigo=" + notificacao.Codigo + "&codigoPonto=" + notificacao.CodigoPonto;
                    try
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ClienteInfo.Token);
                        HttpResponseMessage response = client.DeleteAsync(URL).GetAwaiter().GetResult();
                        if (response.StatusCode == HttpStatusCode.Created)
                        {
                            return true;
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

        public static bool LimparNotificacao(int codigoPonto, DateTime? horarioInicial = null, DateTime? horarioFinal = null)
        {
            using (HttpClientHandler httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (HttpClient client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(urlBase);
                    string URL = urlBase + "/api/notificacao/limpar?codigoPonto=" + codigoPonto + "&horarioInicial=" + horarioInicial + "&horarioFinal=" + horarioFinal;
                    try
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ClienteInfo.Token);
                        HttpResponseMessage response = client.DeleteAsync(URL).GetAwaiter().GetResult();
                        if (response.StatusCode == HttpStatusCode.Created)
                        {
                            return true;
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
        #endregion

        public static ObservableCollection<Medicao> GetMedicoes(int codigoPonto, DateTime? hi = null, DateTime? hf = null)
        {
            using (HttpClientHandler httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (HttpClient client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(urlBase);
                    string his = hi.HasValue ? hi.Value.ToString(CultureInfo.GetCultureInfo("en-US")) : string.Empty;
                    string hfs = hi.HasValue ? hf.Value.ToString(CultureInfo.GetCultureInfo("en-US")) : string.Empty;
                    string URL = urlBase + "/api/medicao?cp=" + codigoPonto + "&hi=" + his + "&hf=" + hfs + "&pti&ptf&pri&prf&fpi&fpf&ci&cf&ti&tf&fi&ff";
                    try
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ClienteInfo.Token);
                        HttpResponseMessage response = client.GetAsync(URL).GetAwaiter().GetResult();
                        if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.BadRequest)
                        {
                            string resultJSON = response.Content.ReadAsStringAsync().Result;
                            ObservableCollection<Medicao> result = JsonConvert.DeserializeObject<ObservableCollection<Medicao>> (resultJSON);
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

        public static void DisplayConnectionError(Page page)
        {
            page.DisplayAlert("Erro de conexão", "Não foi possível estabelecer a conexão com o servidor.", "OK");
        }
    }
}
