using System;
using System.Windows.Input;

namespace OpenSTSM.ViewModels
{
    public abstract class WorkspaceViewModel : ViewModelBase
    {
        RelayCommand _closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                {
                    _closeCommand = new RelayCommand(
                       param => Close(),
                       param => CanClose()
                       );
                }
                return _closeCommand;
            }
        }

        public event Action RequestClose;

        public virtual void Close()
        {
            RequestClose?.Invoke();
        }

        public virtual bool CanClose()
        {
            return true;
        }
    }    
}
