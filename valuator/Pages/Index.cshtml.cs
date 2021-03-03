using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using DBVAL;
using Valuators;

namespace Valuator.Pages
{
    public class IndexModel : PageModel
    {

        private readonly ILogger<IndexModel> _logger;
        private readonly IStorage _storage;

        public IndexModel(ILogger<IndexModel> logger, IStorage storage)
        {
            _logger = logger;
            _storage = storage;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost(string text)
        {

            if (string.IsNullOrEmpty(text))
            {
                return Redirect("/");
            }

            string id = Guid.NewGuid().ToString();
            string textKey = Constants.Text + id;
            string rankKey = Constants.Rank + id;
            string similarityKey = Constants.Simil + id;

            double similarity = GetSimilarity(text);
            double rank = GetRank(text);

            _storage.PutText(text);
            _storage.Put(textKey, text);
            _storage.Put(similarityKey, similarity.ToString());
            _storage.Put(rankKey, rank.ToString());

            return Redirect($"summary?id={id}");
        }

        private double GetRank(string text)
        {
            int nonAlphaCount = 0;
            foreach (var symbol in text)
            {
                if (!Char.IsLetter(symbol))
                {
                    nonAlphaCount++;
                }
            }

            return Math.Round(Convert.ToDouble(nonAlphaCount) / Convert.ToDouble(text.Length), 2);
        }
        private double GetSimilarity(string text)
        {
            return _storage.HasTextDuplicate(text) ? 1 : 0;
        }
    }
}
