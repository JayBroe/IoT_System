using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDataGrabber.ViewModel
{
    using Model;

    public class MeasurementViewModel : INotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    OnPropertyChanged("Nazwa");
                    _name = value;
                }
            }
        }

        private double _value;
        public string Value
        {
            get
            {
                return _value.ToString("N1", CultureInfo.InvariantCulture);
            }
            set
            {
                if (Double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out double tmp) && _value != tmp)
                {
                    OnPropertyChanged("Wartość");
                    _value = tmp;
                }
            }
        }

        private string _unit;
        public string Unit
        {
            get
            {
                return _unit;
            }
            set
            {
                if (_unit != value)
                {
                    _unit = value;
                    OnPropertyChanged("Jednostka");
                }
            }
        }

        public MeasurementViewModel(MeasurementModel model)
        {
            UpdateWithModel(model);
        }

        public void UpdateWithModel(MeasurementModel model)
        {
            _name = model.Name;
            OnPropertyChanged("Nazwa");

            if (model.Value > 180 && model.Unit == "deg")
            {
                _value = model.Value - 360;
            }
            else
            {
                _value = model.Value;
            }
            OnPropertyChanged("Wartość");
            _unit = model.Unit;
            OnPropertyChanged("Jednostka");
        }

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        /**
         * @brief Simple function to trigger event handler
         * @params propertyName Name of ViewModel property as string
         */
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
