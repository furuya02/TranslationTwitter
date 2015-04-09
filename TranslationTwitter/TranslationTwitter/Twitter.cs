using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CoreTweet;

namespace TranslationTwitter
{
    class Twitter
    {


        const string ApiKey = "{ApiKey}";
        const string ApiSecret = "{ApiSecret}";
        const string AccessToke = "{AccessToke}";
        const string AccessTokeSecret = "{AccessTokeSecret}";

        private readonly Tokens _tokens;
        public Twitter() {
            _tokens = CoreTweet.Tokens.Create(ApiKey, ApiSecret, AccessToke, AccessTokeSecret);
        }
        
        //検索
        public async Task<SearchResult> Search(string keyword) {
            try {
                return await _tokens.Search.TweetsAsync(q => keyword);
            }
            catch (Exception ex) {
                var x = 0;
            }
            return null;
        }
    }
}
