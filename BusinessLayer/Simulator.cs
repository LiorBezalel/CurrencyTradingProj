using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Simulator(string connectionString)
    {
        private DataRepo repo = new DataRepo(connectionString);
        private Dictionary<string,CurrencyModel> currenciesDict;
        private List<CurrencyPairModel> currencyPairs;
        private Random rnd = new Random();
        private const double minChange = -0.02;
        private const double maxChange = 0.02;
        private bool init = false;


        public async Task Initializer()
            //Initializing currencies and currencyPairs values with up to date values from the database
        {
            List<CurrencyModel>currenciesList = await repo.GetCurrencies();
            currenciesDict = currenciesList.ToList().ToDictionary(curr => curr.Abbreviation, curr => curr);
            currencyPairs = await repo.GetCurrencyPairs();
            init = true;
        }
        private CurrencyModel getRandomlyCurrency()
        {
            //Randomly picks up a currency out of the currencies list
            return currenciesDict[currenciesDict.Keys.ElementAt(rnd.Next(currenciesDict.Count))];
        }
        private async Task<CurrencyModel> ChangeRandomlyCurrencyVal(CurrencyModel currency) {
            //Given a currency model object the function will randomly updates it's current value and saves it to the database
            decimal newVal = currency.CurrentValue * (decimal)(1 + minChange + (rnd.Next() * (minChange + maxChange)));
            try
            {
                await repo.UpdateCurrencyCurrentVal(currency, newVal);
                currency.CurrentValue = newVal;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to save updated value to currency.id" + currency.Id);
                Console.WriteLine(ex.Message);
            }
           return currency;
            
        }
        private async void CheckForMinMaxChanges(CurrencyModel currency) {
            //Checking if an update of a max or a min val for a pair of currency trades is needed.  
            //This function will be called after every change in a currency current val.
            foreach(CurrencyPairModel pair in currencyPairs)
            {
                if(pair.FromCurrency == currency.Abbreviation || pair.ToCurrency == currency.Abbreviation)
                {
                    decimal newTradeRate;

                    if (pair.FromCurrency == currency.Abbreviation)
                    {
                        CurrencyModel currencyPair = currenciesDict[pair.ToCurrency];
                        newTradeRate = currencyPair.CurrentValue / currency.CurrentValue;

                    }
                    else
                    {
                        CurrencyModel currencyPair = currenciesDict[pair.FromCurrency];
                        newTradeRate = currency.CurrentValue / currencyPair.CurrentValue;
                    }
                    if (newTradeRate > pair.MaxValue)
                    {
                        pair.MaxValue = newTradeRate;
                        await repo.UpdateCurrencyPair(pair);
                    }
                    else if(newTradeRate < pair.MinValue)
                    {
                        pair.MinValue = newTradeRate;
                        await repo.UpdateCurrencyPair(pair);
                    }
                }
            }
        }
        public async Task<List<CurrencyPairModel>> GetLatestCurrencyPairs()
        {
            return await repo.GetCurrencyPairs();
        }
        public async Task Simulate()
        {
            while (true && init)
            {
                //var pairs = await repo.GetCurrencyPairs();
                //foreach (var pair in pairs)
                //{
                //    Console.WriteLine($"{pair.FromCurrency}/{pair.ToCurrency} | Min: {pair.MinValue} | Max: {pair.MaxValue}");
                //}
                //Console.WriteLine("-------------------------------");
                CurrencyModel currency = getRandomlyCurrency();
                currency = await ChangeRandomlyCurrencyVal(currency);
                CheckForMinMaxChanges(currency);
                await Task.Delay(2000);

            }
            if (!init)
            {
                Console.WriteLine("Simulator hasn't been initialized...");
            }

        }

    }
}
