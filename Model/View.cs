using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubCentra_A1.Model
{
    public class View
    {

        public event PropertyChangedEventHandler? PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        #region Loading
        public int Loading_formLoadingProgress { get; set; } = 11;
        public int Loading_formLoadingProgress2 { get; set; } = 0;
        #endregion Loading
    }
}
