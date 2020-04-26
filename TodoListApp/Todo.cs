using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace TodoListApp
{
    
    public class Todo : IEquatable<Todo>, INotifyPropertyChanged
    {
        public string _UID;
        public string _Title;
        public string _Description;
        public bool _isDone;

        public event PropertyChangedEventHandler PropertyChanged;

        public string UID { get { return _UID; } set { _UID = value;NotifyPropertyChanged("UID"); } }
        public string Title { get { return _Title; } set { _Title = value; NotifyPropertyChanged("Title"); } }
        public string Description { get { return _Description; } set { _Description = value; NotifyPropertyChanged("Description"); } }
        public bool isDone { get { return _isDone; } set { _isDone = value; NotifyPropertyChanged("isDone"); } }
        public Todo(string uid, string title, string description, bool done)
        {
            UID = uid;
            Title = title;
            Description = description;
            isDone = done;
        }
        public Todo(string uid)
        {
            UID = uid;
        }
        public Todo()
        {

        }
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool Equals(Todo other)
        {
            if (other == null)
                return false;
            return this.UID.Equals(other.UID);
        }

    }
}
