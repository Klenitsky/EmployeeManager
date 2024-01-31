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

namespace ExchangeRateService.UI.ViewModels
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private ExchangeRateReader _reader;

        public ApplicationViewModel(string connectionString)
        {
            _reader = new ExchangeRateReader(connectionString);

            //for testing purposes when we don't have access to db
            _activeCurrenciesOnRequest = new ObservableCollection<string> { "USD", "GBR", "BYN" };
            _ratesOnRequest = new ObservableCollection<ExchangeRate>( new List<ExchangeRate> { new ExchangeRate { Date= DateTime.Today, Rate = 1}, new ExchangeRate { Date = DateTime.MinValue, Rate = 2 }, }
                );
            //Main currency Loading
           // _activeCurrenciesOnRequest= new ObservableCollection<string>(_reader.GetActiveCurrencies().Select(c => c.Abbreviation).ToList());

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

        private int _currencyOnRequest = 0;

        public int CurrencyOnRequest
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

        private RelayCommand _getOnRequestCommand;
        public RelayCommand GetOnRequestCommand
        {
            get
            {
                return _getOnRequestCommand ??
                        (_getOnRequestCommand = new RelayCommand(obj =>
                        {
                            RatesOnRequest = new ObservableCollection<ExchangeRate>(_reader.GetRatesOnDateRangeAndCurrency(_startDateOnRequest, _endDateOnRequest, _activeCurrenciesOnRequest[_currencyOnRequest]));
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
