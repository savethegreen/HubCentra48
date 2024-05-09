using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubCentra_A1.Model
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion PropertyChanged

        #region Model
        private View _view;
        public ViewModel(View view)
        {
            _view = view;
        }
        #endregion Model

        #region Loading
        #region variable
        public int Loading_formLoadingProgress
        {
            get => _view.Loading_formLoadingProgress;
            set
            {
                _view.Loading_formLoadingProgress = value;
                OnPropertyChanged(nameof(Loading_formLoadingProgress));
            }
        }
        public int Loading_formLoadingProgress2
        {
            get => _view.Loading_formLoadingProgress2;
            set
            {
                _view.Loading_formLoadingProgress2 = value;
                OnPropertyChanged(nameof(Loading_formLoadingProgress2));
            }
        }
        #endregion variable

        #region Function

        #endregion Function
        #endregion Loading
    }
}
