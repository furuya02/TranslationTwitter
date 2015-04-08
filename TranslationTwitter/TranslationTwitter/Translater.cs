using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace TranslationTwitter
{
    class Translator
    {
        private const string ClientId = "{ClientId}";
        private const string ClientSecret = "{ClientSecret}";

        private string _token;


        private async Task<bool> InitializeToken()
        {
            if (_token == null)
            {
                var client = new HttpClient();
                const string url = "https://datamarket.accesscontrol.windows.net/v2/OAuth2-13";
                var content = new FormUrlEncodedContent(new Dictionary<string, string> {
                {"client_id", ClientId},
                {"client_secret", ClientSecret},
                {"scope", "http://api.microsofttranslator.com"},
                {"grant_type", "client_credentials"},
            });
                var response = await client.PostAsync(url, content);
                //JSON形式のレスポンスからaccess_toneを取得する
                var adm = JsonConvert.DeserializeObject<AdmAccessToken>(await response.Content.ReadAsStringAsync());
                _token = adm.access_token;
                return true;
            }
            return false;
        }


        //JSON形式のレスポンスをデシリアライズするためのクラス
        [DataContract]
        public class AdmAccessToken
        {
            [DataMember]
            public string access_token { get; set; }

            //access_token以外は、取得しないので省略
        }

        public async Task<string> Get(string str){

            //初回のみaccess_tokenを取得する
            if (_token == null){
                await InitializeToken();
            }
            var client = new HttpClient();
            var url = string.Format("http://api.microsofttranslator.com/v2/Http.svc/Translate?text={0}&to=ja"
                , WebUtility.UrlEncode(str));
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _token);
            var response = await client.GetStringAsync(url);
            //XML解釈
            var doc = XDocument.Parse(response);
            return doc.Root.FirstNode.ToString();
        }
    }
}
