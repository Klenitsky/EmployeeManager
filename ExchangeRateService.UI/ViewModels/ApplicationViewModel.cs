using ExchangeRateService.UI.Commands;
using ExchangeRateService.UI.DataReaders;
using ExchangeRateService.UI.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ExchangeRateService.UI.ViewModels
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private ExchangeRateReader _reader;

        public ApplicationViewModel(string connectionString)
        {
            _reader = new ExchangeRateReader(connectionString);

            //for testing purposes when we don't have access to db
            //_activeCurrenciesOnRequest = new ObservableCollection<string> { "USD", "GBR", "BYN" };
            //_ratesOnRequest = new ObservableCollection<ExchangeRate>( new List<ExchangeRate> { new ExchangeRate { Date= DateTime.Today, Rate = 1}, new ExchangeRate { Date = DateTime.MinValue, Rate = 2 }, }
            //    );

            //Main currency Loading
            var loadedCurrencies = _reader.GetActiveCurrencies().Select(c => c.Abbreviation).ToList();
             _activeCurrenciesOnRequest= new ObservableCollection<string>(loadedCurrencies);
            _activeCurrenciesInSystem = new ObservableCollection<string>
            {
                "NONE"
            };
            foreach (var currency in loadedCurrencies)
            {
                _activeCurrenciesInSystem.Add(currency);
            }

            CurrencyInSystem = _activeCurrenciesInSystem[0];
            CurrencyOnRequest = _activeCurrenciesOnRequest[0];
            
        }

        private DateTime _startDateOnRequest = DateTime.Today;
        public DateTime StartDateOnRequest
        {
            get { return _startDateOnRequest; }
            set
            { 
                _startDateOnRequest = value;
                OnPropertyChanged("StartDateOnRequest");
            }
        }

        private DateTime _endDateOnRequest = DateTime.Today;
        public DateTime EndDateOnRequest
        {
            get { return _endDateOnRequest; }
            set
            {
                _endDateOnRequest = value;
                OnPropertyChanged("EndDateOnRequest");
            }
        }

        private ObservableCollection<string> _activeCurrenciesOnRequest;
        public ObservableCollection<string> ActiveCurrenciesOnRequest
        {
            get { return _activeCurrenciesOnRequest; }
            set
            {
                _activeCurrenciesOnRequest = value;
                OnPropertyChanged("ActiveCurrenciesOnRequestOnRequest");
            }
        }

        private string _currencyOnRequest;

        public string CurrencyOnRequest
        {
            get { return _currencyOnRequest; }
            set
            {
                _currencyOnRequest = value; OnPropertyChanged("CurrencyOnRequest");
            }
        }
        
        private ObservableCollection<ExchangeRate> _ratesOnRequest;
        public ObservableCollection<ExchangeRate> RatesOnRequest
        {
            get { return _ratesOnRequest; }
            set
            {
                _ratesOnRequest = value; 
                OnPropertyChanged("RatesOnRequest");
            }
        }
        private string _scaleOnRequest;
        public string ScaleOnRequest
        {
            get { return _scaleOnRequest; }
            set
            {
                _scaleOnRequest = value; OnPropertyChanged("ScaleOnRequest");
            }
        }
        private RelayCommand _getOnRequestCommand;
        public RelayCommand GetOnRequestCommand
        {
            get
            {
                return _getOnRequestCommand ??
                        (_getOnRequestCommand = new RelayCommand(obj =>
                        {
                            RatesOnRequest = new ObservableCollection<ExchangeRate>(_reader.GetRatesOnDateRangeAndCurrencyOnRequest(_startDateOnRequest, _endDateOnRequest, _currencyOnRequest).OrderBy(r=>r.Date));
                            ScaleOnRequest = "Scale: " + _ratesOnRequest.First().Scale;
                        }));
            }
        }


        //InSystemPart
        private DateTime _startDateInSystem = DateTime.Today;
        public DateTime StartDateInSystem
        {
            get { return _startDateInSystem; }
            set
            {
                _startDateInSystem = value;
                OnPropertyChanged("StartDateInSystem");
            }
        }

        private DateTime _endDateInSystem = DateTime.Today;
        public DateTime EndDateInSystem
        {
            get { return _endDateInSystem; }
            set
            {
                _endDateInSystem = value;
                OnPropertyChanged("EndDateInSystem");
            }
        }

        private ObservableCollection<string> _activeCurrenciesInSystem;
        public ObservableCollection<string> ActiveCurrenciesInSystem
        {
            get { return _activeCurrenciesInSystem; }
            set
            {
                _activeCurrenciesInSystem = value;
                OnPropertyChanged("ActiveCurrenciesOnRequestInSystem");
            }
        }

        private string _currencyInSystem;

        public string CurrencyInSystem
        {
            get { return _currencyInSystem; }
            set
            {
                _currencyInSystem = value; OnPropertyChanged("CurrencyInSystem");
            }
        }

        private ObservableCollection<ExchangeRate> _ratesInSystem;
        public ObservableCollection<ExchangeRate> RatesInSystem
        {
            get { return _ratesInSystem; }
            set
            {
                _ratesInSystem = value;
                OnPropertyChanged("RatesInSystem");
            }
        }


        private RelayCommand _getInSystemCommand;
        public RelayCommand GetInSystemCommand
        {
            get
            {
                return _getInSystemCommand ??
                        (_getInSystemCommand = new RelayCommand(obj =>
                        {
                            if (_currencyInSystem.ToUpper() == "NONE".ToUpper())
                            {
                                RatesInSystem = new ObservableCollection<ExchangeRate>(_reader.GetRatesOnDateRangeInSystem(_startDateInSystem, _endDateInSystem).OrderBy(r=> r.Date).ThenBy(r=>r.Abbreviation));
                            }
                            else
                            {
                                RatesInSystem = new ObservableCollection<ExchangeRate>(_reader.GetRatesOnDateRangeAndCurrencyInSystem(_startDateInSystem, _endDateInSystem, _currencyInSystem).OrderBy(r=>r.Date));
                            }
                            
                        }));
            }
        }

        private RelayCommand _loadInSystemCommand;
        public RelayCommand LoadInSystemCommand
        {
            get
            {
                return _loadInSystemCommand ??
                        (_loadInSystemCommand = new RelayCommand(obj =>
                        {
                            RatesInSystem = new ObservableCollection<ExchangeRate>();
                            if (_currencyInSystem.ToUpper() == "NONE".ToUpper())
                            {
                                RatesInSystem = new ObservableCollection<ExchangeRate>(_reader.GetRatesOnDateRangeOnRequest(_startDateInSystem, _endDateInSystem).OrderBy(r => r.Date).ThenBy(r => r.Abbreviation));
                            }
                            else
                            {
                                RatesInSystem = new ObservableCollection<ExchangeRate>(_reader.GetRatesOnDateRangeAndCurrencyOnRequest(_startDateInSystem, _endDateInSystem, _currencyInSystem).OrderBy(r => r.Date));
                            }

                        }));
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string property= "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
