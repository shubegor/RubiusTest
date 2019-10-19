using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Ascon.Pilot.SDK;
using RubiusTest.Helpers;
using RubiusTest.Helpers;

namespace RubiusTest.Models
{
    /// <summary>
    /// Модель заметки
    /// </summary>
    public class NoteModel : NotifyPropertyChangedBase, IEditableObject
    {
        private Guid _id;
        /// <summary>
        /// Id заметки
        /// </summary>
        public Guid Id
        {
            get { return _id; }
            set { _id = value;
                RaisePropertyChanged(nameof(Id)); }
        }

        private string _topic;
        /// <summary>
        /// Тема заметки
        /// </summary>
        public string Topic
        {
            get { return _topic; }
            set { _topic = value;
                RaisePropertyChanged(nameof(Topic)); }
        }

        private string _content;
        /// <summary>
        /// Содержание заметки
        /// </summary>
        public string Content
        {
            get { return _content; }
            set { _content = value;
                RaisePropertyChanged(nameof(Content)); RaisePropertyChanged(nameof(ContentShort));
            }
        }
        
        /// <summary>
        /// Длинна сокращенной строки 
        /// </summary>
        private const int ContentShortLength = 100;
        
        /// <summary>
        /// Сокращенная строка для отображения в списке без переносов строки
        /// </summary>
        public string ContentShort => _content.Substring(0,_content.Length> ContentShortLength ? ContentShortLength : _content.Length).Replace(Environment.NewLine, "");

        private DateTime _creationDate;
        /// <summary>
        /// Дата создания заметки
        /// </summary>
        public DateTime CreationDate
        {
            get{return _creationDate;}
            set { _creationDate = value;
                RaisePropertyChanged(nameof(CreationDate)); }
        }

        private DateTime _lastEditDate;
        /// <summary>
        /// Дата последнего изменения заметки
        /// </summary>
        public DateTime LastEditDate
        {
            get { return _lastEditDate; }
            set
            {
                _lastEditDate = value;
                RaisePropertyChanged(nameof(LastEditDate));
            }
        }

        private IPerson _author;

        public IPerson Author
        {
            get { return _author; }
            set { _author = value;
                RaisePropertyChanged(nameof(Author)); }
        }

        private NoteModel _tempValue;
        public void BeginEdit()
        {
            _tempValue = new NoteModel()
            {
                Id = this.Id,
                Topic = this.Topic,
                Content = this.Content,
                CreationDate = this.CreationDate,
                LastEditDate = this.LastEditDate,
                Author = this.Author,
            };
        }

        public void EndEdit()
        {
            _tempValue = null;
        }

        public void CancelEdit()
        {
            if (_tempValue == null) return;
            Id = _tempValue.Id;
            Topic = _tempValue.Topic;
            Content = _tempValue.Content;
            CreationDate = _tempValue.CreationDate;
            LastEditDate = _tempValue.LastEditDate;
            Author = _tempValue.Author;
        }
    }
}
