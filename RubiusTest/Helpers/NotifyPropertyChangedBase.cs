using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RubiusTest.Helpers

{
    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public void RaisePropertyChanged(object source, string property)
        {
            PropertyChanged?.Invoke(source, new PropertyChangedEventArgs(property));
        }
        
    }
}